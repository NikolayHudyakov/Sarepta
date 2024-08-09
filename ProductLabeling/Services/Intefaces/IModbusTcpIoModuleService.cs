using ProductLabeling.Services.Dto;
using System;
using System.Threading.Tasks;

namespace ProductLabeling.Services.Intefaces
{
    internal interface IModbusTcpIoModuleService
    {
        public ModbusTcpIoModuleDto? Dto { get; set; }

        public event Action<bool>? Status;

        public event Action<string>? Error;

        public delegate void ReadDIEventHendler(bool[] leadingEdgeInputs, bool[] trailingEdgeInputs, double timeMillisecond);
        public event ReadDIEventHendler? ReadDI;

        public delegate void ReadEncoderEventHendler(ushort[] buffer);
        public event ReadEncoderEventHendler? ReadEncoder;

        public bool Connected { get; }

        public Task StartAsync();

        public Task StopAsync(ushort startAddressDo, int countDo);

        public Task<double> WriteSingleDOAsync(ushort coilAddress, bool value);

        public Task<double> WriteMultipleDOAsync(ushort startAddress, bool[] data);
    }
}
