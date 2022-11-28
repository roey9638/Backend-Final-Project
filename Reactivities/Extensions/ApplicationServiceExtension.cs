using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Reactivities.Aplication.Activities;
using Reactivities.Aplication.Core;
using Reactivities.DataDBContext;
using Reactivities.Aplication.Interfaces;
using Reactivities.Secuirty;
using Reactivities.Photos;

namespace Reactivities.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Reactivitiess", Version = "v1" });
            });



            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });



            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000");
                });
            });


            services.AddMediatR(typeof(List.Handler).Assembly);

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            services.AddScoped<IUserAccessor, UserAccessor>();

            services.AddScoped<IPhotoAccessor, PhotoAccessor>();

            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));

            services.AddSignalR();

            return services;
        }
    }
}
