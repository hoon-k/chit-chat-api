using System;
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
using Dapper;
using RabbitMQ.Client;
using ChitChatAPI.Common.EventBusRabbitMQ;
using ChitChatAPI.Common.EventBus;

namespace ChitChatAPI.UserAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
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
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                {
                    // var settings = sp.GetRequiredService<IOptions<CatalogSettings>>().Value;
                    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                    var factory = new ConnectionFactory()
                    {
                        HostName = Configuration["EventBusConnection"],
                        DispatchConsumersAsync = true
                    };

                    if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                    {
                        factory.UserName = Configuration["EventBusUserName"];
                    }

                    if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                    {
                        factory.Password = Configuration["EventBusPassword"];
                    }

                    // var retryCount = 5;
                    // if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                    // {
                    //     retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                    // }

                    return new DefaultRabbitMQPersistentConnection(factory);
                })
            ;

            var subscriptionClientName = Configuration["SubscriptionClientName"];
            services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
                {
                    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                    // var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    // var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    // var retryCount = 5;
                    // if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                    // {
                    //     retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                    // }

                    return new RabbitMQEventBus(rabbitMQPersistentConnection, eventBusSubcriptionsManager, subscriptionClientName);
                }
            );
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

            var isLocalContext = Configuration["ExecutionContext"].Equals("Local");
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
