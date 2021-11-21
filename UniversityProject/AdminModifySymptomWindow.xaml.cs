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
    /// Логика взаимодействия для AdminModifySymptomWindow.xaml
    /// </summary>
    
    public partial class AdminModifySymptomWindow : Window
    {
        public Symptom NewSymptom { get; set; }
        public AdminModifySymptomWindow()
        {
            InitializeComponent();
        }
        public AdminModifySymptomWindow(string pass)
        {
            InitializeComponent();
            symptomName.Text = pass;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (symptomName.Text != "")
            {
                NewSymptom = new Symptom() { Name = symptomName.Text };
                DialogResult = true;
                Close();
            }
        }
    }
}
