﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IHK.DB;
using IHK.DB.SeedBuilder;
using IHK.Web.Authorization;
using IHK.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using IHK.Services;
using IHK.Repositorys;
using IHK.MultiUserBlock;

namespace IHK.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DefaultPolicy", policy => policy.Requirements.Add(new AuthPolicyRequirement(UserRoleType.Default)));
                options.AddPolicy("AdminPolicy", policy => policy.Requirements.Add(new AuthPolicyRequirement(UserRoleType.Admin)));
            });

            services.AddSingleton(Configuration);
            services.AddDbContext<DataContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationHandler, AuthPolicyHandler>();
            services.AddSingleton<UserRepository>();
            services.AddSingleton<OptionRepository>();
            services.AddSingleton<MieterRepository>();
            services.AddSingleton<WohnungRepository>();
            services.AddSingleton<GebaeudeRepository>();
            services.AddSingleton<AdresseRepository>();
            services.AddScoped<AccountService>();
            services.AddScoped<OptionService>();
            services.AddScoped<MieterService>();
            services.AddScoped<WohnungService>();
            services.AddScoped<GebaeudeService>();
            services.AddScoped<AdresseService>();

            services.AddMultiUserBlockManager();

        }

        public void Configure(IApplicationBuilder app,IServiceProvider serviceProvider, IHostingEnvironment env, ILoggerFactory loggerFactory, DataContext dataContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error/Error");
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
                LoginPath = "/Account/Login",
                AccessDeniedPath = "/Account/Login",
                LogoutPath = "/Account/Logout",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieSecure = CookieSecurePolicy.SameAsRequest,
                //CookiePath = "/",
                CookieHttpOnly = true,
                SlidingExpiration = true,
                CookieName = "rrAuth",
            });

            app.UseWebSockets();
            app.UseMultiUserBlock(true);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Main}/{action=Index}");
            });
            
            SeedData.Seed(dataContext);
        }
    }
}
