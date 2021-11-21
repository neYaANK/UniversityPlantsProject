using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversityProject.Classes;
using UniversityProject.Data;

namespace UniversityProject.General
{
    public static class Additional
    {
        static public Images GetImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "image files (*.png)|*.png|jpg files (*.jpg)|*.jpg";
            dialog.FilterIndex = 1;
            var res = dialog.ShowDialog();
            if (res.HasValue)
            {
                if (res.Value)
                {
                    return new Images() { FileName = dialog.FileName };
                }
            }
            return null;


        }
        static public bool CheckReadyForUse()
        {
            
            using (var dbContext = new UniversityProjectDbContext())
            {
                if (dbContext.Symptoms.Count() == 0) return false;
                if (dbContext.Diseases.Count() == 0) return false;
                if (dbContext.Plants.Count() == 0) return false;
                
            }
            return true;
        }

    }
}
