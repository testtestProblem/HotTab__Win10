using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectDataAP
{
    class RegistryWindows
    {
        //private Registry registryKey;
        private const string userRoot = "HKEY_CURRENT_USER";
        private const string subkey = @"SOFTWARE\HotTabTest1";
        private static string keyPath = userRoot + "\\" + subkey;

        /*private string keyName(string name)
        {
            return keyPath += "\\" + name;
        }*/

        public static void setValue(string key, object value)
        {
            Registry.SetValue(keyPath, key, value);
        }

        public static string getValue(string key)
        {
            return (string)Registry.GetValue(keyPath, key, "noValue");
        }
    }
}
