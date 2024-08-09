using ProductLabeling.Services.Dto;
using System;
using System.Threading.Tasks;

namespace ProductLabeling.Services.Intefaces
{
    internal interface ISerialPortService
    {
        public SerialPortDto? Dto { get; set; }

        public event Action<bool>? Status;

        public event Action<string>? Error;

        public delegate void DataReceiveEventHandler(string data);
        public event DataReceiveEventHandler? DataReceive;

        public bool Connected { get; }

        public Task StartAsync();

        public Task StopAsync();

        public Task SendDataAsync(string data);
    }
}
