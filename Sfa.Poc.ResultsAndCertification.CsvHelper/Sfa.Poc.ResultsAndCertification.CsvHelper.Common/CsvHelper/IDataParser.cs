﻿using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper
{
    public interface IDataParser<out T> where T : class
    {
        T Parse(BaseModel model, int rownum);
        T GetErrorObjct(string errorMessage);
    }
}
