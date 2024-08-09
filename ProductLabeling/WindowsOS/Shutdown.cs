using System.Diagnostics;

namespace ProductLabeling.WindowsOS
{
    internal class Shutdown
    {
        private const string ProcessName = "shutdown";
        private const string Arguments = "/s /t 0";

        public static void StartProcess()
        {
            if (Process.GetProcessesByName(ProcessName).Length == 0)
                Process.Start(ProcessName, Arguments);
        }
    }
}
