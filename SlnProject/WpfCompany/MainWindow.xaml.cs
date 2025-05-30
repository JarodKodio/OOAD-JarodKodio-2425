using BenchmarkToolLibrary.Models;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool IsCompanyLoggedIn { get; set; } = false;
        public static Company LoggedInCompany { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage(MainFrame));
        }

        private void NavToHome(object sender, RoutedEventArgs e) => MainFrame.Navigate(new CompanyHomePage());
        private void NavToRapporten(object sender, RoutedEventArgs e) => MainFrame.Navigate(new YearreportsPage());
        private void NavToBenchmark(object sender, RoutedEventArgs e) => MainFrame.Navigate(new BenchmarkPage());
        private void NavToGegevens(object sender, RoutedEventArgs e) => MainFrame.Navigate(new CompanyDetailsPage());
        private void NavToLogin(object sender, RoutedEventArgs e)
        {
            IsCompanyLoggedIn = false;
            LoggedInCompany = null;
            MainFrame.Navigate(new LoginPage(MainFrame));
            btnLogin.Content = "Inloggen";
        }
    }
}