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

namespace WpfZalaStock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Product> producten; // Lijst met alle producten
        private StockBeheer stockBeheer; // Beheer voor verkoop en retour

        public MainWindow()
        {
            InitializeComponent();
            stockBeheer = new StockBeheer();
            producten = new List<Product>();

            // Lees het CSV-bestand en laad de producten
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WpfZalaStockStartmateriaal", "producten.csv");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string[] data = line.Split(';');
                    if (data.Length >= 6)
                    {
                        string categorie = data[0];
                        string naam = data[1];
                        string merk = data[2];
                        double prijs = double.Parse(data[3].Replace(',', '.'));
                        Kleur kleur = (Kleur)Enum.Parse(typeof(Kleur), data[4]);
                        int aantalInStock = int.Parse(data[5]);

                        if (categorie == "Kleding" && data.Length >= 8)
                        {
                            Pasvorm pasvorm = (Pasvorm)Enum.Parse(typeof(Pasvorm), data[6]);
                            Lengte lengte = (Lengte)Enum.Parse(typeof(Lengte), data[7]);
                            producten.Add(new Kleding(naam, merk, prijs, kleur, aantalInStock, pasvorm, lengte));
                        }
                        else if (categorie == "Schoenen" && data.Length >= 9)
                        {
                            Breedte breedte = (Breedte)Enum.Parse(typeof(Breedte), data[6]);
                            Sluiting sluiting = (Sluiting)Enum.Parse(typeof(Sluiting), data[7]);
                            Neus neus = (Neus)Enum.Parse(typeof(Neus), data[8]);
                            producten.Add(new Schoen(naam, merk, prijs, kleur, aantalInStock, breedte, sluiting, neus));
                        }
                        else if (categorie == "Sieraden" && data.Length >= 7)
                        {
                            string[] materialenStrings = data[6].Split(',');
                            List<Materiaal> materialen = materialenStrings
                                .Select(m => (Materiaal)Enum.Parse(typeof(Materiaal), m))
                                .ToList();
                            producten.Add(new Sieraad(naam, merk, prijs, kleur, aantalInStock, materialen));
                        }
                    }
                }

                // Toon alle producten standaard in de lijst
                ProductListBox.ItemsSource = producten;
            }
            else
            {
                MessageBox.Show($"Het bestand kon niet worden gevonden: {filePath}");
            }
        }

        // Event handler voor CategorieComboBox
        private void CategorieChanged(object sender, SelectionChangedEventArgs e)
        {
            string categorieNaam = (CategorieComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (categorieNaam == "Alle")
            {
                ProductListBox.ItemsSource = producten;
            }
            else
            {
                ProductListBox.ItemsSource = producten
                    .Where(p => p.GetType().Name == categorieNaam)
                    .ToList();
            }
        }

        // Event handler voor VerkoopButton
        private void VerkoopButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AantalTextBox.Text, out int aantal) && ProductListBox.SelectedItem is Product product)
            {
                try
                {
                    stockBeheer.Verkopen(product, aantal);
                    VerkochtTextBox.Text += $"{product.Naam} ({product.Merk}) - €{product.Prijs} x {aantal} - Totaal: €{product.Prijs * aantal:F2}\n";
                    UpdateFooter();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Event handler voor RetourButton
        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AantalTextBox.Text, out int aantal) && ProductListBox.SelectedItem is Product product)
            {
                stockBeheer.Retourneren(product, aantal);
                GeretourneerdTextBox.Text += $"{product.Naam} ({product.Merk}) - €{product.Prijs} x {aantal} - Totaal: €{product.Prijs * aantal:F2}\n";
                UpdateFooter();
            }
        }

        // Werk de footer bij met de totalen
        private void UpdateFooter()
        {
            double totaalVerkocht = stockBeheer.Verkocht.Sum(item => item.Key.Prijs * item.Value);
            double totaalRetour = stockBeheer.Geretourneerd.Sum(item => item.Key.Prijs * item.Value);

            TotaalVerkochtTextBlock.Text = $"Totaalbedrag verkopen: € {totaalVerkocht:F2}";
            TotaalRetourTextBlock.Text = $"Totaalbedrag retours: -€ {totaalRetour:F2}";
            TotaalTextBlock.Text = $"Totaalbedrag: € {totaalVerkocht - totaalRetour:F2}";
        }

        // toon de productdetails in de TextBlock
        private void ProductListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Haal het geselecteerde product uit de ListBox
            Product geselecteerdProduct = ProductListBox.SelectedItem as Product;

            // Controleer of er een product is geselecteerd
            if (geselecteerdProduct != null)
            {
                // Toon de productdetails in de TextBlock
                ProductDetailsTextBlock.Text = geselecteerdProduct.ToString();
            }
            else
            {
                // Als er geen product is geselecteerd, laat de TextBlock leeg
                ProductDetailsTextBlock.Text = "Selecteer een product...";
            }
        }
    }
}