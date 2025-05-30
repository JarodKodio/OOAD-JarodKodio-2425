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
                System.Windows.MessageBox.Show("Je moet eerst inloggen.");
                NavigationService?.Navigate(new LoginPage(null));
                return;
            }
            InitializeComponent();

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

                // Als geen geldig logo:

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
                return null;
            }
        }
    }
}
