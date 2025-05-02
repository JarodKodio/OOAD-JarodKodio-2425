using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfZalaStock
{
    class Schoen : Product
    {
        public string Maat { get; set; }
        public string Materiaal { get; set; }
        public Schoen(string naam, string merk, double prijs, Kleur kleur, int aantalInStock, string maat, string materiaal)
            : base(naam, merk, prijs, kleur, aantalInStock)
        {
            Maat = maat;
            Materiaal = materiaal;
        }
        public override string ToString()
        {
            return $"{base.ToString()} - Maat: {Maat} - Materiaal: {Materiaal}";
        }
    }
    {
    }
}
