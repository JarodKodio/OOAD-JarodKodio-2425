using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Net.Mime.MediaTypeNames;

namespace WpfMatchFiles
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

        private HashSet<string> LeesTriplets(string path)
        {
            string tekst = File.ReadAllText(path);

            // Verwijder alle niet-a-Z karakters en vervang ze door spaties
            tekst = Regex.Replace(tekst, "[^a-z ]", " ");
            tekst = Regex.Replace(tekst, @"\s+", " "); // Verwijder dubbele spaties

            // Maak een lijst van alle woorden
            List<string> woorden = tekst.Split(' ').ToList();

            // Maak een lijst van alle triplets
            HashSet<string> triplets = new HashSet<string>();
            for (int i = 0; i < woorden.Count - 2; i++) // -2 omdat we 3 woorden tegelijk nodig hebben
            {
                triplets.Add(woorden[i] + " " + woorden[i + 1] + " " + woorden[i + 2]);
            }
            return triplets;
        }

        // Bereken de overeenkomst tussen twee lijsten van triplets (AI)
        private double BerekenOverenkomst(List<string> lijst1, List<string> lijst2) 
        {
            if (lijst1.Count == 0 || lijst2.Count == 0)
            {
                lblMessage.Content = "Beide bestanden moeten triplets bevatten!";
                return 0;
            }

            HashSet<string> set1 = new HashSet<string>(lijst1);
            HashSet<string> set2 = new HashSet<string>(lijst2);

            int gemeenschappelijkeTriplets = set1.Intersect(set2).Count();
            int totaalUniekeTriplets = set1.Union(set2).Count();

            double overeenkomstPercentage = (double)gemeenschappelijkeTriplets / totaalUniekeTriplets * 100;
            return Math.Round(overeenkomstPercentage, 2);
        }
        
        private void BtnEerstebestand_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            string chosenFileName;
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                // user picked a file and pressed OK
                chosenFileName = dialog.FileName;
                txtEersteBestand.Text = chosenFileName;
            }
            else
            {
                // user cancelled or escaped dialog window
            }
        }

        private void BtnTweedebestand_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            string chosenFileName;
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                // user picked a file and pressed OK
                chosenFileName = dialog.FileName;
                txtTweedeBestand.Text = chosenFileName;
            }
            else
            {
                // user cancelled or escaped dialog window
            }
        }

        private void BtnVergelijk_Click(object sender, RoutedEventArgs e)
        {
            HashSet<string> triplets1 = LeesTriplets(txtEersteBestand.Text);
            HashSet<string> triplets2 = LeesTriplets(txtTweedeBestand.Text);

            double overeenkomst = BerekenOverenkomst(triplets1.ToList(), triplets2.ToList());
            txtOvereenkomst.Text = $"Overeenkomst: {overeenkomst}%";
        }
    }
}