using System;
using System.Management;
using System.Text;
using System.Security.Cryptography;

namespace IDProcessorRegistrator
{
    static internal class RegIDProcessor
    {
        private const string Salt = "d4f6ds54few5f1sd4f";
        private const string SubKey = "ProdLabHash";
        private const string KeyName = "RegValue";

        static public bool CheckHash()
        {
            byte[] hashValue = MD5.HashData(Encoding.UTF8.GetBytes($"{GetIDProcessor()}{Salt}"));
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(SubKey);
            var ret = key.GetValue(KeyName);
            key.Close();
            return ret != null ? Equals(Convert.ToBase64String(hashValue), (string)ret) : false;
        }

        static public void CreateHash()
        {
            byte[] hashValue = MD5.HashData(Encoding.UTF8.GetBytes($"{GetIDProcessor()}{Salt}"));
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(SubKey);
            key.SetValue(KeyName, Convert.ToBase64String(hashValue));
            key.Close();
        }

        static private string GetIDProcessor()
        {
            string processorID = string.Empty;
            ManagementObjectSearcher managementObjectSearcher = new("SELECT ProcessorId FROM Win32_Processor");
            ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
            foreach (ManagementObject managementObject in managementObjectCollection)
                processorID = (string)managementObject["ProcessorId"];
            return processorID;
        }
    }
}
