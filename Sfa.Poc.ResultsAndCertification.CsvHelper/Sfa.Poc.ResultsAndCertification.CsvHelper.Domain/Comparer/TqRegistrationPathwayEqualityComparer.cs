using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Comparer
{
    public class TqRegistrationPathwayEqualityComparer : IEqualityComparer<TqRegistrationPathway>
    {
        private TqRegistrationSpecialismEqualityComparer _tqRegistrationSpecialismComparer;
        public TqRegistrationPathwayEqualityComparer()
        {
            _tqRegistrationSpecialismComparer = new TqRegistrationSpecialismEqualityComparer();
        }

        public bool Equals(TqRegistrationPathway x, TqRegistrationPathway y)
        {
            if (x == null && x == null)
                return true;
            else if (x == null || x == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.TqRegistrationProfileId == y.TqRegistrationProfileId && x.TqProviderId == y.TqProviderId
                    && Equals(x.StartDate, y.StartDate)
                    && Equals(x.EndDate, y.EndDate)
                    && x.Status == y.Status;
        }

        public int GetHashCode(TqRegistrationPathway regPathway)
        {
            unchecked
            {
                var hashCode = regPathway.TqRegistrationProfileId.GetHashCode();
                hashCode = (hashCode * 397) ^ regPathway.TqProviderId.GetHashCode();
                hashCode = (hashCode * 397) ^ regPathway.StartDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (regPathway.EndDate != null ? regPathway.EndDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ regPathway.Status.GetHashCode();

                foreach (var registrationSpecialism in regPathway.TqRegistrationSpecialisms)
                {
                    hashCode = (hashCode * 397) ^ _tqRegistrationSpecialismComparer.GetHashCode(registrationSpecialism);
                }
                return hashCode;
            }
        }
    }
}
