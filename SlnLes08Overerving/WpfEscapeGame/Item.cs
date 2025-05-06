using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    public class Item : Actor
    {
        // properties  
        public bool IsLocked { get; set; } = false;
        public Item Key { get; set; }
        public Item HiddenItem { get; set; }

        public bool IsPortable { get; set; } = false;

        // constructor  
        public Item(string name, string description, bool isLocked = false, Item key = null, Item hiddenItem = null, bool isPortable = false)
            : base(name, description)
        {
            IsLocked = isLocked;
            Key = key;
            HiddenItem = hiddenItem;
            IsPortable = isPortable;
        }

        // Default constructor fix  
        public Item()
            : base(string.Empty, string.Empty)
        {
        }
    }
}
