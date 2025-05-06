using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfEscapeGame
{
    public class Room : Actor
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Door> Doors { get; set; } = new List<Door>();
        public Image Image { get; set; }

        public Room(string name, string description, Image image)
            : base(name, description)
        {
            Image = image;
        }

        // Default constructor fix  
        public Room()
            : base(string.Empty, string.Empty)
        {
        }
    }
}

