using System;
using LSeeDee.Services;

namespace LSeeDee.Screens
{
    public class TimeScreen : IScreen
    {
        private readonly Display _display;

        public TimeScreen(Display display)
        {
            _display = display;
        }

        public void Draw()
        {
            var now = DateTime.Now;
            _display.WriteText(1, 5, now.ToString("dd-MM-yyyy"));
            _display.WriteText(2, 6, now.ToString("HH:mm:ss"));
        }

        public bool IsEnabled()
        {
            return true;
        }
    }
}