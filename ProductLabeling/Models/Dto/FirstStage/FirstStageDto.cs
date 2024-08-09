namespace ProductLabeling.Models.Dto.FirstStage
{
    internal class FirstStageDto
    {
        public FirstStageAutoReadingDto AutoReading { get; set; } = new();
        public FirstStageHandReadingDto HandReading { get; set; } = new();
    }
}
