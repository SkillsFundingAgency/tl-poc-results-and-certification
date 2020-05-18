using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Configuration;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Authentication;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Authentication.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Globalization;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Clients;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Filters;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.WebConfigurationHelper;
using Sfa.Tl.ResultsAndCertification.Web.WebConfigurationHelper;

namespace Sfa.Poc.ResultsAndCertification.Layout.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Startup> _logger;
        private readonly IWebHostEnvironment _env;

        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        public Startup(IConfiguration configuration, ILogger<Startup> logger, IWebHostEnvironment env)
        {
            _config = configuration;
            _logger = logger;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ResultsAndCertificationConfiguration = ConfigurationLoader.Load(
               _config[Constants.EnvironmentNameConfigKey],
               _config[Constants.ConfigurationStorageConnectionStringConfigKey],
               _config[Constants.VersionConfigKey],
               _config[Constants.ServiceNameConfigKey]);

            //services.AddControllersWithViews();
            //services.AddRazorPages();
            services.AddApplicationInsightsTelemetry();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = "tlevels-rc-x-csrf";
                options.FormFieldName = "_csrfToken";
                options.HeaderName = "X-XSRF-TOKEN";
            });

            services.AddMvc(config => {
                if (!_env.IsDevelopment())
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                }

                config.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                // TODO: Need to add custom exception filter
                config.Filters.Add<CustomExceptionFilterAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddRazorRuntimeCompilation();

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(5);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //});

            services.Configure<CookieTempDataProviderOptions>(options => {
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton(ResultsAndCertificationConfiguration);
            services.AddHttpClient<ITokenRefresher, TokenRefresher>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ITokenServiceClient, TokenServiceClient>();
            services.AddHttpClient<IResultsAndCertificationInternalApiClient, ResultsAndCertificationInternalApiClient>();
            
            services
                .AddTransient<CustomCookieAuthenticationEvents>()
                //.AddTransient<AccessTokenHttpMessageHandler>()
                .AddHttpContextAccessor();
            //services.AddHttpContextAccessor();
            services.AddWebAuthentication(ResultsAndCertificationConfiguration, _logger, _env);
            services.AddAuthorization();
			
			RegisterDependencies(services);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("en-GB");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(opts => opts.EnabledWithBlockMode());
            app.UseXfo(xfo => xfo.Deny());
            app.UseCsp(options => options
                .ScriptSources(s =>
                {
                    s.Self()
                        .CustomSources("https://az416426.vo.msecnd.net/",
                            "https://www.google-analytics.com/analytics.js",
                            "https://www.googletagmanager.com/",
                            "https://tagmanager.google.com/",
                            "https://www.smartsurvey.co.uk/")
                        .UnsafeInline();
                }
                ));

            app.UseStaticFiles();
            app.UseRouting();
            //app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton<IWebConfigurationService, WebConfigurationService>();
            //services.AddTransient<ITlevelLoader, TlevelLoader>();
            //services.AddTransient<IProviderLoader, ProviderLoader>();
        }
    }
}
