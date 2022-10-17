using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reactivities.Aplication.Activities;
using Reactivities.Extensions;
using Reactivities.Middleware;
using System.Net;

namespace Reactivities
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Wee Added [AddFluentValidation()] to the [AddControllers()] So that our [Apllication] is [aware] the [we using] [AddFluentValidation()]
            //The [config] is to [tell Where] the [Validators] are [coming from]. And the [Validation] are in the [ Aplication Project / (Create class/CommandVlidator) ]
            services.AddControllers(opt =>
            {
                //This is to [Make] a [policy/Rule] that all the [Users] [Requires] to be [Authenticated]
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //This will [Add] he [[policy/Rule]] And [make sure], That [Every Single] [Endpoint] in the [API]. [Now] [Requires] [Authentication]. [Unless] we [tell it] [Otherwise].
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddFluentValidation(config =>
            {
                //Here we [specifying] that we want to [use] [something] which is [Create class] the [Aplication Project] in [this case].
                config.RegisterValidatorsFromAssemblyContaining<Create>();
            });
            services.AddApplicationServices(_config);

            services.AddIdentityServices(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //This Have To Be Here At The Top!!!
            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reactivities v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            //This [Has] to be [here] [Before] the [Line] [app.UseAuthorization()] That's [Below VV]
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
