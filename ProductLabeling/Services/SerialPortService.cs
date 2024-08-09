using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ProductLabeling.Services.Intefaces.ISerialPortService;

namespace ProductLabeling.Services
{
    internal class SerialPortService : ISerialPortService
    {
        private const int ErrorTimeout = 1000;
        private const int ConnStatusTimeout = 1000;
        private const int DataBits = 8;
        private const int ReadTimeOut = 100;

        private bool _startStopFlag;
        private Thread? _connectionStatusThread;
        private SerialPort? _serialPort;
        private bool _connected;
        
        public SerialPortDto? Dto { get; set; }

        public bool Connected => _connected;

        public event Action<bool>? Status;
        public event Action<string>? Error;
        public event DataReceiveEventHandler? DataReceive;

        public async Task StartAsync() => await Task.Run(Start);

        public async Task StopAsync() => await Task.Run(Stop);

        public Task SendDataAsync(string data)
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            if (!_startStopFlag)
            {
                _startStopFlag = true;

                _connectionStatusThread = new Thread(ConnectionStatusCycle);
                _connectionStatusThread.Start();
            }
        }

        private void ConnectionStatusCycle()
        {
            while (_startStopFlag)
            {
                if (Dto == null)
                {
                    Thread.Sleep(ErrorTimeout);
                    continue;
                }

                if (_connected = (_serialPort != null && _serialPort!.IsOpen))
                {
                    Status?.Invoke(true);
                    Thread.Sleep(ConnStatusTimeout);
                    continue;
                }

                Status?.Invoke(false);
                try
                {
                    _serialPort = new SerialPort()
                    {
                        PortName = Dto.PortName!,
                        BaudRate = Dto.Baudrate,
                        Parity = Parity.None,
                        DataBits = DataBits,
                        StopBits = StopBits.One
                    };
                    _serialPort.DataReceived += SerialPortDataReceived;

                    _serialPort.Open();
                }
                catch (Exception ex)
                {
                    Error?.Invoke(ex.Message);
                    Thread.Sleep(ErrorTimeout);
                }
            }
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            StringBuilder stringBuilder = new();

            Thread.Sleep(ReadTimeOut);

            try
            {
                while (_startStopFlag)
                {
                    stringBuilder.Append(_serialPort!.ReadExisting());
                    
                    if (_serialPort.BytesToRead == 0) break;
                }

                DataReceive?.Invoke(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex.Message);
            }
        }

        private void Stop()
        {
            if (_startStopFlag)
            {
                _startStopFlag = false;

                _connectionStatusThread?.Join();

                _serialPort?.Close();

                Status?.Invoke(false);
            }
        }
    }
}
