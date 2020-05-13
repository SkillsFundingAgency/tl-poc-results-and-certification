using Microsoft.AspNetCore.Builder;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Middleware;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Extensions
{
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void ConfigureExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
