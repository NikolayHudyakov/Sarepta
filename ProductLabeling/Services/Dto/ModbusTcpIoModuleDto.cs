namespace ProductLabeling.Services.Dto
{
    internal class ModbusTcpIoModuleDto
    {
        public string? IPAddress { get; set; }
        public int Port { get; set; }
        public byte SlaveAddress { get; set; }
        public ushort DiStartAddress { get; set; }
        public ushort DiNumberOfPoints { get; set; }

        public ushort EncoderStartAddress { get; set; }
        public ushort EncoderNumberOfPoints { get; set; }
        public int EncoderPollingPeriod { get; set; }

    }
}
