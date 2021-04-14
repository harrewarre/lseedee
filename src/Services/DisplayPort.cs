using System.IO.Ports;
using LSeeDee.Options;
using LSeeDee.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LSeeDee
{
    public class DisplayPort
    {
        private readonly DisplayOptions _displayOptions;
        private readonly ILogger<DisplayPort> _logger;
        private readonly SerialPort _port;

        public DisplayPort(IOptions<DisplayOptions> displayOptions, ILogger<DisplayPort> logger)
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

            _logger.LogInformation($"DisplayPort ready on port {_displayOptions.Port} @ {_displayOptions.Speed} baud");
        }

        public void Write(byte[] rawData)
        {
            _port.Write(rawData, 0, rawData.Length);
        }

        public void SendCommand(Command command)
        {
            Write(new byte[] { (byte)command });
        }

        public void WriteText(string text)
        {
            _port.Write(text);
        }
    }
}