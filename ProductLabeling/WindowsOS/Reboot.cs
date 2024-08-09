using System.Diagnostics;

namespace ProductLabeling.WindowsOS
{
    internal class Reboot
    {
        private const string ProcessName = "shutdown";
        private const string Arguments = "/r /t 0";

        public static void StartProcess()
        {
            if (Process.GetProcessesByName(ProcessName).Length == 0)
                Process.Start(ProcessName, Arguments);
        }
    }
}
