using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;

namespace IHK.MultiUserBlock
{
    public static class MultiUserBlockExtensions
    {

        public static IApplicationBuilder UseMultiUserBlock(this IApplicationBuilder app,
                                                                  PathString path,
                                                                  MultiUserBlockHandlerBase handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<MultiUserBlockMiddleware>(handler));
        }

        public static IServiceCollection AddMultiUserBlockManager(this IServiceCollection services)
        {
            services.AddSingleton<MultiUserBlockWebSocketManager>();
            services.AddSingleton<MultiUserBlockHandler>();

            //foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            //{
            //    if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
            //    {
            //        services.AddSingleton(type);
            //    }
            //}

            return services;
        }
    }
}
