using ProductLabeling.Views.Windows;

namespace ProductLabeling.Commands
{
    internal class AppSettingShowDialogCommand : Command
    {
        public override bool CanExecute(object? parameter) => true;

        public override void Execute(object? parameter)
        {
            if (new PasswordWindow().ShowDialog() ?? false)
                new AppSettingsWindow().ShowDialog();
        }
    }
}
