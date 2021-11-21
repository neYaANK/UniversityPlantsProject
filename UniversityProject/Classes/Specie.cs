using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UniversityProject.Classes
{
   public class Specie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
