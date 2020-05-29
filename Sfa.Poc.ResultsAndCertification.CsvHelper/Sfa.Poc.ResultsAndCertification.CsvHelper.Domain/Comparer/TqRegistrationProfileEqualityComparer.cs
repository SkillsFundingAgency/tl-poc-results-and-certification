using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Comparer
{
    public class TqRegistrationProfileEqualityComparer : IEqualityComparer<TqRegistrationProfile>
    {
        private TqRegistrationPathwayEqualityComparer _tqRegistrationPathwayComprarer;
        public TqRegistrationProfileEqualityComparer()
        {
            _tqRegistrationPathwayComprarer = new TqRegistrationPathwayEqualityComparer();
        }

        public bool Equals(TqRegistrationProfile x, TqRegistrationProfile y)
        {
            if (x == null && x == null)
                return true;
            else if (x == null || x == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.UniqueLearnerNumber == y.UniqueLearnerNumber && string.Equals(x.Firstname, y.Firstname)
                    && string.Equals(x.Lastname, y.Lastname) && Equals(x.DateofBirth, y.DateofBirth);
        }

        public int GetHashCode(TqRegistrationProfile reg)
        {
            unchecked
            {
                var hashCode = reg.UniqueLearnerNumber.GetHashCode();
                hashCode = (hashCode * 397) ^ (reg.Firstname != null ? reg.Firstname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (reg.Lastname != null ? reg.Lastname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (reg.DateofBirth != null ? reg.DateofBirth.GetHashCode() : 0);

                foreach (var registrationPathway in reg.TqRegistrationPathway)
                {
                    hashCode = (hashCode * 397) ^ _tqRegistrationPathwayComprarer.GetHashCode(registrationPathway);
                }
                return hashCode;
            }
        }
    }
}
