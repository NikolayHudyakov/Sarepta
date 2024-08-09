using ProductLabeling.Services.Dto;

namespace ProductLabeling.Models.Dto.FirstStage
{
    internal class FirstStageHandReadingDto
    {

        public SerialPortDto? HandCodeReader { get; set; } = new SerialPortDto();

    }
}
