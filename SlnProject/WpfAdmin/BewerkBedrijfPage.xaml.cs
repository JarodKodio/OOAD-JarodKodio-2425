using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace WpfAdmin
{
    /// <summary>
    /// Interaction logic for BewerkBedrijfPage.xaml
    /// </summary>
    public partial class BewerkBedrijfPage : Page
    {
        private BenchmarkToolLibrary.Models.Company _bedrijf;

        public BewerkBedrijfPage(BenchmarkToolLibrary.Models.Company bedrijf)
        {
            if (!MainWindow.IsAdminLoggedIn)
            {
                MessageBox.Show("Je moet eerst inloggen.");
                NavigationService?.Navigate(new LoginPage(null));
                return;
            }

            InitializeComponent();
            _bedrijf = bedrijf;

            txtNaam.Text = _bedrijf.Name;
            txtContact.Text = _bedrijf.Contact;
            txtEmail.Text = _bedrijf.Email;
            txtStatus.Text = _bedrijf.Status;
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _bedrijf.Name = txtNaam.Text;
                _bedrijf.Contact = txtContact.Text;
                _bedrijf.Email = txtEmail.Text;
                _bedrijf.ChangeStatus(txtStatus.Text);

                _bedrijf.Update(); // SQL-call
                MessageBox.Show("Bedrijf succesvol bijgewerkt.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het opslaan van het bedrijf:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UploadLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Afbeeldingen|*.jpg;*.jpeg;*.png"
            };

            try
            {
                if (dialog.ShowDialog() == true)
                {
                    _bedrijf.Logo = File.ReadAllBytes(dialog.FileName); // kan I/O error geven
                    MessageBox.Show("Logo geselecteerd.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het laden van de afbeelding:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
