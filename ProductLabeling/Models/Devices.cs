using ProductLabeling.DataBase.Interfases;
using ProductLabeling.Models.Interfaces;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductLabeling.Models
{
    internal class Devices : AppSettingsProvider, IDevices
    {
        #region IoModule
        private const ushort DO_0 = 0;
        private const ushort DO_1 = 1;
        private const ushort DO_2 = 2;
        private const ushort DO_3 = 3;
        private const ushort DO_4 = 4;
        private const ushort DO_5 = 5;
        private const ushort DO_6 = 6;
        private const ushort DO_7 = 7;
        private const ushort DO_8 = 8;
        private const ushort DO_9 = 9;
        private const ushort DO_10 = 10;
        private const ushort DO_11 = 11;
        private const ushort DO_12 = 12;
        private const ushort DO_13 = 13;
        private const ushort DO_14 = 14;
        private const ushort DO_15 = 15;

        private const ushort DI_0 = 0;
        private const ushort DI_1 = 1;
        private const ushort DI_2 = 2;
        private const ushort DI_3 = 3;
        private const ushort DI_4 = 4;
        private const ushort DI_5 = 5;
        private const ushort DI_6 = 6;
        private const ushort DI_7 = 7;
        private const ushort DI_8 = 8;
        private const ushort DI_9 = 9;
        private const ushort DI_10 = 10;
        private const ushort DI_11 = 11;
        private const ushort DI_12 = 12;
        private const ushort DI_13 = 13;
        private const ushort DI_14 = 14;
        private const ushort DI_15 = 15;

        #endregion

        private const int PeriodViewVelosity = 500;

        private readonly IModbusTcpIoModuleService _ioModule;
        private readonly ITcpClientService _codeReader;
        private readonly ISerialPortService _handCodeReader;
        private readonly IDataBase _dataBase;
        private readonly IRejecterService _rejecter;

        private readonly TrafficLight _trafficLight;

        private double _velosity;
        private CancellationTokenSource? _cancellationTokenSource;

        public Devices(ISerializerService<AppSettingsDto> appSettings, IModbusTcpIoModuleService ioModule,
            ITcpClientService codeReader, ISerialPortService handCodeReader, IDbService db, IRejecterService rejecter) : base(appSettings)
        {
            _ioModule = ioModule;
            _codeReader = codeReader;
            _handCodeReader = handCodeReader;
            _dataBase = db.DataBase;
            _rejecter = rejecter;

            _ioModule.Dto = dto.IoModule;
            _codeReader.Dto = dto.FirstStage.AutoReading.CodeReader;
            _handCodeReader.Dto = dto.FirstStage.HandReading.HandCodeReader;

            _trafficLight = new TrafficLight()
            {
                Red = (value) => _ioModule.WriteSingleDOAsync(DO_2, value),
                Yellow = (value) => _ioModule.WriteSingleDOAsync(DO_3, value),
                Green = (value) => _ioModule.WriteSingleDOAsync(DO_4, value),
                Buzzer = (value) => _ioModule.WriteSingleDOAsync(DO_5, value)
            };
        }

        public event Action<double>? Velosity;

        public IModbusTcpIoModuleService IoModule => _ioModule;
        public ITcpClientService CodeReader => _codeReader;
        public ISerialPortService HandCodeReader => _handCodeReader;
        public IRejecterService Rejecter => _rejecter;
        public IDataBase DataBase => _dataBase;
        public TrafficLight TrafficLight => _trafficLight;

        #region IoModule
        public void SubReadEncoderAsync()
        {
            _ioModule.ReadEncoder += IoReadEncoder;

            _cancellationTokenSource = new();
            CancellationToken token = _cancellationTokenSource.Token;

            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Velosity?.Invoke(_velosity);
                    Thread.Sleep(PeriodViewVelosity);
                }
            });
        }

        public void UnSubReadEncoderAsync()
        {
            _ioModule.ReadEncoder -= IoReadEncoder;

            _cancellationTokenSource?.Cancel();
        }

        private void IoReadEncoder(ushort[] buffer)
        {
            float rpm = UshortArrayToFloat(buffer) / dto.FirstStage.AutoReading.EncoderResolution;
            _velosity = Math.Abs(RpmToVelocity(rpm, dto.FirstStage.AutoReading.WheelDiameter));
            _rejecter.Velocity = _velosity;
        }

        private float UshortArrayToFloat(ushort[] buffer) =>
            BitConverter.ToSingle(new byte[4]
            {
                (byte)(buffer[1] & 0xFF),
                (byte)(buffer[1] >> 8),
                (byte)(buffer[0] & 0xFF),
                (byte)(buffer[0] >> 8)
            }, 0);

        private double RpmToVelocity(float rpm, int diameter) => rpm / 60 * Math.PI * diameter / 1000;
        #endregion
    }
}
