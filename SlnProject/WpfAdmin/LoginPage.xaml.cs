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

namespace WpfAdmin
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
            if (txtUsername.Text == "admin" && txtPassword.Password == "admin")
            {
                MainWindow.IsAdminLoggedIn = true;
                _mainFrame.Navigate(new AdminHomePage());
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.btnLogin.Content = "Uitloggen";
                }
            }
            else
            {
                MessageBox.Show("Foute inloggegevens", "Login mislukt", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
