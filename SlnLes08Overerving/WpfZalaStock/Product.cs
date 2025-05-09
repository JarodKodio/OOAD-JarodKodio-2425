﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfZalaStock
{
    enum Kleur
    {
        Rood,
        Groen,
        Blauw,
        Geel,
        Wit,
        Zwart,
        Bruin,
        Goud,
        Zilver,
    }
    class Product
    {
        public string Naam { get; set; }
        public string Merk { get; set; }
        public double Prijs { get; set; }
        public Kleur Kleur { get; set; }
        public int AantalInStock { get; set; }

        public Product(string naam, string merk, double prijs, Kleur kleur, int aantalInStock)
        {
            Naam = naam;
            Merk = merk;
            Prijs = prijs;
            Kleur = kleur;
            AantalInStock = aantalInStock;
        }
        public override string ToString()
        {
            return $"{Naam} ({Merk}) - {Prijs} EUR - {Kleur} - {AantalInStock} in stock";
        }
        public int Verkoop(int aantal)
        {
            if (aantal > AantalInStock)
                throw new InvalidOperationException("Niet genoeg voorraad beschikbaar.");
            AantalInStock -= aantal;
            return AantalInStock;
        }

        public int Retourneer(int aantal)
        {
            AantalInStock += aantal;
            return AantalInStock;
        }

    }
}
