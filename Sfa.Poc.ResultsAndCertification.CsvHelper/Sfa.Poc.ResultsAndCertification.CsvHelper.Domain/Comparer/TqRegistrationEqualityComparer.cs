using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Comparer
{
    public class TqRegistrationEqualityComparer : IEqualityComparer<TqRegistration>
    {
        private TqSpecialismRegistrationEqualityComparer _tqSpecialismRegistrationComprarer;
        public TqRegistrationEqualityComparer()
        {
            _tqSpecialismRegistrationComprarer = new TqSpecialismRegistrationEqualityComparer();
        }

        public bool Equals(TqRegistration x, TqRegistration y)
        {
            if (x == null && x == null)
                return true;
            else if (x == null || x == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.UniqueLearnerNumber == y.UniqueLearnerNumber && string.Equals(x.Firstname, y.Firstname)
                    && string.Equals(x.Lastname, y.Lastname)
                    && x.TqProviderId == y.TqProviderId && x.Status == y.Status;
                    //&& Equals(x.TqSpecialismRegistrations, y.TqSpecialismRegistrations);

            //return x.UniqueLearnerNumber == y.UniqueLearnerNumber && string.Equals(x.Firstname, y.Firstname) 
            //        && string.Equals(x.Lastname, y.Lastname) && Equals(x.DateofBirth, y.DateofBirth)
            //        && x.TqProviderId == y.TqProviderId && Equals(x.StartDate, y.StartDate) && x.Status == y.Status 
            //        && Equals(x.TqSpecialismRegistrations, y.TqSpecialismRegistrations);
        }

        public int GetHashCode(TqRegistration reg)
        {
            unchecked
            {
                var hashCode = reg.UniqueLearnerNumber.GetHashCode();
                hashCode = (hashCode * 397) ^ (reg.Firstname != null ? reg.Firstname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (reg.Lastname != null ? reg.Lastname.GetHashCode() : 0);
                //hashCode = (hashCode * 397) ^ (reg.DateofBirth != null ? reg.DateofBirth.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ reg.TqProviderId.GetHashCode();
                //hashCode = (hashCode * 397) ^ (reg.StartDate != null ? reg.StartDate.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ reg.Status.GetHashCode();
                //hashCode = (hashCode * 397) ^ (reg.TqSpecialismRegistrations != null ? reg.TqSpecialismRegistrations.GetHashCode() : 0);
                
                foreach(var specReg in reg.TqSpecialismRegistrations)
                {
                    hashCode = (hashCode * 397) ^ _tqSpecialismRegistrationComprarer.GetHashCode(specReg);
                }
                return hashCode;
            }
        }
    }
}
