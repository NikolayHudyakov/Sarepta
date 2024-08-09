using System;

namespace ProductLabeling.Models.Interfaces.FirstStage
{
    internal interface IFirstStageAutoReading : IModelName
    {
        public event Action<bool>? Status;

        public event Action<string>? Message;

        public event Action<string>? CodeReaderData;

        public event Action<double>? TimeDI;

        public event Action<double>? TimeCodeReader;

        public event Action<double>? TimeDb;

        public Product? SelectedProduct { get; set; }

        public Counter Counter { get; }
    }
}
