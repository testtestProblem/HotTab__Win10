using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CollectDataAP
{
    class RegistryWindows
    {
        private const string userRoot = "HKEY_CURRENT_USER";
        private static string subkey = @"SOFTWARE\HotTabTest1";
        //private static string subkey = @"SOFTWARE\HotTab10";
        private static string subkey1 = "abcd";
        private static string subkey2 = "efgh";
        //private static string keyPath = userRoot + "\\" + subkey;

        //private static RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(subkey);
        //Registry.CurrentUser.CreateSubKey(subkey);

        /*private string keyName(string name)
        {
            return keyPath += "\\" + name;
        }*/
        //[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum, Write = @"HKEY_CURRENT_USER\\SOFTWARE\\HotTabTest1")]
        public static void setValue(string key, string value)
        {/*
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(subkey1, true);
            RegistryKey registryKey2 =registryKey.CreateSubKey(subkey2, true);
            //registryKey.SetValue(key, (string)value);
            registryKey2.SetValue(key, (string)value);
            registryKey2.Close();
            registryKey.Close();
            registryKey2.Dispose();
            registryKey.Dispose();*/

            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(subkey, RegistryKeyPermissionCheck.ReadWriteSubTree);
            registryKey.SetValue(key, (string)value, RegistryValueKind.String);
            registryKey.Close();
        }

        public static string getValue(string key)
        {/*
            //RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(subkey);
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(subkey1);
            RegistryKey registryKey2 = registryKey.CreateSubKey(subkey2);
            string data = registryKey2.GetValue(key, "") as string;
            registryKey2.Close();
            registryKey.Close();
            registryKey2.Dispose();
            registryKey.Dispose();*/

            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(subkey);
            string data = registryKey.GetValue(key, "") as string;
            registryKey.Close();

            Console.WriteLine("registry key,data -> " +key+","+ data);

            if (data == "" || data == null) return "nothing";
            else return data;
        }
    }
}
