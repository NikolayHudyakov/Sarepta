using System.Windows;

namespace ProductLabeling.Commands
{
    internal class CloseWindowCommand : Command
    {
        public override void Execute(object? parameter) => (parameter as Window)!.Close();

        public override bool CanExecute(object? parameter) => parameter is Window;
    }
}
