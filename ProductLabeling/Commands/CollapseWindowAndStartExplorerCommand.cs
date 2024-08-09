using ProductLabeling.Views.Windows;
using ProductLabeling.WindowsOS;
using System.Windows;

namespace ProductLabeling.Commands
{
    internal class CollapseWindowAndStartExplorerCommand : Command
    {
        public override void Execute(object? parameter)
        {
            if (!new PasswordWindow().ShowDialog() ?? true) return;

            (parameter as Window)!.WindowState = WindowState.Minimized;
            
            Explorer.StartProcess();
        }

        public override bool CanExecute(object? parameter) => parameter is Window;
    }
}
