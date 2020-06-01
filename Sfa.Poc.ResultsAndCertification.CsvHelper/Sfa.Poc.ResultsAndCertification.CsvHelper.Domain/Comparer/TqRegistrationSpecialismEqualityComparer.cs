using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Comparer
{
    public class TqRegistrationSpecialismEqualityComparer : IEqualityComparer<TqRegistrationSpecialism>
    {
        public bool Equals(TqRegistrationSpecialism x, TqRegistrationSpecialism y)
        {
            if (x == null && x == null)
                return true;
            else if (x == null || x == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.TqRegistrationPathwayId == y.TqRegistrationPathwayId
                     && x.TlSpecialismId == y.TlSpecialismId && x.Status == y.Status;
        }

        public int GetHashCode(TqRegistrationSpecialism reg)
        {
            unchecked
            {
                var hashCode = reg.TqRegistrationPathwayId;
                hashCode = (hashCode * 397) ^ reg.TlSpecialismId.GetHashCode();
                hashCode = (hashCode * 397) ^ reg.Status.GetHashCode();
                return hashCode;
            }
        }
    }
}
