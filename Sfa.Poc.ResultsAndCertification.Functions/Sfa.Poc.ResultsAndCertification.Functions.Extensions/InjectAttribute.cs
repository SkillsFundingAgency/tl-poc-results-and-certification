using Microsoft.Azure.WebJobs.Description;
using System;

namespace Sfa.Poc.ResultsAndCertification.Functions.Extensions
{
    /// <summary>
    /// Attribute used to inject a dependency into the function completes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class InjectAttribute : Attribute
    {
    }
}
