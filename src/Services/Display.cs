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
            SendCommand(Command.WrapOff);
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

        public void WriteText(int line, int col, string text)
        {
            _port.SendCommand(Command.ScrollText);
            _port.Write(new byte[] { (byte)255, (byte)1, (byte)65 });

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

            SetCursorPosition(line, col);
            _port.WriteText(text);
        }

        public void WriteScrollingText(int line, string text)
        {
            _port.SendCommand(Command.ScrollText);
            _port.Write(new byte[] { (byte)255, (byte)1, (byte)65 });

            SetCursorPosition(line, 0);

            if (text.Length < 20)
            {
                _port.WriteText(text);
                return;
            }

            var visibleCharacters = text.Substring(0, 20);
            var hiddenCharacters = text.Substring(20);

            if (hiddenCharacters.Length > 20)
            {
                hiddenCharacters = hiddenCharacters.Substring(0, 19);
            }
            else
            {
                hiddenCharacters.PadRight(20, ' ');
            }

            _port.WriteText(visibleCharacters);

            for (int i = 0; i < hiddenCharacters.Length - 1; i++)
            {
                _port.SendCommand(Command.SetHiddenCharacter);
                _port.Write((byte)i);
                _port.WriteText(hiddenCharacters[i].ToString());
            }

            _port.SendCommand(Command.ScrollText);
            _port.Write(new byte[] { (byte)line, (byte)1, (byte)35 });
        }
        public void SetCursorPosition(int line, int col)
        {
            _port.SendCommand(Command.SetCursorPosition);
            _port.Write(new byte[] { (byte)col, (byte)line });
        }
    }
}