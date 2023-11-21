using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace CollectDataAP
{
    class Program
    {
        static void Main(string[] args)
        {
            //LowLevelKeyboardProc _proc = HookCallback; //The function called when a key is pressed
            //IntPtr _hookID = IntPtr.Zero;

            //IntPtr handle;

            //handle = Process.GetCurrentProcess().MainWindowHandle;

            //hookID = SetHook(_proc);   //Set our hook
            //Application.Run();         //Start a standard application method loop 

            new Thread(() => t_KeyCode()).Start();


            Connect2UWP connect2UWP = new Connect2UWP();
            DeviceState deviceState = new DeviceState();

            string choice = "";

            connect2UWP.InitializeAppServiceConnection();


            Console.WriteLine("Choose the number you want:\n" +
                "[1] Get device state\n" +
                "[2] Send device state to UWP\n" +
                "[3] Set device state\n" +
                "[4] Parse device state code\n" +
                "[5] Send data to UWP for test\n" +
                "[0] Exit");

            uint deviceStateCode = deviceState.GetDeviceStatePower();
            Console.WriteLine("\nDevice state code: " + deviceStateCode.ToString());
            Console.WriteLine(deviceState.ParseDeviceStatePowerCode());

            while ((choice = Console.ReadLine()) != "0")
            {
                if (choice == "1")
                {
                    deviceStateCode = deviceState.GetDeviceStatePower();
                    Console.WriteLine("Device state code: " + deviceStateCode.ToString());
                    Console.WriteLine(deviceState.ParseDeviceStatePowerCode());
                }
                if (choice == "2")
                {
                    deviceStateCode = deviceState.GetDeviceStatePower();
                    Console.WriteLine("Device state code: " + deviceStateCode.ToString());
                    Console.WriteLine(deviceState.ParseDeviceStatePowerCode());

                    connect2UWP.SendData2UWP(deviceStateCode);
                }
                else if (choice == "3")
                {
                    string temp = Console.ReadLine();

                    deviceState.SetDeviceStatePower((uint)Convert.ToInt32(temp));
                    Console.WriteLine("OK");
                }
                else if (choice == "4")
                {
                    Console.WriteLine(deviceState.ParseDeviceStatePowerCode());
                }
                else if (choice == "5")
                {
                    connect2UWP.Send2UWP_2("Hi!", "UWP");
                }
            }
        }

        private static void t_KeyCode()
        {
            HotKey.KeyCode();
        }
    }
}
