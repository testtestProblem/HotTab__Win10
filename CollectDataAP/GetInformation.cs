using CollectDataAP;
using GlobalVar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Win8Hottab;

namespace CollectDataAP
{
    class GetInformation
    {
        public void InitializeWMIHandler()
        {
            HotTabWMIInformation wmiHandler = new HotTabWMIInformation();
            try
            {
                //OK1 BIOS Version
                GlobalVariable.sBIOSVersion = wmiHandler.GetWMI_BIOSVersion();

                //OK1 OS Name
                //HotTabDLL.OSName = wmiHandler.Get_OSName();

                //Units SN
                GlobalVariable.sUnitsSN = wmiHandler.GetWMI_BIOSSerialNumber();

                //MainBoardVersion
                GlobalVariable.sMainBoardVersion = wmiHandler.GetWMI_BIOSMainBoard();

                //OS Version
                //HotTabRegistry.RegistryRead("SOFTWARE", "OSVersion", ref GlobalVariable.OSVersion);

                if (GlobalVariable.sBIOSVersion == "")
                {
                    Environment.Exit(1);
                }

                //GlobalVariable.sBIOSVersion = GlobalVariable.sBIOSVersion.Trim().Remove(0, 0);

                HotTabDLL.WinIO_GetECMBVersion(out GlobalVariable.sECMBVersion);
                HotTabDLL.WinIO_GetECVersion(out GlobalVariable.sECVersion);
            }
            catch
            {
                //Do Nothing
            }  
        }


        //if return 0 is error
        //if return 1 is good
        public int InitGlobalVariable()
        {
            if (!GlobalVariable.Load_HottabCfg())
            {
                Console.WriteLine("HottabCfg.ini not exist!");
            }
            return 1;
        }

        public string GetApplicationVersion()
        {
            char[] cdelimiterChars2 = { '.' };
            string[] words2 = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split(cdelimiterChars2);

            return words2[0].ToString() + "." + words2[1].ToString() + "." + words2[2].ToString();
        }

        // Show OS BIOS EC data
        public string GetInfomation()
        {
            string tempForUWP = "", tempForOutput = "";

            string sModelName = "";
            if (GlobalVariable.sECMBVersion == "IBWH")
            {
                sModelName = "A8XV1";
            }
            else if (GlobalVariable.sECMBVersion == "ID8H")
            {
                sModelName = "A7";
            }
            else if (GlobalVariable.sECMBVersion == "ID82")
            {
                sModelName = "A10XV1";
            }
            else if (GlobalVariable.sECMBVersion == "IB10")
            {
                if ((GlobalVariable.sECVersion[0] == '0') || (GlobalVariable.sECVersion[0] == '1'))
                {
                    sModelName = "A10XV2";
                }
                else
                {
                    sModelName = "A10XV3";
                }
            }
            else
            {
                sModelName = "";
            }

            if ((GlobalVariable.sBIOSVersion[0] < 0x30) || (GlobalVariable.sBIOSVersion[0] > 0x39))
                GlobalVariable.sBIOSVersion = GlobalVariable.sBIOSVersion.Substring(1, GlobalVariable.sBIOSVersion.Length - 1);

            Console.WriteLine(tempForOutput = ("BIOS Ver.: " + sModelName + "(" + GlobalVariable.sBIOSVersion + ")"));
            tempForUWP += tempForOutput + "\n";
            Console.WriteLine(tempForOutput = ("EC Ver.: " + sModelName + "(" + GlobalVariable.sECVersion + ")"));
            tempForUWP += tempForOutput + "\n";

            if ((GlobalVariable.sECVersion[0] == '0') || (GlobalVariable.sECVersion[0] == '1'))
            {
                Console.WriteLine(tempForOutput = ("HotTab Ver.: " + "A10XV2" + "(" + GetApplicationVersion() + ")"));
                tempForUWP += tempForOutput + "\n";
            }
            else
            {
                Console.WriteLine(tempForOutput = ("HotTab Ver.: " + "A10XV3" + "(" + GetApplicationVersion() + ")"));
                tempForUWP += tempForOutput + "\n";
            }
            Console.WriteLine(tempForOutput = ("OS Ver.: " + GlobalVariable.OSVersion));
            tempForUWP += tempForOutput + "\n";
            Console.WriteLine(tempForOutput = ("Units Ver.: " + GlobalVariable.sUnitsSN));
            tempForUWP += tempForOutput + "\n";

            Console.WriteLine("");
            
            return tempForUWP;
        }


    }
}
