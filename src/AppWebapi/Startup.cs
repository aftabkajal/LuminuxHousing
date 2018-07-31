using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Interfaces;
using AppInfrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppWebapi
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
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddMvc();
        }

        // This method gets called when Testing Environment is used
        // Use this method to set Testing services, like Testng database
        public void ConfigureTestingServices(IServiceCollection services)
        {
            // Configure in-memory database
            services.AddDbContext<LuminuxContext>(options =>
                options.UseInMemoryDatabase("Luminux"));

            ConfigureServices(services);
        }

        // This method gets called when Production Environment is used
        // Use this method to set Production services, like Production database
        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<LuminuxContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LuminuxConnection"))
            );

            ConfigureServices(services);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
