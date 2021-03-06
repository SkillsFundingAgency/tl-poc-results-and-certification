﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.Dfe.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Web.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public CustomExceptionFilterAttribute(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CustomExceptionFilterAttribute>();
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(default(int), context.Exception, context.Exception.Message);

            var result = new ViewResult { ViewName = "~/Views/Home/Error.cshtml" };
            var modelMetadata = new EmptyModelMetadataProvider();
            result.ViewData = new ViewDataDictionary(
                modelMetadata, context.ModelState)
            {
                {
                    "Exception", context.Exception
                }
            };

            result.ViewData.Model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier };
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
