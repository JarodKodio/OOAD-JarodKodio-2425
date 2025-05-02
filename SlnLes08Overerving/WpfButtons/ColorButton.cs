using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfButtons
{
    enum ButtonType
    {
        Ok,
        Cancel,
        No
    }
    class ColorButton : Button
    {
        public ButtonType Type { get; set; }
        public ColorButton(ButtonType type)
        {
            Type = type;
            this.Content = type.ToString();
            switch (type)
            {
                case ButtonType.Ok:
                    this.Background = System.Windows.Media.Brushes.Green;
                    break;
                case ButtonType.Cancel:
                    this.Background = System.Windows.Media.Brushes.Red;
                    break;
                case ButtonType.No:
                    this.Background = System.Windows.Media.Brushes.Yellow;
                    break;
            }
            this.Foreground = Brushes.White;
            this.Margin = new Thickness(5);
            this.Padding = new Thickness(10, 5, 10, 5);
        }
    }
}
