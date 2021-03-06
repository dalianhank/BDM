using System;
using System.Text;
using AutoMapper;
using BDM.Data.Concrete;
using BDM.Data.Container;
using BDM.Data.Ioc;
using BDM.Lambda.Mapping;
using BDM.Lambda.Service;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BDM.Lambda
{
        public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {
                    var secretBytes = Encoding.UTF8.GetBytes("not_too_short_secret_otherwise_it_might_error");
                    var key = new SymmetricSecurityKey(secretBytes);
                    
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false                         
                    };
                });
       
            services.AddTransient<IBrokerService, BrokerService>();
            services.AddTransient<IBrokerContainer, BrokerContainer>();
            
            services.AddControllers();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BDMMapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            //services.AddDbContext<BDMEntitiesDB>(options => options.UseNpgsql("Server=localhost;User Id=admin;Password=password;Database=BDMPGDatabase;Port=3306;"));
            services.AddDbContext<BDMEntitiesDB>(options => options.UseNpgsql(Configuration["ConnectionStrings:DataAccessPGProvider"]));
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            // services.AddDbContext<BDMEntitiesDB>(options =>
            //                                         options.UseNpgsql(
            //                                                 connectionString
            //                                                 )
            //                                     );
            
            services.AddCors(); 
            //services.AddRepository<DataObj.Broker>(); 
            services.AddUnitScope<BDMEntitiesDB>(); 

                       
            
            services.AddMvc(option => option.EnableEndpointRouting = false);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
                {
                    var securityScheme = new OpenApiSecurityScheme()
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT" // Optional
                    };

                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "bearerAuth"
                                }
                            },
                            new string[] {}
                        }
                    };

                    options.AddSecurityDefinition("bearerAuth", securityScheme);
                    options.AddSecurityRequirement(securityRequirement);
            });

            services.AddHealthChecks()
                .AddCheck("WebAPI", () => HealthCheckResult.Healthy(), new[]{"service"})
                .AddNpgSql(connectionString, 
                            name: "database", 
                            tags: new []{"database"});
            
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
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BDM API V1");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseCors(options => options.WithOrigins("http://localhost:4200")
                                                        .AllowAnyHeader()
                                                        .AllowAnyMethod());  //add consume app domain here

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();
            
            app.UseAuthentication();

            app.UseMvc();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/service", new HealthCheckOptions(){
                        Predicate = _ =>_.Tags.Contains("service"),
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health/db", new HealthCheckOptions(){
                        Predicate = _ =>_.Tags.Contains("database"),
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
