namespace WebApplication1
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(this.ConfigureMvc);
        }

        private void ConfigureMvc(MvcOptions mvc)
        {
            mvc.OutputFormatters.Insert(0, new CsvFormatter());
            mvc.OutputFormatters.Insert(0, new XmlFormatter());
            mvc.OutputFormatters.Insert(0, new JsonFormatter());

            mvc.FormatterMappings.SetMediaTypeMappingForFormat("csv", "text/csv");
            mvc.FormatterMappings.SetMediaTypeMappingForFormat("xml", "text/xml");
            mvc.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
    }
}
