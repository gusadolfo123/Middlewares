using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // uso de map, este sirve para crear una rama de canalizacion basada en una ruta especificada
            app.Map("/map1", HandleMapTest1);
            app.Map("/map2", HandleMapTest2);

            // se ejecuta cuando en la queryparameter se encuentra la palabra branch
            app.MapWhen(context => context.Request.Query.ContainsKey("branch"), HandleBranch);

            // multiples segmentos
            app.Map("/map3/seg1", HandleMultiSeg);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Delegado no Mapeado");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void HandleMultiSeg(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"Llamado desde Segmento Multiple");
            });
        }

        private void HandleBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var branch = context.Request.Query["branch"];
                await context.Response.WriteAsync($"Valor de la variable branch: {branch}");
            });
        }

        private void HandleMapTest2(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Prueba {nameof(HandleMapTest2)}");
            });
        }

        private void HandleMapTest1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"Prueba {nameof(HandleMapTest1)}");
            });
        }
    }
}
