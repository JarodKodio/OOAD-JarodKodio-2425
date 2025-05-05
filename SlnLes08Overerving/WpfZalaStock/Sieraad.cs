using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfZalaStock
{
    enum Materiaal
    {
        Goud,
        Zilver,
        Brons,
        Platinum,
        Hout,
    }
    class Sieraad : Product
    {
        public List<Materiaal> Materialen { get; set; }

        public Sieraad(string naam, string merk, double prijs, Kleur kleur, int aantalInStock, List<Materiaal> materialen)
            : base(naam, merk, prijs, kleur, aantalInStock)
        {
            Materialen = materialen;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Materialen: {string.Join(", ", Materialen)}";
        }
    }
}
