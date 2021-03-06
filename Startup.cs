using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using krusing_down_the_aisle_backend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace krusing_down_the_aisle_backend
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
         //string connection = Configuration.GetConnectionString("DATABASE"); //DEV
         string connection = Environment.GetEnvironmentVariable("DATABASE"); //PROD

         services.AddDbContext<DataContext>(options => options.UseMySql(connection));

         services.AddCors(options =>
         {
            options.AddPolicy("AllowAll",
               builder =>
               {
                  builder
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials();
               });
         });

         services.AddMvc()
            .AddJsonOptions(
               options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseCors("AllowAll");

         app.UseMvc();
      }
   }
}
