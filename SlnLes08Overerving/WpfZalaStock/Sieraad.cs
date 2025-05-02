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
        public Sieraad(string naam, string beschrijving, double prijs, string maat, string kleur) : base(naam, beschrijving, prijs)
        {
            Maat = maat;
            Kleur = kleur;
        }
        public string Maat { get; set; }
        public string Kleur { get; set; }
    }
}
