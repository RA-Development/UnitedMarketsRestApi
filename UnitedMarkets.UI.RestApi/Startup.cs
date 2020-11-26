using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Infrastructure.Data;
using UnitedMarkets.Infrastructure.Data.Repositories;

namespace UnitedMarkets.UI.RestApi
{
    public class Startup
    {
        public IConfiguration _conf { get; }
        private IWebHostEnvironment _env { get; }


        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _conf = configuration;
            _env = webHostEnvironment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsDevelopment())
            {
                services.AddDbContext<UnitedMarketsDbContext>(opt =>
                {
                    opt
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .UseSqlite("Data Source=UnitedMarketsSqLite.db")
                        .EnableSensitiveDataLogging(); // BE AWARE ...   only in dev mode
                }, ServiceLifetime.Transient);
            }

            if (_env.IsProduction())
            {
            }

            // Configure the default CORS policy.
            services.AddCors(options =>
                options.AddDefaultPolicy(
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyOrigin(); })
            );

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IFilterValidator, FilterValidator>();
            services.AddScoped<IProductValidator, ProductValidator>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductSqLiteRepository>();

            services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                using var scope = app.ApplicationServices.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<UnitedMarketsDbContext>();
                var dataInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();

                dataInitializer.InitData();
            }


            if (_env.IsProduction())
            {
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}