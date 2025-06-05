using BenchmarkToolLibrary.Models;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace WpfCompany
{
    /// <summary>
    /// Interaction logic for CompanyDetailsPage.xaml
    /// </summary>
    public partial class CompanyDetailsPage : Page
    {
        public CompanyDetailsPage()
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
                Company bedrijf = MainWindow.LoggedInCompany;
                txtNaam.Text = bedrijf.Name;
                txtContact.Text = bedrijf.Contact;
                txtEmail.Text = bedrijf.Email;
                txtTelefoon.Text = bedrijf.Phone;
                txtAdres.Text = bedrijf.Address;
                txtWachtwoord.Password = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het laden van bedrijfsgegevens:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Company bedrijf = MainWindow.LoggedInCompany;
                bedrijf.Name = txtNaam.Text;
                bedrijf.Contact = txtContact.Text;
                bedrijf.Email = txtEmail.Text;
                bedrijf.Phone = txtTelefoon.Text;
                bedrijf.Address = txtAdres.Text;

                if (!string.IsNullOrWhiteSpace(txtWachtwoord.Password))
                {
                    // Gebruik reflectie om privé-wachtwoordveld te zetten
                    PropertyInfo prop = typeof(Company).GetProperty("Password", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (prop != null)
                    {
                        prop.SetValue(bedrijf, txtWachtwoord.Password);
                    }
                    else
                    {
                        MessageBox.Show("Wachtwoord kon niet ingesteld worden (interne fout).");
                    }
                }

                bedrijf.Update(); // SQL
                MessageBox.Show("Gegevens succesvol opgeslagen.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het opslaan van de gegevens:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
