﻿using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Authentication.Interfaces;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Authentication
{
    public class TokenRefresher : ITokenRefresher
    {
        private static readonly TimeSpan TokenRefreshThreshold = TimeSpan.FromSeconds(30);

        private readonly HttpClient _httpClient;
        //private readonly IDiscoveryCache _discoveryCache;
        private readonly ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;
        private readonly ILogger<TokenRefresher> _logger;

        public TokenRefresher(
            HttpClient httpClient,
            //IDiscoveryCache discoveryCache,
            ResultsAndCertificationConfiguration resultsAndCertificationConfiguration,
            ILogger<TokenRefresher> logger)
        {
            _httpClient = httpClient;
            //_discoveryCache = discoveryCache;
            _resultsAndCertificationConfiguration = resultsAndCertificationConfiguration;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TokenRefreshResult> TryRefreshTokenIfRequiredAsync(
            string refreshToken,
            string expiresAt,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return TokenRefreshResult.Failed();
            }

            DateTime expiresAtDate;
            var isExpiresAtDateOk = DateTime.TryParse(expiresAt, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out expiresAtDate);

            if (!isExpiresAtDateOk || expiresAtDate >= GetRefreshThreshold())
            {
                return TokenRefreshResult.NoRefreshNeeded();
            }

            //var discovered = await _discoveryCache.GetAsync();
            var refreshTokenResult = await _httpClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest
                {
                    Address = _resultsAndCertificationConfiguration.DfeSignInSettings.TokenEndpoint,//discovered.TokenEndpoint,
                    ClientId = _resultsAndCertificationConfiguration.DfeSignInSettings.ClientId,
                    ClientSecret = _resultsAndCertificationConfiguration.DfeSignInSettings.ClientSecret,
                    RefreshToken = refreshToken
                }, ct);

            if (refreshTokenResult.IsError)
            {
                _logger.LogDebug(
                    "Token refresh failed with message: {refreshTokenErrorDescription}",
                    refreshTokenResult.ErrorDescription);
                return TokenRefreshResult.Failed();
            }

            var newAccessToken = refreshTokenResult.AccessToken;
            var newRefreshToken = refreshTokenResult.RefreshToken;
            var newExpiresAt = CalculateNewExpiresAt(refreshTokenResult.ExpiresIn);

            return TokenRefreshResult.Success(newAccessToken, newRefreshToken, newExpiresAt);
        }

        private static string CalculateNewExpiresAt(int expiresIn)
        {
            // TODO: abstract usages of DateTime to ease unit tests
            var returnVal = (DateTime.UtcNow + TimeSpan.FromSeconds(expiresIn)).ToString("o", CultureInfo.InvariantCulture);
            return returnVal;
        }

        private static DateTime GetRefreshThreshold()
        {
            // TODO: abstract usages of DateTime to ease unit tests
            var returnVal = DateTime.UtcNow + TokenRefreshThreshold;
            return returnVal;
        }
    }
}
