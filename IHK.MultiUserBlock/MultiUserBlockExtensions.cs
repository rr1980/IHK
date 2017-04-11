using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;

namespace IHK.MultiUserBlock
{
    public static class MultiUserBlockExtensions
    {

        public static IApplicationBuilder UseMultiUserBlock(this IApplicationBuilder app, bool debug = false)
        {
            if (debug)
            {
                app.Map("/mubdebug", (_app) => _app.UseMiddleware<MultiUserBlockMiddlewareDebug>());
            }

            //app.Map("/mub", (_app) => _app.UseMiddleware<MultiUserBlockMiddleware>());
            app.Map("/mub", (_app) => _app.UseMiddleware<MultiUserBlockMiddleware>());


            return app;
        }

        public static IServiceCollection AddMultiUserBlockManager(this IServiceCollection services)
        {
            //services.AddSingleton<MultiUserBlockWebSocketManager>();
            //services.AddSingleton<MultiUserBlockHandler>();

            services.AddScoped<MultiUserBlockWebService>();

            //foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            //{
            //if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
            //    {
            //        services.AddSingleton(type);
            //    }
            //}

            return services;
        }
    }
}
