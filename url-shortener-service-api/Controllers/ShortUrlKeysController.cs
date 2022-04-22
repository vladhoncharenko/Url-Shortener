using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlShortenerService.Data;

namespace UrlShortenerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortUrlKeysController : ControllerBase
    {
        private readonly IShortUrlRepo _shortUrlRepo;
        private readonly IShortUrlKeyRepo _shortUrlKeyRepo;
        private readonly ILogger<UrlShortenerController> _logger;

        public ShortUrlKeysController(ILogger<UrlShortenerController> logger, IShortUrlRepo shortUrlRepo, IShortUrlKeyRepo shortUrlKeyRepo)
        {
            _logger = logger;
            _shortUrlRepo = shortUrlRepo;
            _shortUrlKeyRepo = shortUrlKeyRepo;
        }

        [HttpPost]
        public async Task<ActionResult> TriggerDataRetentionEngine()
        {
            await _shortUrlKeyRepo.DeleteAsync(DateTime.Today.AddMonths(-1));
            _logger.LogInformation("Data purging was run successfully");

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> GenerateNewShortUrlKeys()
        {
            await _shortUrlKeyRepo.GenerateAsync(100);
            _logger.LogInformation("New URL keys auto generation was run successfully");

            return Ok();
        }
    }
}
