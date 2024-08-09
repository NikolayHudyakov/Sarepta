using ProductLabeling.Services.Dto;

namespace ProductLabeling.Models.Dto.FirstStage
{
    internal class FirstStageAutoReadingDto
    {
        public TcpClientDto? CodeReader { get; set; } = new TcpClientDto();
        public string? CodeReaderNoreadString { get; set; }
        public string? CodeDelimiter { get; set; }
        public double RejecterDistance { get; set; }
        public int RejecterDuration { get; set; }
        public bool RejecterIsEnable { get; set; }
        public int RejecterDelay { get; set; }
        public int RejecterMode { get; set; }
        public int BuzzerDuration { get; set; }
        public int YellowDuration { get; set; }
        public int StopLineCount { get; set; }
        public int WheelDiameter { get; set; }
        public int EncoderResolution { get; set; }
        public int CodeReaderRecieveTimeout { get; set; }
        public int CodeReaderTriggerDuration { get; set; }
        public int StopLinePulseDuration { get; set; }
    }
}
