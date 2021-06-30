using System.Collections.Generic;
using System.Timers;
using System.Linq;
using LSeeDee.Screens;

namespace LSeeDee.Services
{
    public class Renderer
    {
        private static readonly int SCREEN_DISPLAYTIME_MS = 10000;
        private static readonly int SCREEN_REFRESHINTERVAL_MS = 1000;

        private readonly List<IScreen> _screens;
        private IScreen _currentScreen = null;
        private readonly DisplayPort _port;

        public Renderer(IEnumerable<IScreen> screens, DisplayPort port)
        {
            _screens = screens.ToList();
            _currentScreen = screens.First();

            _port = port;
        }

        public void Start()
        {
            var drawTimer = new Timer(SCREEN_REFRESHINTERVAL_MS);
            var screenTicks = 0;

            drawTimer.Elapsed += (sender, args) =>
            {
                _port.SendCommand(Types.Command.ClearDisplayAndEraseData);
                _currentScreen.Draw();
                screenTicks += SCREEN_REFRESHINTERVAL_MS;

                if (screenTicks >= SCREEN_DISPLAYTIME_MS)
                {
                    _currentScreen = GetNextScreen();
                    screenTicks = 0;
                }
            };

            drawTimer.Enabled = true;
            drawTimer.AutoReset = true;
        }

        private IScreen GetNextScreen()
        {
            if (_currentScreen == null)
            {
                return _screens.First();
            }

            if (_currentScreen == _screens.Last())
            {
                return _screens.First();
            }

            var index = _screens.IndexOf(_currentScreen);
            index++;

            return _screens[index];
        }
    }
}