using Microsoft.Win32;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ProductLabeling.RegistryEdit
{
    public class RegEdit
    {
        private const string Salt = "d4f6ds54few5f1sd4f";
        private const string DefaultSubKey = "ProdLabHash";

        protected virtual string SubKey => DefaultSubKey;

        public bool CheckHash(string key, string value)
        {
            string? ret = GetHash(key);
            return ret != null && Equals(StringToHash(value), ret);
        }

        public void CreateHash(string key, string value)
        {
            byte[] hashValue = MD5.HashData(Encoding.UTF8.GetBytes($"{value}{Salt}"));
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(SubKey);
            registryKey.SetValue(key, Convert.ToBase64String(hashValue));
            registryKey.Close();
        }

        public static string StringToHash(string value)
        {
            byte[] hashValue = MD5.HashData(Encoding.UTF8.GetBytes($"{value}{Salt}"));
            return Convert.ToBase64String(hashValue);
        }

        public string? GetHash(string key)
        {
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(SubKey);
            var ret = registryKey.GetValue(key);
            registryKey.Close();
            return  ret as string;
        }
    }
}
