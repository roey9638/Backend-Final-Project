using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reactivities.Aplication.Core;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Reactivities.Middleware
{
    public class ExceptionMiddleware
    {
        public RequestDelegate _next { get; }
        public ILogger<ExceptionMiddleware> _logger { get; }
        public IHostEnvironment _env { get; }


        //The [IHostEnvironmen env] [param] well [tell us] if we [In] [Development Mode] or [Production Mode]
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //When a [Reques] [Comes in]. I'ts gonna [pass] [straight through] our [excptional handling middlware]. And [Continue] to the [Other] [middleware].
                //But if theres an [Exception] it will go to the [catch]
                await _next(context);
            }
            catch (Exception ex)
            {
                //Here i want to [print] the [Error] inside the [terminal window] where we [run] our [application].
                _logger.LogError(ex, ex.Message);

                //Here the [ContentType] that's [going] to be [Returning]. And it's ["application/json"].
                context.Response.ContentType = "application/json";

                //So this [ (int)HttpStatusCode.InternalServerError ] will [Set] the [StatusCode] to be (500).
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //The [response] is [going] to [check] if we [using] the [Development Mode].
                var response = _env.IsDevelopment()
                    //If [we are] [In] [Development Mode]. Then will send the Back the [Full Exception] with the [Stack trace]. Continue Down VV
                    ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())

                    //And If we [are Not] [In] [Development Mode]
                    : new AppException(context.Response.StatusCode, "Server Error");

                //This will [ensure] that our [Response] is in [CamelCase] [Rather] than [title Case].
                //[Maybe Not True] --> But it will be an [Optinol]
                var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                //Here we [Conver] the [response] and the [options] to [Json].
                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
