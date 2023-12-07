using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace HotTab_Device2
{
    class DeviceState
    {
        /// <summary>
        /// 0: The device power off     1: The device power on
        /// lowwer byte
        /// Bit7      Bit6        Bit5        Bit4        Bit3        Bit2    Bit1   
        /// AllLED    -           WebCamRear  Bluetooth   -           GPS     Gobi3G
        /// upper byte
        /// Bit7      Bit6        Bit5        Bit4        Bit3        Bit2    Bit1   
        /// -         -           Expand COM  Expand USB  GPS Antenna RFID    WebCamFront
        /// </summary>
        public enum Modules : uint
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

        /*
        public void ParseDeviceStatePowerCode(uint deviceStatePower)
        {
            if ((deviceStatePower & (uint)Modules.Wifi) == (uint)Modules.Wifi)
            {
                btn_wifi.Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(@"/Assets/device/enable/G_Wi-Fi.bmp", UriKind.RelativeOrAbsolute)),
                    Stretch = Stretch.Uniform
                };
            }
        }
        */



    }

}
