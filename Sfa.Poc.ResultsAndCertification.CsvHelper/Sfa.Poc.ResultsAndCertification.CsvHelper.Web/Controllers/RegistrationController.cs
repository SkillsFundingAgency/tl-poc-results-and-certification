using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Utilities.CsvHelper.Service;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Controllers
{
    public class RegistrationController : Controller
    {
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

            // Next: Check datatype validations, common model to consolidate and send reply.


            // 2. Validate data types
            // TODO: CsvHelper

            // 4. Validate against the data

            // 5. proecess
            // 6. 

            // TODO: Stop timer

            return View("ViewAll", regdata);
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