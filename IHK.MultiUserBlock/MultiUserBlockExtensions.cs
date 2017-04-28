using IHK.Common.MultiUserBlockCommon;
using IHK.MultiUserBlock.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddOptions();
            services.Configure<MultiUserBlockSettings>(configuration.GetSection("MultiUserBlock"));

            services.AddSingleton<IMultiUserBlockManager, MultiUserBlockManager>();
            services.AddSingleton<IMultiUserBlockWebService, MultiUserBlockWebService>();
            return services;
        }
    }
}
