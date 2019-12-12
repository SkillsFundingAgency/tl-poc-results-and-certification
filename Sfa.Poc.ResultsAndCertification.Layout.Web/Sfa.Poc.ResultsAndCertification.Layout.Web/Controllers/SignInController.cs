using Microsoft.AspNetCore.Mvc;

namespace Sfa.Poc.ResultsAndCertification.Layout.Web.Controllers
{
    public class SignInController : Controller
    {
        public IActionResult Index()
        {
            return View("SignIn");
        }

        public IActionResult SignIn()
        {
            return RedirectToAction("Index");
        }
    }
}
