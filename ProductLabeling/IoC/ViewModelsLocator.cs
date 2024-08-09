using Microsoft.Extensions.DependencyInjection;
using ProductLabeling.ViewModels;

namespace ProductLabeling.IoC
{
    internal class ViewModelsLocator
    {
        public MainWindowViewModel MainWindowViewModel => App.Host.Services.GetRequiredService<MainWindowViewModel>();
        public AppSettingsWindowViewModel AppSettingsWindowViewModel => App.Host.Services.GetRequiredService<AppSettingsWindowViewModel>();
        public AddProductWindowViewModel AddProductWindowViewModel => App.Host.Services.GetRequiredService<AddProductWindowViewModel>();
        public PasswordWindowWiewModel PasswordWindowWiewModel => App.Host.Services.GetRequiredService<PasswordWindowWiewModel>();
    }
}
