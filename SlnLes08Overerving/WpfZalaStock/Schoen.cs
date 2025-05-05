using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfZalaStock
{
    enum Breedte
    {
        Smal,
        Normaal,
        Wijd
    }

    enum Sluiting
    {
        Veters,
        Rits,
        Klittenband
    }

    enum Neus
    {
        Rond,
        Puntig,
        Vierkant
    }
    class Schoen : Product
    {
        public Breedte Breedte { get; set; }
        public Sluiting Sluiting { get; set; }
        public Neus Neus { get; set; }

        public Schoen(string naam, string merk, double prijs, Kleur kleur, int aantalInStock, Breedte breedte, Sluiting sluiting, Neus neus)
            : base(naam, merk, prijs, kleur, aantalInStock)
        {
            Breedte = breedte;
            Sluiting = sluiting;
            Neus = neus;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Breedte: {Breedte} - Sluiting: {Sluiting} - Neus: {Neus}";
        }
    }
}
