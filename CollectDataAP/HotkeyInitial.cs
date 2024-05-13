using GlobalVar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollectDataAP
{
    class HotkeyInitial
    {
        public class IniFile
        {
            public string path;

            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section,
              string key, string val, string filePath);

            //For reading .ini file
            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section,
              string key, string def, StringBuilder retVal,
              int size, string filePath);

            public void InitFile(string INIPath)
            {
                path = INIPath;
            }

            public void IniWriteValue(string Section, string Key, string Value)
            {
                WritePrivateProfileString(Section, Key, Value, this.path);
            }

            public string IniReadValue(string Section, string Key)
            {
                StringBuilder temp = new StringBuilder(255);
                GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
                return temp.ToString();
            }
        }

        public HotkeyInitial()
        {
            if (RegistryWindows.getValue("F1S") == "nothing" || RegistryWindows.getValue("F1S") == "noValue")
            {
                IniFile inifile = new IniFile();
                inifile.path = "C:\\Program Files\\HotTab\\HottabCfg.ini";

                if (!File.Exists(inifile.path))
                {
                    MessageBox.Show("Can not find "+ inifile.path, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                HotkeyFunc.funcName[0] = inifile.IniReadValue("FunctionKey", "F1S");
                HotkeyFunc.funcName[4] = inifile.IniReadValue("FunctionKey", "F1L");
                HotkeyFunc.funcName[1] = inifile.IniReadValue("FunctionKey", "F2S");
                HotkeyFunc.funcName[5] = inifile.IniReadValue("FunctionKey", "F2L");
                HotkeyFunc.funcName[2] = inifile.IniReadValue("FunctionKey", "F3S");
                HotkeyFunc.funcName[3] = inifile.IniReadValue("FunctionKey", "F3L");
                /*
                RegistryWindows.setValue("F1S", HotkeyFunc.funcName[0]);
                RegistryWindows.setValue("F1L", HotkeyFunc.funcName[4]);
                RegistryWindows.setValue("F2S", HotkeyFunc.funcName[1]);
                RegistryWindows.setValue("F2L", HotkeyFunc.funcName[5]);
                RegistryWindows.setValue("F3S", HotkeyFunc.funcName[2]);
                RegistryWindows.setValue("F3L", HotkeyFunc.funcName[3]);
                */
                //RegistryWindows.setValue("aabb", "1234");

                //ProcessStart.processStart_reg("aabbccdd", "123412");

                ProcessStart.processStart_reg("F1S", HotkeyFunc.funcName[0]);
                ProcessStart.processStart_reg("F1L", HotkeyFunc.funcName[4]);
                ProcessStart.processStart_reg("F2S", HotkeyFunc.funcName[1]);
                ProcessStart.processStart_reg("F2L", HotkeyFunc.funcName[5]);
                ProcessStart.processStart_reg("F3S", HotkeyFunc.funcName[2]);
                ProcessStart.processStart_reg("F3L", HotkeyFunc.funcName[3]);

                Console.WriteLine("reg ok");
            }
            else
            {
                HotkeyFunc.changeFuncName(HotkeyFunc.HotkeyList.f1Short, RegistryWindows.getValue("F1S"));
                HotkeyFunc.changeFuncName(HotkeyFunc.HotkeyList.f1Long, RegistryWindows.getValue("F1L"));
                HotkeyFunc.changeFuncName(HotkeyFunc.HotkeyList.f2Short, RegistryWindows.getValue("F2S"));
                HotkeyFunc.changeFuncName(HotkeyFunc.HotkeyList.f2Long, RegistryWindows.getValue("F2L"));
                HotkeyFunc.changeFuncName(HotkeyFunc.HotkeyList.f3Short, RegistryWindows.getValue("F3S"));
                HotkeyFunc.changeFuncName(HotkeyFunc.HotkeyList.f3Long, RegistryWindows.getValue("F3L"));
            }
        }
    }
}
