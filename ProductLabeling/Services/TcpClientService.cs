using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ProductLabeling.Services.Intefaces.ITcpClientService;

namespace ProductLabeling.Services
{
    internal class TcpClientService : ITcpClientService
    {
        private const int PingTimeout = 1000;
        private const int ErrorTimeout = 1000;
        private const int ConnStatusTimeout = 1000;

        private bool _startStopFlag;
        private Thread? _connectionStatusThread;
        private Thread? _receiveThread;
        private TcpClient? _tcpClient;
        private bool _connected;
        private NetworkStream? _networkStream;

        public TcpClientService() { }

        public TcpClientDto? Dto { get; set; }

        public bool Connected => _connected;

        public event Action<bool>? Status;
        public event Action<string>? Error;
        public event DataReceiveEventHandler? DataReceive;

        public async Task StartAsync() => await Task.Run(Start);

        public async Task StopAsync() => await Task.Run(Stop);

        public async Task SendDataAsync(string data)
        {
            if (Dto == null || !_connected)
                return;

            try
            {
                 await _networkStream?.WriteAsync(Encoding.UTF8.GetBytes(data)).AsTask()!;
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex.Message);
                Thread.Sleep(ErrorTimeout);
            }
        }

        private void Start()
        {
            if (!_startStopFlag)
            {
                _startStopFlag = true;

                _connectionStatusThread = new Thread(ConnectionStatusCycle);
                _connectionStatusThread.Start();

                _receiveThread = new Thread(ReceiveCycle);
                _receiveThread.Start();
            }
        }

        private void Stop()
        {
            if (_startStopFlag)
            {
                _startStopFlag = false;

                _connectionStatusThread?.Join();
                _tcpClient?.Close();

                Status?.Invoke(false);
                _connected = false;
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
                    
                if (_connected = GetConnected())
                {
                    Status?.Invoke(true);
                    Thread.Sleep(ConnStatusTimeout);
                    continue;
                }

                Status?.Invoke(false);
                _tcpClient = new TcpClient();

                try
                {
                    _tcpClient.Connect(Dto.IPAddress!, Dto.Port);
                    _networkStream = _tcpClient.GetStream();
                }
                catch (Exception ex)
                {
                    Error?.Invoke(ex.Message);
                    _tcpClient?.Close();
                    Thread.Sleep(ErrorTimeout);
                }
            }
        }

        private void ReceiveCycle()
        {
            byte[]? data = null;

            while (_startStopFlag)
            {
                if (Dto == null || !_connected)
                {
                    Thread.Sleep(ErrorTimeout);
                    continue;
                }
                  
                data ??= new byte[_tcpClient!.ReceiveBufferSize];
                try
                {
                    int bytesCount = _networkStream!.Read(data, 0, data.Length);
                    
                    DataReceive?.Invoke(Encoding.UTF8.GetString(data, 0, bytesCount));
                }
                catch (Exception ex)
                {
                    if (_startStopFlag)
                        Error?.Invoke(ex.Message);
                    Thread.Sleep(ErrorTimeout);
                }
                
            }
        }

        private bool GetConnected()
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    return ping.Send(Dto!.IPAddress!, PingTimeout).Status == IPStatus.Success &&
                        _tcpClient != null && _tcpClient!.Connected;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
