using System.Diagnostics;
using System.Timers;
using LSeeDee.Services;

namespace LSeeDee
{
    public class CpuTime
    {
        private readonly Display _display;
        private readonly PerformanceCounter _counter;

        public CpuTime(Display display)
        {
            _display = display;
            _counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            var timer = new Timer(1000);

            timer.Elapsed += (sender, args) =>
            {
                WriteCpuTime();
            };

            timer.Enabled = true;
            timer.AutoReset = true;

            WriteCpuTime();
        }

        private void WriteCpuTime()
        {
            _display.WriteText(1, 0, GetCpuTime());
        }

        private string GetCpuTime()
        {
            return $"CPU : {_counter.NextValue().ToString("0.00")}%   ";
        }
    }
}