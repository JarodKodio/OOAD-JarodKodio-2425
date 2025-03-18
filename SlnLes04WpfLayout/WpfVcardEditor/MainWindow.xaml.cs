using System.IO;
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
using Microsoft.Win32;

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string currentFilePath = null;

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
    "Ben je zeker dat je de applicatie wil afsluiten?",
    "Toepassing sluiten",
    MessageBoxButton.OKCancel,
    MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.OK)
            {
                Environment.Exit(0);
            }
        }

        private void OpenAboutWindow_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "vCard bestanden|*.vcf|Alle bestanden|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                string filePath = dialog.FileName;
                currentFilePath = filePath;

                btnSave.IsEnabled = true;

                string[] regels = File.ReadAllLines(filePath); // lees regels bestand in

                foreach (string regel in regels)
                {
                    if (regel.Contains("N;"))
                    {
                        string naamRegel = regel.Replace("N;CHARSET=UTF-8:", "");
                        string[] naam = naamRegel.Split(';');

                        txtAchternaam.Text = naam[0];
                        txtVoornaam.Text = naam[1];
                    }
                    else if (regel.Contains("GENDER:"))
                    {
                        string geslachtRegel = regel.Replace("GENDER:", "");
                        if (geslachtRegel == "M")
                        {
                            cbxGeslacht.SelectedIndex = 0;
                        }
                        else if (geslachtRegel == "F")
                        {
                            cbxGeslacht.SelectedIndex = 1;
                        }
                        else
                        {
                            cbxGeslacht.SelectedIndex = 2;
                        }
                    }
                    else if (regel.Contains("BDAY:"))
                    {
                        string geboortedatumRegel = regel.Replace("BDAY:", "");
                        DateTime convertedGeboortedatum = DateTime.ParseExact(geboortedatumRegel, "yyyyMMdd", null);
                        dtpGeboortedatum.SelectedDate = convertedGeboortedatum;
                    }
                    else if (regel.Contains("EMAIL;") && regel.Contains("type=HOME"))
                    {
                        string emailRegel = regel.Replace("EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:", "");
                        txtEmail.Text = emailRegel;
                    }
                    else if (regel.Contains("TEL;") && regel.Contains("TYPE=HOME"))
                    {
                        string telefoonRegel = regel.Replace("TEL;TYPE=HOME,VOICE:", "");
                        txtTelefoon.Text = telefoonRegel;
                    }
                }
            }
        }

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "vCard bestanden|*.vcf|Alle bestanden|*.*";
            dialog.FileName = "savedfile.vcf";

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;
                List<string> vcardContent = CreateVcardContent();

                File.WriteAllLines(dialog.FileName, vcardContent);
                currentFilePath = filePath;

                MessageBox.Show("vcard opgeslagen", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (currentFilePath != null)
            {
                List<string> vcardContent = CreateVcardContent();

                File.WriteAllLines(currentFilePath, vcardContent);

                MessageBox.Show("vCard is opgeslagen!", "Bevestiging", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Er is nog geen bestand geopend om op te slaan!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            btnSave.IsEnabled = false;
        }

        private List<string> CreateVcardContent()
        {
            List<string> vcardContent = new List<string>();

            vcardContent.Add("BEGIN:VCARD");
            vcardContent.Add("VERSION:3.0");

            if (!string.IsNullOrEmpty(txtAchternaam.Text) && !string.IsNullOrEmpty(txtVoornaam.Text))
            {
                vcardContent.Add("N;CHARSET=UTF-8:" + txtAchternaam.Text + ";" + txtVoornaam.Text);
            }
            switch (cbxGeslacht.SelectedIndex)
            {
                case 0:
                    vcardContent.Add("GENDER:M");
                    break;
                case 1:
                    vcardContent.Add("GENDER:F");
                    break;
                default:
                    vcardContent.Add("GENDER:0");
                    break;
            }
            if (dtpGeboortedatum.SelectedDate != null)
            {
                vcardContent.Add("BDAY:" + dtpGeboortedatum.SelectedDate.Value.ToString("yyyyMMdd"));
            }
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                vcardContent.Add("EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:" + txtEmail.Text);
            }
            if (!string.IsNullOrEmpty(txtTelefoon.Text))
            {
                vcardContent.Add("TEL;TYPE=HOME,VOICE:" + txtTelefoon.Text);
            }

            return vcardContent;
        }
    }
}
