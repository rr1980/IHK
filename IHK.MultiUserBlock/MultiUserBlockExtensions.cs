using IHK.Common;
using IHK.Common.MultiUserBlockCommon;
using IHK.MultiUserBlock.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace IHK.MultiUserBlock
{
    public static class MultiUserBlockExtensions
    {

        public static IApplicationBuilder UseMultiUserBlock(this IApplicationBuilder app,bool debug)
        {
            app.UseWebSockets();

            if (debug)
            {
                app.Map("/mubdebug", (_app) => _app.UseMiddleware<MultiUserBlockMiddlewareDebug>());
            }

            app.Map("/mub", (_app) => _app.UseMiddleware<MultiUserBlockMiddleware>());

            return app;
        }

        public static IServiceCollection AddMultiUserBlockWebService(this IServiceCollection services, IConfigurationRoot configuration)
        {
            //var settings = configuration.GetSection("MultiUserBlock").Get<MultiUserBlockSettings>();

            services.AddOptions();
            services.Configure<MultiUserBlockSettings>(configuration.GetSection("MultiUserBlock"));

            services.AddSingleton<IMultiUserBlockManager, MultiUserBlockManager>();
            services.AddSingleton<IMultiUserBlockWebService, MultiUserBlockWebService>();
            return services;
        }
    }
}
