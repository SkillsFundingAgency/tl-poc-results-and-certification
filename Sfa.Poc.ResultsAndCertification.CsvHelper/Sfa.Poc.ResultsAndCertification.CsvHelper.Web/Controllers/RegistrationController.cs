using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Client.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;

        private const string InvalidRegistrations = "InvalidRegistrations";
        private const string ValidationErrors = "ValidationErrors";
        private const string ElapsedTime = "ElapsedTime";

        public RegistrationController(IResultsAndCertificationInternalApiClient internalApiClient)
        {
            _internalApiClient = internalApiClient;
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

            var results = await _internalApiClient.ProcessBulkRegistrationsAsync(model.RegistrationFile);
            //var results = await _internalApiClient.ProcessBulkRegistrationsAsync1(new BulkRegistrationRequest
            //{
            //    performedBy = "Test User",
            //    Ukprn = 10009696,
            //    RegistrationFile = model.RegistrationFile
            //});

            watch.Stop();

            // Temp code for validation
            ViewBag.ElapsedTime = watch.ElapsedMilliseconds;
            TempData["ValidationErrors1"] = JsonConvert.SerializeObject(results.ValidationErrors);

            TempData["ValidationErrors2"] = Convert.ToBase64String(results.ErrorFileBytes);
            return View();
        }

        public FileResult GetRejectedData()
        {
            //var tempData = TempData[ValidationErrors] as string;
            //var model = JsonConvert.DeserializeObject<IEnumerable<ValidationError>>(tempData);
            //var result = model.Select(x => x.RawRow);
            var result = string.Empty;

            return File(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)), "text/csv", "RejectedData.csv");
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