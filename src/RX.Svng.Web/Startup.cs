using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RX.Svng.Web.Cache.Configs;
using RX.Svng.Web.Cache.Repositories;
using RX.Svng.Web.Data.Repositories;
using RX.Svng.Web.Filters;
using RX.Svng.Web.Service.Configs;
using RX.Svng.Web.Service.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace RX.Svng.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            BuildConnectionStrings();
        }

        private const string AllowAnyOrigin = "_allowAnyOrigin";
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowAnyOrigin, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddAuthorization(x =>
            {
                x.AddPolicy("Admin", policy => policy.Requirements.Add(new CognitoGroupAuthorizationRequirement("Admin")));
            });

            services.AddTransient<AwsCredentialsConfiguration>();
            services.AddSingleton<IAuthorizationHandler, CognitoGroupAuthorizationHandler>();
            BindFromAssemblyContaining<IUserLoginService>(services);
            BindFromAssemblyContaining<ICacheRepository>(services);
            BindFromAssemblyContaining<IPharmacyRepository>(services);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(c =>
                {
                    c.Audience = Configuration["UserPoolAppClientId"] ?? throw new ApplicationException("missing required app settings");
                    c.Authority = (Configuration["IdpUrl"] ?? throw new ApplicationException("missing required app settings")) + (Configuration["UserPoolId"] ?? throw new ApplicationException("missing required app settings"));
                });

            services.AddSingleton(Configuration);
            services.AddApiVersioning();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "RX Pharmacy API", Version = "v1" });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        private static void BindFromAssemblyContaining<T>(IServiceCollection services)
        {
            services.Scan(x => x
                            .FromAssemblyOf<T>()
                            .AddClasses()
                            .AsImplementedInterfaces()
                            .WithScopedLifetime());
        }
        private void BuildConnectionStrings()
        {
            RedisConfiguration.RedisConnectionString = Configuration["RedisConnection"];
            RedisConfiguration.RedisDataBaseId = Configuration["RedisDataBaseId"];
        }


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

            app.UseCors(AllowAnyOrigin);
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "RX Pharmacy");
            });
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
