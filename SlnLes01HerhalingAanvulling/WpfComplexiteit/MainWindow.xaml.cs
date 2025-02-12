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

namespace WpfComplexiteit
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

        private bool IsKlinker(char c)
        {
            char[] letters = { 'a', 'e', 'i', 'o', 'u' };
            if (letters.Contains(c))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int AantalLettergrepen(string woord)
        {
            int lettergrepen = 0;
            bool isVorigeKlinker = false;

            foreach (char c in woord)
            {
                if (IsKlinker(c) && !isVorigeKlinker)
                {
                    lettergrepen++;
                    isVorigeKlinker = true;
                }
                else
                {
                    isVorigeKlinker = false;
                }
            }
            return lettergrepen;
        }

        private double Complexiteit(string woord)
        {
            int aantalLetters = woord.Length;
            int aantalLettergrepen = AantalLettergrepen(woord);

            if (aantalLettergrepen == 0)
                return 0;

            double complexiteit = (Convert.ToDouble(aantalLetters) / 3.0) + Convert.ToDouble(aantalLettergrepen);

            foreach (char c in woord)
            {
                if (c == 'x' || c == 'y' || c == 'q')
                {
                    complexiteit += 1.0;
                }
            }

            return complexiteit;
        }

        private void btnAnalyseer_Click(object sender, RoutedEventArgs e)
        {
            string woord = txtWoord.Text.ToLower();

            lblKarakters.Content = "aantal karaketers: " + woord.Length;
            lblLettergrepen.Content = "aantal lettergrepen: " + AantalLettergrepen(woord).ToString();
            lblComplexiteit.Content = "complexiteit: " + Math.Round(Complexiteit(woord), 1).ToString();
        }
    }
}