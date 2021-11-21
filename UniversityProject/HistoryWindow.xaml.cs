using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UniversityProject.Data;
using UniversityProject.General;

namespace UniversityProject
{
    /// <summary>
    /// Логика взаимодействия для HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public HistoryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new UniversityProjectDbContext())
            {
                history.ItemsSource = dbContext.Histories.Include(c => c.Plant).Include(c => c.Disease).Select(c => new ListViewHistoryItem() { Id = c.Id, PlantId = c.PlantId, PlantName = c.Plant.Name, DiseaseId = c.DiseaseId, DiseaseName = c.Disease.Name, Time = c.Time }).ToList();
            }
        }
    }
}
