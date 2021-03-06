﻿using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Sfa.Poc.ResultsAndCertification.Dfe.Api.Client.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Models.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Api.Client.Clients
{
    public class TokenServiceClient : ITokenServiceClient
    {
        private const int _tokenExpiryTime = 30;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly ResultsAndCertificationConfiguration _config;

        public TokenServiceClient(IHttpContextAccessor httpContextAccessor, ResultsAndCertificationConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = configuration;
        }

        public string GetToken()
        {
            var userClaims = _httpContextAccessor.HttpContext.User.Claims;

            var roleClaims = new List<Claim>();
            if (userClaims != null)
            {
                roleClaims = userClaims.Where(c => c.Type == ClaimTypes.Role).ToList();
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.DfeSignInSettings.APISecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config.DfeSignInSettings.Issuer,
                Audience = _config.DfeSignInSettings.Audience,
                Subject = new ClaimsIdentity(roleClaims),
                Expires = DateTime.UtcNow.AddSeconds(_tokenExpiryTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
