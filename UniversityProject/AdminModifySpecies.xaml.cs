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
    /// Логика взаимодействия для AdminModifySpecies.xaml
    /// </summary>
    public partial class AdminModifySpecies : Window
    {
        public Specie NewSpecie { get; set; }
        public AdminModifySpecies()
        {
            InitializeComponent();
        }
        public AdminModifySpecies(string pass)
        {
            InitializeComponent();
            specieName.Text = pass;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (specieName.Text != "")
            {
                NewSpecie = new Specie() { Name = specieName.Text };
                DialogResult = true;
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
