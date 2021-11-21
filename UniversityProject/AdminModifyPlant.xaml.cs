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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UniversityProject.Classes;
using UniversityProject.Data;
using UniversityProject.General;

namespace UniversityProject
{
    /// <summary>
    /// Логика взаимодействия для AdminModifyPlant.xaml
    /// </summary>
    public partial class AdminModifyPlant : Window
    {
        private Images Image { get; set; }
        //private List<Specie> Species { get; set; } = new List<Specie>();

        public Plant NewPlant { get; set; }

        public List<Disease> DiseasesToAdd { get; set; } = new List<Disease>();
        public AdminModifyPlant()
        {
            InitializeComponent();
        }
        public AdminModifyPlant(string nname, string vvariety, string ddescription, Images img, Specie specie, List<Disease> newDiseases)
        {
            InitializeComponent();

            name.Text = nname;
            Image = img;
            variety.Text = vvariety;
            descriptionTextBox.Document.Blocks.Clear();
            descriptionTextBox.Document.Blocks.Add(new Paragraph(new Run(ddescription)));
            uploadResult.Content = img.FileName.Substring(img.FileName.LastIndexOf('\\') + 1);
            imagePlant.Source = new BitmapImage(new Uri(img.FileName));
            SpecieCombo.SelectedItem = specie;
            DiseasesToAdd = newDiseases;

            foreach (var item in newDiseases)
            {
                diseaseListView.Items.Add(new ListViewDiseaseItem() { Name = item.Name, Category = item.DiseaseCategory.Name, Id = item.Id, CategoryId = item.DiseaseCategoryId });
            }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new UniversityProjectDbContext())
            {

                SpecieCombo.ItemsSource = dbContext.Species.ToList();
                SpecieCombo.DisplayMemberPath = "Name";
                SpecieCombo.SelectedValuePath = "Id";
                SpecieCombo.SelectedIndex = 0;

                categoryComboBox.ItemsSource = dbContext.DiseasesCategories.ToList();
                categoryComboBox.SelectedValuePath = "Id";
                categoryComboBox.DisplayMemberPath = "Name";
                categoryComboBox.SelectionChanged += categoryComboBox_SelectionChanged;
                categoryComboBox.SelectedIndex = 0;

                diseaseComboBox.SelectedValuePath = "Id";
                diseaseComboBox.DisplayMemberPath = "Name";
                diseaseComboBox.ItemsSource = dbContext.Diseases.Include(c => c.DiseaseCategory).Where(k => k.DiseaseCategoryId == (categoryComboBox.SelectedItem as DiseaseCategory).Id).ToList();
                diseaseComboBox.SelectedIndex = 0;

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var item = new ListViewDiseaseItem();
            item.Id = (int)diseaseComboBox.SelectedValue;
            item.Name = (diseaseComboBox.SelectedItem as Disease).Name;
            item.Category = (categoryComboBox.SelectedItem as DiseaseCategory).Name;

            bool contains = false;
            foreach (ListViewDiseaseItem it in diseaseListView.Items)
            {
                if (it.Name == item.Name & it.Category == item.Category)
                {
                    diseaseListView.Items[diseaseListView.Items.IndexOf(it)] = item;
                    contains = true;
                    break;

                }
            }
            if (!contains) diseaseListView.Items.Add(item);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e) => diseaseListView.Items.Remove(diseaseListView.SelectedItem);

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var dbContext = new UniversityProjectDbContext())
            {
                diseaseComboBox.ItemsSource = dbContext.Diseases.Include(c => c.DiseaseCategory).Where(k => k.DiseaseCategoryId == (categoryComboBox.SelectedItem as DiseaseCategory).Id).ToList();
                diseaseComboBox.SelectedIndex = 0;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Image = Additional.GetImage();
            if (Image != null)
            {
                uploadResult.Content = Image.FileName.Substring(Image.FileName.LastIndexOf('\\') + 1);
                imagePlant.Source = new BitmapImage(new Uri(Image.FileName));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var textboxtext = new TextRange(descriptionTextBox.Document.ContentStart, descriptionTextBox.Document.ContentEnd).Text;
            if (name.Text != "" &
                variety.Text != "" &
                textboxtext != "" &
                (Image != null))
            {
                using (var dbContext = new UniversityProjectDbContext())
                {
                    NewPlant = new Plant() { Name = name.Text, Variety = variety.Text, Description = textboxtext, PlantImage = Image, Species = SpecieCombo.SelectedItem as Specie };
                    DiseasesToAdd.Clear();
                    foreach (ListViewDiseaseItem it in diseaseListView.Items)
                    {
                        DiseasesToAdd.Add(new Disease() { Id = it.Id, DiseaseCategoryId = it.Id });
                    }
                    DialogResult = true;
                    Close();
                }


            }
        }
    }
    
}
