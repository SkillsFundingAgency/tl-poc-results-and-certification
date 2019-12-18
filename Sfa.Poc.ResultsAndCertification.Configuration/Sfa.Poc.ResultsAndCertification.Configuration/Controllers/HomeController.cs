using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Poc.ResultsAndCertification.Configuration.Configuration;
using Sfa.Poc.ResultsAndCertification.Configuration.Models;

namespace Sfa.Poc.ResultsAndCertification.Configuration.Controllers
{
    public class HomeController : Controller
    {
        private ResultsAndCertificationConfiguration Configuration { get; }

        public HomeController(ResultsAndCertificationConfiguration config)
        {
            Configuration = config;
        }
        public IActionResult Index()
        {
            return View(Configuration);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
