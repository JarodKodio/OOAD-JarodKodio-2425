using System;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace ConsoleAnagram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = System.IO.Path.Combine(folderPath, "1000woorden.txt");

            try
            {
                string content = File.ReadAllText(filePath);
            }
            catch (FileNotFoundException)
            { // file not found
                Console.WriteLine($"File {filePath} not found");
            }
            catch (IOException)
            { // unable to open for reading
                Console.WriteLine($"Unable to open {filePath}");
            }
            catch (Exception)
            { // use general Exception as fallback
                Console.WriteLine($"Unknown error reading {filePath}");
            }

            Console.WriteLine("CONSOLE ANAGRAM");
            Console.WriteLine("===============");
            Console.WriteLine("");
            Console.WriteLine("Kies het aantal letters (5-15): ");
            int aantalLetters = int.Parse(Console.ReadLine());
            if (aantalLetters < 5 || aantalLetters > 15)
            {
                Console.WriteLine("Het aantal letters moet tussen 5 en 15 liggen.");
                return;
            }

            string[] woorden = System.IO.File.ReadAllLines(filePath);
            string[] gefilterdeWoorden = woorden.Where(w => w.Length == aantalLetters).ToArray();

            Random rnd = new Random();

            int randomIndex = rnd.Next(0, gefilterdeWoorden.Length);
            string woord = gefilterdeWoorden[randomIndex];
            string anagram = new string(woord.ToCharArray().OrderBy(c => rnd.Next()).ToArray());

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool geraden = false;
            while (!geraden)
            {
                Console.WriteLine($"Anagram: {anagram}");
                Console.WriteLine("Raad het woord (druk op enter voor een nieuw anagram): ");
                string geradenWoord = Console.ReadLine();

                if (geradenWoord == woord)
                {
                    stopwatch.Stop();
                    Console.WriteLine("Proficiat, je hebt het woord geraden!");
                    Console.WriteLine($"Je hebt {stopwatch.Elapsed.Seconds} seconden nodig gehad.");
                    geraden = true;
                }
                else
                {
                    Console.WriteLine("Raad het woord (druk op enter voor een nieuw anagram): ");
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        // Schud opnieuw en toon het nieuwe anagram
                        anagram = new string(woord.ToCharArray().OrderBy(c => rnd.Next()).ToArray());
                    }
                }
            }
        }
    }
}
