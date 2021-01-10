using AutoLotModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proiect_Medii
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();
        CollectionViewSource countriesViewSource;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            countriesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("countriesViewSource")));
            countriesViewSource.Source = ctx.Countries.Local;
            ctx.Countries.Load();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnCancel.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            BindingOperations.ClearBinding(capitalTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(countryIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(countryNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(gDPTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(populationTextBox, TextBox.TextProperty);
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
            btnCancel.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            countriesDataGrid.IsEnabled = false;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            BindingOperations.ClearBinding(capitalTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(countryIdTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(countryNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(gDPTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(populationTextBox, TextBox.TextProperty);
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnNext.IsEnabled = true;
            btnPrev.IsEnabled = true;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // using AutoLotModel;
            Countries countries = null;
            if (action == ActionState.New)
            {
                try
                {
                    countries = new Countries()
                    {
                        CountryName = countryNameTextBox.Text.Trim(),
                        Capital = capitalTextBox.Text.Trim(),
                        Population = Decimal.Parse(populationTextBox.Text.Trim()),
                        GDP = Decimal.Parse(gDPTextBox.Text.Trim()),
                    };
                    ctx.Countries.Add(countries);
                    countriesViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Edit)
            {
                try
                {
                    countries = (Countries)countriesDataGrid.SelectedItem;
                    countries.CountryName = countryNameTextBox.Text.Trim();
                    countries.Capital = capitalTextBox.Text.Trim();
                    countries.Population = Decimal.Parse(populationTextBox.Text.Trim());
                    countries.GDP = Decimal.Parse(gDPTextBox.Text.Trim());

                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                countriesViewSource.View.Refresh();
                countriesViewSource.View.MoveCurrentTo(countries);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    countries = (Countries)countriesDataGrid.SelectedItem;
                    ctx.Countries.Remove(countries);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                countriesViewSource.View.Refresh();

            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
           countriesViewSource.View.MoveCurrentToNext();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            countriesViewSource.View.MoveCurrentToPrevious();
        }

    }
}
