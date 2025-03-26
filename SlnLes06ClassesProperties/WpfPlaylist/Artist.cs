using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlaylist
{
    class Artist
    {
        public string Naam { get; set; }
        public DateTime Geboortedatum { get; set; }
        public string Beschrijving { get; set; }
        public string Afbeelding { get; set; }

        public Artist(string naam, DateTime geboortedatum, string beschrijving, string afbeelding)
        {
            Naam = naam;
            Geboortedatum = geboortedatum;
            Beschrijving = beschrijving;
            Afbeelding = afbeelding;
        }

        public override string ToString()
        {
            return $"{Naam} - {Geboortedatum} - {Beschrijving}";
        }
    }
}
