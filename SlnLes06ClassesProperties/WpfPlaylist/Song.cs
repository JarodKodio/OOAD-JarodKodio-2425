using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfPlaylist
{
    class Song
    {
        public string Naam { get; set; }
        public Artist Artiest { get; set; }
        public string Jaar { get; set; }
        public string Lengte { get; set; }
        public Uri Mp3 { get; set; }

        private static MediaPlayer mediaPlayer = new ();
        private static Random random = new ();
        public Song(string naam, Artist artiest, string jaar, string lengte, Uri mp3)
        {
            Naam = naam;
            Artiest = artiest;
            Jaar = jaar;
            Lengte = lengte;
            this.Mp3 = mp3;
        }

        public override string ToString()
        {
            return $"{Naam} - {Artiest.Naam} - {Jaar} - {Lengte}";
        }

        public void Play()
        {
            mediaPlayer.Stop();
            mediaPlayer.Open(Mp3);
            mediaPlayer.Play();
        }

        public void Stop()
        {
            mediaPlayer.Stop();
        }

        public Song Shuffle(Song[] songs)
        {
            int randomIndex = random.Next(songs.Length);
            return songs[randomIndex];
        }
    }
}
