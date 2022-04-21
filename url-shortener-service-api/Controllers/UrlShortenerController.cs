using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlShortenerService.DTOs;
using UrlShortenerService.Services;

namespace UrlShortenerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ShortLinkService _shortLinkService;
        private readonly ILogger<UrlShortenerController> _logger;

        public UrlShortenerController(ILogger<UrlShortenerController> logger, ShortLinkService shortLinkService, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _shortLinkService = shortLinkService;
        }

        [HttpPut("{url}", Name = "CreateAShortLink")]
        public async Task<ActionResult<string>> Put(ShortLinkCreateDTO shortLinkCreateDTO)
        {
            var createdShortLink = await _shortLinkService.AddNewShortLinkAsync(shortLinkCreateDTO);
            var shortLinkOutput = _mapper.Map<ShortLinkReadDTO>(createdShortLink);

            return Created(nameof(Get), shortLinkOutput);
        }

        [HttpGet("/{url}", Name = "RedirectToTheOriginalLink")]
        public async Task<ActionResult> Get(string shortLinkKeyWithUrl)
        {
            var shortLinkUrl = await _shortLinkService.GetShortLinkRedirectUrlAsync(shortLinkKeyWithUrl);

            return RedirectPermanent(shortLinkUrl);
        }

        [HttpGet("{page}/{pageCapacity}", Name = "GetShortenedUrls")]
        public ActionResult<IEnumerable<ShortLinkReadDTO>> GetShortenedUrls(int page, int pageCapacity = 10)
        {
            var shortLinkUrls = _shortLinkService.GetShortLinks(page, pageCapacity);

            return Ok(shortLinkUrls);
        }
    }
}
