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
using Windows.UI.Popups;

namespace CollectDataAP
{
    class Program
    {
        public static IntPtr handle;

        public const string appName = "Hottab_win10_test";
        public static bool createdNew;
        public static Mutex mutex = new Mutex(true, appName, out createdNew);

        static void Main(string[] args)
        {
            //MutexForConsole();
            
            

            //Mutex mutex = new Mutex(true, appName, out createdNew);
            
            //Mutex.OpenExisting
            if (!createdNew)
            {
                //MessageBox.Show(appName + " is already running! Exiting the application.");
                MessageBox.Show("Hottab" + " is already running! Please exiting the application.", "Hottab",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //Console.ReadKey();
                return;
            }
            else
            {
                //MessageBox.Show(appName + "Start is running!");
            }

            handle = Process.GetCurrentProcess().Handle;

            //add thread for hotkey
            new Thread(() => t_KeyCode()).Start();


            Connect2UWP connect2UWP = new Connect2UWP();
            DeviceState deviceState = new DeviceState();

            string choice = "";

            connect2UWP.InitializeAppServiceConnection();

            Tommy.Tommy_Start();

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

            //reduce cpu usage rate
            Application.Run();

            /*
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
            }*/
        }

        //if have second console, closed it
        static void MutexForConsole()
        {
            const string appName = "MyAppName";
            bool createdNew;

            Mutex mutex = new Mutex(true, appName, out createdNew);
            
            if (!createdNew)
            {
                Console.WriteLine(appName + " is already running! Exiting the application.");
                //Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private static async void ErrorMessage()
        {
            // Create the message dialog and set its content
            var messageDialog = new MessageDialog("Can not get mutex!");

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Try again"));
            messageDialog.Commands.Add(new UICommand(
                "Close"));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        private static void t_KeyCode()
        {
            HotKey.KeyCode();
        }
    }
}
