using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    class Item
    {
        // properties
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLocked { get; set; } = false;
        public Item Key { get; set; }
        public Item HiddenItem { get; set; }

        public bool IsPortable { get; set; } = false;

        // methodes
        public override string ToString()
        {
            return Name;
        }
    }
}
