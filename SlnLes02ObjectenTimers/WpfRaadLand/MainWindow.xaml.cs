using System.Diagnostics;
using System.Media;
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
using System.Windows.Threading;

namespace WpfRaadLand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new ();
        Stopwatch stopwatch = new ();
        Random rnd = new ();
        string rndland;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_tick;
        }

        private void Timer_tick(object? sender, EventArgs e)
        {
            if (stopwatch.Elapsed.TotalSeconds >= 5)
            {
                timer.Stop();
                stopwatch.Stop();
                btnStart.IsEnabled = true;
                txtCommentaar.Text = "Helaas je tijd is op";

            }
        }
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image img)
            {
                string gekozenLand = img.Tag.ToString();
                if (gekozenLand == rndland )
                {
                    txtCommentaar.Text = "Juist!";
                }
                else
                {
                    txtCommentaar.Text = $"Fout het juiste anrwoord is {rndland}";
                }
                timer.Stop ();
                stopwatch.Stop ();
                btnStart.IsEnabled = true;
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            stopwatch.Restart();
            timer.Start();
            rndland = RandomLand();


            btnStart.IsEnabled = false;
            txtCommentaar.Text = "Raad het land";

        }

        private string RandomLand()
        {
            string[] landen = { "Argentina", "Nieuwzeeland", "Japan", "Marokko", "Finland" };
            string randomLand = landen[rnd.Next(landen.Length)];

            return randomLand;
        }
    }
}