using BenchmarkToolLibrary.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfCompany
{
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

            try
            {
                lstRapporten.ItemsSource = Yearreport.GetAll()
                    .Where(r => r.CompanyId == MainWindow.LoggedInCompany.Id)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het laden van de jaarrapporten:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Verwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (lstRapporten.SelectedItem is Yearreport selected)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Wil je jaarrapport voor {selected.Year} verwijderen?",
                    "Bevestiging",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        Yearreport.Delete(selected.Id);

                        lstRapporten.ItemsSource = Yearreport.GetAll()
                            .Where(r => r.CompanyId == MainWindow.LoggedInCompany.Id)
                            .ToList();

                        MessageBox.Show("Rapport succesvol verwijderd.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fout bij het verwijderen van het rapport:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een rapport.");
            }
        }
    }
}
