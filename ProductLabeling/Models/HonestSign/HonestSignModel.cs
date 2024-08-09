using ProductLabeling.Models.Interfaces;
using ProductLabeling.Models.Interfaces.FirstStage;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductLabeling.Models.HonestSign
{
    internal class HonestSignModel : Model
    {
        private readonly IDevices _devices;
        private readonly IFirstStage _firstStage;

        public HonestSignModel(ISerializerService<AppSettingsDto> appSettings, IDevices devices, 
            IEnumerable<IFirstStage> firstStage) : base(appSettings, devices)
        {
            _devices = devices;
            _firstStage = firstStage.First(stage => stage.Model == ModelName.HonestSign);
        }
        
        public override string Name => "Честный знак";

        public override IFirstStage FirstStage => _firstStage;

        public override async Task OnAsync()
        {
            await Task.Run(() => Task.WaitAll(
                _devices.IoModule.StartAsync(),
                _devices.CodeReader.StartAsync(),
                _devices.HandCodeReader.StartAsync(),
                _devices.DataBase.StartAsync()
                ));

            _devices.SubReadEncoderAsync();

            await base.OnAsync();
        }

        public override async Task OffAsync()
        {
            await base.OffAsync();

            _devices.UnSubReadEncoderAsync();

            await Task.Run(() => Task.WaitAll(
                _devices.IoModule.StopAsync(StartAddresDo, CountDo),
                _devices.CodeReader.StopAsync(),
                _devices.HandCodeReader.StopAsync(),
                _devices.DataBase.StopAsync()
                ));
        }

        protected override bool GetStatusModel() =>
            _devices.IoModule.Connected &&
            _devices.CodeReader.Connected &&
            _devices.HandCodeReader.Connected &&
            _devices.DataBase.Connected;
    }
}
