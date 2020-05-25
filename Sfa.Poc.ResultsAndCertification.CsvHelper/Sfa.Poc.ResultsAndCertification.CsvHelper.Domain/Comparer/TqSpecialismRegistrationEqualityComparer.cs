using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Comparer
{
    public class TqSpecialismRegistrationEqualityComparer : IEqualityComparer<TqSpecialismRegistration>
    {
        public bool Equals(TqSpecialismRegistration x, TqSpecialismRegistration y)
        {
            if (x == null && x == null)
                return true;
            else if (x == null || x == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.TlSpecialismId == y.TlSpecialismId && x.Status == y.Status;
            //return x.TqRegistrationId == y.TqRegistrationId && x.TlSpecialismId == y.TlSpecialismId && x.Status == y.Status;
        }

        public int GetHashCode(TqSpecialismRegistration reg)
        {
            unchecked
            {
                //var hashCode = reg.TqRegistrationId;
                //hashCode = (hashCode * 397) ^ reg.TlSpecialismId.GetHashCode();
                var hashCode = reg.TlSpecialismId;
                hashCode = (hashCode * 397) ^ reg.Status.GetHashCode();
                return hashCode;
            }
        }
    }
}
