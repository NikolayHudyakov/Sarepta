using Microsoft.Extensions.DependencyInjection;
using ProductLabeling.Models;
using ProductLabeling.Models.HonestSign;
using ProductLabeling.Models.HonestSign.FirstStage;
using ProductLabeling.Models.Interfaces;
using ProductLabeling.Models.Interfaces.FirstStage;

namespace ProductLabeling.IoC
{
    internal static class ModelsRegistrator
    {
        public static IServiceCollection AddModels(this IServiceCollection services) => services
            .AddSingleton<IDevices, Devices>()
            .AddSingleton<IFirstStageAutoReading, FirstStageAutoReadingHS>()
            .AddSingleton<IFirstStageHandReading, FirstStageHandReadingHS>()
            .AddSingleton<IFirstStage, FirstStageHS>()
            .AddSingleton<IModel, HonestSignModel>();
    }
}
