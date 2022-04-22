using AutoMapper;
using UrlShortenerService.DTOs;
using UrlShortenerService.Models;

namespace UrlShortenerService.Profiles
{
    public class ShortUrlProfile : Profile
    {
        public ShortUrlProfile()
        {
            CreateMap<ShortUrl, ShortUrlReadDTO>().ForMember(l => l.ShortenedUrl, sl => sl.MapFrom(s => "https://localhost:5001/" + s.UrlKey));
            CreateMap<ShortUrlCreateDTO, ShortUrl>();
        }
    }
}