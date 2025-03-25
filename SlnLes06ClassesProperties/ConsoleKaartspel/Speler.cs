public class Speler
{
    // Properties
    public string Naam { get; set; }
    public List<Kaart> Kaarten { get; set; }

    // Property om te checken of de speler nog kaarten heeft
    public bool HeeftNogKaarten
    {
        get { return Kaarten.Count > 0; }
    }

    // Constructor met alleen naam
    public Speler(string naam)
    {
        Naam = naam;
        Kaarten = new List<Kaart>(); // Lege hand bij start
    }

    // Constructor met naam en startkaarten
    public Speler(string naam, List<Kaart> kaarten)
    {
        Naam = naam;
        Kaarten = kaarten;
    }

    // Methode om een willekeurige kaart af te leggen
    public Kaart LegKaart()
    {
        if (Kaarten.Count == 0)
        {
            throw new InvalidOperationException($"{Naam} heeft geen kaarten meer!");
        }

        Random random = new Random();
        int index = random.Next(Kaarten.Count);
        Kaart kaart = Kaarten[index];
        Kaarten.RemoveAt(index);
        return kaart;
    }
}
