using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using GlobalVar;
using System.Runtime.InteropServices;
using System.IO;

namespace Win8Hottab_unknow
{
    public class HotTabRegistry
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern long WritePrivateProfileString(string section,
          string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section,
          string key, string def, StringBuilder retVal,
          int size, string filePath);

        private const string HotTabRegisterPath = @"SOFTWARE\HotTab";

        private string HotTabRegisterIniPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\hottab\\HottabRegIni.ini";

        private RegistryKey registryKey;

        private int mode = 0;//0:register, 1:ini

        public bool DeleteSubKeyTree()
        {
            try
            {
                // Setting
                RegistryKey rk = Registry.CurrentUser;
                RegistryKey sk1 = rk.OpenSubKey(HotTabRegisterPath);
                // If the RegistryKey exists, I delete it
                if (sk1 != null)
                    rk.DeleteSubKeyTree(HotTabRegisterPath);

                return true;
            }
            catch (Exception e)
            {
                //an error!
                //ShowErrorMessage(e, "Deleting SubKey " + HotTabRegisterPath);
                return false;
            }
        }

        public HotTabRegistry()
        {
            GlobalVariable.DebugMessage("winmate", "HotTabRegistry start", GlobalVariable.bDebug);

            string programSrcPatch = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\hottab\\";
            System.IO.Directory.CreateDirectory(programSrcPatch);

            /* winmate 20160414 brain fix register load default >>> */
            try
            {
                if (IniReadValue("FLAG", "FIRSTBOOT") == "TRUE")
                {
                    DeleteSubKeyTree();
                }
                IniWriteValue("FLAG", "FIRSTBOOT", "FALSE");
            }
            catch
            {
                // error
            }
            /* winmate 20160414 brain fix register load default <<< */

            try
            {
                registryKey = Registry.CurrentUser.CreateSubKey(HotTabRegisterPath); // for Win7
            }
            catch
            {
                mode = 1;
                GlobalVariable.DebugMessage("winmate", "HotTab register ini create.", GlobalVariable.bDebug);
            }

            GlobalVariable.DebugMessage("winmate", "HotTabRegistry end", GlobalVariable.bDebug);
        }

        public bool getAutoRotationFlag()
        {
            bool rotationFlag = false;
            RegistryKey rotation;
            rotation = Registry.LocalMachine;
            rotation = rotation.CreateSubKey(@"SOFTWARE\Mircosoft\Windows\CurrentVersion\AutoRotation");
            rotationFlag = (bool)rotation.GetValue("Enable");
            return rotationFlag;
        }

        public bool RegistryRead(String name, ref String value)
        {

            if (mode == 1)
            {
                value = IniReadValue("SETTING", name);

                if (value == "")
                    return false;
                else
                    return true;
            }
            else
            {
                try
                {
                    value = registryKey.GetValue(name).ToString();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool RegistryWrite(String name, String value)
        {
            if (mode == 1)
            {
                IniWriteValue("SETTING", name, value);
                return true;
            }
            else
            {
                try
                {
                    registryKey.SetValue(name, value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.HotTabRegisterIniPath);
        }

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.HotTabRegisterIniPath);
            return temp.ToString();
        }

        //Static Method Declare
        public static bool RegistryRead(String registerPath, String name, ref String value)
        {
            try
            {
                RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey registryKey = localMachineX64View.OpenSubKey(registerPath);

                value = registryKey.GetValue(name).ToString();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
