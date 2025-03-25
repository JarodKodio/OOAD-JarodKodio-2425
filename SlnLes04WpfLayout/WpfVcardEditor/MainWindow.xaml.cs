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
        private bool isChanged = false;

        // applicatie afsluiten
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

        // about window openen
        private void OpenAboutWindow_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        // vcf files openen
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
                string[] regels = null;
                try
                {
                    regels = File.ReadAllLines(filePath); // lees regels bestand in
                }
                catch (FileNotFoundException)
                { // file not found
                    MessageBox.Show("Bestand niet gevonden.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (IOException)
                { // unable to open for reading
                    MessageBox.Show("Bestand kon niet worden geopend.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                { // use general Exception as fallback
                    MessageBox.Show("Er is een onverwachte fout opgetreden", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // loop door de regels van het bestand
                foreach (string regel in regels)
                {
                    if (regel.Contains("N;"))
                    {
                        string naamRegel = regel.Replace("N;CHARSET=UTF-8:", "");
                        string[] naam = naamRegel.Split(';'); // splits op ;

                        if (naam.Length >= 2)
                        {
                            txtAchternaam.Text = naam[0];
                            txtVoornaam.Text = naam[1];
                        }
                    }
                    else if (regel.Contains("FN;"))
                    {
                        string naamRegel = regel.Replace("FN;CHARSET=UTF-8:", "");
                        string[] naam = naamRegel.Split(' '); // split op spatie

                        if (naam.Length >= 2)
                        {
                            txtAchternaam.Text = naam[0];
                            txtVoornaam.Text = naam[1];
                        }
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
                    else if (regel.Contains("PHOTO;"))
                    {
                        string base64Image = regel.Replace("PHOTO;ENCODING=BASE64;TYPE=image/jpeg:", "");
                        imgProfielfoto.Source = ReadBase64Image(base64Image);
                    }
                    txtHuidigeFile.Text = filePath;
                }
            }
        }

        // vcf files opslaan als
        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Filter = "vCard bestanden|*.vcf|Alle bestanden|*.*";
                dialog.FileName = "savedfile.vcf";

                if (dialog.ShowDialog() == true)
                {
                    string filePath = dialog.FileName;
                    List<string> vcardContent = CreateVcardContent();

                    // schrijf de vcard content naar het bestand
                    File.WriteAllLines(dialog.FileName, vcardContent);
                    currentFilePath = filePath;

                    MessageBox.Show("vcard opgeslagen", "Opslaan", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (FileNotFoundException)
            { // file not found
                MessageBox.Show("Bestand niet gevonden.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException)
            { // unable to open for reading
                MessageBox.Show("Bestand kon niet worden geopend.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            { // use general Exception as fallback
                MessageBox.Show("Er is een onverwachte fout opgetreden", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // vcf files opslaan
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

        // event handler voor alle textboxes en comboboxes
        private void Card_Changed(object sender, EventArgs e)
        {
            isChanged = true;
        }
        private List<string> CreateVcardContent()
        {
            List<string> vcardContent = new List<string>();

            vcardContent.Add("BEGIN:VCARD");
            vcardContent.Add("VERSION:3.0");

            try
            {
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
                if (imgProfielfoto.Source != null)
                {
                    string base64Image = GetBase64FromImage(imgProfielfoto);
                    vcardContent.Add("PHOTO;ENCODING=BASE64;TYPE=JPEG:" + base64Image);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Onverwachte fout bij inlezen van vCard: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return vcardContent;
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            if (isChanged)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Ben je zeker dat je een nieuwe kaart wil maken? Alle niet opgeslagen wijzigingen gaan verloren!",
                    "Nieuwe kaart",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation);

                if (result == MessageBoxResult.OK)
                {
                    txtAchternaam.Text = "";
                    txtVoornaam.Text = "";
                    cbxGeslacht.SelectedIndex = -1;
                    dtpGeboortedatum.SelectedDate = null;
                    txtEmail.Text = "";
                    txtTelefoon.Text = "";
                    imgProfielfoto.Source = null;
                    lblFotoPath.Content = "";
                    txtHuidigeFile.Text = "";

                    isChanged = false;
                }
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Afbeeldingen|*.png;*.jpg;|Alle bestanden|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                try
                {
                    string filePath = dialog.FileName;
                    BitmapImage bitmap = new BitmapImage(new Uri(filePath));
                    lblFotoPath.Content = filePath;
                    imgProfielfoto.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Er is een fout opgetreden bij het inlezen van de afbeelding: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private BitmapImage ReadBase64Image(string base64String)
        {
            // Verwijder de eventuele nieuwe regels of witruimtes die per ongeluk in de string kunnen staan
            base64String = base64String.Replace("\n", "").Replace("\r", "").Trim();

            try
            {
                // Decoderen van de Base64 string naar bytes
                byte[] imageBytes = Convert.FromBase64String(base64String);

                // Zet de bytes om naar een BitmapImage voor WPF
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();

                    return bitmap;
                }
            }
            catch (FormatException ex)
            {
                // Als de Base64-string ongeldig is, geef een foutmelding
                MessageBox.Show("Ongeldige Base64-string: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        // Deze methode zet een BitmapImage om naar een Base64 string -CHAT GPT
        private string GetBase64FromImage(Image image)
        {
            if (image.Source is BitmapImage bitmapImage)
            {
                // Converteer de BitmapImage naar bytes
                byte[] imageBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(ms);
                    imageBytes = ms.ToArray();
                }

                // Zet de bytes om naar een Base64 string
                return Convert.ToBase64String(imageBytes);
            }

            return null;
        }
    }
}
