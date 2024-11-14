using ProductLabeling.Models.Interfaces;
using ProductLabeling.Models.Interfaces.FirstStage;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ProductLabeling.Models
{
    internal abstract class Model : AppSettingsProvider, IModel
    {
        private const string FileNameProducts = "products.json";

        protected readonly string productsPath = Path.Combine(AppContext.BaseDirectory, FileNameProducts);

        private const int StatusTimeout = 1000;

        protected const ushort StartAddresDo = 0;
        protected const ushort CountDo = 8;

        private Task? _trafficLightSetStatus;

        private readonly IDevices _devices;

        private bool _isEnable;

        public Model(ISerializerService<AppSettingsDto> appSettings, IDevices devices) : base(appSettings)
        {
            _devices = devices;
        }

        public IDevices Devices => _devices;

        public virtual string Name => throw new NotImplementedException();

        public virtual IFirstStage FirstStage => throw new NotImplementedException();
        
        public virtual async Task OffAsync() =>
            await Task.Run(async () =>
            {
                _isEnable = false;
                if (_trafficLightSetStatus != null)
                    await _trafficLightSetStatus;
            });

        public virtual async Task OnAsync() =>
        await Task.Run(() =>
        {
            _isEnable = true;
            _trafficLightSetStatus = new Task(TrafficLightSetSatus);
            _trafficLightSetStatus.Start();
        });

        private void TrafficLightSetSatus()
        {
            bool startStatus = false;
            bool previousStatus = false;

            while (_isEnable)
            {
                Thread.Sleep(StatusTimeout);

                bool status = GetStatusModel();

                if (status == previousStatus && startStatus)
                    continue;

                startStatus = true;
                previousStatus = status;

                _devices.TrafficLight.LightGreen(status);
                _devices.TrafficLight.LightRed(!status);
            }
        }

        protected virtual bool GetStatusModel() => throw new NotImplementedException();
    }
}
