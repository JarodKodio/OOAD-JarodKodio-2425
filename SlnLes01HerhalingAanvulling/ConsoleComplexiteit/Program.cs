namespace ConsoleComplexiteit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string woord;

            do
            {
                Console.Write("Geef een woord (enter om te stoppen): ");
                woord = Console.ReadLine().ToLower();

                if (woord != "")
                {
                    Console.WriteLine($"Aantal karakters: {woord.Length}");
                    Console.WriteLine($"Aantal lettergrepen: {AantalLettergrepen(woord)}");
                    Console.WriteLine($"Complexiteit: {Math.Round(Complexiteit(woord), 1)}");
                }
            }
            while (woord != "");

            Console.WriteLine("Bedankt en tot ziens");
        }

        static bool IsKlinker(char c)
        {
            char[] letters = { 'a', 'e', 'i', 'o', 'u' };
            if (letters.Contains(c))
            {
                return true;
            }
            else 
            { 
                return false;
            }
        }

        static int AantalLettergrepen(string woord)
        {
            int lettergrepen = 0;
            bool isVorigeKlinker = false;

            foreach (char c in woord)
            {
                if (IsKlinker(c) && !isVorigeKlinker)
                {
                    lettergrepen++;
                    isVorigeKlinker = true;
                }
                else
                {
                    isVorigeKlinker = false;
                }
            }
            return lettergrepen;
        }

        static double Complexiteit(string woord)
        {
            int aantalLetters = woord.Length;
            int aantalLettergrepen = AantalLettergrepen(woord);

            if (aantalLettergrepen == 0)
                return 0;

            double complexiteit = (Convert.ToDouble(aantalLetters) / 3.0) + Convert.ToDouble(aantalLettergrepen);

            foreach (char c in woord)
            {
                if (c == 'x' || c == 'y' || c == 'q')
                {
                    complexiteit += 1.0;
                }
            }
            
            return complexiteit;
        }
    }
}
