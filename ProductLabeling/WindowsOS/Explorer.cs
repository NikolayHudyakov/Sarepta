using System.Diagnostics;

namespace ProductLabeling.WindowsOS
{
    internal static class Explorer
    {
        private const string ProcessName = "explorer";

        public static void StartProcess()
        {
            if (Process.GetProcessesByName(ProcessName).Length == 0)
                Process.Start(ProcessName);
        }
    }
}
