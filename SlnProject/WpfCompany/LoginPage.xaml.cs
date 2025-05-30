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
using BenchmarkToolLibrary;
using BenchmarkToolLibrary.Models;

namespace WpfCompany
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
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

            Company? company = Company.GetByLogin(loginInput);

            if (company == null)
            {
                MessageBox.Show("Login niet gevonden: '" + loginInput + "'", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MainWindow.IsCompanyLoggedIn = true;
            MainWindow.LoggedInCompany = company;
            _mainFrame.Navigate(new CompanyHomePage());
        }
    }
}
