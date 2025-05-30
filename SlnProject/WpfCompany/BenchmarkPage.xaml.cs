using BenchmarkToolLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OxyPlot.Axes;
using OxyPlot.Series;

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

            try
            {
                LoadYears();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het laden van jaarrapporten:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadYears()
        {
            List<Yearreport> allReports = Yearreport.GetAll(); // SQL
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
            try
            {
                if (cmbJaren.SelectedItem == null)
                {
                    MessageBox.Show("Selecteer een jaartal.");
                    return;
                }

                int selectedYear = (int)cmbJaren.SelectedItem;
                var bedrijf = MainWindow.LoggedInCompany;

                if (bedrijf == null)
                {
                    MessageBox.Show("Er is geen bedrijf ingelogd.");
                    return;
                }

                int companyId = bedrijf.Id;
                string companySector = bedrijf.NacecodeCode;

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

                var model = new OxyPlot.PlotModel { Title = "Benchmarkresultaten" };
                var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
                var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, Title = "Kost (€)" };
                model.Axes.Add(categoryAxis);
                model.Axes.Add(valueAxis);

                var barSeries = new BarSeries { Title = "Kostenvergelijking", BarWidth = 0.5 };
                var barItems = new List<BarItem>();
                var labels = new List<string>();

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

                        barItems.Add(new BarItem((double)value));
                        labels.Add(type + " (Jij)");

                        barItems.Add(new BarItem((double)gemiddeld));
                        labels.Add(type + " (Gem)");
                    }
                }

                // Automatische hoogte
                int hoogte = labels.Count * 30;
                plotView.MinHeight = hoogte < 400 ? 400 : hoogte;

                categoryAxis.Labels.AddRange(labels);
                barSeries.ItemsSource = barItems;
                model.Series.Add(barSeries);
                plotView.Model = model;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij benchmarken:\n" + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
