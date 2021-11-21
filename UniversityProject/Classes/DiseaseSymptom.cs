using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityProject.Classes
{
    public class DiseaseSymptom
    {
        public int DiseaseId { get; set; }
        public Disease Disease { get; set; }
        public int SymptomId { get; set; }
        public Symptom Symptom { get; set; }
        public int SymptomPower { get; set; }

    }
}
