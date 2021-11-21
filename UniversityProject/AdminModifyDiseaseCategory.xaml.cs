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

namespace UniversityProject
{
    /// <summary>
    /// Логика взаимодействия для AdminModifyDiseaseCategory.xaml
    /// </summary>
    public partial class AdminModifyDiseaseCategory : Window
    {
        public DiseaseCategory NewDiseaseCategory { get; set; }
        public AdminModifyDiseaseCategory()
        {
            InitializeComponent();
        }
        public AdminModifyDiseaseCategory(string name)
        {
            InitializeComponent();
            diseaseName.Text = name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (diseaseName.Text != "")
            {
                NewDiseaseCategory = new DiseaseCategory() { Name = diseaseName.Text };
                DialogResult = true;
                Close();
            }
        }
    }
}
