using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using asp.net_core_belarus.Data;
using asp.net_core_belarus.Filters;
using asp.net_core_belarus.Loggin;
using asp.net_core_belarus.Middleware;
using asp.net_core_belarus.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace asp.net_core_belarus
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ILogger Logger { get; private set; }

        public IConfiguration Configuration { get ; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NorthwindDB>(options =>
            {
                var connectionString = Configuration.GetConnectionString("Northwind");
                options.UseSqlServer(connectionString);
                Logger.LogInformation(Environment.NewLine + "INFO :  READ CONFIGURATION : ConnectionStrings/Northwind: {0}" + Environment.NewLine, connectionString);
            });
            services.AddScoped<INorthwindService, NorthwindServiceDB>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<LogginActionFilter>();
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP-NET-CORE-BELARUS API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory()+"\\wwwroot\\", "log.txt"));
            Logger = loggerFactory.CreateLogger("FileLogger");
            Logger.LogInformation("INFO :   APPLICATION START ON {0}" + Environment.NewLine, Path.Combine(Directory.GetCurrentDirectory()));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors(builder => builder.AllowAnyOrigin());
            app.UseStatusCodePages();
            app.UseMiddleware<ResponseRewindMiddleware>();
            app.UseMiddleware<ImageCacheMiddleware>();
            app.UseSwagger(c =>
            {
               // c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP-NET-CORE-BELARUS API V1");
            });



        app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "images",
                    template: "images/{image_id}",
                    defaults: new { controller = "Images", action = "GetImage" });

            });
        }
    }
}
