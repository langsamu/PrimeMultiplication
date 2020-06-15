// MIT License, Copyright 2020 Samu Lang

namespace PrimeMultiplication.Web
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Net.Http.Headers;
    using Swashbuckle.AspNetCore.SwaggerUI;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Required for testing")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Usage is convention based")]
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRewriter(new RewriteOptions().AddRewrite("^openapi$", "swagger/index.html", false).AddRewrite("^(swagger|favicon)-.+$", "swagger/$0", true));
            app.UseSwaggerUI(ConfigureSwaggerUI);
            app.UseExceptionHandler("/error");
            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(this.configuration);
            services.AddCors(ConfigureCors);
            services.AddMvc(ConfigureMvc);
        }

        private static void ConfigureCors(CorsOptions cors)
        {
            cors
                .AddDefaultPolicy(policy => policy
                .AllowAnyOrigin()
                .WithHeaders(HeaderNames.ContentType)
                .WithMethods(HttpMethods.Post));
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

        private static void ConfigureSwaggerUI(SwaggerUIOptions swaggerUI)
        {
            swaggerUI.DocumentTitle = "Prime Multiplication OpenAPI";
            swaggerUI.SwaggerEndpoint("/openapi.json", "live");
            swaggerUI.DefaultModelsExpandDepth(-1);
            swaggerUI.DisplayRequestDuration();
            swaggerUI.InjectStylesheet("./openapi.css");
            swaggerUI.InjectJavascript("./openapi.js");
            swaggerUI.EnableDeepLinking();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
