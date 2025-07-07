using Microsoft.Extensions.Logging;
using ProductLabeling.DataBase.Interfases;
using ProductLabeling.Logging;
using ProductLabeling.Models.Dto.FirstStage;
using ProductLabeling.Models.Interfaces;
using ProductLabeling.Models.Interfaces.FirstStage;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ProductLabeling.Models.HonestSign.FirstStage
{
    internal class FirstStageAutoReadingHS : Stage, IFirstStageAutoReading
    {
        private readonly IModbusTcpIoModuleService _ioModule;
        private readonly ITcpClientService _codeReader;
        private readonly IDataBase _dataBase;
        private readonly IRejecterService _rejecter;
        private readonly TrafficLight _trafficLight;
        private readonly FirstStageAutoReadingDto _dto;
        private readonly EventWaitHandle _receiveCodeReaderDataEwh = new AutoResetEvent(false);
        private readonly object _lockObjWaitGetData = new();
        private readonly Stopwatch _timerCodeReader = new();
        private readonly Stopwatch _timerDb = new();
        private string _codeReaderData = string.Empty;
        private int _rejectedCount;

        public FirstStageAutoReadingHS(ISerializerService<AppSettingsDto> appSettings, IDevices devices, ILogger<FirstStageAutoReadingHS> logger) : base(appSettings, devices, logger)
        {
            _ioModule = devices.IoModule;
            _codeReader = devices.CodeReader;
            _dataBase = devices.DataBase;
            _rejecter = devices.Rejecter;
            _trafficLight = devices.TrafficLight;

            _dto = dto.FirstStage.AutoReading;

            _ioModule.ReadDI += IoModuleReadDI;
            _codeReader.DataReceive += CodeReaderDataReceive;
        }

        public ModelName Model => ModelName.HonestSign;

        public Product? SelectedProduct { get; set; }

        public event Action<bool>? Status;
        public event Action<string>? Message;
        public event Action<string>? CodeReaderData;
        public event Action<double>? TimeDI;
        public event Action<double>? TimeCodeReader;
        public event Action<double>? TimeDb;

        #region IoModule
        private void IoModuleReadDI(bool[] leadingEdgeInputs, bool[] trailingEdgeInputs, double timeMillisecond)
        {
            if (leadingEdgeInputs[DI_0])
            {
                ExecuteStage();
                TimeDI?.Invoke(timeMillisecond);
            }
        }
        #endregion

        #region CodeReader
        private void CodeReaderDataReceive(string data)
        {
            _codeReaderData = data;
            _receiveCodeReaderDataEwh.Set();
        }
        #endregion

        #region Stage
        private void ExecuteStage()
        {
            long id = DateTime.Now.Ticks;

            WriteDOPulseAsync(DO_0, _dto.CodeReaderTriggerDuration);

            switch (_dto.RejecterMode)
            {
                case 0: _rejecter.RejectByDistanceAsync(id, _dto.RejecterDistance, _dto.RejecterIsEnable, Reject); break;
                case 1: _rejecter.RejectByDelayAsync(id, _dto.RejecterDelay, _dto.RejecterIsEnable, Reject); break;
            }

            ProcessingAsync(id);

            Counter.Total++;
        }

        private void Reject() => WriteDOPulseAsync(DO_5, _dto.RejecterDuration);

        private async void ProcessingAsync(long id)
        {
            string error = await Task.Run(Processing);

            if (error == string.Empty)
            {
                await _rejecter.UpdateAsync(id);
                Message?.Invoke("Запись добавлена");
                Status?.Invoke(true);

                Counter.Ok++;
                _rejectedCount = 0;
                return;
            }

            logger.Warn(error);
            logger.Warn($"CodeReader {_timerCodeReader.Elapsed.TotalMilliseconds} ms");
            logger.Warn($"DataBase {_timerDb.Elapsed.TotalMilliseconds} ms");
            Message?.Invoke(error);
            Status?.Invoke(false);

            Counter.Ng++;

            _trafficLight.LightYellow(_dto.YellowDuration);

            _rejectedCount++;

            if (_rejectedCount >= _dto.StopLineCount)
            {
                WriteDOPulseAsync(DO_6, _dto.StopLinePulseDuration);
                _trafficLight.OnBuzzer(_dto.StopLinePulseDuration);
                _rejectedCount = 0; 
            }
        }

        private string Processing()
        {
            string? code = GetData();

            CodeReaderData?.Invoke($"{code}");

            if (code == null)
                return "Таймаут получения данных";

            if (code == _dto.CodeReaderNoreadString)
                return "Код не считан";

            Counter.Read++;

            if (SelectedProduct == null)
                return "Продукт не выбран";

            if (!code.Contains(SelectedProduct.Gtin))
                return "Не соответствие Gtin";

            var dateTimeNow = GetDateTimeNow();
            var dateNowForBatch = GetDateNow();

            _timerDb.Restart();

            var res = _dataBase.ExecuteSqlRaw(
                """
                INSERT INTO codes (dtime_ins, code, status, dtime_status, production_date)
                VALUES ({0}, {1}, {2}, {3}, {4});
                """,
                DateTime.Now, code, Status1, DateTime.Now, DateTime.Now);

            _timerDb.Stop();
            TimeDb?.Invoke(_timerDb.Elapsed.TotalMilliseconds);

            if (res == 0)
                return "Код не был записан в базу данных";

            if (res > 1)
                return $"Колличество новых записей: {res}";

            return string.Empty;
        }

        private string? GetData()
        {
            lock (_lockObjWaitGetData)
            {
                _timerCodeReader.Restart();

                _receiveCodeReaderDataEwh.Reset();
                bool signalState = _receiveCodeReaderDataEwh.WaitOne(_dto.CodeReaderRecieveTimeout);

                _timerCodeReader.Stop();
                TimeCodeReader?.Invoke(_timerCodeReader.Elapsed.TotalMilliseconds);

                return signalState ? _codeReaderData : null;
            }
        }
        #endregion

        ~FirstStageAutoReadingHS()
        {
            _ioModule.ReadDI -= IoModuleReadDI;
            _codeReader.DataReceive -= CodeReaderDataReceive;
        }

    }
}
