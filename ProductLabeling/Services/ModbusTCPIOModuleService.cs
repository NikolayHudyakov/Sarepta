using NModbus;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using static ProductLabeling.Services.Intefaces.IModbusTcpIoModuleService;

namespace ProductLabeling.Services
{
    internal class ModbusTcpIoModuleService : IModbusTcpIoModuleService
    {
        private const int PingTimeout = 1000;
        private const int ErrorTimeout = 1000;
        private const int ConnStatusTimeout = 1000;

        private bool _startStopFlag;
        private Thread? _connectionStatusThread;
        private Thread? _readDIThread;
        private Thread? _readEncoderThread;
        private TcpClient? _tcpClient;
        private IModbusMaster? _modbusMaster;
        private Task<double>? _writeMultipleDOTask;
        private Task<double>? _writeSingleDOTask;
        private bool _connected;

        public ModbusTcpIoModuleService() { }

        public ModbusTcpIoModuleDto? Dto { get; set; }

        public bool Connected => _connected;

        public event Action<bool>? Status;
        public event Action<string>? Error;
        public event ReadDIEventHendler? ReadDI;
        public event ReadEncoderEventHendler? ReadEncoder;

        public async Task StartAsync() =>
            await Task.Run(Start);
        public async Task StopAsync(ushort startAddressDo, int countDo) =>
            await Task.Run(() => Stop(startAddressDo, countDo));

        public async Task<double> WriteSingleDOAsync(ushort coilAddress, bool value) =>
            await (_writeSingleDOTask = _WriteSingleDOAsync(coilAddress, value));

        public async Task<double> WriteMultipleDOAsync(ushort startAddress, bool[] data) =>
            await (_writeMultipleDOTask = _WriteMultipleDOAsync(startAddress, data));

        private void Start()
        {
            if (!_startStopFlag)
            {
                _startStopFlag = true;

                _connectionStatusThread = new Thread(ConnectionStatusCycle);
                _connectionStatusThread.Start();

                _readDIThread = new Thread(ReadDICycle);
                _readDIThread.Start();

                _readEncoderThread = new Thread(ReadEncoderCycle);
                _readEncoderThread.Start();
            }
        }

        private void Stop(ushort startAddressDo, int countDo)
        {
            if (_startStopFlag)
            {
                _startStopFlag = false;

                _writeSingleDOTask?.Wait();
                _writeMultipleDOTask?.Wait();
                _connectionStatusThread?.Join();
                _readDIThread?.Join();
                _readEncoderThread?.Join();

                _WriteMultipleDOAsync(startAddressDo, new bool[countDo]).Wait();
                
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

                if (_connected = TcpClientConnected())
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
                    _modbusMaster = new ModbusFactory().CreateMaster(_tcpClient);
                }
                catch (Exception ex)
                {
                    Error?.Invoke(ex.Message);
                    _tcpClient?.Close();
                    Thread.Sleep(ErrorTimeout);
                }
            }
        }

        private void ReadDICycle()
        {
            var changeFlag = false;
            DateTime dateTime;
            bool[]? _previousInputs = null;
            bool[]? _leadingEdgeInputs = null;
            bool[]? _trailingEdgeInputs = null;
            bool[]? inputs;

            while (_startStopFlag)
            {
                if (Dto == null || !_connected || Dto.DiNumberOfPoints == 0)
                {
                    Thread.Sleep(ErrorTimeout);
                    continue;
                }
                    
                dateTime = DateTime.Now;
                _previousInputs ??= new bool[Dto.DiNumberOfPoints];
                _leadingEdgeInputs ??= new bool[Dto.DiNumberOfPoints];
                _trailingEdgeInputs ??= new bool[Dto.DiNumberOfPoints];
                try
                {
                    inputs = _modbusMaster?.ReadInputs(Dto!.SlaveAddress, Dto.DiStartAddress, Dto.DiNumberOfPoints);

                    for (int i = 0; i < Dto.DiNumberOfPoints; i++)
                    {
                        _leadingEdgeInputs[i] = false;
                        _trailingEdgeInputs[i] = false;
                        if (inputs![i] != _previousInputs[i])
                        {
                            if (inputs![i])
                                _leadingEdgeInputs[i] = true;
                            else
                                _trailingEdgeInputs[i] = true;
                            changeFlag = true;
                        }
                        _previousInputs[i] = inputs![i];
                    }
                    if (changeFlag)
                    {
                        ReadDI?.Invoke(_leadingEdgeInputs, _trailingEdgeInputs, (DateTime.Now - dateTime).TotalMilliseconds);
                        changeFlag = false;
                    }  
                }
                catch (Exception ex)
                {
                    Error?.Invoke(ex.Message);
                    Thread.Sleep(ErrorTimeout);
                }
                _writeSingleDOTask?.Wait();
                _writeMultipleDOTask?.Wait();
            }
        }

        private void ReadEncoderCycle()
        {
            ushort[]? inputRegisters;

            while (_startStopFlag)
            {
                if (Dto == null || !_connected || Dto.EncoderNumberOfPoints == 0)
                {
                    Thread.Sleep(ErrorTimeout);
                    continue;
                }

                try
                {
                    inputRegisters = _modbusMaster?.ReadInputRegisters(Dto.SlaveAddress, Dto.EncoderStartAddress, Dto.EncoderNumberOfPoints);

                    ReadEncoder?.Invoke(inputRegisters!);

                    Thread.Sleep(Dto.EncoderPollingPeriod);
                }
                catch (Exception ex)
                {
                    Error?.Invoke(ex.Message);
                    Thread.Sleep(ErrorTimeout);
                }
            }
        }

        private async Task<double> _WriteSingleDOAsync(ushort coilAddress, bool value)
        {
            if (Dto == null || !_connected)
                return 0;

            try
            {
                var dateTime = DateTime.Now;
                await _modbusMaster!.WriteSingleCoilAsync(Dto!.SlaveAddress, coilAddress, value);
                return (DateTime.Now - dateTime).TotalMicroseconds;
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex.Message);
                return 0;
            }
        }

        private async Task<double> _WriteMultipleDOAsync(ushort startAddress, bool[] data)
        {
            if (Dto == null || !_connected)
                return 0;

            try
            {
                var dateTime = DateTime.Now;
                await _modbusMaster!.WriteMultipleCoilsAsync(Dto!.SlaveAddress, startAddress, data);
                return (DateTime.Now - dateTime).TotalMicroseconds;
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex.Message);
                return 0;
            }
        }

        private bool TcpClientConnected()
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
