using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace ChelHackApi
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
            services.AddMvc(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            ConfigureApiBehavior(services);

            services.AddResponseCaching();
            services.AddResponseCompression();

            services.AddTransient(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                var connectionString = new ConnectionString(configuration.GetConnectionString("GoodsDatabase"));
                var database = new MongoClient(connectionString.ToString()).GetDatabase(connectionString.DatabaseName);
                
                database.GetCollection<Good>(nameof(Good)).Indexes.CreateOne(new CreateIndexModel<Good>(Builders<Good>.IndexKeys
                    .Text(x => x.Title)
                    .Text(x => x.Brand)
                    .Text(x => x.Category)));
                
                return database;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowAnyOrigin()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
            );

            //app.UseHttpsRedirection();
            app.UseResponseCaching();
            app.UseMvc();
        }


        public static IServiceCollection ConfigureApiBehavior(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ErrorModel(context.ModelState);

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            return services;
        }
    }
}
