using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace UniversityProject.General
{
    class SymptomPowerItemEqualityComparer : IEqualityComparer<SymptomPowerItem>
    {
        public bool Equals([AllowNull] SymptomPowerItem x, [AllowNull] SymptomPowerItem y)
        {
            return x.Id == y.Id & x.Name == y.Name & x.Power == y.Power ;
        }

        public int GetHashCode([DisallowNull] SymptomPowerItem obj)
        {
            return base.GetHashCode();
        }
    }
}
