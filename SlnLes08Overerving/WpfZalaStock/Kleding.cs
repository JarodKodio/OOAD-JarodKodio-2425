using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfZalaStock
{
    enum Pasvorm
    {
        Slim,
        Fitted,
        Straight,
        Relaxed,
        Regular,
        Baggy,
        Bootcut,
        Tapered,
        Flared
    }

    enum Lengte
    {
        Kort,
        Normaal,
        Lang
    }
    class Kleding : Product
    {
        public Pasvorm Pasvorm { get; set; }
        public Lengte Lengte { get; set; }

        public Kleding(string naam, string merk, double prijs, Kleur kleur, int aantalInStock, Pasvorm pasvorm, Lengte lengte)
            : base(naam, merk, prijs, kleur, aantalInStock)
        {
            Pasvorm = pasvorm;
            Lengte = lengte;
        }

        public override string ToString()
        {
            return $"{base.ToString()} - Pasvorm: {Pasvorm} - Lengte: {Lengte}";
        }
    }
}
