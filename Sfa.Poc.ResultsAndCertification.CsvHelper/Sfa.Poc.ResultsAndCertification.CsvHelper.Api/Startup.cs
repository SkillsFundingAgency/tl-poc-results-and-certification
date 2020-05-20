using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Extensions;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Infrastructure;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Configuration;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Services;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Repositories;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;
using System;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api
{
    public class Startup
    {
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ResultsAndCertificationConfiguration = ConfigurationLoader.Load(
               Configuration[Constants.EnvironmentNameConfigKey],
               Configuration[Constants.ConfigurationStorageConnectionStringConfigKey],
               Configuration[Constants.VersionConfigKey],
               Configuration[Constants.ServiceNameConfigKey]);

            services.AddControllers();

            services.AddMvc(config =>
            {
                //config.Filters.Add<ValidateModelAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .ConfigureApiBehaviorOptions(options =>
            {
                //options.SuppressConsumesConstraintForFormFileParameters = true;
                //options.SuppressInferBindingSourcesForParameters = true;
                //options.SuppressModelStateInvalidFilter = true;
                //options.SuppressMapClientErrors = true;

            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new BadRequestObjectResult(new BadRequestResponse(actionContext.ModelState));
                };
            });

            RegisterDependencies(services);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ResultsAndCertification WebAPI", Version = "v1" });
                //c.OperationFilter<SwaggerFileOperationFilter>();
                //c.OperationFilter<FormFileSwaggerFilter>();

                //// Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
            IdentityModelEventSource.ShowPII = true;
            services.AddApiAuthentication(ResultsAndCertificationConfiguration).AddApiAuthorization();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseXContentTypeOptions();
            app.UseExceptionHandler("/errors/500");
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            //Accept All HTTP Request Methods from all origins
            //app.UseCors(builder =>
            //    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            //app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
            app.ConfigureExceptionHandlerMiddleware();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ResultsAndCertification WebApi");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            //Inject AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Inject DbContext
            services.AddDbContext<ResultsAndCertificationDbContext>(options =>
                options.UseSqlServer(ResultsAndCertificationConfiguration.SqlConnectionString,
                    builder => builder.UseNetTopologySuite()
                                      .EnableRetryOnFailure()));

            services.AddSingleton(ResultsAndCertificationConfiguration);

            RegisterRepositories(services);
            RegisterApplicationServices(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddTransient<ICsvHelperService, CsvHelperService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
        }
    }
}
