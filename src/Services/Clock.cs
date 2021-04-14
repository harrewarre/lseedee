using System;
using System.Timers;
using LSeeDee.Services;
using Microsoft.Extensions.Logging;

namespace LSeeDee
{
    public class Clock
    {
        private readonly ILogger<Clock> _logger;

        private readonly Display _display;
        private readonly Timer _timer;

        public Clock(Display display, ILogger<Clock> logger)
        {
            _logger = logger;

            _display = display;
            _timer = new Timer(1000);

            _timer.Elapsed += (sender, args) =>
            {
                WriteNow();
            };

            _timer.Enabled = true;
            _timer.AutoReset = true;

            WriteNow();

            _logger.LogInformation("Clock started!");
        }

        private void WriteNow()
        {
            var now = DateTime.Now;
            _display.WriteText(0, 0, now.ToString("yyyy-MM-dd"), false);
            _display.WriteText(0, 15, now.ToString("HH:mm"), false);

            _logger.LogInformation($"Tick {now}");
        }
    }
}