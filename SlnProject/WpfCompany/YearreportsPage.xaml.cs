using BenchmarkToolLibrary.Models;
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

namespace WpfCompany
{
    /// <summary>
    /// Interaction logic for YearreportsPage.xaml
    /// </summary>
    public partial class YearreportsPage : Page
    {
        public YearreportsPage()
        {
            if (!MainWindow.IsCompanyLoggedIn)
            {
                MessageBox.Show("Je moet eerst inloggen.");
                NavigationService?.Navigate(new LoginPage(null));
                return;
            }
            InitializeComponent();

            lstRapporten.ItemsSource = Yearreport.GetAll()
                .Where(r => r.CompanyId == MainWindow.LoggedInCompany.Id)
                .ToList();
        }

        private void Verwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (lstRapporten.SelectedItem is Yearreport selected)
            {
                MessageBoxResult result = MessageBox.Show($"Wil je jaarrapport voor {selected.Year} verwijderen?", "Bevestiging", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Yearreport.Delete(selected.Id);
                    lstRapporten.ItemsSource = Yearreport.GetAll()
                        .Where(r => r.CompanyId == MainWindow.LoggedInCompany.Id)
                        .ToList();
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een rapport.");
            }
        }
    }
}
