namespace MaterialUI
{
    using System;
    using Core.Extension.DependencyInjection;
    using DependencyInjection.Analyzer;
    using EntityFrameworkCore.SqlProfile;
    using MaterialUI.Entity;
    using MaterialUI.Html;
    using MaterialUI.Html.TextBoxs;
    using MaterialUI.ViewConfiguration.Schedule;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Route.Generator;
    using Serilog;
    using Serilog.Events;

    public class Startup
    {
        public static ServiceProvider ServiceProvider { get; set; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.LogConfig();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MaterialUIContext>(options =>
            {
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
                options.UseSqlProfile(); // Add when use SqlServer
            });
            services.AddSingleton(typeof(IGetHtml), typeof(GetHtml));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDependencyInjectionAnalyzer();
            JsonExtension.AddService(services);
            services.AddRouteAnalyzer();
            services.AddSingleton(typeof(EmptyColumn<,>));
            services.AddScoped(typeof(LogDialog));

            ServiceProvider = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseMyExceptionHandler(loggerFactory);
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(365),
                    };
                },
            });
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Schedule}/{action=Index}/{id?}");
            });
        }

        private void LogConfig()
        {
            Serilog.Log.Logger = new LoggerConfiguration()
           .Enrich.FromLogContext()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("System", LogEventLevel.Information)
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.Async(
               a =>
               {
                   a.RollingFile("File/logs/log-{Date}-Debug.txt");
               }))
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.Async(
               a =>
               {
                   a.RollingFile("File/logs/log-{Date}-Information.txt");
               }))
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.Async(
               a =>
               {
                   a.RollingFile("File/logs/log-{Date}-Warning.txt");
               }))
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.Async(
               a =>
               {
                   a.RollingFile("File/logs/log-{Date}-Error.txt");
               }))
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.Async(
               a =>
               {
                   a.RollingFile("File/logs/log-{Date}-Fatal.txt");
               }))
           // 所有情况
           .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => true)).WriteTo.Async(
               a =>
               {
                   a.RollingFile("File/logs/log-{Date}-All.txt");
               })
          .CreateLogger();
        }
    }
}
