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
        private readonly IShortLinkService _shortLinkService;
        private readonly ILogger<UrlShortenerController> _logger;

        public UrlShortenerController(ILogger<UrlShortenerController> logger, IShortLinkService shortLinkService, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _shortLinkService = shortLinkService;
        }

        [HttpPut(Name = "CreateAShortLink")]
        public async Task<ActionResult<string>> Put(ShortLinkCreateDTO shortLinkCreateDTO)
        {
            var createdShortLink = await _shortLinkService.AddNewShortLinkAsync(shortLinkCreateDTO);
            var shortLinkOutput = _mapper.Map<ShortLinkReadDTO>(createdShortLink);

            return Created(nameof(Get), shortLinkOutput);
        }

        [HttpGet("/{shortLinkKey}", Name = "RedirectToTheOriginalLink")]
        public async Task<ActionResult> Get(string shortLinkKey)
        {
            var shortLinkUrl = await _shortLinkService.GetShortLinkRedirectUrlAsync(shortLinkKey);

            return RedirectPermanent(shortLinkUrl);
        }

        [HttpGet("{page}/{pageCapacity}", Name = "GetShortenedUrls")]
        public ActionResult<IEnumerable<ShortLinkReadDTO>> GetShortenedUrls(int page, int pageCapacity = 10)
        {
            var shortLinks = _shortLinkService.GetShortLinks(page, pageCapacity);

            return Ok(_mapper.Map<IEnumerable<ShortLinkReadDTO>>(shortLinks));
        }
    }
}
