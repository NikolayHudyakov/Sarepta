using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;

namespace ProductLabeling.Models
{
    internal abstract class AppSettingsProvider
    {
        protected readonly AppSettingsDto dto;

        protected AppSettingsProvider(ISerializerService<AppSettingsDto> appSettings) => dto = appSettings.Deserialize();
    }
}
