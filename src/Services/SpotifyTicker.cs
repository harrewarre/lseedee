using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using LSeeDee.Services;

namespace LSeeDee
{
    public class SpotifyTicker
    {
        private readonly Timer _timer;
        private readonly Display _display;

        private string currentTitle = "";

        public SpotifyTicker(Display display)
        {
            _display = display;
            _timer = new Timer(1000);

            _timer.Elapsed += (sender, elapsedArgs) =>
            {
                var title = TryGetTrackTitle();
                if (currentTitle != title)
                {
                    currentTitle = title;
                    _display.WriteScrollingText(1, title);
                }
            };

            _timer.Enabled = true;
            _timer.AutoReset = true;
        }

        private string TryGetTrackTitle()
        {
            var proc = Process.GetProcessesByName("Spotify").FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));

            if (proc == null)
            {
                return "Spotify is not running!";
            }

            if (proc.MainWindowTitle.Contains("Spotify", StringComparison.InvariantCultureIgnoreCase))
            {
                return "No track is playing";
            }

            return proc.MainWindowTitle;
        }
    }
}