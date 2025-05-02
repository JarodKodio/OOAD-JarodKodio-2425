using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfButtons
{
    public enum DirectionType
    {
        Up,
        Down
    }
    class CounterButton : Button
    {
        public string Prefix { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int Count { get; set; }
        public DirectionType Direction { get; set; }
        public bool Loop { get; set; }

        public CounterButton(string prefix, int min, int max, DirectionType direction)
        {
            Prefix = prefix;
            Min = min;
            Max = max;
            Count = min;
            Direction = direction;
            Loop = false;
            this.Content = $"{Prefix} {Count}";
            MouseRightButtonDown += CounterButton_MouseRightButtonDown; // right click to reset
        }

        protected override void OnClick()
        {
            if (Direction == DirectionType.Up)
            {
                Count++;
                if (Count > Max)
                {
                    if (Loop)
                        Count = Min;
                    else
                        Count = Max;
                }
            }
            else 
            {
                Count--;
                if (Count < Min)
                {
                    if (Loop)
                        Count = Max;
                    else
                        Count = Min;
                }
            }

            this.Content = $"{Prefix} {Count}";
            base.OnClick(); // Zorgt voor standaard button-gedrag
        }

        private void CounterButton_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Reset(); 
        }

        private void Reset()
        {
            Count = Min;
            this.Content = $"{Prefix} {Count}";
        }
    }
}
