using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CollectDataAP
{
    class GetInfoBattery
    {
        [DllImport(@"WMIO2.dll")]
        private static extern bool GetBattery1SpecificInfo(uint uiCommand, out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        private static extern bool GetBattery2SpecificInfo(uint uiCommand, out uint uiValue);

        [DllImport(@"WMIO2.dll")]
        private static extern bool GetBackupBatteryStatus(out uint uiValue);

        static public uint getBatRelativeCharge(int batIndex) 
        {
            uint data = 0;
            if (batIndex == 1)
            {
                if (GetBattery1SpecificInfo(0x0D, out data)) return data;
                else return 0;
            }
            else if(batIndex == 2)
            {
                if (GetBattery2SpecificInfo(0x0D, out data)) return data;
                else return 0;
            }

            return 0;
        }


    }
}
