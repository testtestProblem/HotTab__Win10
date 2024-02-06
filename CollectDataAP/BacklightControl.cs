using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// reference: https://github.com/JeroenvO/screen-brightness/blob/master/BrightnessConsoleJvO/Class1.cs
/// </summary>
/// 
namespace CollectDataAP
{
    class BacklightControl
    {
        public byte[] bLevels; //array of valid level values
        private string[] args;
        //constructor
        public BacklightControl(string[] args)
        {
            this.args = args;
            Console.WriteLine("BL constructor");
            startFunction(args);
        }
        /*
        public static int Get()
        {
            var mclass = new ManagementClass("WmiMonitorBrightness")
            {
                Scope = new ManagementScope(@"\\.\root\wmi")
            };
            var instances = mclass.GetInstances();
            foreach (ManagementObject instance in instances)
            {
                return (byte)instance.GetPropertyValue("CurrentBrightness");
            }
            return 0;
        }*/

        /*
         * Check the arguments and call the functions
         * * */
        private void startFunction(string[] args)
        {
            bLevels = GetBrightnessLevels(); //get the level array for this system
            if (bLevels.Count() == 0) //"WmiMonitorBrightness" is not supported by the system
            {
                Console.WriteLine("Sorry, Your System does not support this brightness control...");
            }
            else
            {
                //if (Array.FindIndex(args, item => item.Contains("%")) > -1)
                if(args[1]=="%")
                {  //set brightness to level from args
                    string sPercent = args[Array.FindIndex(args, item => item.Contains("%"))];
                    if (sPercent.Length > 1)
                    {
                        int iPercent = Convert.ToInt16(sPercent.Split('%').ElementAt(0));
                        startup_brightness(iPercent);

                        Console.WriteLine("startup_brightness(iPercent); "+iPercent);
                    }
                }
                //if (Array.FindIndex(args, item => item.Contains("+")) > -1)
                if (args[1] == "+")
                { //increase brightess with a number
                    //string sIncreasePercent = args[Array.FindIndex(args, item => item.Contains("+"))];
                    //if (sIncreasePercent.Length > 1)
                    //{
                    //    int iIncreaesePercent = Convert.ToInt16(sIncreasePercent.Split('+').ElementAt(0));
                    int iIncreaesePercent = Convert.ToInt16(args[0]);    
                    int curBrightness = GetBrightness();
                        // iIncreaesePercent = 10;
                        startup_brightness(curBrightness + iIncreaesePercent);

                        Console.WriteLine("startup_brightness(curBrightness + iIncreaesePercent); " + (curBrightness + iIncreaesePercent));
                    //}
                }
                //if (Array.FindIndex(args, item => item.Contains("-")) > -1)
                if (args[1] == "-")
                { //decrease brightness with a number
                  //string sDecreasePercent = args[Array.FindIndex(args, item => item.Contains("-"))];

                    //if (sDecreasePercent.Length > 1)
                    //{
                    // int iDecreasePercent = Convert.ToInt16(sDecreasePercent.Split('-').ElementAt(0));
                    int iDecreasePercent = Convert.ToInt16(args[0]);
                    int curBrightness = GetBrightness();
                    startup_brightness(curBrightness - iDecreasePercent);

                    Console.WriteLine("startup_brightness(curBrightness - iDecreasePercent); " + (curBrightness + iDecreasePercent));
                    //}
                }
            }
        }
        /*
         * Convert the brightness percentage to a byte and set the brightness using setBrightness
         * */
        private byte startup_brightness(int iPercent)
        {
            if (iPercent >= 0 && iPercent <= bLevels[bLevels.Count() - 1])
            {
                byte level = 100;
                foreach (byte item in bLevels)
                {
                    if (item >= iPercent)
                    {
                        level = item;
                        break;
                    }
                }
                SetBrightness(level);
                return level;
                //check_brightness();
            }
            return 0;
        }
        /*
         * Returns the current brightness setting
         * This have some bug.
         * Don't use this function
         * */
        static public int GetBrightness()
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

            System.Management.ManagementObjectCollection moc = mos.Get();

            //store result
            byte curBrightness = 0;
            foreach (System.Management.ManagementObject o in moc)
            {
                curBrightness = (byte)o.GetPropertyValue("CurrentBrightness");
                break; //only work on the first object
            }

            moc.Dispose();
            mos.Dispose();

            return (int)curBrightness;
        }
        /*
         * Get the array of allowed brightnesslevels for this system
         * */
        static public byte[] GetBrightnessLevels()
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightness");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);
            byte[] BrightnessLevels = new byte[0];

            try
            {
                System.Management.ManagementObjectCollection moc = mos.Get();

                //store result
                foreach (System.Management.ManagementObject o in moc)
                {
                    BrightnessLevels = (byte[])o.GetPropertyValue("Level");
                    break; //only work on the first object
                }

                moc.Dispose();
                mos.Dispose();
            }
            catch (Exception)
            {
                Console.WriteLine("Sorry, Your System does not support this brightness control...");
            }

            return BrightnessLevels;
        }
        /*
         * Set the brightnesslevel to the targetBrightness
         * */
        static public void SetBrightness(byte targetBrightness)
        {
            //define scope (namespace)
            System.Management.ManagementScope s = new System.Management.ManagementScope("root\\WMI");

            //define query
            System.Management.SelectQuery q = new System.Management.SelectQuery("WmiMonitorBrightnessMethods");

            //output current brightness
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(s, q);

            System.Management.ManagementObjectCollection moc = mos.Get();

            foreach (System.Management.ManagementObject o in moc)
            {
                o.InvokeMethod("WmiSetBrightness", new Object[] { UInt32.MaxValue, targetBrightness }); //note the reversed order - won't work otherwise!
                break; //only work on the first object
            }

            moc.Dispose();
            mos.Dispose();
        }

        /// <summary>
        /// Get bacllight brightness
        /// </summary>
        /// <returns> int 0 ~ 100; error is -1 </returns>
        static public int getBrighness2()
        {
            ManagementScope scope;
            SelectQuery query;

            int data = -1;

            scope = new ManagementScope("root\\WMI");
            query = new SelectQuery("SELECT * FROM WmiMonitorBrightness");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                using (ManagementObjectCollection objectCollection = searcher.Get())
                {
                    foreach (ManagementObject mObj in objectCollection)
                    {
                        Console.WriteLine(mObj.ClassPath);
                        foreach (var item in mObj.Properties)
                        {
                            Console.WriteLine(item.Name + " " + item.Value.ToString());
                            if (item.Name == "CurrentBrightness")
                            {
                                //Do something with CurrentBrightness
                                data = (Convert.ToInt32(item.Value));
                                //if (data > 50) data = 100;
                                //else data = data * 2;
                                //return data <= 100 ? data : 100;
                            }
                        }
                    }
                }
               // return -1;
            }

            return data <= 100 ? data : 100;
        }
    }
}
