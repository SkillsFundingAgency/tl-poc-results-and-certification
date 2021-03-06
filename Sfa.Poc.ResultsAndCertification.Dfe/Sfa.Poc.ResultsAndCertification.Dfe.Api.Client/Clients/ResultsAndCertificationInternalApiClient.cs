﻿using Newtonsoft.Json;
using Sfa.Poc.ResultsAndCertification.Dfe.Api.Client.Interfaces;
using Sfa.Poc.ResultsAndCertification.Dfe.Models;
using Sfa.Poc.ResultsAndCertification.Dfe.Models.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Api.Client.Clients
{
    public class ResultsAndCertificationInternalApiClient : IResultsAndCertificationInternalApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _internalApiUri;
        private readonly ITokenServiceClient _tokenServiceClient;

        public ResultsAndCertificationInternalApiClient(HttpClient httpClient, ITokenServiceClient tokenService, ResultsAndCertificationConfiguration configuration)
        {
            _tokenServiceClient = tokenService;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _internalApiUri = configuration.ResultsAndCertificationInternalApiUri.TrimEnd('/');
            _httpClient.BaseAddress = new System.Uri(_internalApiUri);
        }

        public async Task<RegisteredTqProviderDetails> GetRegisteredTqProviderInformation(int tqProviderId)
        {
            var requestUri = $"/api/tqprovider/registeredtqprovider-information/{tqProviderId}";
            var response = await GetAsync<RegisteredTqProviderDetails>(requestUri);
            return response;
        }

        private void SetBearerToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenServiceClient.GetToken());
        }

        private async Task<T> GetAsync<T>(string requestUri)
        {
            SetBearerToken();
            var response = await _httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsAsync<T>();
            return data;
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>
        private async Task<T> PostAsync<T>(string requestUri, T content)
        {
            var response = await _httpClient.PostAsync(requestUri, CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsAsync<T>();
            return data;
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }
    }
}
