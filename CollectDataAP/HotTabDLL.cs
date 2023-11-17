//#define MyPC

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using GlobalVar;
using System.Management;
using System.Globalization;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Security.Principal;

namespace Win8Hottab
{
    /// <summary>
    /// ���?DLL Import?��???��??�相?�函?
    /// </summary>
    /// 


    enum ECBufferEvent : int
    {
        ECBufferEvent_WaitInputBufferEmpty = 0,
        ECBufferEvent_WaitOutputBufferFull,
        ECBufferEvent_CleanOutputBuffer
    }

    enum GSensor_Orientation : int
    {
        Orientation_Left = 0,
        Orientation_Right,
        Orientation_Up,
        Orientation_Down,
        Orientation_Top,
        Orientation_Bottom
    }

    class HotTabDLL
    {

        #region Struct Declare

        //Resolution
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;

            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;

            public short dmLogPixels;
            public short dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct POWERBROADCAST_SETTING
        {
            public Guid PowerSetting;
            public uint DataLength;
            public uint Data;
        };


        #endregion


        public static bool IsSensorHubExist = false;
        public static String OSName = "";
    
        public static bool bIsScreenRotating = false;
        public static bool bIsShowBrightness;
        private static uint uiECBrightness;
        private static uint uiPMBrightness;
        private static int iSkipNotifyECBrightness;
        private static int iSkipNotifyPMBrightness;

        public static bool bIsReadingEC = false;
        public static bool bIsBrightnessReadingEC = false;


        #region User32 DLL Declare For All

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern Int32 PostMessage(Int32 hWnd, Int32 wMsg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern Int32 RegisterWindowMessage(string lpString);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(int lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE lpDevMode, int dwflags);

        #endregion

        #region Kernel32 DLL Declare For All

        [DllImport("kernel32.dll")]
        public static extern void Sleep(int dwMilliseconds);

        #endregion

        #region Winmate WinIO DLL Declare

        [DllImport(@"WMIO2.dll")]
        public static extern bool WinIO_ReadCommand(uint uiCommand, out uint piData);

        [DllImport(@"WMIO2.dll")]
        public static extern bool WinIO_WriteCommand(uint uiCommand, uint uiData);

        [DllImport(@"WMIO2.dll")]
        public static extern bool WinIO_ReadFromECSpace(uint uiAddress, out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool WinIO_WriteToECSpace(uint uiAddress, uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool SetDevice(byte uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetDevice(out byte uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool SetDevice2(byte uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetDevice2(out byte uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetLightVariable(out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetAccelerometer(out double uiXValue, out double uiYValue, out double uiZValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool SetLightSensorOnOff(uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool SetFlashOn();

        [DllImport(@"WMIO2.dll")]
        public static extern bool SetFlashOff();

        [DllImport(@"WMIO2.dll")]
        public static extern bool SetAutoScreenRotationLock(uint uiMode);

        [DllImport(@"WMIO2.dll")]
        public static extern bool SetAutoBrightnessStatus(bool bValue, uint iValue);


        [DllImport(@"WMIO2.dll")]
        public static extern bool GetACStatus(out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetBattery1FullChargeCapacity(out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetBattery2FullChargeCapacity(out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetBattery1RemainingCapacity(out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetBattery2RemainingCapacity(out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetBattery1Current(out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetBattery2Current(out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetBattery1ChargingVoltage(out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetBattery2ChargingVoltage(out uint uiValue);

        #endregion

        #region WinIO Function For All
        //public static FormDebug formDebug = new FormDebug();


        //Get EC Version
        public static bool WinIO_GetECVersion(out string version)
        {
            uint bValue;

            version = "";

            WinIO_ReadFromECSpace(0x05, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x06, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x07, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x08, out bValue);
            if ((bValue >= 0x20) && (bValue <= 0x7A))
                version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x09, out bValue);
            if ((bValue >= 0x20) && (bValue <= 0x7A))
                version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x0A, out bValue);
            if ((bValue >= 0x20) && (bValue <= 0x7A))
                version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x0B, out bValue);
            if ((bValue >= 0x20) && (bValue <= 0x7A))
                version += Convert.ToChar(bValue).ToString();

            return true;
        }

        //Get EC Panel dimming type
        public static bool WinIO_GetECPanelDimmingType(out string type)
        {
            uint bValue;

            type = "";

            WinIO_ReadFromECSpace(0x04, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            type = Convert.ToChar(bValue).ToString();

            return true;
        } 

        //Get MB Version
        public static bool WinIO_GetECMBVersion(out string version)
        {
            uint bValue;

            version = "";

            WinIO_ReadFromECSpace(0x00, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x01, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x02, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x03, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            return true;
        }

        //Get SUB Version
        public static bool WinIO_GetECSubVersion(out string version)
        {
            uint bValue;

            version = "";

            WinIO_ReadFromECSpace(0x08, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x09, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x0A, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            WinIO_ReadFromECSpace(0x0B, out bValue);
            if ((bValue < 0x20) || (bValue > 0x7A))
                bValue = 0x5F;
            version += Convert.ToChar(bValue).ToString();

            return true;
        } 



    }
}
#endregion