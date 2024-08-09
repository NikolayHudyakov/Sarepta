using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;

namespace ProductLabeling.ViewModels
{
    internal abstract class ViewModelAppSettings : ViewModel
    {
        private readonly ISerializerService<AppSettingsDto> _appSettings;
        protected readonly AppSettingsDto dto;

        protected ViewModelAppSettings(ISerializerService<AppSettingsDto> appSettings)
        {
            _appSettings = appSettings;
            dto = _appSettings.Deserialize();
        }

        protected void SetAppSettings() => _appSettings.Serialize(dto);
    }
}
