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

namespace WpfAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool IsAdminLoggedIn { get; set; } = false;
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage(MainFrame));
        }

        private void NavToHome(object sender, RoutedEventArgs e) => MainFrame.Navigate(new AdminHomePage());
        private void NavToBedrijven(object sender, RoutedEventArgs e) => MainFrame.Navigate(new BedrijvenBeheerPage());
        private void NavToRegistraties(object sender, RoutedEventArgs e) => MainFrame.Navigate(new RegistratiesPage());
        private void NavToLogin(object sender, RoutedEventArgs e)
        {
            IsAdminLoggedIn = false;
            MainFrame.Navigate(new LoginPage(MainFrame));
        }
    }
}