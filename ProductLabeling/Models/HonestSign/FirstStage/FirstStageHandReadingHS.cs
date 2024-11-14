using Microsoft.Extensions.Logging;
using ProductLabeling.DataBase.Interfases;
using ProductLabeling.Logging;
using ProductLabeling.Models.Interfaces;
using ProductLabeling.Models.Interfaces.FirstStage;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProductLabeling.Models.HonestSign.FirstStage
{
    internal class FirstStageHandReadingHS : Stage, IFirstStageHandReading
    {
        private readonly ISerialPortService _codeReader;
        private readonly IDataBase _dataBase;
        private readonly ILogger _logger;

        private readonly Stopwatch _timerDb = new();

        public FirstStageHandReadingHS(ISerializerService<AppSettingsDto> appSettings, IDevices devices, ILogger<FirstStageHandReadingHS> logger) : base(appSettings, devices, logger)
        {
            _codeReader = devices.HandCodeReader;
            _dataBase = devices.DataBase;
            _logger = logger;
            _codeReader.DataReceive += CodeReaderDataReceiveAsync;
        }

        public Product? SelectedProduct { get; set; }

        public int HandCodeReaderSelectedMode { get; set; }

        public ModelName Model => ModelName.HonestSign;

        public event Action<bool>? Status;
        public event Action<string>? Message;
        public event Action<string>? CodeReaderData;
        public event Action<double>? TimeDb;

        #region CodeReader
        private async void CodeReaderDataReceiveAsync(string data)
        {
            string[] codes = data.Trim().Split("\r\n");

            foreach (string code in codes)
            {
                CodeReaderData?.Invoke(code);

                await Task.Run(() =>
                {
                    string error = string.Empty;

                    switch (HandCodeReaderSelectedMode)
                    {
                        case 1:
                            error = AddCode(code);

                            if (error == string.Empty)
                            {
                                Message?.Invoke("Код записан в базу данных");
                                Status?.Invoke(true);
                                Counter.Ok++;
                                return;
                            }
                            Message?.Invoke(error);
                            Status?.Invoke(false);
                            _logger.Error(error);
                            break;

                        case 0:
                            error = RemoveCode(code);

                            if (error == string.Empty)
                            {
                                Message?.Invoke("Код удален из базы данных");
                                Status?.Invoke(true);
                                Counter.Ng++;
                                return;
                            }
                            Message?.Invoke(error);
                            Status?.Invoke(false);
                            _logger.Error(error);
                            break;
                    }
                });
            }
        }

        private string AddCode(string data)
        {
            if (SelectedProduct == null)
                return "Продукт не выбран";

            if (!data.Contains(SelectedProduct.Gtin))
                return "Не соответствие Gtin";

            _timerDb.Restart();

            var res = _dataBase.ExecuteSqlRaw(
                """
                INSERT INTO codes (dtime_ins, code, status, dtime_status)
                VALUES ({0}, {1}, {2}, {3});
                """,
                DateTime.Now, data, Status1, DateTime.Now);

            _timerDb.Stop();
            TimeDb?.Invoke(_timerDb.Elapsed.TotalMilliseconds);

            if (res == 0)
                return "Код не был записан в базу данных";

            if (res > 1)
                return $"Колличество новых записей: {res}";

            return string.Empty;
        }

        private string RemoveCode(string data)
        {
            if (SelectedProduct == null)
                return "Продукт не выбран";

            if (!data.Contains(SelectedProduct.Gtin))
                return "Не соответствие Gtin";

            _timerDb.Restart();

            var res = _dataBase.ExecuteSqlRaw(
                """
                DELETE FROM codes
                WHERE code = {0};
                """,
                data);

            _timerDb.Stop();
            TimeDb?.Invoke(_timerDb.Elapsed.TotalMilliseconds);

            if (res == 0)
                return "Кода нет в базе данных или ошибка удаления";

            if (res > 1)
                return $"Колличество удаленых записей: {res}";

            return string.Empty;
        }
        #endregion

        ~FirstStageHandReadingHS() => _codeReader.DataReceive -= CodeReaderDataReceiveAsync;
    }
}
