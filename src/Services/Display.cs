using System.IO.Ports;
using System.Text;
using LSeeDee.Options;
using LSeeDee.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LSeeDee.Services
{
    public class Display
    {
        private readonly DisplayOptions _displayOptions;
        private readonly ILogger<Display> _logger;

        private readonly SerialPort _port;

        public Display(IOptions<DisplayOptions> displayOptions, ILogger<Display> logger)
        {
            _displayOptions = displayOptions.Value;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(_displayOptions.Port))
            {
                _logger.LogCritical("Display port value is required!");
                return;
            }

            if (_displayOptions.Speed == 0)
            {
                _logger.LogCritical("Display speed is required!");
                return;
            }

            _port = new SerialPort(_displayOptions.Port, _displayOptions.Speed);
            _port.Open();

            WriteCommand(Command.ClearDisplay);
            WriteCommand(Command.HideCursor);

            _logger.LogInformation($"Display ready on port {_displayOptions.Port} @ {_displayOptions.Speed} baud");
        }

        private void WriteToScreen(byte[] data)
        {
            _port.Write(data, 0, data.Length);
        }

        private void WriteCommand(Command command)
        {
            WriteToScreen(new byte[] { (byte)command });
        }

        private void WriteByte(byte data)
        {
            WriteToScreen(new byte[] { data });
        }

        public void WriteText(int line, int col, string text, bool scrollLine)
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

            if (scrollLine)
            {
                // enable scroll
            }
            else
            {
                //disable scroll
            }

            SetCursorPosition(line, col);
            _port.Write(text);
        }

        public void SetCursorPosition(int line, int col)
        {
            WriteToScreen(new byte[] { (byte)Command.SetCursorPosition, (byte)col, (byte)line });
        }
    }
}