using AutoMapper;
using UrlShortenerService.DTOs;
using UrlShortenerService.Models;

namespace UrlShortenerService.Profiles
{
    public class ShortLinkProfile : Profile
    {
        public ShortLinkProfile()
        {
            CreateMap<ShortLink, ShortLinkReadDTO>().ForMember(l => l.ShortenedUrl, sl => sl.MapFrom(s => "https://localhost:5001/" + s.LinkKey));
            CreateMap<ShortLinkCreateDTO, ShortLink>();
        }
    }
}