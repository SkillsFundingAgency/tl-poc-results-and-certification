using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Authentication.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Authentication
{
    public class TokenService : ITokenService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly ResultsAndCertificationConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TokenService(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, ResultsAndCertificationConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetToken()
        {
            var identity = _httpContextAccessor.HttpContext.User.Claims;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.DfeSignInSettings.APISecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config.DfeSignInSettings.Issuer,
                Audience = _config.DfeSignInSettings.Audience,
            Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, "General User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
           return tokenHandler.WriteToken(token);

            
           
            //var authClaims = new Claim[]
            //    {
            //        new Claim(ClaimTypes.Role, "General User")
            //    };

            //var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecureKey"));

            // token = new JwtSecurityToken(
            //    issuer: "http://dotnetdetail.net",
            //    audience: "http://dotnetdetail.net",
            //    expires: DateTime.Now.AddHours(3),
            //    claims: authClaims,
            //    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            //    );

            //return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
