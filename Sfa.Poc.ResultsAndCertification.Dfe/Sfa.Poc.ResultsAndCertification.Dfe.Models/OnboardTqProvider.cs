using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.Dfe.Models
{
    public class OnboardTqProvider
    {        
        public string AoCode { get; set; }
        public List<ProviderTq> Providers { get; set; }
    }

    public class ProviderTq
    {
        public long UkPrnNumber { get; set; }
        public List<RouteTq> Routes { get; set; }
    }

    public class RouteTq
    {
        public string RouteId { get; set; }
        public List<PathwayTq> Pathways { get; set; }
    }

    public class PathwayTq
    {
        public string PathwayId { get; set; }
        public List<SpecialismTq> Specialisms { get; set; }
    }

    public class SpecialismTq
    {
        public string SpecialismId { get; set; }
    }
}
