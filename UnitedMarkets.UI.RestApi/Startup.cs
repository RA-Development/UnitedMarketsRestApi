using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UnitedMarkets.Core.ApplicationServices;
using UnitedMarkets.Core.ApplicationServices.HelperServices;
using UnitedMarkets.Core.ApplicationServices.Services;
using UnitedMarkets.Core.ApplicationServices.Validators;
using UnitedMarkets.Core.DomainServices;
using UnitedMarkets.Core.Entities;
using UnitedMarkets.Core.Entities.AuthenticationModels;
using UnitedMarkets.Core.Filtering;
using UnitedMarkets.Core.PriceCalculator;
using UnitedMarkets.Infrastructure.Data;
using UnitedMarkets.Infrastructure.Data.Repositories;

namespace UnitedMarkets.UI.RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Conf = configuration;
            Env = webHostEnvironment;
        }

        private IConfiguration Conf { get; }
        private IWebHostEnvironment Env { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); }
            );
            if (Env.IsDevelopment())
            {
                services.AddDbContext<UnitedMarketsDbContext>(opt =>
                {
                    opt
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .UseLoggerFactory(loggerFactory)
                        .UseSqlite("Data Source=UnitedMarketsSqLite.db")
                        .EnableSensitiveDataLogging(); // BE AWARE ...   only in dev mode
                }, ServiceLifetime.Transient);
            }
            
            if (Env.IsProduction())
            {
                services.AddDbContext<UnitedMarketsDbContext>(opt =>
                {
                    opt
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .UseSqlServer(Conf.GetConnectionString("defaultConnection"));
                }, ServiceLifetime.Transient);
            }
            
            // Register repositories and services for dependency injection.
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddScoped<IValidator<Filter>, FilterValidator>();
            services.AddScoped<IValidator<Product>, ProductValidator>();
            services.AddScoped<IValidator<LoginInputModel>, LoginInputModelValidator>();
            services.AddScoped<IValidatorExtended<OrderLine>, OrderLineValidator>();
            services.AddScoped<IValidatorExtended<Order>, OrderValidator>();

            services.AddScoped<IPriceCalculator, PriceCalculator>();

            services.AddScoped<IService<Market>, MarketService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IService<Order>, OrderService>();
            services.AddScoped<IService<Product>, ProductService>();
            services.AddScoped<IService<Status>, StatusService>();


            services.AddScoped<IRepository<Market>, MarketSqLiteRepository>();
            services.AddScoped<IRepository<User>, UserSqLiteRepository>();
            services.AddScoped<IRepository<Order>, OrderSqLiteRepository>();
            services.AddScoped<IRepository<Product>, ProductSqLiteRepository>();
            services.AddScoped<IRepository<Status>, StatusSqLiteRepository>();


            services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // Configure the default CORS policy. TODO: Specified policies.
            services.AddCors(options =>
                options.AddDefaultPolicy(
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); })
            );

            // Create a byte array with random values to generate a key for signing JWT tokens.
            var secretBytes = new byte[40];
            var rand = new Random();
            rand.NextBytes(secretBytes);

            // Add JWT based authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    //ValidAudience = "TodoApiClient",
                    ValidateIssuer = false,
                    //ValidIssuer = "TodoApi",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    ValidateLifetime = true, //validate the expiration and not before values in the token
                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

            services.AddSingleton<IAuthenticationHelper>(new
                AuthenticationHelper(secretBytes));
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
            else
            {
                using var scope = app.ApplicationServices.CreateScope();
                var ctx = scope.ServiceProvider.GetService<UnitedMarketsDbContext>();
                ctx.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //TODO: Change to use policies specific to the environment.
            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}