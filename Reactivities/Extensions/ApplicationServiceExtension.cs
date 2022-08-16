using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Reactivities.Aplication.Activities;
using Reactivities.Aplication.Core;
using Reactivities.DataDBContext;

namespace Reactivities.Extensions
{
    public static class ApplicationServiceExtension
    {
        //The [this IServiceCollection] makes it an [Extension] [Methood].
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



            //All of this will allow us to do any [CRUD] [methood] from the [React] which is [localhost:3000]
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
                });
            });



            //For the [Mediator] to work. We added him as a [service]
            //We need to [tell this] [where our] [handlers] are [located] and which [assembly] they [located] at. Continue Downn VV
            //In this case are [handlers] [located] in [List.Handler]
            services.AddMediatR(typeof(List.Handler).Assembly);



            //We need to [tell this] [where our] [Mapping] is [located] and which [assembly] they [located] at. Continue Downn VV
            //In this case are [Mapping] [located] in [MappingProfiles]
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);


            return services;
        }
    }
}
