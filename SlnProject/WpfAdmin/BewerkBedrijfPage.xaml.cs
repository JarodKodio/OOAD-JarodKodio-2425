using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            {
                InitializeComponent();
                _bedrijf = bedrijf;

                txtNaam.Text = _bedrijf.Name;
                txtContact.Text = _bedrijf.Contact;
                txtEmail.Text = _bedrijf.Email;
                txtStatus.Text = _bedrijf.Status;
            }
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            _bedrijf.Name = txtNaam.Text;
            _bedrijf.Contact = txtContact.Text;
            _bedrijf.Email = txtEmail.Text;
            _bedrijf.ChangeStatus(txtStatus.Text);

            _bedrijf.Update();
            MessageBox.Show("Bedrijf succesvol bijgewerkt.");
        }
        private void UploadLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Afbeeldingen|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() == true)
            {
                _bedrijf.Logo = File.ReadAllBytes(dialog.FileName);
                MessageBox.Show("Logo geselecteerd.");
            }
        }
    }

}
