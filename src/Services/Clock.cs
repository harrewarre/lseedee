using System;
using System.Timers;
using LSeeDee.Services;
using Microsoft.Extensions.Logging;

namespace LSeeDee
{
    public class Clock
    {
        private readonly Display _display;
        private readonly Timer _timer;

        public Clock(Display display)
        {
            _display = display;
            _timer = new Timer(1000);

            _timer.Elapsed += (sender, args) =>
            {
                WriteNow();
            };

            _timer.Enabled = true;
            _timer.AutoReset = true;

            WriteNow();
        }

        private void WriteNow()
        {
            var now = DateTime.Now;
            _display.WriteText(0, 0, now.ToString("yyyy-MM-dd"));
            _display.WriteText(0, 12, now.ToString("HH:mm:ss"));
        }
    }
}