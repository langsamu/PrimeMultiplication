// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Required for testing")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Usage is convention based")]
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(ConfigureMvc);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler("/error");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureMvc(MvcOptions mvc)
        {
            mvc.OutputFormatters.Insert(0, new CsvFormatter());
            mvc.OutputFormatters.Insert(0, new XmlFormatter());
            mvc.OutputFormatters.Insert(0, new JsonFormatter());

            mvc.FormatterMappings.SetMediaTypeMappingForFormat("csv", "text/csv");
            mvc.FormatterMappings.SetMediaTypeMappingForFormat("xml", "text/xml");
            mvc.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
