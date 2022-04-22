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
    public class ShortUrlKeysController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IShortUrlRepo _shortUrlRepo;
        private readonly IShortUrlKeyRepo _shortUrlKeyRepo;
        private readonly ILogger<UrlShortenerController> _logger;

        public ShortUrlKeysController(ILogger<UrlShortenerController> logger, IShortUrlRepo shortUrlRepo, IShortUrlKeyRepo shortUrlKeyRepo, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _shortUrlRepo = shortUrlRepo;
            _shortUrlKeyRepo = shortUrlKeyRepo;
        }

        [HttpPost]
        public async Task<ActionResult> TriggerDataRetentionEngine()
        {
            await _shortUrlKeyRepo.DeleteAsync(DateTime.Today.AddMonths(-1));

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> GenerateNewShortUrlKeys()
        {
            await _shortUrlKeyRepo.GenerateAsync(100);

            return Ok();
        }
    }
}
