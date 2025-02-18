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
using Microsoft.VisualBasic;

namespace WpfEllipsen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int MaxElipsen = 25;
        int elipsenTeller = 0;
        DispatcherTimer timer;
        Random rnd = new ();
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (elipsenTeller < MaxElipsen)
            {
                elipsenTeller++;
                TekenEllipsen();
            }
            else
            {
                timer.Stop();
            }
        }

        private void TekenEllipsen()
        {
            // Genereer willekeurige waarden
            double width = rnd.Next(20, 101);
            double height = rnd.Next(20, 101);
            double xPos = rnd.NextDouble() * (canvas1.Width - width);
            double yPos = rnd.NextDouble() * (canvas1.Height - height);

            // Willekeurige kleur genereren 
            Color kleur = Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));

            Ellipse elipse = new ()
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush(kleur),
            };

            elipse.SetValue(Canvas.LeftProperty, xPos);
            elipse.SetValue(Canvas.TopProperty, yPos);

            canvas1.Children.Add(elipse);
        }

        private void BtnTekenen_Click(object sender, RoutedEventArgs e)
        {
            elipsenTeller = 0;
            canvas1.Children.Clear();

            timer.Start();
        }
    }
}