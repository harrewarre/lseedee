using System;
using System.Diagnostics;
using System.Linq;
using LSeeDee.Services;

namespace LSeeDee.Screens
{
    public class SpotifyScreen : IScreen
    {
        private readonly Display _display;

        private struct SpotifyData
        {
            public string Title { get; set; }
            public bool IsRunning { get; set; }
        }

        public SpotifyScreen(Display display)
        {
            _display = display;
        }

        public void Draw()
        {
            var data = GetSpotifyData();
            _display.WriteText(0, 1, "Spotify");

            if (!data.IsRunning)
            {
                _display.WriteText(2, 0, "Not playing :-(");
                return;
            }

            var textData = data.Title.Split('-', 2);

            _display.WriteText(2, 0, textData[0].Trim());
            _display.WriteText(3, 0, textData[1].Trim());
        }

        public bool IsEnabled()
        {
            return true;
        }

        private SpotifyData GetSpotifyData()
        {
            var proc = Process.GetProcessesByName("Spotify").FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));

            if (proc == null)
            {
                return new SpotifyData { Title = "Spotify is not running!", IsRunning = false };
            }

            if (proc.MainWindowTitle.Contains("Spotify", StringComparison.InvariantCultureIgnoreCase))
            {
                return new SpotifyData { Title = "No track is playing", IsRunning = false };
            }

            return new SpotifyData { Title = proc.MainWindowTitle, IsRunning = true };
        }
    }
}