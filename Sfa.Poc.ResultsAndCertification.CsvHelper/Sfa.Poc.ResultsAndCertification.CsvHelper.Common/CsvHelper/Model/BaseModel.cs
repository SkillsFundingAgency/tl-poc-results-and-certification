using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model
{
    public class BaseModel : ValidationState
    {
        [Ignore]
        public virtual IFormFile File { get; set; }

        [Ignore]
        public virtual Stream FileStream { get; set; }
    }
}
