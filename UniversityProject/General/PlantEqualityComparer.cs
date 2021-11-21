using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using UniversityProject.Classes;

namespace UniversityProject.General
{
    class PlantEqualityComparer : IEqualityComparer<Plant>
    {
        public bool Equals([AllowNull] Plant x, [AllowNull] Plant y)
        {
            return x.Name == y.Name &
                x.SpeciesId == x.SpeciesId &
                x.Variety == y.Variety;
        }

        public int GetHashCode([DisallowNull] Plant obj)
        {
            return base.GetHashCode();
        }
    }
}
