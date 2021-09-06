using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Weather.Core;
using Weather.Core.Interfaces.Repositories;
using Weather.Core.Interfaces.Services;
using Weather.Core.Services;
using Weather.Infrastructure.Azure.Repositories;

namespace Weather.Api
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
            services.AddControllers();

            var azureSettings = new AzureSettings
            {
                ConnectionString = Configuration.GetConnectionString("AzureConnectionString")
            };
            services.AddSingleton(azureSettings);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            

            builder.RegisterType<DeviceService>().As<IDeviceService>();
            builder.RegisterType<DeviceRepository>().As<IDeviceRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
