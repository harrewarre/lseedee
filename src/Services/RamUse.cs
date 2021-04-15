using System;
using System.Diagnostics;
using System.Timers;
using LSeeDee.Services;

namespace LSeeDee
{
    public class RamUse
    {
        private readonly Display _display;
        private readonly PerformanceCounter _counter;

        public RamUse(Display display)
        {
            _display = display;
            _counter = new PerformanceCounter("Memory", "Available MBytes");

            var timer = new Timer(1000);

            timer.Elapsed += (sender, args) =>
            {
                WriteRamUse();
            };

            timer.Enabled = true;
            timer.AutoReset = true;

            WriteRamUse();
        }

        private void WriteRamUse()
        {
            _display.WriteText(2, 0, GetRamUse());
        }

        private string GetRamUse()
        {
            var total = 32000;
            var percent = Math.Abs(100 - ((_counter.NextValue() / total) * 100));
            return $"RAM : {percent.ToString("0.00")}%   ";
        }
    }
}