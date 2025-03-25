using System;

public class Deck
{
    // Variablelen

    // Properties
    public List<Kaart> Kaarten { get; set; }

    // Constructor met parameters
    public Deck()
    {
        Kaarten = new List<Kaart>();
        foreach (string kleur in new string[] { "Harten", "Ruiten", "Klaveren", "Schoppen" })
        {
            for (int nummer = 1; nummer <= 13; nummer++)
            {
                Kaarten.Add(new Kaart(nummer, kleur));
            }
        }
    }

    // Methoden

    // Methode om kaarten te schudden https://stackoverflow.com/questions/33643104/shuffling-a-stackt
    public void Schudden()
    {
        Random random = new Random();
        for (int i = 0; i < Kaarten.Count; i++)
        {
            int randomIndex = random.Next(Kaarten.Count);
            Kaart temp = Kaarten[i];
            Kaarten[i] = Kaarten[randomIndex];
            Kaarten[randomIndex] = temp;
        }
    }

    // Methode die een kaart van de stapel neemt
    public Kaart NeemKaart()
    {
        if (Kaarten.Count == 0)
        {
            throw new InvalidOperationException("De stapel is leeg");
        }

        // Hulp ai gebruik gemaakt van verwijzingen
        Kaart kaart = Kaarten[0];
        Kaarten.RemoveAt(0);
        return kaart;
    }
}
