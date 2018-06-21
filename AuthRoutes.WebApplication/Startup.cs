using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthRoutes.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AuthRoutes.Core.Services;

namespace AuthRoutes.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddControllersAsServices();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies", options =>
                {
                    options.AccessDeniedPath = new PathString("/Home/Error");
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = SystemConfig.IdentityServerUrl;
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "webapplication";
                    options.ClientSecret = "webapplication";
                    options.SaveTokens = true;

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add(IdentityConfig.Scopes.WebApplication);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "sub",
                        RoleClaimType = "role",
                    };
                });

            services.AddAuthorization(config =>
            {
                //config.AddPolicy(ApplicationIdentityServerConstants.Policy.AccessAdmin,
                //    policy => policy.RequireClaim(AccessClaims.AdminManagement.ClaimType));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IWebApiHttpClient, WebApiHttpClient>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
