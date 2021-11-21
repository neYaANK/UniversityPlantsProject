using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UniversityProject.Classes
{
    public class History
    {
        [Key]
        public int Id { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        public int DiseaseId { get; set; }
        public Disease Disease { get; set; }
        public DateTime Time { get; set; }
    }
}
