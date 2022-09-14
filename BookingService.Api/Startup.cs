using BookingService.Api.Services;
using BookingService.Api.Services.Interfaces;
using BookingService.Infrastructure;
using BookingService.Infrastructure.Repositories;
using BookingService.Infrastructure.Repositories.Interfaces;
using BookingService.Infrastructure.SeedWorking;
using BookingService.Infrastructure.SeedWorking.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace BookingService.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<BookingServiceSettings>(Configuration);

            services.AddCors();
            services.AddControllers();

            services.AddMediatR(AppDomain.CurrentDomain.Load("BookingService.Domain"));

            services.AddDbContext<BookingServiceContext>(options =>
            {
                options.UseSqlServer(Configuration["BookingServiceContext"]);
                //options.UseInMemoryDatabase("MemoryDB");
            });

            var authKey = Encoding.ASCII.GetBytes( Configuration["AuthSecret"]);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(authKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddScoped<IUnitOfWork, UnitOfWork<BookingServiceContext>>();

            services.AddScoped<IHotelRoomRepository, HotelRoomRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSingleton<IAuthTokenService, AuthTokenService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingService", Description = "API to manage hotel rooms reservations", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger()
               .UseSwaggerUI(options =>
               {
                   options.RoutePrefix = string.Empty;
                   options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
               });
        }
    }
}
