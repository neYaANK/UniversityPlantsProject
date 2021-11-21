using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UniversityProject.Classes
{
    public class Plant
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int SpeciesId { get; set; }
        public Specie Species { get; set; }
        public string Variety { get; set; }
        public string Description { get; set; }
        public int PlantImageId { get; set; }
        public Images PlantImage { get; set; }
        public List<Disease> Diseases { get; set; }
    }
}
