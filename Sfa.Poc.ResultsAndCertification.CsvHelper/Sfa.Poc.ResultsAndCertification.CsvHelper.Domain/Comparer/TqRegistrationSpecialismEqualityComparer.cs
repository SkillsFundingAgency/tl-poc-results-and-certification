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
            {
                var retVal = 
                    //x.TqRegistrationPathwayId == y.TqRegistrationPathwayId &&
                    x.TlSpecialismId == y.TlSpecialismId
                    && Equals(x.StartDate, y.StartDate)
                    && Equals(x.EndDate, y.EndDate)
                    && x.Status == y.Status;
                return retVal;
            }
        }

        public int GetHashCode(TqRegistrationSpecialism regSpecialism)
        {
            unchecked
            {
                var hashCode = regSpecialism.TlSpecialismId.GetHashCode();
                //var hashCode = reg.TqRegistrationPathwayId.GetHashCode();
                //hashCode = (hashCode * 397) ^ reg.TlSpecialismId.GetHashCode();
                hashCode = (hashCode * 397) ^ regSpecialism.StartDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (regSpecialism.EndDate != null ? regSpecialism.EndDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ regSpecialism.Status.GetHashCode();
                return hashCode;
            }
        }
    }
}
