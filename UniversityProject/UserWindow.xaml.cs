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
using UniversityProject.Classes;
using UniversityProject.Data;
using UniversityProject.General;

namespace UniversityProject
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new UniversityProjectDbContext())
            {
                powerSymptomShown.Content = "Вираженість симптома: " + powerSymptop.Value;
                comboPlant.ItemsSource = dbContext.Plants.ToList();
                comboPlant.SelectedValuePath = "Id";
                comboPlant.DisplayMemberPath = "Name";
                comboPlant.SelectedIndex = 0;

                comboSymptom.ItemsSource = dbContext.Symptoms.ToList();
                comboSymptom.SelectedValuePath = "Id";
                comboSymptom.DisplayMemberPath = "Name";
                comboSymptom.SelectedIndex = 0;
            }
        }

        private void powerSymptop_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (powerSymptomShown == null) return;
            powerSymptomShown.Content = "Вираженість симптома: " + powerSymptop.Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var toAdd = new SymptomPowerItem { Id = (comboSymptom.SelectedItem as Symptom).Id, Name = (comboSymptom.SelectedItem as Symptom).Name, Power = (int)powerSymptop.Value };
            bool contains = false;

            foreach (SymptomPowerItem i in toSearch.Items)
            {
                if (i.Name == toAdd.Name)
                {
                    contains = true;
                    toSearch.Items[toSearch.Items.IndexOf(i)] = toAdd;
                    break;
                }

            }
            if (!contains) toSearch.Items.Add(toAdd);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (toSearch.SelectedItem != null) toSearch.Items.Remove(toSearch.SelectedItem);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new UniversityProjectDbContext())
            {
                var selPlant = dbContext.Plants.Include(c => c.PlantImage).Include(c => c.Species).Where(c => c.Id == (int)comboPlant.SelectedValue).Single();
                var colByPlant = dbContext.Diseases.Include(c => c.Plants).Include(c=>c.DiseaseImage).Where(c => c.Plants.Contains(selPlant)).Include(c=>c.DiseaseCategory).Include(c => c.DiseaseSymptoms).ThenInclude(c => c.Symptom).ToList();
                var symptoms = toSearch.Items.Cast<SymptomPowerItem>().Select(c => new SymptomPowerItem() { Id = c.Id, Name = c.Name, Power = c.Power }).ToList();
                var res = colByPlant.Where(c =>
                {
                    var lst = c.DiseaseSymptoms.Select(v => new SymptomPowerItem() { Id = v.SymptomId, Name = v.Symptom.Name, Power = v.SymptomPower }).ToList();

                    var kk = lst.Except(symptoms,new SymptomPowerItemEqualityComparer()).ToList();
                    return kk.Count() == (lst.Count() - symptoms.Count);
                }
                ).ToList();
                userResult.ItemsSource = res;
                userResult.DisplayMemberPath = "Name";
                userResult.SelectedValuePath = "Id";
            }
        }

        private void userResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (userResult.SelectedItem != null)
            {
                var it = userResult.SelectedItem as Disease;
                var dialog = new AdminModifyDiseaseWindow(it.Name, it.DiseaseCategory, it.DiseaseImage, it.DiseaseSymptoms,true);
                
                var res = dialog.ShowDialog();
                if(res.Value & dialog.DialogResult.Value)
                {
                    using (var dbContext = new UniversityProjectDbContext())
                    {
                        var history = new History();
                        history.DiseaseId = (userResult.SelectedItem as Disease).Id;
                        history.PlantId = (comboPlant.SelectedItem as Plant).Id;
                        history.Time = DateTime.Now;
                        dbContext.Histories.Add(history);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        private void comboPlant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboPlant.SelectedItem != null)
            {
                toSearch.Items.Clear();
                userResult.ItemsSource = null;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var dialog = new HistoryWindow();
            dialog.ShowDialog();
        }
    }

}
