using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlShortenerService.Data;
using UrlShortenerService.DTOs;
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
        private readonly IShortUrlKeyRepo _shortUrlKeyRepo;
        private readonly ILogger<UrlShortenerController> _logger;

        private readonly IShortUrlKeyService _shortUrlKeyService;


        public UrlShortenerController(ILogger<UrlShortenerController> logger, IShortUrlRepo shortUrlRepo, IShortUrlKeyRepo shortUrlKeyRepo, IMapper mapper, IShortUrlKeyService shortUrlKeyService)
        {
            _logger = logger;
            _mapper = mapper;
            _shortUrlRepo = shortUrlRepo;
            _shortUrlKeyRepo = shortUrlKeyRepo;
            _shortUrlKeyService = shortUrlKeyService;
        }

        [HttpPut(Name = "CreateAShortUrl")]
        public async Task<ActionResult<string>> Put(ShortUrlCreateDTO shortUrlCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please pass a valid URL.");

            var shortUrlKey = await _shortUrlKeyService.GetAsync();
            var createdShortUrl = new ShortUrl()
            {
                UrlKey = shortUrlKey.UrlKey,
                OriginalUrl = shortUrlCreateDTO.OriginalUrl,
                CreatedOn = DateTime.UtcNow
            };

            await _shortUrlRepo.AddAsync(createdShortUrl);
            await _shortUrlRepo.SaveChangesAsync();

            var shortUrlOutput = _mapper.Map<ShortUrlReadDTO>(createdShortUrl);

            return Created(nameof(Get), shortUrlOutput);
        }

        [HttpGet("/{shortUrlKey}", Name = "RedirectToTheOriginalUrl")]
        public async Task<ActionResult> Get(string shortUrlKey)
        {
            if (string.IsNullOrEmpty(shortUrlKey) || shortUrlKey.Length > 6)
                return BadRequest("Url Key should be six characters alphanumeric string.");

            var shortUrl = _shortUrlRepo.Get(shortUrlKey);
            if (shortUrl == null || String.IsNullOrEmpty(shortUrl.OriginalUrl))
                throw new ArgumentNullException(nameof(shortUrlKey));

            _shortUrlRepo.RegisterRedirect(shortUrl);
            await _shortUrlRepo.SaveChangesAsync();

            return RedirectPermanent(shortUrl.OriginalUrl);
        }

        [HttpGet("{page}/{pageCapacity}", Name = "GetShortenedUrls")]
        public ActionResult<IEnumerable<ShortUrlReadDTO>> GetShortenedUrls(int page, int pageCapacity = 10)
        {
            if (page <= 0 || pageCapacity <= 0)
                return BadRequest("Page Number and Page Capacity should be greater than zero.");

            var shortUrls = _shortUrlRepo.Get(page, pageCapacity);

            return Ok(_mapper.Map<IEnumerable<ShortUrlReadDTO>>(shortUrls));
        }
    }
}
