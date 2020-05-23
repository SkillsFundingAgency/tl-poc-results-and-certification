using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model
{
    public class BaseModel : ValidationState
    {
        public virtual IFormFile File { get; set; }
    }
}
