using Microsoft.Extensions.DependencyInjection;
using ProductLabeling.Models;
using ProductLabeling.Services;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System.Collections.ObjectModel;

namespace ProductLabeling.IoC
{
    internal static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddSingleton<ISerializerService<AppSettingsDto>, AppSettingsService>()
            .AddSingleton<ISerializerService<ObservableCollection<Product>>, ProductsService>()
            .AddTransient<IModbusTcpIoModuleService, ModbusTcpIoModuleService>()
            .AddTransient<ITcpClientService, TcpClientService>()
            .AddTransient<IDbService, DbService>()
            .AddSingleton<IRejecterService, RejecterService>()
            .AddTransient<ISerialPortService, SerialPortService>();
    }
}
