using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityProject.General
{
    class ListViewHistoryItem
    {
        public int Id { get; set; }
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; }
        public DateTime Time { get; set; }
    }
}
