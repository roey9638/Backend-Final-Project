using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Reactivities.DataDBContext;
using Reactivities.Modules;
using Reactivities.Secuirty;
using Reactivities.Services;
using System.Net;
using System.Text;

namespace Reactivities.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            //The [AddIdentityCore] [Adds and Configures] the [identity system] for the [specified] [User Type].
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
            //This [Registers] are [user stores] and are [role stores] with our [application]
            .AddEntityFrameworkStores<DataContext>()
            //This [Provides] the [API] for [User Sign In]
            .AddSignInManager<SignInManager<AppUser>>();

            //The [config["TokenKey"]] is from the [File] [appsettings.Development.json]. It will [Pass] in the [Key] we want to [Use].
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            //We [need this] [In order] to [get] [access] to the [sign] [SignInManager] Above^^, [When] we need it.
            //The [JwtBearerDefaults.AuthenticationScheme] is the [Type Authentication] that we [using].
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    //Here we [tell our] [API] How to [Validate] that the [Token] is [Valid]
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //This to to [Validate] [against] the [encrypted] [Key]
                        ValidateIssuerSigningKey = true,

                        //This is to [check] if the [Token] that has a [Key] the [User] has [Send] to the [API] That [is not matched] [with] the [Key] [Here VV]. Continue Down VV
                        //Then the [User] will [get] [Unauthorized] [back from] our [API]
                        IssuerSigningKey = key,

                        ValidateIssuer = false,

                        ValidateAudience = false,
                    };
                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsActivityHost", policy =>
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });

            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();

            //This will [mean] that our [TokenService] is [available] when we [Inject] it [into] our [AccountController]. Continue Down VV
            //And it will be [Scoped] to the [life time] of the [Request] to our [API]
            services.AddScoped<TokenService>();

            return services;
        }
    }
}
