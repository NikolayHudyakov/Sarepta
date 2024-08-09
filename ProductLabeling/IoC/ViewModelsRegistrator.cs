using Microsoft.Extensions.DependencyInjection;
using ProductLabeling.ViewModels;

namespace ProductLabeling.IoC
{
    internal static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModel(this IServiceCollection services) => services
            .AddTransient<MainWindowViewModel>()
            .AddSingleton<AppSettingsWindowViewModel>()
            .AddTransient<AddProductWindowViewModel>()
            .AddTransient<PasswordWindowWiewModel>();
    }
}
