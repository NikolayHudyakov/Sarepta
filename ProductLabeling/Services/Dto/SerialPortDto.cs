using System.IO.Ports;

namespace ProductLabeling.Services.Dto
{
    class SerialPortDto
    {
        public string? PortName { get; set; }
        public int Baudrate { get; set; }

    }
}
