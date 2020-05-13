using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("page-not-found", Name = "PageNotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("no-permission", Name = "FailedLogin")]
        public IActionResult FailedLogin()
        {
            return View();
        }
        
        [AllowAnonymous]
        public IActionResult Cookies()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/home/error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return statusCode switch
            {
                404 => RedirectToRoute("PageNotFound"),
                403 => RedirectToRoute("FailedLogin"),
                //if (Request.Path.ToString().Contains("404"))
                //    return RedirectToRoute("PageNotFound");

                //if (Request.Path.ToString().Contains("403"))
                //    return RedirectToRoute("FailedLogin");

                _ => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }),
            };
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //[AllowAnonymous]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
