using System.Windows;

namespace ProductLabeling.Commands
{
    internal class CloseDialogWindowCommand : Command
    {
        public bool? DialogResult { get; set; }

        public override void Execute(object? parameter) => (parameter as Window)!.DialogResult = DialogResult;

        public override bool CanExecute(object? parameter) => parameter is Window;
    }
}
