using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UrlShortenerService.Cache;
using UrlShortenerService.Data;
using UrlShortenerService.DTOs;
using UrlShortenerService.Messaging;
using UrlShortenerService.Models;
using UrlShortenerService.Services;

namespace UrlShortenerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IShortUrlRepo _shortUrlRepo;
        private readonly ICacheService _cacheService;
        private readonly IShortUrlKeyRepo _shortUrlKeyRepo;
        private readonly IShortUrlKeyService _shortUrlKeyService;
        private readonly ILogger<UrlShortenerController> _logger;
        private readonly IMessagesSender<UrlRedirectMessage> _messagesSender;

        public UrlShortenerController(ILogger<UrlShortenerController> logger, IShortUrlRepo shortUrlRepo,
            IShortUrlKeyRepo shortUrlKeyRepo, IMapper mapper, IShortUrlKeyService shortUrlKeyService,
            IMessagesSender<UrlRedirectMessage> messagesSender, ICacheService cacheService)
        {
            _logger = logger;
            _mapper = mapper;
            _shortUrlRepo = shortUrlRepo;
            _shortUrlKeyRepo = shortUrlKeyRepo;
            _shortUrlKeyService = shortUrlKeyService;
            _messagesSender = messagesSender;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Creates new shorten for provided URL
        /// </summary>
        /// <param name="shortUrlCreateDTO">Object with an original url to shorten</param>
        /// <returns>Returns created Short URL object</returns>
        [HttpPut(Name = "CreateAShortUrl")]
        public async Task<ActionResult<ShortUrlReadDTO>> Put(ShortUrlCreateDTO shortUrlCreateDTO)
        {
            if (!ModelState.IsValid || shortUrlCreateDTO.OriginalUrl.Contains(" "))
                return BadRequest("Please pass a valid URL.");

            var shortUrlKey = await _shortUrlKeyService.GetAsync();
            var createdShortUrl = new ShortUrl()
            {
                UrlKey = shortUrlKey.UrlKey,
                OriginalUrl = shortUrlCreateDTO.OriginalUrl,
                CreatedOn = DateTime.UtcNow,
                LastRedirect = null
            };

            await _shortUrlRepo.AddAsync(createdShortUrl);
            await _shortUrlRepo.SaveChangesAsync();
            await _cacheService.SetAsync<ShortUrl>(createdShortUrl.UrlKey, createdShortUrl);

            var shortUrlOutput = _mapper.Map<ShortUrlReadDTO>(createdShortUrl);

            return Created(nameof(Get), shortUrlOutput);
        }

        /// <summary>
        /// Redirects request to an original (shortened) URL
        /// </summary>
        /// <param name="shortUrlKey">Key to match the original URL</param>
        /// <returns>Redirects to the original URL</returns>
        [HttpGet("/{shortUrlKey}", Name = "RedirectToTheOriginalUrl")]
        public async Task<ActionResult> Get(string shortUrlKey)
        {
            if (string.IsNullOrEmpty(shortUrlKey) || shortUrlKey.Length > 6)
                return BadRequest("Url Key should be six characters alphanumeric string.");

            var shortUrl = await _cacheService.GetAsync<ShortUrl>(shortUrlKey);
            if (shortUrl == null)
            {
                shortUrl = _shortUrlRepo.Get(shortUrlKey);
                if (shortUrl != null)
                    await _cacheService.SetAsync<ShortUrl>(shortUrl.UrlKey, shortUrl);
            }

            if (shortUrl == null || String.IsNullOrEmpty(shortUrl?.OriginalUrl))
                return BadRequest("Please pass a valid URL key.");

            await _messagesSender.SendMessageAsync(new UrlRedirectMessage(shortUrl.UrlKey));

            return RedirectPermanent(shortUrl.OriginalUrl);
        }

        /// <summary>
        /// Returns shortened URLs by portions
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageCapacity">Amount of items per page</param>
        /// <returns>Collection of shortened URLs</returns>
        [HttpGet("{page}/{pageCapacity}", Name = "GetShortenedUrls")]
        public ActionResult<IEnumerable<ShortUrlReadDTO>> GetShortenedUrls(int page, int pageCapacity = 10)
        {
            if (page <= 0 || pageCapacity <= 0)
                return BadRequest("Page Number and Page Capacity should be greater than zero.");

            var shortUrls = _shortUrlRepo.Get(page, pageCapacity);
            var response = new { Count = shortUrls.Item2, Data = _mapper.Map<IEnumerable<ShortUrlReadDTO>>(shortUrls.Item1) };

            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
