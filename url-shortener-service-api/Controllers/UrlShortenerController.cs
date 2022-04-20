using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlShortenerService.DTOs;

namespace UrlShortenerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly ILogger<UrlShortenerController> _logger;

        public UrlShortenerController(ILogger<UrlShortenerController> logger)
        {
            _logger = logger;
        }

        [HttpPut("{url}", Name = "CreateAShortLink")]
        public async Task<ActionResult<string>> Put(ShortLinkCreateDTO url)
        {
            return Ok("https://test.com");
        }

        [HttpGet("/{url}", Name = "RedirectToTheOriginalLink")]
        public async Task<ActionResult> Get(string url)
        {
            return RedirectPermanent("https://www.google.com");
        }

        [HttpGet("{page}/{pageCapacity}", Name = "GetShortenedUrls")]
        public async Task<ActionResult<IEnumerable<ShortLinkReadDTO>>> Get(int page, int pageCapacity = 10)
        {
            return Ok("https://test.com");
        }
    }
}
