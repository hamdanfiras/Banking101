using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking101.Models;
using Banking101.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Banking101
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
            // we are getting the connection string from appsettings.json and passing to BankingDB through DbContextOptions 
            // constructor parameter
            string connString = Configuration.GetConnectionString("BankingConnection");
            // write code to decreypt connection from somwhere and put in connString variable
          

            services.AddDbContext<BankingDB>(options =>
            {
                options.UseSqlServer(connString);
            });

            // lazy loading
            //services.AddDbContext<BankingDB>(options =>
            //{
            //    options.UseLazyLoadingProxies().UseSqlServer(connString);
            //});
            



            services.Configure<SMSServiceOptions>(Configuration.GetSection("SMSService"));

            //Microsoft.EntityFrameworkCore
            //Microsoft.EntityFrameworkCore.SqlServer

            //services.AddScoped<ICodeSender, DummyCodeSender>();
            services.AddTransient<ICodeSender, DummyCodeSender>();
            //services.AddTransient<ICodeSender, EmailCodeSender>();
            services.AddTransient<IBulkCodeSender, BulkCodeSender>();

            services.AddScoped<CurrencyCalculator>();


            services.AddControllersWithViews();
            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Add("/{0}.cshtml");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("POWERBY", "TEAM FORMATECH");
                await next.Invoke();
            });

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "profile",
                    pattern: "profile/{id?}",
                    defaults: new { controller = "Profile", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}/{id?}");
            });

            // we used this routing method before asp.net core 3.x
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action}/{param}",
            //        defaults: new { controller = "Some", action = "ShowParam" },
            //        constraints: new { param = "[0-9]+" });
            //});

        }
    }
}
