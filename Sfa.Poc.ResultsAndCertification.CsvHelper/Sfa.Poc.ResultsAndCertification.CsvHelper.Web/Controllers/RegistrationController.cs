﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Utilities.CsvHelper.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Utilities.CsvHelper.Service;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private const string InvalidRegistrations = "InvalidRegistrations";
        private const string ValidationErrors = "ValidationErrors";

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

        [HttpPost]
        public async Task<IActionResult> UploadAsync(RegistrationsFileViewModel model)
        {
            // TODO: start timer
            if (!ModelState.IsValid)
            {
                // 1. File meta info validation 
                return View(model);
            }

            var csvParserService = new CsvHelperService();
            var regdata = await csvParserService.ReadDataAsync(model.RegistrationFile);

            // Step: Do we have to report back this results?
            var invalidData = regdata.Where(x => !x.IsValid);
            
            // Step: data-valiation against to DB.
            var validData = regdata.Where(x => x.IsValid);
            // if file validations are passed then proceed with db validations. 

            // Step: db-validations. 


            // Temp code for validation
            var errors = new List<ValidationError>();
            foreach(var item in invalidData)
                errors.AddRange(item.ValidationErrors);
            TempData[ValidationErrors] = JsonConvert.SerializeObject(errors);

            // TODO: Stop timer
            return View("ViewAll", validData);
        }

        public FileResult GetRejectedData()
        {
            var tempData = TempData[ValidationErrors] as string;
            var model = JsonConvert.DeserializeObject<IEnumerable<ValidationError>>(tempData);
            var result = model.Select(x => x.RawRow);
            return File(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result)), "text/csv", "RejectedData.csv");
        }

        public IActionResult GetValidationErrors()
        {
            var tempData = TempData[ValidationErrors] as string;
            var model = JsonConvert.DeserializeObject<IEnumerable<ValidationError>>(tempData);
            return View("ValidationErrorList", model);
        }

        public IActionResult ViewAll()
        {
            var result = new List<Registration>();
            return View(result);
        }

        public async Task<IActionResult> DownloadAsync()
        {
            await Task.Run(() => true);
            return View();
        }
    }
}