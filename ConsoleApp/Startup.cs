using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // middleware 1
            app.Use(async (context, next) => {
                Debug.WriteLine($"Logica del delegado 1");
                await next();
                Debug.WriteLine($"Mas Logica del delegado 1");
            });

            // middleware 2
            app.Run(async (context) =>
            {
                Debug.WriteLine($"Logica del delegado 2");
                await context.Response.WriteAsync("Hello World!");
                Debug.WriteLine($"Mas logica del delegado 2");
            });
        }

    }
}
