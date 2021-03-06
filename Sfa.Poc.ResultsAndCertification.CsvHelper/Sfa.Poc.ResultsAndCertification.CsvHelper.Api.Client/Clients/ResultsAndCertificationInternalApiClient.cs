﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Clients
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
            //_httpClient.DefaultRequestHeaders.Accept.Clear();
            //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _internalApiUri = "https://localhost:5001";
            _httpClient.BaseAddress = new System.Uri(_internalApiUri);
        }

        public async Task<RegisteredTqProviderDetails> GetRegisteredTqProviderInformation(int tqProviderId)
        {
            var requestUri = $"/api/tqprovider/registeredtqprovider-information/{tqProviderId}";
            var response = await GetAsync<RegisteredTqProviderDetails>(requestUri);
            return response;
        }

        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request)
        {
            var requestUri = $"/api/registration/bulk-upload1";
            var response = await PostAsync<BulkRegistrationRequest, BulkRegistrationResponse>(requestUri, request);
            return response;
        }


        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync1(BulkRegistrationRequest request)
        {
            var requestUri = $"/api/registration/bulk-upload1";

            using var content = new MultipartFormDataContent();
            //content.Add(new StreamContent(request.RegistrationFile.OpenReadStream())
            //{
            //    Headers =
            //    {
            //        ContentLength = request.RegistrationFile.Length,
            //        ContentType = new MediaTypeHeaderValue(request.RegistrationFile.ContentType)
            //    }
            //}, "Attachment", "FileImport");

            var response = await _httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<BulkRegistrationResponse>(await response.Content.ReadAsStringAsync());
        }


        public async Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(IFormFile registrationFile)
        {
            var requestUri = $"/api/registration/bulk-upload";

            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(registrationFile.OpenReadStream())
            {
                Headers =
                {
                    ContentLength = registrationFile.Length,
                    ContentType = new MediaTypeHeaderValue(registrationFile.ContentType)
                }
            }, "Attachment", "FileImport");

            var response = await _httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<BulkRegistrationResponse>(await response.Content.ReadAsStringAsync());
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

        private async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest content)
        {
            //await SetBearerToken();
            var response = await _httpClient.PostAsync(requestUri, CreateHttpContent<TRequest>(content));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private HttpContent CreateHttpContent<T>(T content, string contentType)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, contentType);
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
