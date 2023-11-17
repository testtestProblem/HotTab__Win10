﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
 
namespace GlobalVar
{

    public class GlobalVariable
    {
        #region Const Declare 
        public const int DEV_NULL = -1;
        public const int DEV_WIFI = 0;
        public const int DEV_3G = 1;
        public const int DEV_GPS = 2;
        public const int DEV_BLUETOOTH = 3;
        public const int DEV_CAMERA = 4;


        public const int DEV_BARCODE = 0;
        //public const int DEV_CAMERAFRONT = 1;
        public const int DEV_RFID = 1;
        public const int DEV_RFID_CONFIG = 2;
        public const int DEV_COMPORT = 3;
        public const int DEV_USB = 4;
        public const int DEV_ANT_GPS = 5;
        public const int DEV_Ant_WWAN = 6;
        public const int DEV_KEYBOARD = 7;
        public const int DEV_TOUCH_SET = 8;

        public const int DEV_ORIENTATION_0 = 0;
        public const int DEV_ORIENTATION_90 = 1;
        public const int DEV_ORIENTATION_180 = 2;
        public const int DEV_ORIENTATION_270 = 3; 
        #endregion

        //HotTab Version
        //public static string sHotTabVersion = "40.2.24";
        //BIOS Version
        public static string sBIOSVersion = "";
        //EC Version
        public static string sECVersion = "";
        //EC MB Version
        public static string sECMBVersion = "";
        //SMBIOS Units SN
        public static string sUnitsSN = "";
        //MainBoard Ver.
        public static string sMainBoardVersion = "";
        //OS Ver.
        public static string OSVersion = "";

        //HottabCfg.ini >>
        public static string HotTabBoardName = "IB80";
        public static string HotTabCfgVersion = "0";
        public static uint bDebug = 0;                              //0:disable debug message, 1:enable debug message
        public static uint FlashControlType = 1;                    //0:Null, 1:USB, 2:GPIO, 3:USB and GPIO
        public static uint DeviceCount = 2;                         //1:Device1, 2:Device1,Device2
        public static uint DeviceControl = 0;                       //0:default, 1:only wifi and bluetooth control
        public static uint StartOrientation = DEV_ORIENTATION_0;    //0:0 deg, 1:90 deg, 2:180 deg, 3: 270 deg
        public static uint CameraForceOrientation = 0;              //0:Null, 1:0 deg, 2:90 deg, 3:180 deg, 4:270 deg
        public static uint BatteryType = 0;                         //0:Left,Right, 1:Main,Second, 2:Main

        public static uint SmallBatteryUseAtoD = 0;                 //0:use smbus information, 1:use voltage
        public static uint SmallBatteryPowerSettingBrightnessLimit = 0; //brightness limit 0 - 100
        public static uint SmallBatteryPowerSettingProcessorLimit = 50;  //processor limit 0% - 100%
        public static uint SmallBatteryDisplay = 0;                 //0:small battery not display, 1:display small battery

        public static uint BarcodeType = 0;                         //0:Normal(BS523/MDI3100/M3/), 1:MDL-1000, 2:ISDC-RS, 3:Moto(SE4500DL)
        public static string BarcodeCOMLocation = "COM15";
        public static uint BarcodeVisible = 0;                      //0:not dispaly, 1:display
        public static uint BarcodeIdentifierCode = 1;               //0:removed, 1:reserved

        public static uint SoftwareHotSwapSupport = 0;              //0:not support, 1:support
        public static uint OemSpecialVersion = 0;                   //0:Normal

        public static uint TouchSetSupport = 0;                     //0:not support 1: support
        public static uint TouchModeDefault = 0;                    //0:Hand/Rain Mode, 1:Stylus Mode, 2:Glove Mode

        public static uint AutoBrightnessSupport = 0;               //0:Disable 1: Enable
        public static uint AutoRotationSupport = 0;                 //0:Disable 1: Enable
        public static uint SensorHubExist = 0;                      //0:Disable 1: Enable

        public static bool autoRotationFlag = true;

        public static uint UserPermissionLock = 0;                   //0:unlock 1:lock

        public static bool HidenShowFormFlag = false;
        public static bool RotationFlagFormDevice = false;
        public static bool RotationFlagFormShortCut = false;
        public static bool RotationFlagFormMain = false;
        public static bool RotationFlagFormSetting = false;
        public static bool RotationFlagFormSettingFun1S = false;
        public static bool RotationFlagFormSettingFun1L = false;
        public static bool RotationFlagFormSettingFun2S = false;
        public static bool RotationFlagFormSettingFun2L = false;
        public static bool RotationFlagFormSettingFun3S = false;
        public static bool RotationFlagFormSettingFun3L = false;
        public static bool rotationFlag = false;
        public static bool RotationFlagFormCamera = false;

       
        //HOttabCfg.ini <<
        public static int[] DeviceOrder = { DEV_WIFI, DEV_3G, DEV_GPS, DEV_BLUETOOTH, DEV_CAMERA, DEV_NULL, DEV_NULL, DEV_NULL };
        public static int[] DeviceOrder2 = { DEV_BARCODE, DEV_RFID, DEV_RFID_CONFIG, DEV_COMPORT, DEV_USB, DEV_ANT_GPS, DEV_Ant_WWAN, DEV_KEYBOARD, DEV_TOUCH_SET };

        public static bool Load_HottabCfg()
        {

            IniFile inifile = new IniFile();
            //string patch = System.Windows.Forms.Application.StartupPath;
            //string patch = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            //inifile.path = patch + "\\HottabCfg.ini";
            inifile.path = "C:\\HottabCfg.ini";

            if (!File.Exists(inifile.path))
                return false;

            //Version
            HotTabBoardName = inifile.IniReadValue("Version", "ProjectName");
            HotTabCfgVersion = inifile.IniReadValue("Version", "CfgVersion");

            //OEM
            OemSpecialVersion = IniReadUIntValue("OEM", "OemSpecialVersion");


            return true;
        }

        public static uint IniReadUIntValue(string Section, string Key)
        {
            IniFile inifile = new IniFile();
            //string patch = System.Windows.Forms.Application.StartupPath;
            string src;
            string dest;
            uint value;

            inifile.path = "C:\\HottabCfg.ini";
            src = inifile.IniReadValue(Section, Key);

            dest = src.Replace("0X", "");
            dest = dest.Replace("0x", "");

            if (dest == "")
                return 0;

            value = Convert.ToUInt16(dest);

            return value;
        }


        public static void DebugMessage(string token, string msg, uint en)
        {
            if (en == 1)
            {
                Trace.WriteLine(token + " ==> " + msg);
            }
        }
    }


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
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
    }
}
