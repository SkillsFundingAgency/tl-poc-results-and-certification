using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sfa.Poc.ResultsAndCertification.Dfe.Api.Infrastructure;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Api.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new BadRequestResponse(context.ModelState));
            }
            base.OnActionExecuting(context);
        }
    }
}
