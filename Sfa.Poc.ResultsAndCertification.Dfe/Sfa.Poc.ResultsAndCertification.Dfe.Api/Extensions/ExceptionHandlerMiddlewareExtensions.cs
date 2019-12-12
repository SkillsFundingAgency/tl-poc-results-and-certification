using Microsoft.AspNetCore.Builder;
using Sfa.Poc.ResultsAndCertification.Dfe.Api.Middleware;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Api.Extensions
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void ConfigureExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
