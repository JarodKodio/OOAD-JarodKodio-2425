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

namespace WpfTaken
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

        Stack<string> takenStack = new Stack<string>();

        private void CheckForm()
        {
            txtMelding.Text = "";

            if (string.IsNullOrWhiteSpace(txtTaak.Text))
            {
                txtMelding.Text += "Gelieve een taak in te voegen.\n";
            }

            if (cbxPrioriteit.SelectedIndex == -1)
            {
                txtMelding.Text += "Gelieve een prioriteit in te kiezen.\n";
            }

            if (string.IsNullOrWhiteSpace(txtDeadline.Text))
            {
                txtMelding.Text += "Gelieve een deadline te kiezen.\n";
            }

            if (!(rdnAdam.IsChecked == true || rdnBilal.IsChecked == true || rdnChelsey.IsChecked == true))
            {
                txtMelding.Text += "Gelieve een persoon te kiezen.\n";
            }

            if (txtMelding.Text == "")
            {
                btnToevoegen.IsEnabled = true;
            }
            else
            {
                btnToevoegen.IsEnabled = false;
            }
        }

        private void ClearForm()
        {
            txtDeadline.Text = string.Empty;
            txtTaak.Text = string.Empty;
            txtMelding.Text = string.Empty;
            cbxPrioriteit.SelectedIndex = -1;
            rdnAdam.IsChecked = false;
            rdnBilal.IsChecked = false;
            rdnChelsey.IsChecked = false;
        }

        private void TxtTaak_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckForm();
        }

        private void CbxPrioriteit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckForm();
        }

        private void TxtDeadline_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckForm();
        }

        private void RdnAdam_Checked(object sender, RoutedEventArgs e)
        {
            CheckForm();
        }

        private void RdnBilal_Checked(object sender, RoutedEventArgs e)
        {
            CheckForm();
        }

        private void RdnChelsey_Checked(object sender, RoutedEventArgs e)
        {
            CheckForm();
        }

        private void ToevoegenTaak()
        {
            if (txtMelding.Text != "")
            {
                return;
            }

            string prioriteit = cbxPrioriteit.SelectedItem.ToString();
            string taakTekst = $"{txtTaak.Text} ({txtDeadline.Text}; door ";

            if (rdnAdam.IsChecked == true)
            {
                taakTekst += "Adam)";
            }
            else if (rdnBilal.IsChecked == true)
            {
                taakTekst += "Bilal)";
            }
            else if (rdnChelsey.IsChecked == true)
            {
                taakTekst += "Chelsey)";
            }

            // Voeg taak toe aan stack
            takenStack.Push(taakTekst);

            // Update de ListBox
            lbxTaken.Items.Clear();
            foreach (string taak in takenStack)
            {
                lbxTaken.Items.Add(taak);
            }

            ClearForm();
            btnTerugzetten.IsEnabled = true;
            btnVerwijderen.IsEnabled = true;
            btnToevoegen.IsEnabled = false;
        }

        private void VerwijderTaak()
        {
            // Als er geen taak geselecteerd is, doe niets
            if (lbxTaken.SelectedItem == null)
            {
                txtMelding.Text = "Selecteer een taak om te verwijderen.";
                return;
            }

            string geselecteerdeTaak = lbxTaken.SelectedItem.ToString();
            takenStack = new Stack<string>(takenStack.Where(item => item != geselecteerdeTaak));

            // Update listbox
            lbxTaken.Items.Clear();
            foreach (string taak in takenStack)
            {
                lbxTaken.Items.Add(taak);
            }

            ClearForm();
            CheckForm();
        }

        private void TerugZetten()
        {
            if (takenStack.Count == 0)
            {
                txtMelding.Text = "Er zijn geen taken om terug te zetten.";
            }
            else
            {
                // Verwijder de laatste taak uit de stack
                string taak = takenStack.Pop();

                // Update listbox
                lbxTaken.Items.Clear();
                foreach (string item in takenStack)
                {
                    lbxTaken.Items.Add(item);
                }
            }
        }

        private void BtnVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            VerwijderTaak();
        }

        private void BtnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            ToevoegenTaak();
        }
    }
}