using System.IO.Ports;
using LSeeDee.Options;
using LSeeDee.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LSeeDee
{
    public class DisplayPort
    {
        private readonly DisplayOptions _displayOptions;
        private readonly ILogger<DisplayPort> _logger;
        private readonly SerialPort _port;

        private readonly BlockingCollection<byte[]> _outputQueue = new BlockingCollection<byte[]>();

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

            Task.Factory.StartNew(() => OutputProcessor(), TaskCreationOptions.LongRunning);
        }

        public void Write(byte[] rawData)
        {
            SendData(rawData);
        }

        public void SendCommand(Command command)
        {
            Write(new byte[] { (byte)command });
        }

        private void SendData(byte[] data)
        {
            _outputQueue.Add(data);
        }

        private void OutputProcessor()
        {
            while (_outputQueue.TryTake(out var data, Timeout.Infinite))
            {
                _port.Write(data, 0, data.Length);
            }
        }
    }
}