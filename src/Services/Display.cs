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

            SendCommand(Command.ClearDisplay);
            SendCommand(Command.HideCursor);
        }

        private void WriteToScreen(byte[] data)
        {
            _port.Write(data);
        }

        private void SendCommand(Command command)
        {
            _port.SendCommand(command);
        }

        private void WriteByte(byte data)
        {
            _port.Write(new byte[] { data });
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
            _port.WriteText(text);
        }

        public void SetCursorPosition(int line, int col)
        {
            WriteToScreen(new byte[] { (byte)Command.SetCursorPosition, (byte)col, (byte)line });
        }
    }
}