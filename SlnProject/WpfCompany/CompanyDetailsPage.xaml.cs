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

            txtNaam.Text = MainWindow.LoggedInCompany.Name;
            txtContact.Text = MainWindow.LoggedInCompany.Contact;
            txtEmail.Text = MainWindow.LoggedInCompany.Email;
            txtTelefoon.Text = MainWindow.LoggedInCompany.Phone;
            txtAdres.Text = MainWindow.LoggedInCompany.Address;
            txtWachtwoord.Password = "";
        }

        private void Opslaan_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoggedInCompany.Name = txtNaam.Text;
            MainWindow.LoggedInCompany.Contact = txtContact.Text;
            MainWindow.LoggedInCompany.Email = txtEmail.Text;
            MainWindow.LoggedInCompany.Phone = txtTelefoon.Text;
            MainWindow.LoggedInCompany.Address = txtAdres.Text;

            if (!string.IsNullOrWhiteSpace(txtWachtwoord.Password))
            {
                System.Reflection.PropertyInfo prop = typeof(Company).GetProperty("Password", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (prop != null)
                {
                    prop.SetValue(MainWindow.LoggedInCompany, txtWachtwoord.Password);
                }
            }

            MainWindow.LoggedInCompany.Update();
            MessageBox.Show("Gegevens opgeslagen.");
        }
    }
}
