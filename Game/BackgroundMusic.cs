using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

namespace Game
{
    class BackgroundMusic
    {
        private List<Song> Playlist;
        private Random random;
        private int currentSongIndex = 0;

        public BackgroundMusic()
        {
            random = new Random();
            MediaPlayer.Volume = 0.7f;
            MediaPlayer.MediaStateChanged += MediaPlayerOnMediaStateChanged;
        }

        // Adds the songs to the list that can be played.
        public void LoadSongs(params string[] songNames)
        {
            Playlist = new List<Song>(songNames.Length);
            foreach (var song in songNames)
            {
                Playlist.Add(OutlivedGame.Instance().Get<Song>("Sound/" + song));
            }
        }

        private void MediaPlayerOnMediaStateChanged(object sender, EventArgs eventArgs)
        {
            if (Playlist != null)
            {
                //MediaPlayer.Stop();
                //MediaPlayer.MoveNext();
                currentSongIndex = random.Next(0, Playlist.Count - 1);
                MediaPlayer.Play(Playlist[currentSongIndex]);
            }
        }

        public void PlayMusic()
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                currentSongIndex = random.Next(0, Playlist.Count - 1);
                MediaPlayer.Play(Playlist[currentSongIndex]);
            }
        }
    }
}
