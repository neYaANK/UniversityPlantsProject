using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UniversityProject.Classes
{
    public class Disease
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int DiseaseCategoryId { get; set; }
        public DiseaseCategory DiseaseCategory { get; set; }
        public List<Plant> Plants { get; set; }
        public int DiseaseImageId { get; set; }
        public Images DiseaseImage { get; set; }
        public List<DiseaseSymptom> DiseaseSymptoms { get; set; }
    }
}
