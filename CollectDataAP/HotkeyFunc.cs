using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPlayerController;

namespace CollectDataAP
{
    class HotkeyFunc
    {
        public void volumeUp()
        {
            int vol = (int)AudioManager.GetMasterVolume();
            AudioManager.SetMasterVolume((vol + 2) <= 100 ? vol + 2 : 100);
        }

        public void volumeDown()
        {
            int vol = (int)AudioManager.GetMasterVolume();
            AudioManager.SetMasterVolume((vol - 2) >= 0 ? vol - 2 : 0);
        }

        public void backlight20()
        {
            BacklightControl.SetBrightness(20);
        }

        public void backlight100()
        {
            BacklightControl.SetBrightness(100);
        }

        public void calculatorWin10()
        {
            System.Diagnostics.Process.Start("calc");
        }

        public void cmdWin10()
        {
            System.Diagnostics.Process.Start("calc");
        }
        
        public void customizeApp(string path)
        {
            Process.Start(path);
        }

    }
}
