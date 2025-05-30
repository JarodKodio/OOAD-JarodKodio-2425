using System;
using System.Windows;
using System.Windows.Controls;
using BenchmarkToolLibrary.Models;

namespace WpfCompany
{
    /// <summary>
    public partial class LoginPage : Page
    {
        private Frame _mainFrame;

        public LoginPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string loginInput = txtLogin.Text.Trim();
            string passwordInput = txtPassword.Password.Trim();

            if (string.IsNullOrWhiteSpace(loginInput) || string.IsNullOrWhiteSpace(passwordInput))
            {
                MessageBox.Show("Vul zowel login als wachtwoord in.", "Ongeldige invoer", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Company? company = Company.GetByLogin(loginInput);

                if (company == null)
                {
                    MessageBox.Show($"Login niet gevonden: '{loginInput}'", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Hier zou je wachtwoordcontrole doen als dat geïmplementeerd is

                MainWindow.IsCompanyLoggedIn = true;
                MainWindow.LoggedInCompany = company;

                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.btnLogin.Content = "Uitloggen";
                }

                _mainFrame.Navigate(new CompanyHomePage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het inloggen:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
