using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Poc.ResultsAndCertification.Dfe.Api.Infrastructure;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : Controller
    {
        [AllowAnonymous]
        [Route("error/{code}")]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}