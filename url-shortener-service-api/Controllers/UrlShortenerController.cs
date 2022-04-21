using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            if (!ModelState.IsValid)
                return BadRequest("Please pass a valid URL.");

            ShortLinkReadDTO shortLinkOutput;

            try
            {
                var createdShortLink = await _shortLinkService.AddNewShortLinkAsync(shortLinkCreateDTO);
                shortLinkOutput = _mapper.Map<ShortLinkReadDTO>(createdShortLink);
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is ArgumentOutOfRangeException)
                {
                    return BadRequest();
                }
                else
                {
                    _logger.LogError("Issue during creating a new short link:", e.Message, e.InnerException);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Created(nameof(Get), shortLinkOutput);
        }

        [HttpGet("/{shortLinkKey}", Name = "RedirectToTheOriginalLink")]
        public async Task<ActionResult> Get(string shortLinkKey)
        {
            if (string.IsNullOrEmpty(shortLinkKey) || shortLinkKey.Length > 6)
                return BadRequest("Link Key should be six characters alphanumeric string.");

            string shortLinkUrl;

            try
            {
                shortLinkUrl = await _shortLinkService.GetShortLinkRedirectUrlAsync(shortLinkKey);
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is ArgumentOutOfRangeException)
                {
                    return BadRequest();
                }
                else
                {
                    _logger.LogError("Issue during resolving a short link:", e.Message, e.InnerException);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return RedirectPermanent(shortLinkUrl);
        }

        [HttpGet("{page}/{pageCapacity}", Name = "GetShortenedUrls")]
        public ActionResult<IEnumerable<ShortLinkReadDTO>> GetShortenedUrls(int page, int pageCapacity = 10)
        {
            if (page <= 0 || pageCapacity <= 0)
                return BadRequest("Page Number and Page Capacity should be greater than zero.");

            IEnumerable<ShortLink> shortLinks;

            try
            {
                shortLinks = _shortLinkService.GetShortLinks(page, pageCapacity);
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is ArgumentOutOfRangeException)
                {
                    return BadRequest();
                }
                else
                {
                    _logger.LogError("Issue during getting a list of short links:", e.Message, e.InnerException);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }


            return Ok(_mapper.Map<IEnumerable<ShortLinkReadDTO>>(shortLinks));
        }
    }
}
