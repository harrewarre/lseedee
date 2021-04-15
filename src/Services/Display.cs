using System.Linq;
using System.Text;
using LSeeDee.Options;
using LSeeDee.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LSeeDee.Services
{
    public class Display
    {
        private readonly ILogger<Display> _logger;
        private readonly DisplayPort _port;

        public Display(DisplayPort port, ILogger<Display> logger)
        {
            _logger = logger;
            _port = port;

            _port.SendCommand(Command.ClearDisplay);
            _port.SendCommand(Command.HideCursor);
            _port.SendCommand(Command.WrapOff);
        }

        public void WriteText(int line, int col, string text)
        {
            if (col < 0)
            {
                _logger.LogError($"Cannot write to positing {col}, value out of bounds");
                return;
            }

            if (line > 3)
            {
                _logger.LogError($"Cannot write to line {line}, value out of bounds!");
                return;
            }

            var data = new byte[] { (byte)Command.SetCursorPosition, (byte)col, (byte)line }
                .Concat(Encoding.UTF8.GetBytes(text))
                .ToArray();

            _port.Write(data);
        }
    }
}