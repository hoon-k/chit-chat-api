﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Npgsql;

namespace ChitChatAPI.UserAPI
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
            services.AddSwaggerGen(setup => {
                setup.SwaggerDoc(
                    "v1",
                    new Info {
                        Title = "ChiChat User API",
                        Version = "v1"
                    });
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var isLocalContext = Configuration["EXECUTION_CONTEXT"].Equals("Local");
            if (!isLocalContext && env.IsDevelopment())
            {
                var connString = Configuration["ConnectionString"];
                DataBaseSeedAsync.SeedAsync(connString).Wait();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(setup => {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "ChitChat User API V1");
            });
            app.UseMvc();
        }
    }
}
