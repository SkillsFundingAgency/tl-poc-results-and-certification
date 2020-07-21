using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;
using System;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        //public static IServiceCollection AddRequiredMvcComponents(this IServiceCollection services)
        //{
        //    return services;
        //}

        public static IServiceCollection AddApiAuthentication(this IServiceCollection services,
            ResultsAndCertificationConfiguration configuration)
        {
            //// configure jwt authentication
            //var key = Encoding.ASCII.GetBytes(configuration.DfeSignInSettings.APISecret);
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidIssuer = configuration.DfeSignInSettings.Issuer,
            //        ValidAudience = configuration.DfeSignInSettings.Audience,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});

            services
                .AddAuthentication(auth =>
                {
                    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    //auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                //.AddAuthentication("Bearer")
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, auth =>
                {
                    auth.Authority = configuration.DfeSignInSettings.Authority;
                    //auth.RequireHttpsMetadata = false;

                    auth.Audience = "TLevelsRC"; //configuration.DfeSignInSettings.Audience;
                    //auth.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    //{
                    //    ValidateIssuer = false,
                    //    //ValidateAudience = true,
                    //    ValidateIssuerSigningKey = true,
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                    //    .GetBytes(configuration.DfeSignInSettings.APISecret)),
                    //    //RequireExpirationTime = false
                    //};
                });

            return services;
        }

        public static IServiceCollection AddApiAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    //.RequireClaim("scope", "openid")
                    .Build();

                options.DefaultPolicy = policy;
            });

            return services;
        }
    }
}
