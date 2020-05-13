using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Controllers
{
    public class SignInController : Controller
    {
        public IActionResult Index()
        {
            return View("SignIn");
        }

        [HttpGet]
        public async Task SignIn(string returnUrl = "/signIn/postsignin")
        {
            var returnUrl1 = Url.Action("PostSignIn", "SignIn");
            await HttpContext.ChallengeAsync(new AuthenticationProperties() { RedirectUri = returnUrl1 });
        }

        public IActionResult PostSignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "TlevelHome");
            }
            else
            {
                return RedirectToAction("FailedLogin", "Home");
            }
        }


        [HttpGet]
        public async Task SignedOut()
        {
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public IActionResult SignOutComplete()
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
