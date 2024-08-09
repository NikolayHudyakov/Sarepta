using ProductLabeling.DataBase.Interfases;
using ProductLabeling.Services.Intefaces;
using System;

namespace ProductLabeling.Models.Interfaces
{
    internal interface IDevices
    {
        public event Action<double>? Velosity;

        public IModbusTcpIoModuleService IoModule { get; }
        public ITcpClientService CodeReader { get; }
        public ISerialPortService HandCodeReader { get; }
        public IRejecterService Rejecter { get; }
        public IDataBase DataBase { get; }
        public TrafficLight TrafficLight { get; }

        public void SubReadEncoderAsync();
        public void UnSubReadEncoderAsync();
    }
}
