using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UrlShortenerService.Cache;
using UrlShortenerService.Data;
using UrlShortenerService.Models;
using UrlShortenerService.Services;
using UrlShortenerService.Utils;

namespace UrlShortenerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment CurrentEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));
            }
            else
            {
                services.AddDbContext<AppDbContext>(options =>
                                  options.UseCosmos(Configuration.GetValue<string>("CosmosDb:Account"),
                                  Configuration.GetValue<string>("CosmosDb:Key"),
                                  Configuration.GetValue<string>("CosmosDb:DatabaseName"))
                              );
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{Configuration.GetValue<string>("Redis:Server")}:{Configuration.GetValue<int>("Redis:Port")}";
            });

            services.AddScoped<IShortUrlKeyRepo, ShortUrlKeyRepo>();
            services.AddScoped<IShortUrlRepo, ShortUrlRepo>();
            services.AddScoped<IUrlKeyGenerationService, UrlKeyGenerationService>();
            services.AddSingleton<IStackCacheService<ShortUrlKey>, StackCacheService<ShortUrlKey>>();
            services.AddScoped<IUrlUtil, UrlUtil>();
            services.AddScoped<ICacheService, CacheService>();

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UrlShortenerService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UrlShortenerService v1"));
                PrepDb.PrepPopulation(app);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            dbContext.Database.EnsureCreated();
        }
    }
}
