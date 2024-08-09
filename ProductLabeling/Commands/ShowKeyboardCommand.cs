using System.Diagnostics;
using System.IO;

namespace ProductLabeling.Commands
{
    class ShowKeyboardCommand : Command
    {
        private const string KeyboardPath = @"C:\WINDOWS\system32\osk.exe";

        public override bool CanExecute(object? parameter) => File.Exists(KeyboardPath);

        public override void Execute(object? parameter) => 
            Process.Start(new ProcessStartInfo { FileName = KeyboardPath, UseShellExecute = true });
        

    }
}
