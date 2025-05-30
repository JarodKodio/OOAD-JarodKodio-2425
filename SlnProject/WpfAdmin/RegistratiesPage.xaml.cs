using System;
using System.Collections.Generic;
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
using BenchmarkToolLibrary.Models;

namespace WpfAdmin
{
    /// <summary>
    /// Interaction logic for RegistratiesPage.xaml
    /// </summary>
    public partial class RegistratiesPage : Page
    {
        public RegistratiesPage()
        {
            if (!MainWindow.IsAdminLoggedIn)
            {
                MessageBox.Show("Je moet eerst inloggen.");
                NavigationService?.Navigate(new LoginPage(null));
                return;
            }
            {
                InitializeComponent();
                lstRegistraties.ItemsSource = BenchmarkToolLibrary.Models.Company.GetAll() // alle bedrijven met status "pending"
                    .Where(c => c.Status == "pending")
                    .ToList();
            }
        }
        private void Goedkeuren_Click(object sender, RoutedEventArgs e)
        {
            if (lstRegistraties.SelectedItem is BenchmarkToolLibrary.Models.Company selected)
            {
                selected.ChangeStatus("active");
                selected.Update();
                lstRegistraties.ItemsSource = BenchmarkToolLibrary.Models.Company.GetAll()
                    .Where(c => c.Status == "pending").ToList();
            }
            else
            {
                MessageBox.Show("Selecteer eerst een registratie.");
            }
        }

        private void Afkeuren_Click(object sender, RoutedEventArgs e)
        {
            if (lstRegistraties.SelectedItem is BenchmarkToolLibrary.Models.Company selected)
            {
                selected.ChangeStatus("rejected");
                selected.Update();
                lstRegistraties.ItemsSource = BenchmarkToolLibrary.Models.Company.GetAll()
                    .Where(c => c.Status == "pending").ToList();
            }
            else
            {
                MessageBox.Show("Selecteer eerst een registratie.");
            }
        }
    }
}
