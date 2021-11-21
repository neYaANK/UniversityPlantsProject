using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using UniversityProject.Classes;

namespace UniversityProject.General
{
    class DiseaseEqualityComparer : IEqualityComparer<Disease>
    {
        public bool Equals([AllowNull] Disease x, [AllowNull] Disease y)
        {
            return x.Name == y.Name &
                x.DiseaseCategoryId == y.DiseaseCategoryId;
        }

        public int GetHashCode([DisallowNull] Disease obj)
        {
            return base.GetHashCode();
        }
    }


}

