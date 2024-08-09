using ProductLabeling.Models.Interfaces;
using ProductLabeling.Models.Interfaces.FirstStage;
using System.Collections.Generic;
using System.Linq;

namespace ProductLabeling.Models.HonestSign.FirstStage
{
    internal class FirstStageHS : IFirstStage
    {
        private readonly IFirstStageAutoReading _autoReading;
        private readonly IFirstStageHandReading _handReading;

        public FirstStageHS(IEnumerable<IFirstStageAutoReading> autoReadings, IEnumerable<IFirstStageHandReading> handReadings)
        {
            _autoReading = autoReadings.First(auto => auto.Model == ModelName.HonestSign);
            _handReading = handReadings.First(hand => hand.Model == ModelName.HonestSign);
        }
           
        public IFirstStageAutoReading AutoReading => _autoReading;

        public IFirstStageHandReading HandReading => _handReading;

        public ModelName Model => ModelName.HonestSign;
    }
}
