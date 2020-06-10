using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ResultsAndCertificationConfiguration _configuration;

        private const string InvalidRegistrations = "InvalidRegistrations";
        private const string ValidationErrors = "ValidationErrors";
        private const string ElapsedTime = "ElapsedTime";

        public RegistrationController(ResultsAndCertificationConfiguration configuration, IResultsAndCertificationInternalApiClient internalApiClient, IBlobStorageService blobStorageService)
        {
            _internalApiClient = internalApiClient;
            _blobStorageService = blobStorageService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UploadAsync()
        {
            var model = new RegistrationsFileViewModel();
            return View(model);
        }

        [HttpGet]
        public FileResult DownloadErrors()
        {
            var bytearray = Convert.FromBase64String(TempData["ValidationErrors2"] as string);
            return File(bytearray, "text/csv", "ValidationErrors.csv");
        }

        [HttpPost]
        public async Task<IActionResult> BulkRegistrationsAsync(RegistrationsFileViewModel model)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();

            if (!ModelState.IsValid)
            {
                return View("Upload", model);
            }

            var fileName = $"inputfile_{DateTime.Now.Ticks}.csv";

            var bulkRegistrationRequest = new BulkRegistrationRequest
            {
                performedBy = "testuser@test.com",
                Ukprn = 10009696, // NCFE
                BlobReferencePath = fileName
            };

            using var fileStream = model.RegistrationFile.OpenReadStream();
            await _blobStorageService.UploadFileAsync(_configuration.BlobStorageConnectionString, "registrations", $"{bulkRegistrationRequest.Ukprn}/processing/{fileName}", fileStream);

            ////await _blobStorageService.MoveFileAsync(_configuration.BlobStorageConnectionString, "registrations", "1000008/processing/inputfile.csv", "1000008/processed/inputfile.csv");



            var results = await _internalApiClient.ProcessBulkRegistrationsAsync(bulkRegistrationRequest);

            //var results = await _internalApiClient.ProcessBulkRegistrationsAsync(model.RegistrationFile);

            watch.Stop();

            // Temp code for validation
            ViewBag.ElapsedTime = watch.ElapsedMilliseconds;
            //TempData[ValidationErrors] = JsonConvert.SerializeObject(results.ValidationErrors);
            
            TempData[ValidationErrors] = Convert.ToBase64String(results.ErrorFileBytes);
            return View();
        }

        [HttpGet]
        public async Task<FileResult> GetRejectedData()
        {
            //var tempData = TempData[ValidationErrors] as string;
            //var model = JsonConvert.DeserializeObject<IEnumerable<ValidationError>>(tempData);
            //var result = model.Select(x => x.RawRow);
            var result = string.Empty;

            var fileStream = await _blobStorageService.DownloadFileAsync(_configuration.BlobStorageConnectionString, "registrations", "1000008/processed/inputfile.csv");

            fileStream.Position = 0;
            return File(fileStream, "text/csv", "RejectedData.csv");
            //return File(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)), "text/csv", "RejectedData.csv");
        }

        public IActionResult GetValidationErrors()
        {
            var tempData = TempData["ValidationErrors2"] as string;
            var model = JsonConvert.DeserializeObject<IEnumerable<ValidationError>>(tempData);
            return View("ValidationErrorList", model);
        }

        public async Task<IActionResult> DownloadAsync()
        {
            await Task.Run(() => true);
            return View();
        }
    }
}