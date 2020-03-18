using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.Notifications.Interface;
using Sfa.Poc.ResultsAndCertification.Notifications.Models;

namespace Sfa.Poc.ResultsAndCertification.Notifications.Controllers
{
    public class HomeController : Controller
    {
        private const string TlevelsQueryTemplateName = "Query TLevels";
        private readonly ILogger<HomeController> _logger;
        private readonly INotificationService _notificationService;

        public HomeController(ILogger<HomeController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View(new NotificationViewModel
            {
                TemplateName = TlevelsQueryTemplateName
            }); ;
        }

        [HttpPost]
        public async Task<IActionResult> Index(NotificationViewModel notification)
        {
            if (ModelState.IsValid)
            {
                var template = !string.IsNullOrWhiteSpace(notification.TemplateName)
                    ? notification.TemplateName
                    : TlevelsQueryTemplateName;               

                var tokens = (dynamic)new
                {
                    first_name = notification.ReciepientName,
                    tlevel_name = notification?.TlevelName ?? "Education and childcare: Education",
                    sender_name = notification?.SenderName ?? "Tlevel Administrator"
                };
                    

                await _notificationService.SendNotification(template, notification.EmailTo, tokens);

                return View(notification);
            }
            else
            {
                // there is something wrong with the data values
                return View(notification);
            }
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
