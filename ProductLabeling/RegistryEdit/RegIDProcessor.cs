using System.Linq;
using System.Management;

namespace ProductLabeling.RegistryEdit
{
    public static class RegIDProcessor
    {
        private const string Key = "RegValue";

        private static readonly RegEdit _regEdit = new();

        public static bool CheckHash() => _regEdit.CheckHash(Key, GetIDProcessor());

        private static string GetIDProcessor()
        {
            string processorID = string.Empty;
            ManagementObjectSearcher managementObjectSearcher = new("SELECT ProcessorId FROM Win32_Processor");
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            foreach (ManagementObject managementObject in managementObjectCollection.Cast<ManagementObject>())
                processorID = (string)managementObject["ProcessorId"];
            return processorID;
        }
    }
}
