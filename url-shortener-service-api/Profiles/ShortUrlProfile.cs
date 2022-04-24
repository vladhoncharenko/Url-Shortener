using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using UrlShortenerService.DTOs;
using UrlShortenerService.Models;

namespace UrlShortenerService.Profiles
{
    public class ShortUrlProfile : Profile
    {
        public ShortUrlProfile(IConfiguration configuration)
        {
            var apiUrl = configuration.GetValue<string>("EnvVars:ApiUrl");
            CreateMap<ShortUrl, ShortUrlReadDTO>().ForMember(l => l.ShortenedUrl, sl => sl.MapFrom(s => apiUrl + s.UrlKey));
            CreateMap<ShortUrlCreateDTO, ShortUrl>();
        }
    }
}