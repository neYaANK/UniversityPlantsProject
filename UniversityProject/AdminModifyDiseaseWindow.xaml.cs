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
using UniversityProject.Classes;
using UniversityProject.Data;
using UniversityProject.General;

namespace UniversityProject
{
    /// <summary>
    /// Логика взаимодействия для AdminModifyDiseaseWindow.xaml
    /// </summary>
    public partial class AdminModifyDiseaseWindow : Window
    {
        public Images Image { get; set; }
        public Disease NewDisease { get; set; } = new Disease();

        public AdminModifyDiseaseWindow()
        {
            InitializeComponent();
        }

        public AdminModifyDiseaseWindow(string editname,DiseaseCategory dcategory,Images editimg, List<DiseaseSymptom> editsymptoms,bool readOnly=false)
        {
            InitializeComponent();
            Image = editimg;
            name.Text = editname;
            CategoryCombo.SelectedItem = dcategory;
            uploadResult.Content= editimg.FileName.Substring(editimg.FileName.LastIndexOf('\\') + 1);
            diseaseImage.Source = new BitmapImage(new Uri(editimg.FileName));
            foreach(DiseaseSymptom item in editsymptoms)
            {
                symptomsListView.Items.Add(new SymptomPowerItem() { Name = item.Symptom.Name, Power = item.SymptomPower, Id=item.SymptomId });
            }


            name.IsReadOnly = readOnly;
            CategoryCombo.IsReadOnly = readOnly;
            UploadBtn.IsEnabled = !readOnly;
            Toolbar.IsEnabled = !readOnly;
            CategoryCombo.IsReadOnly = readOnly;
            CategoryCombo.IsEnabled = !readOnly;
            NextBtn.Content = "Обрати";
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new UniversityProjectDbContext())
            {
                
                CategoryCombo.ItemsSource = dbContext.DiseasesCategories.ToList();
                CategoryCombo.DisplayMemberPath = "Name";
                CategoryCombo.SelectedValuePath = "Id";
                CategoryCombo.SelectedIndex = 0;

                symptomsComboBox.ItemsSource = dbContext.Symptoms.ToList();
                symptomsComboBox.DisplayMemberPath = "Name";
                symptomsComboBox.SelectedValuePath = "Id";
                symptomsComboBox.SelectedIndex = 0;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Image = Additional.GetImage();
            if (Image != null)
            {
                uploadResult.Content = Image.FileName.Substring(Image.FileName.LastIndexOf('\\') + 1);
                diseaseImage.Source = new BitmapImage(new Uri(Image.FileName));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var item = new SymptomPowerItem();
            item.Id = (int)symptomsComboBox.SelectedValue;
            item.Name = (symptomsComboBox.SelectedItem as Symptom).Name;
            item.Power = (int)symptomPower.Value;
            bool contains = false;
            foreach (SymptomPowerItem it in symptomsListView.Items)
            {
                if (it.Name == item.Name)
                {
                    symptomsListView.Items[symptomsListView.Items.IndexOf(it)] = item;
                    contains = true;
                    break;
                    
                }
            }
            if(!contains) symptomsListView.Items.Add(item);


        }

        private void Button_Click_2(object sender, RoutedEventArgs e) => symptomsListView.Items.Remove(symptomsListView.SelectedItem);

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (name.Text != "" & symptomsListView.Items.Count>0)
            {
                NewDisease.Name = name.Text;
                NewDisease.DiseaseCategory = CategoryCombo.SelectedItem as DiseaseCategory;
                NewDisease.DiseaseImage = Image;
                NewDisease.DiseaseSymptoms = new List<DiseaseSymptom>();
                foreach(SymptomPowerItem item in symptomsListView.Items)
                {
                    NewDisease.DiseaseSymptoms.Add(new DiseaseSymptom() { SymptomId = item.Id, SymptomPower = item.Power });
                }
                DialogResult = true;
                Close();
            }
        }
    }
    
}
