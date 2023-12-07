using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CollectDataAP
{
    class DeviceState
    {
        [DllImport(@"WMIO2.dll")]
        public static extern bool SetDevice(byte uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetDevice(out byte uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool SetDevice2(byte uiValue);

        [DllImport(@"WMIO2.dll")]
        public static extern bool GetDevice2(out byte uiValue);

        private uint? deviceStatePower = null;

        public enum DeviceStatePower : uint
        {
            Wifi = 0x01,
            Gobi3G = 0x2,
            GPS = 0x4,
            Bluetooth = 0x10,
            WebCamRear = 0x20,
            AllLED = 0x80,

            Barcode = 0x100,
            WebCamFront = 0x200,
            RFID = 0x400,
            GPSAntenna = 0x800,
            ExpandUSB = 0x1000,
            ExpandCOM = 0x2000,

            //for initial all device state
            initAll = 0x8000
        };


        //0: The device power off     1: The device power on
        //lowwer byte
        //Bit7      Bit6        Bit5        Bit4        Bit3        Bit2    Bit1            Bit0
        //AllLED    -           WebCamRear  Bluetooth   -           GPS     Gobi3G          Wifi
        //upper byte
        //Bit7      Bit6        Bit5        Bit4        Bit3        Bit2    Bit1            Bit0
        //-         -           Expand COM  Expand USB  GPS Antenna RFID    WebCamFront     Barcode
        public uint GetDeviceStatePower()
        {
            byte temp;
            uint devicestate = 0;

            GetDevice(out temp);
            devicestate =  temp;

            GetDevice2(out temp);
            devicestate += (((uint)temp) << 8);

            deviceStatePower = devicestate;

            return devicestate;
        }

        public string ParseDeviceStatePowerCode()
        {
            string s_temp = "";

            if (deviceStatePower == null) return "No any device data!\n";
            else
            {
                foreach (uint i in Enum.GetValues(typeof(DeviceStatePower)))
                {
                    if((deviceStatePower & i) >= 1)
                    {
                        s_temp += (DeviceStatePower)i + "\n";
                    }
                }
            }

            return s_temp;
        }

        public bool SetDeviceStatePower(uint data)
        {
            byte data1 = (byte)data;
            byte data2 = (byte)(data>>8);

            if(SetDevice(data1) == false) return false;
            if(SetDevice2(data2) == false) return false;

            return true;
        }

    }
}
