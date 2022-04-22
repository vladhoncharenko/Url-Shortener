using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UrlShortenerService.Data;
using UrlShortenerService.Services;

namespace UrlShortenerService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShortUrlKeysController : ControllerBase
    {
        private readonly IShortUrlRepo _shortUrlRepo;
        private readonly IShortUrlKeyRepo _shortUrlKeyRepo;
        private readonly ILogger<UrlShortenerController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IShortUrlKeyService _shortUrlKeyService;

        public ShortUrlKeysController(ILogger<UrlShortenerController> logger, IShortUrlRepo shortUrlRepo, IShortUrlKeyRepo shortUrlKeyRepo, IConfiguration configuration, IShortUrlKeyService shortUrlKeyService)
        {
            _logger = logger;
            _shortUrlRepo = shortUrlRepo;
            _shortUrlKeyRepo = shortUrlKeyRepo;
            _configuration = configuration;
            _shortUrlKeyService = shortUrlKeyService;
        }

        /// <summary>
        /// Triggers the data retention engine that deletes old keys and shortened URLs
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> TriggerDataRetentionEngine()
        {
            _shortUrlKeyRepo.Delete(DateTime.Today.AddMonths(-_configuration.GetValue<int>("EnvVars:UrlsDataRetentionPeriodInMonths")));
            _shortUrlRepo.Delete(DateTime.Today.AddMonths(-_configuration.GetValue<int>("EnvVars:UrlsDataRetentionPeriodInMonths")));

            await _shortUrlKeyRepo.SaveChangesAsync();
            await _shortUrlRepo.SaveChangesAsync();

            _logger.LogInformation("Data purging was run successfully");

            return Ok();
        }

        /// <summary>
        /// Triggers new keys generation mechanism
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GenerateNewShortUrlKeys()
        {
            await _shortUrlKeyService.GenerateAsync(_configuration.GetValue<int>("EnvVars:AmountOfKeysAutomaticallyGenerated"));
            _logger.LogInformation("New URL keys auto generation was run successfully");

            return Ok();
        }
    }
}
