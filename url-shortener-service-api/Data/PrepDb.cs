using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.ShortUrls.Any())
            {
                Console.WriteLine("Seeding Short Urls Data");
                context.ShortUrls.AddRange(
                    new ShortUrl()
                    {
                        UrlKey = "4r5t6y",
                        OriginalUrl = "http://google.com",
                        RedirectsCount = 3,
                        CreatedOn = new DateTime(2022, 1, 11),
                        LastRedirect = new DateTime(2022, 2, 11),
                    },
                    new ShortUrl()
                    {
                        UrlKey = "r5t6yg",
                        OriginalUrl = "http://fb.com",
                        RedirectsCount = 0,
                        CreatedOn = new DateTime(2022, 1, 12),
                        LastRedirect = new DateTime(2022, 2, 12),
                    },
                    new ShortUrl()
                    {
                        UrlKey = "dfgvb6",
                        OriginalUrl = "http://n-ix.com",
                        RedirectsCount = 5,
                        CreatedOn = new DateTime(2022, 1, 13),
                        LastRedirect = new DateTime(2022, 2, 13),
                    }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("We already have Short Urls data");
            }

            if (!context.ShortUrlKeys.Any())
            {
                Console.WriteLine("Seeding Short Url Keys Data");
                context.ShortUrlKeys.AddRange(
                    new ShortUrlKey()
                    {
                        UrlKey = "9j4ed5"
                    },
                    new ShortUrlKey()
                    {
                        UrlKey = "e4f5t6"
                    },
                    new ShortUrlKey()
                    {
                        UrlKey = "3e45t6"
                    }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("We already have Short Url Keys data");
            }
        }
    }
}