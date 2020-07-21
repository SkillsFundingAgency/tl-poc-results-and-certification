﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Infrastructure
{
    public class BadRequestResponse : ApiResponse
    {

        public BadRequestResponse() : base(400)
        {
        }

        public BadRequestResponse(string message) : base(400, message)
        {
        }

        public IEnumerable<string> Errors { get; }

        public BadRequestResponse(ModelStateDictionary modelState) : base(400)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
        }
    }
}
