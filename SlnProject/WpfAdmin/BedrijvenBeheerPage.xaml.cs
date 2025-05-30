using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfAdmin
{
    /// <summary>
    /// Interaction logic for BedrijvenBeheerPage.xaml
    /// </summary>
    public partial class BedrijvenBeheerPage : Page
    {
        public BedrijvenBeheerPage()
        {
            if (!MainWindow.IsAdminLoggedIn)
            {
                MessageBox.Show("Je moet eerst inloggen.");
                NavigationService?.Navigate(new LoginPage(null));
                return;
            }

            InitializeComponent();

            try
            {
                lstBedrijven.ItemsSource = BenchmarkToolLibrary.Models.Company.GetAll()
                    .Where(c => c.Status == "active" || c.Status == "suspended")
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het laden van bedrijven:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Bewerken_Click(object sender, RoutedEventArgs e)
        {
            if (lstBedrijven.SelectedItem is BenchmarkToolLibrary.Models.Company selected)
            {
                NavigationService?.Navigate(new BewerkBedrijfPage(selected));
            }
            else
            {
                MessageBox.Show("Selecteer eerst een bedrijf om te bewerken.");
            }
        }

        private void Verwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (lstBedrijven.SelectedItem is BenchmarkToolLibrary.Models.Company selected)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Weet je zeker dat je {selected.Name} wilt verwijderen?",
                    "Bevestiging",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        BenchmarkToolLibrary.Models.Company.Delete(selected.Id);

                        lstBedrijven.ItemsSource = BenchmarkToolLibrary.Models.Company.GetAll()
                            .Where(c => c.Status == "active" || c.Status == "suspended")
                            .ToList();

                        MessageBox.Show("Bedrijf succesvol verwijderd.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fout bij het verwijderen van het bedrijf:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een bedrijf om te verwijderen.");
            }
        }
    }
}
