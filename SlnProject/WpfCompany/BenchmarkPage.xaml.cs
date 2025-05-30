using BenchmarkToolLibrary.Models;
using System;
using System.Collections.Generic;
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
using OxyPlot.Wpf;
using OxyPlot.Series;
using OxyPlot.Legends;
namespace WpfCompany
{
    /// <summary>
    /// Interaction logic for BenchmarkPage.xaml
    /// </summary>
    public partial class BenchmarkPage : Page
    {
        public BenchmarkPage()
        {
            if (!MainWindow.IsCompanyLoggedIn)
            {
                MessageBox.Show("Je moet eerst inloggen.");
                NavigationService?.Navigate(new LoginPage(null));
                return;
            }
            InitializeComponent();
            LoadYears();
        }

        private void LoadYears()
        {
            List<Yearreport> allReports = Yearreport.GetAll();
            List<int> years = allReports
                .Select(r => r.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            cmbJaren.ItemsSource = years;

            if (years.Contains(DateTime.Now.Year))
            {
                cmbJaren.SelectedItem = DateTime.Now.Year;
            }
        }

        private void Benchmark_Click(object sender, RoutedEventArgs e)
        {
            if (cmbJaren.SelectedItem == null)
            {
                MessageBox.Show("Selecteer een jaartal.");
                return;
            }

            int selectedYear = (int)cmbJaren.SelectedItem;
            int companyId = MainWindow.LoggedInCompany.Id;
            string companySector = MainWindow.LoggedInCompany.NacecodeCode;

            List<Yearreport> allReports = Yearreport.GetAll();
            Yearreport? ownReport = allReports.FirstOrDefault(r => r.CompanyId == companyId && r.Year == selectedYear);
            List<Yearreport> others = allReports.Where(r => r.CompanyId != companyId && r.Year == selectedYear).ToList();

            if (ownReport == null || others.Count == 0)
            {
                MessageBox.Show("Geen vergelijkingsgegevens beschikbaar.");
                return;
            }

            List<Cost> allCosts = Cost.GetAll();
            List<Cost> ownCosts = allCosts.Where(c => c.YearreportId == ownReport.Id).ToList();
            List<Cost> otherCostsRaw = allCosts
                .Where(c => others.Select(o => o.Id).Contains(c.YearreportId) && c.CosttypeType != null)
                .ToList();

            Dictionary<string, decimal> otherAverages = new Dictionary<string, decimal>();
            foreach (IGrouping<string, Cost> group in otherCostsRaw.GroupBy(c => c.CosttypeType))
            {
                string type = group.Key;
                decimal average = group.Average(c => c.Value);
                otherAverages[type] = average;
            }

            OxyPlot.PlotModel model = new OxyPlot.PlotModel { Title = "Benchmarkresultaten" };
            OxyPlot.Axes.CategoryAxis categoryAxis = new OxyPlot.Axes.CategoryAxis { Position = OxyPlot.Axes.AxisPosition.Left };
            OxyPlot.Axes.LinearAxis valueAxis = new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, Title = "Kost (€)" };
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);

            OxyPlot.Series.BarSeries barSeries = new OxyPlot.Series.BarSeries { Title = "Kostenvergelijking", BarWidth = 0.5 };
            List<OxyPlot.Series.BarItem> barItems = new List<OxyPlot.Series.BarItem>();
            List<string> labels = new List<string>();
            ownCosts = ownCosts
                .GroupBy(c => c.CosttypeType)
                .Select(g => g.First())
                .ToList();
            foreach (Cost cost in ownCosts)
            {
                string type = cost.CosttypeType;
                decimal value = cost.Value;

                if (otherAverages.ContainsKey(type))
                {
                    decimal gemiddeld = otherAverages[type];
                    decimal verschil = value - gemiddeld;


                    barItems.Add(new OxyPlot.Series.BarItem((double)value));
                    labels.Add(type + " (Jij)");

                    barItems.Add(new OxyPlot.Series.BarItem((double)gemiddeld));
                    labels.Add(type + " (Gem)");

                    // Zo past de grafiek zich aan op basis van het aantal items.
                    int hoogte = labels.Count * 30;
                    plotView.MinHeight = hoogte < 400 ? 400 : hoogte;
                }
            }

            categoryAxis.Labels.AddRange(labels);
            barSeries.ItemsSource = barItems;
            model.Series.Add(barSeries);
            plotView.Model = model;
        }
    }
}


