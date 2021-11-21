using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UniversityProject.Classes;
using UniversityProject.General;

namespace UniversityProject
{
    /// <summary>
    /// Логика взаимодействия для AdminAddWindow.xaml
    /// </summary>
    public partial class AdminAddWindow : Window
    {
        public TableType Choice { get; set; }
        public AdminAddWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (specie.IsChecked.GetValueOrDefault())
            {
                Choice = TableType.Specie;
            }
            else if (plant.IsChecked.GetValueOrDefault())
            {
                Choice = TableType.Plant;
            }
            else if (diseaseCategory.IsChecked.GetValueOrDefault())
            {
                Choice = TableType.Disease_Category;
            }
            else if (disease.IsChecked.GetValueOrDefault())
            {
                Choice = TableType.Disease;
            }
            else if (symptom.IsChecked.GetValueOrDefault())
            {
                Choice = TableType.Symptom;
            }
            DialogResult = true;
            Close();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            specie.IsChecked = true;
            
        }
    }
}
