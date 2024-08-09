namespace ProductLabeling.Models.Interfaces.FirstStage
{
    internal interface IFirstStage : IModelName
    {
        public IFirstStageAutoReading AutoReading { get; }
        public IFirstStageHandReading HandReading { get; }
    }
}
