using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

            InitializeComponent();

            try
            {
                lstRegistraties.ItemsSource = Company.GetAll()
                    .Where(c => c.Status == "pending")
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het laden van registraties:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Goedkeuren_Click(object sender, RoutedEventArgs e)
        {
            if (lstRegistraties.SelectedItem is Company selected)
            {
                try
                {
                    selected.ChangeStatus("active");
                    selected.Update();

                    lstRegistraties.ItemsSource = Company.GetAll()
                        .Where(c => c.Status == "pending").ToList();

                    MessageBox.Show("Registratie goedgekeurd.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij het goedkeuren:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een registratie.");
            }
        }

        private void Afkeuren_Click(object sender, RoutedEventArgs e)
        {
            if (lstRegistraties.SelectedItem is Company selected)
            {
                try
                {
                    selected.ChangeStatus("rejected");
                    selected.Update();

                    lstRegistraties.ItemsSource = Company.GetAll()
                        .Where(c => c.Status == "pending").ToList();

                    MessageBox.Show("Registratie afgekeurd.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij het afkeuren:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een registratie.");
            }
        }
    }
}
