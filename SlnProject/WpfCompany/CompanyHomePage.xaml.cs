using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfCompany
{
    /// <summary>
    /// Interaction logic for CompanyHomePage.xaml
    /// </summary>
    public partial class CompanyHomePage : Page
    {
        public CompanyHomePage()
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
                if (MainWindow.LoggedInCompany != null)
                {
                    byte[] logoData = MainWindow.LoggedInCompany.Logo;

                    if (logoData != null && logoData.Length > 0)
                    {
                        BitmapImage? logo = ByteArrayToImage(logoData);
                        if (logo != null)
                        {
                            imgLogo.Source = logo;
                            return;
                        }
                    }

                    // Geen geldig logo of conversiefout
                    imgLogo.Source = new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.png"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het laden van het logo:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                imgLogo.Source = new BitmapImage(new Uri("pack://application:,,,/Images/placeholder.png"));
            }
        }

        private BitmapImage? ByteArrayToImage(byte[] data)
        {
            if (data == null || data.Length == 0) return null;

            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.StreamSource = ms;
                    img.EndInit();
                    img.Freeze();
                    return img;
                }
            }
            catch
            {
                return null; // veilig terugvallen in hoofdcode
            }
        }
    }
}
