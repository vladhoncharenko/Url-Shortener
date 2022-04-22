using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlShortenerService.Data;
using UrlShortenerService.DTOs;
using UrlShortenerService.Models;

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

        public UrlShortenerController(ILogger<UrlShortenerController> logger, IShortUrlRepo shortUrlRepo, IShortUrlKeyRepo shortUrlKeyRepo, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _shortUrlRepo = shortUrlRepo;
            _shortUrlKeyRepo = shortUrlKeyRepo;
        }

        [HttpPut(Name = "CreateAShortUrl")]
        public async Task<ActionResult<string>> Put(ShortUrlCreateDTO shortUrlCreateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Please pass a valid URL.");

            var shortUrlKey = await _shortUrlKeyRepo.GetAsync();
            var createdShortUrl = new ShortUrl()
            {
                UrlKey = shortUrlKey.UrlKey,
                OriginalUrl = shortUrlCreateDTO.OriginalUrl,
                CreatedOn = DateTime.UtcNow
            };

            await _shortUrlRepo.AddShortUrlAsync(createdShortUrl);
            await _shortUrlRepo.SaveChangesAsync();

            var shortUrlOutput = _mapper.Map<ShortUrlReadDTO>(createdShortUrl);

            return Created(nameof(Get), shortUrlOutput);
        }

        [HttpGet("/{shortUrlKey}", Name = "RedirectToTheOriginalUrl")]
        public async Task<ActionResult> Get(string shortUrlKey)
        {
            if (string.IsNullOrEmpty(shortUrlKey) || shortUrlKey.Length > 6)
                return BadRequest("Url Key should be six characters alphanumeric string.");

            var shortUrl = await _shortUrlRepo.ResolveShortUrlAsync(shortUrlKey);

            return RedirectPermanent(shortUrl.OriginalUrl);
        }

        [HttpGet("{page}/{pageCapacity}", Name = "GetShortenedUrls")]
        public ActionResult<IEnumerable<ShortUrlReadDTO>> GetShortenedUrls(int page, int pageCapacity = 10)
        {
            if (page <= 0 || pageCapacity <= 0)
                return BadRequest("Page Number and Page Capacity should be greater than zero.");

            var shortUrls = _shortUrlRepo.GetShortUrls(page, pageCapacity);

            return Ok(_mapper.Map<IEnumerable<ShortUrlReadDTO>>(shortUrls));
        }
    }
}
