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
            if (!context.ShortLinks.Any())
            {
                Console.WriteLine("Seeding Short Links Data");
                context.ShortLinks.AddRange(
                    new ShortLink()
                    {
                        LinkKey = "4r5t6y",
                        OriginalUrl = "http://google.com",
                        RedirectsCount = 3,
                        CreatedOn = new DateTime(2022, 1, 11),
                        LastRedirect = new DateTime(2022, 2, 11),
                    },
                    new ShortLink()
                    {
                        LinkKey = "r5t6yg",
                        OriginalUrl = "http://fb.com",
                        RedirectsCount = 0,
                        CreatedOn = new DateTime(2022, 1, 12),
                        LastRedirect = new DateTime(2022, 2, 12),
                    },
                    new ShortLink()
                    {
                        LinkKey = "dfgvb6",
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
                Console.WriteLine("We already have Short Links data");
            }

            if (!context.ShortLinkKeys.Any())
            {
                Console.WriteLine("Seeding Short Link Keys Data");
                context.ShortLinkKeys.AddRange(
                    new ShortLinkKey()
                    {
                        LinkKey = "9j4ed5"
                    },
                    new ShortLinkKey()
                    {
                        LinkKey = "e4f5t6"
                    },
                    new ShortLinkKey()
                    {
                        LinkKey = "3e45t6"
                    }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("We already have Short Link Keys data");
            }
        }
    }
}