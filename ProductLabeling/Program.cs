using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Reflection;

namespace ProductLabeling
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var processName = Assembly.GetExecutingAssembly().GetName().Name;

            if (Process.GetCurrentProcess().ProcessName != processName ||
                Process.GetProcessesByName(processName).Length != 1)
                return;

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(App.CurrentDirectory)
                .ConfigureServices(App.ConfigureServices)
                .ConfigureLogging(App.ConfigureLogging);
    }
}
