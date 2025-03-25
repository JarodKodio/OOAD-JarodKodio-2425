using System;
using System.Linq; // Nodig voor .Contains()

public class Kaart
{
    private string[] kleuren = { "Harten", "Ruiten", "Klaveren", "Schoppen" };
    private int nummer;
    private string kleur;

    // Property Nummer
    public int Nummer
    {
        get { return nummer; }
        set
        {
            if (value < 1 || value > 13)
            {
                throw new ArgumentOutOfRangeException("Nummer moet tussen 1 en 13 liggen");
            }
            nummer = value;
        }
    }

    // Property Kleur
    public string Kleur
    {
        get { return kleur; }
        set
        {
            if (!kleuren.Contains(value)) 
            {
                throw new ArgumentOutOfRangeException("Kleur moet Harten, Ruiten, Klaveren of Schoppen zijn");
            }
            kleur = value; 
        }
    }

    // Lege constructor
    public Kaart()
    {
    }

    // Constructor met parameters
    public Kaart(int nummer, string kleur)
    {
        Nummer = nummer;
        Kleur = kleur;
    }
}
