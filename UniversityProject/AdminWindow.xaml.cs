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
using UniversityProject.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Reflection;
using UniversityProject.General;

namespace UniversityProject
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }
        public string MoveToDbFolder(Images img)
        {
            if (!Directory.Exists("Images"))
            {
                Directory.CreateDirectory("Images");
            }


            var ext = new FileInfo(img.FileName).Extension;
            var toReturn = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Images\\image" + Environment.TickCount + ext;
            File.Copy(img.FileName, toReturn);
            if (System.IO.Path.GetDirectoryName(img.FileName) == (Assembly.GetExecutingAssembly().Location + "\\Images") ) File.Delete(img.FileName);
            return toReturn;





        }





        private void OnSpeciesExpand(object sender, EventArgs eventArgs)
        {
            var parent = sender as TreeViewItem;

            using (var dbContext = new UniversityProjectDbContext())
            {
                foreach (TreeViewItem it in parent.Items)
                {
                    var item = dbContext.Plants.Single(c => c.Id == (it.Tag as DSTagData).Id);
                    if (it.Items.Count == 0)
                    {

                        foreach (var subItem in dbContext.Diseases.Include(k => k.Plants).Where(b => b.Plants.Contains(item)))
                        {
                            var treeSubItem = new TreeViewItem();
                            treeSubItem.Header = subItem.Name;
                            treeSubItem.Tag = new DSTagData() { Id = subItem.Id, Type = TableType.Disease};
                            it.Items.Add(treeSubItem);
                        }
                    }

                }
            }

        }

        private void LoadFirst()
        {
            using (var dbContext = new UniversityProjectDbContext())
            {
                diseases.Items.Clear();
                foreach (var upperItem in dbContext.Species)
                {
                    var upperTreeItem = new TreeViewItem();
                    upperTreeItem.Header = upperItem.Name;
                    upperTreeItem.Tag = new DSTagData() { Id = upperItem.Id, Type = TableType.Specie };
                    upperTreeItem.Expanded += OnSpeciesExpand;
                    foreach (var item in dbContext.Plants.Where(c => c.SpeciesId == upperItem.Id))
                    {
                        var treeItem = new TreeViewItem();
                        treeItem.Header = item.Name;
                        treeItem.Tag = new DSTagData() { Id = item.Id, Type = TableType.Plant};

                        upperTreeItem.Items.Add(treeItem); ;
                    }
                    diseases.Items.Add(upperTreeItem);
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFirst();

        }



        private void diseases_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedData.SelectedItem != null & diseases.SelectedItem != null)
            {
                (SelectedData.SelectedItem as ListBoxItem).IsSelected = false;
                diseases.Focus();
            }

            var item = sender as TreeView;
            if (item.SelectedItem != null)
            {
                if (((item.SelectedItem as TreeViewItem).Tag as DSTagData).Type == TableType.Disease || ((item.SelectedItem as TreeViewItem).Tag as DSTagData).Type == TableType.Plant)
                {
                    var tag = ((item.SelectedItem as TreeViewItem).Tag as DSTagData);
                    using (var dbContext = new UniversityProjectDbContext())
                    {
                        var imgId = 0;

                        switch (tag.Type)
                        {
                            case TableType.Disease:
                                imgId = dbContext.Diseases.Single(c => c.Id == tag.Id).DiseaseImageId;
                                break;
                            case TableType.Plant:
                                imgId = dbContext.Plants.Single(c => c.Id == tag.Id).PlantImageId;
                                break;
                        }

                        var img = dbContext.Images.Where(c => c.Id == imgId).Single();



                        SelectedImage.Source = BitmapFromUri(new Uri(img.FileName));
                    }

                }
                else SelectedImage.Source = null;
                var tagg = ((item.SelectedItem as TreeViewItem).Tag as DSTagData);
                using (var dbContext = new UniversityProjectDbContext())
                {
                    switch (tagg.Type)
                    {

                        case TableType.Plant:
                            var selectedPlant = dbContext.Plants.Include(c => c.PlantImage).Include(c => c.Species).Single(c => c.Id == tagg.Id);
                            SelectedName.Content = selectedPlant.Name;
                            SelectedName.Tag = diseases.SelectedItem;
                            SelectedData.Items.Clear();
                            var selectedItem = new ListBoxItem();
                            selectedItem.Content = selectedPlant.Variety;
                            selectedItem.Tag = null;

                            SelectedData.Items.Add(selectedItem);

                            selectedItem = new ListBoxItem();
                            selectedItem.Content = selectedPlant.Description;
                            selectedItem.Tag = null;

                            SelectedData.Items.Add(selectedItem);
                            break;
                        case TableType.Disease:
                            var selectedDisease = dbContext.Diseases.Include(c => c.DiseaseSymptoms).ThenInclude(b => b.Symptom).Include(k => k.DiseaseCategory).Include(c => c.DiseaseImage).Single(f => f.Id == tagg.Id);
                            SelectedName.Content = selectedDisease.Name;
                            SelectedName.Tag = diseases.SelectedItem;
                            SelectedData.Items.Clear();
                            selectedItem = new ListBoxItem();
                            selectedItem.Content = selectedDisease.DiseaseCategory.Name;
                            selectedItem.Tag = new DSTagData() { Id = selectedDisease.DiseaseCategoryId, Type = TableType.Disease_Category };
                            SelectedData.Items.Add(selectedItem);

                            foreach (var disease in selectedDisease.DiseaseSymptoms)
                            {
                                selectedItem = new ListBoxItem();
                                selectedItem.Content = $"{disease.Symptom.Name} - {disease.SymptomPower}";
                                selectedItem.Tag = new DSTagData() { Id = disease.SymptomId, Type = TableType.Symptom };
                                SelectedData.Items.Add(selectedItem);
                            }
                            break;
                        default:
                            SelectedName.Content = "";
                            SelectedData.Items.Clear();
                            break;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var choiceWin = new AdminAddWindow();
            var resChoice = choiceWin.ShowDialog();
            if (resChoice.Value == true & choiceWin.DialogResult.Value == true)
            {
                using (var dbContext = new UniversityProjectDbContext())
                {
                    switch (choiceWin.Choice)
                    {
                        case TableType.Specie:
                            var speciesDialog = new AdminModifySpecies();
                            var res = speciesDialog.ShowDialog();
                            if (res.Value & speciesDialog.DialogResult.Value)
                            {
                                if (!dbContext.Species.Select(c => c.Name).Contains(speciesDialog.NewSpecie.Name))
                                {



                                    dbContext.Species.Add(speciesDialog.NewSpecie);
                                }
                            }
                            break;
                        case TableType.Plant:
                            var plantDialog = new AdminModifyPlant();
                            res = plantDialog.ShowDialog();
                            if (res.Value & plantDialog.DialogResult.Value)
                            {
                                PlantEqualityComparer comparer = new PlantEqualityComparer();
                                if (!dbContext.Plants.ToList().Contains(plantDialog.NewPlant, comparer))
                                {
                                    plantDialog.NewPlant.PlantImage.FileName = MoveToDbFolder(plantDialog.NewPlant.PlantImage);
                                    var plant = plantDialog.NewPlant;
                                    dbContext.Species.Attach(plant.Species);
                                    var lst = plantDialog.DiseasesToAdd.Select(c => new Disease() { Id = c.Id }).ToList();
                                    dbContext.Diseases.AttachRange(lst);
                                    plant.Diseases = lst;
                                    dbContext.Plants.Add(plant);
                                }

                            }
                            break;
                        case TableType.Disease:
                            var disease = new AdminModifyDiseaseWindow();
                            res = disease.ShowDialog();
                            if (res.Value & disease.DialogResult.Value)
                            {
                                DiseaseEqualityComparer comparer = new DiseaseEqualityComparer();
                                if (!dbContext.Diseases.ToList().Contains(disease.NewDisease, comparer))
                                {
                                    disease.Image.FileName = MoveToDbFolder(disease.NewDisease.DiseaseImage);
                                    dbContext.DiseasesCategories.Attach(disease.NewDisease.DiseaseCategory);
                                    dbContext.Diseases.Add(disease.NewDisease);
                                }
                            }
                            break;
                        case TableType.Disease_Category:
                            var diseaseCategory = new AdminModifyDiseaseCategory();
                            res = diseaseCategory.ShowDialog();
                            if (res.Value & diseaseCategory.DialogResult.Value)
                            {
                                if (!dbContext.DiseasesCategories.Select(c => c.Name).Contains(diseaseCategory.NewDiseaseCategory.Name))
                                {
                                    dbContext.DiseasesCategories.Add(diseaseCategory.NewDiseaseCategory);
                                }
                            }
                            break;
                        case TableType.Symptom:
                            var symptomDialog = new AdminModifySymptomWindow();
                            res = symptomDialog.ShowDialog();
                            if (res.Value & symptomDialog.DialogResult.Value)
                            {
                                if (!dbContext.Symptoms.Select(c => c.Name).Contains(symptomDialog.NewSymptom.Name))
                                {
                                    dbContext.Symptoms.Add(symptomDialog.NewSymptom);
                                }
                            }
                            break;

                    }
                    dbContext.SaveChanges();
                    LoadFirst();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DSTagData tagData = null;
            if (diseases.SelectedItem != null) tagData = (diseases.SelectedItem as TreeViewItem).Tag as DSTagData;
            else if (SelectedData.SelectedItem != null) tagData = (SelectedData.SelectedItem as ListBoxItem).Tag as DSTagData;

            if (tagData != null)
            {

                switch (tagData.Type)
                {
                    case TableType.Specie:
                        using (var dbContext = new UniversityProjectDbContext())
                        {
                            var specieDialog = new AdminModifySpecies(dbContext.Species.Where(c => c.Id == tagData.Id).Select(c => c.Name).Single());
                            var res = specieDialog.ShowDialog();
                            if (res.Value & specieDialog.DialogResult.Value)
                            {
                                dbContext.Species.Where(c => c.Id == tagData.Id).Single().Name = specieDialog.NewSpecie.Name;
                                dbContext.SaveChanges();
                            }
                        }
                        break;
                    case TableType.Symptom:
                        using (var dbContext = new UniversityProjectDbContext())
                        {
                            var symptomDialog = new AdminModifySymptomWindow(dbContext.Symptoms.Where(c => c.Id == tagData.Id).Select(c => c.Name).Single());
                            var res = symptomDialog.ShowDialog();
                            if (res.Value & symptomDialog.DialogResult.Value)
                            {
                                dbContext.Symptoms.Where(c => c.Id == tagData.Id).Single().Name = symptomDialog.NewSymptom.Name;
                                dbContext.SaveChanges();

                            }
                        }
                        break;
                    case TableType.Disease_Category:
                        using (var dbContext = new UniversityProjectDbContext())
                        {
                            var categoryDialog = new AdminModifyDiseaseCategory(dbContext.DiseasesCategories.Where(c => c.Id == tagData.Id).Select(c => c.Name).Single());
                            var res = categoryDialog.ShowDialog();
                            if (res.Value & categoryDialog.DialogResult.Value)
                            {
                                dbContext.DiseasesCategories.Where(c => c.Id == tagData.Id).Single().Name = categoryDialog.NewDiseaseCategory.Name;
                                dbContext.SaveChanges();

                            }
                        }
                        break;
                    case TableType.Plant:

                        Plant oldPlant = null;
                        var existing = new List<Disease>();
                        using (var dbContext = new UniversityProjectDbContext())
                        {
                            if (dbContext.Diseases.Count() == 0 || dbContext.Species.Count() == 0) break;
                            var oldPlantt = dbContext.Plants.Where(c => c.Id == tagData.Id).Include(c => c.Diseases).ThenInclude(c => c.DiseaseCategory).Include(c => c.PlantImage).Include(c => c.Species).Single();
                            existing.AddRange(oldPlantt.Diseases);
                            oldPlantt.Diseases.Clear();
                            dbContext.SaveChanges();
                            oldPlant = oldPlantt;
                        }
                        using (var dbContext = new UniversityProjectDbContext())
                        {

                            var plantDialog = new AdminModifyPlant(oldPlant.Name, oldPlant.Variety, oldPlant.Description, oldPlant.PlantImage, oldPlant.Species, existing);
                            var res = plantDialog.ShowDialog();
                            if (res.Value & plantDialog.DialogResult.Value)
                            {

                                var newPlant = dbContext.Plants.Include(c => c.Diseases).Include(c => c.Species).Where(c => c.Id == tagData.Id).Include(c => c.PlantImage).Single();
                                //dbContext.Plants.Attach(newPlant);
                                newPlant.Diseases.Clear();

                                newPlant.Name = plantDialog.NewPlant.Name;

                                newPlant.Variety = plantDialog.NewPlant.Variety;

                                newPlant.Description = plantDialog.NewPlant.Description;

                                newPlant.PlantImage.FileName = MoveToDbFolder(newPlant.PlantImage);

                                var dis = plantDialog.DiseasesToAdd;

                                dbContext.Diseases.AttachRange(dis);

                                newPlant.Diseases = dis;

                                var spec = dbContext.Species.Where(c => c.Id == oldPlant.SpeciesId).Single();
                                newPlant.Species = dbContext.Species.Where(c => c.Id == spec.Id).Single();

                                dbContext.SaveChanges();
                            }
                        }
                        break;
                    case TableType.Disease:
                        using (var dbContext = new UniversityProjectDbContext())
                        {
                            if (dbContext.Symptoms.Count() == 0 || dbContext.DiseasesCategories.Count() == 0) break;
                            var oldDisease = dbContext.Diseases.Where(c => c.Id == tagData.Id).Include(c => c.DiseaseSymptoms).ThenInclude(k => k.Symptom).Include(c => c.DiseaseCategory).Include(c => c.DiseaseImage).Single();
                            var diseaseDialog = new AdminModifyDiseaseWindow(oldDisease.Name, oldDisease.DiseaseCategory, oldDisease.DiseaseImage, oldDisease.DiseaseSymptoms);
                            var res = diseaseDialog.ShowDialog();
                            if (res.Value & diseaseDialog.DialogResult.Value)
                            {
                                var newDisease = dbContext.Diseases.Where(c => c.Id == tagData.Id).Include(c => c.DiseaseSymptoms).Single();

                                newDisease.Name = diseaseDialog.NewDisease.Name;

                                var img = diseaseDialog.NewDisease.DiseaseImage;
                                dbContext.Images.Attach(img);
                                newDisease.DiseaseImage.FileName = MoveToDbFolder(newDisease.DiseaseImage);

                                var dis = diseaseDialog.NewDisease.DiseaseSymptoms;
                                dbContext.DiseaseSymptoms.AttachRange(dis);
                                newDisease.DiseaseSymptoms = dis;

                                var category = diseaseDialog.NewDisease.DiseaseCategory;
                                newDisease.DiseaseCategory = dbContext.DiseasesCategories.Where(c => c.Id == category.Id).Single();

                                dbContext.SaveChanges();
                            }
                        }
                        break;
                }

                SelectedImage.Source = null;
                SelectedName.Content = "";
                SelectedData.Items.Clear();
                LoadFirst();


            }
        }
        private void SelectedData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (diseases.SelectedItem != null & SelectedData.SelectedItem != null)
            {
                (diseases.SelectedItem as TreeViewItem).IsSelected = false;
                SelectedData.Focus();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DSTagData tagData = null;
            if (diseases.SelectedItem != null) tagData = (diseases.SelectedItem as TreeViewItem).Tag as DSTagData;
            else if (SelectedData.SelectedItem != null) tagData = (SelectedData.SelectedItem as ListBoxItem).Tag as DSTagData;

            switch (tagData.Type)
            {
                case TableType.Specie:
                    using (var dbContext = new UniversityProjectDbContext())
                    {
                        foreach (var item in dbContext.Plants.Where(c => c.SpeciesId == tagData.Id).Include(c => c.PlantImage).ToList())
                        {
                            File.Delete(item.PlantImage.FileName);
                        }

                        dbContext.Species.Remove(dbContext.Species.Where(c => c.Id == tagData.Id).Single());
                        dbContext.SaveChanges();
                    }
                    break;
                case TableType.Disease_Category:
                    using (var dbContext = new UniversityProjectDbContext())
                    {
                        foreach (var item in dbContext.Diseases.Where(c => c.DiseaseCategoryId == tagData.Id).Include(c => c.DiseaseImage).ToList())
                        {
                            File.Delete(item.DiseaseImage.FileName);
                        }

                        dbContext.DiseasesCategories.Remove(dbContext.DiseasesCategories.Where(c => c.Id == tagData.Id).Single());
                        dbContext.SaveChanges();
                    }
                    break;
                case TableType.Plant:


                    using (var dbContext = new UniversityProjectDbContext())
                    {
                        var plantImageFileName = dbContext.Plants.Where(c => c.Id == tagData.Id).Include(c => c.PlantImage).Single().PlantImage.FileName;
                        dbContext.Plants.Remove(dbContext.Plants.Where(c => c.Id == tagData.Id).Single());
                        File.Delete(plantImageFileName);
                        dbContext.SaveChanges();
                    }
                    break;
                case TableType.Symptom:

                    using (var dbContext = new UniversityProjectDbContext())
                    {
                        var it = (SelectedName.Tag as TreeViewItem).Tag as DSTagData;
                        var dis = dbContext.Diseases.Where(c => c.Id == it.Id).Single();

                        if (MessageBox.Show("Видалити з данної хвороби?", "Інфо", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            dbContext.DiseaseSymptoms.Remove(dbContext.DiseaseSymptoms.Where(c => c.DiseaseId == dis.Id & c.SymptomId == tagData.Id).Single());
                            dbContext.SaveChanges();
                        }
                        else if (MessageBox.Show("Видалити з данної хвороби?", "Інфо", MessageBoxButton.YesNo) == MessageBoxResult.No)
                        {
                            dbContext.Symptoms.Remove(dbContext.Symptoms.Where(c => c.Id == dis.Id).Single());
                            dbContext.SaveChanges();
                        }
                    }
                    break;
                case TableType.Disease:
                    ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(diseases.SelectedItem as TreeViewItem);
                    using (var dbContext = new UniversityProjectDbContext())
                    {
                        var it = parent.Tag as DSTagData;
                        var curr = (diseases.SelectedItem as TreeViewItem).Tag as DSTagData;
                        var res = MessageBox.Show("Видалити тільки з даної рослини?", "Інфо", MessageBoxButton.YesNo);
                        if (res == MessageBoxResult.Yes)
                        {
                            var selPlant = dbContext.Plants.Where(c => c.Id == it.Id).Include(c => c.Diseases).Single();
                            selPlant.Diseases.Remove(dbContext.Diseases.Where(c => c.Id == curr.Id).Single());

                        }
                        else if (res == MessageBoxResult.No)
                        {
                            var disFileName = dbContext.Diseases.Where(c => c.Id == curr.Id).Include(c => c.DiseaseImage).Single().DiseaseImage.FileName;
                            dbContext.Diseases.Remove(dbContext.Diseases.Where(c => c.Id == curr.Id).Single());

                            File.Delete(disFileName);
                        }

                        dbContext.SaveChanges();
                    }

                    break;


            }
            LoadFirst();
            SelectedData.Items.Clear();
            SelectedName.Content = "";
            SelectedName.Tag = null;
            SelectedImage.Source = null;



        }
    }
}
