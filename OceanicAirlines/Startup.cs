using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OceanicAirlines.Infrastructure.Data;
using OceanicAirlines.Services;

namespace OceanicAirlines
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
            services.AddRazorPages();
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddSession();
            services.AddMemoryCache();

            services.AddHttpClient();

            services.AddSingleton<IInputValidationService, InputValidationService>();
            services.AddSingleton<ISupportedTypesDataService, SupportedTypesDataService>();
            services.AddSingleton<IIntegrationApiClient, IntegrationApiClient>();
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<IPriceCalculationService, PriceCalculationService>();
            services.AddSingleton<IDijsktraAlgorithmService, DijsktraAlgorithmService>();
            services.AddSingleton<ISegmentService, SegmentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
