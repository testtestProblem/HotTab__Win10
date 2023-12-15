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
        public delegate void HotkeyFuncList();
        //public delegate void hotkeyFuncList2(string a);

        //public HotkeyFuncList[] hotkeyFuncLists = new HotkeyFuncList[6];
        //static public HotkeyFuncList[] hotkeyFuncLists = new HotkeyFuncList[]
        //{
        //    new hotkeyFuncLists=volumeUp();
        //}

        //TODO: Using delegate
        public void funcList(string s)
        {
            switch (s)
            {
                case "volumeUp":
                    volumeUp();
                    break;

                case "volumeDown":
                    volumeDown();
                    break;

                case "backlight20":
                    backlight20();
                    break;

                case "backlight100":
                    backlight100();
                    break;

                case "calculatorWin10":
                    calculatorWin10();
                    break;

                case "cmdWin10":
                    cmdWin10();
                    break;

                default:
                    customizeApp(s);
                    break;
            }
        }

        private void volumeUp()
        {
            int vol = (int)AudioManager.GetMasterVolume();
            AudioManager.SetMasterVolume((vol + 2) <= 100 ? vol + 2 : 100);
        }

        private void volumeDown()
        {
            int vol = (int)AudioManager.GetMasterVolume();
            AudioManager.SetMasterVolume((vol - 2) >= 0 ? vol - 2 : 0);
        }

        private void backlight20()
        {
            BacklightControl.SetBrightness(20);
        }

        private void backlight100()
        {
            BacklightControl.SetBrightness(100);
        }

        private void calculatorWin10()
        {
            System.Diagnostics.Process.Start("calc");
        }

        private void cmdWin10()
        {
            System.Diagnostics.Process.Start("cmd");
        }

        private void customizeApp(string path)
        {
            Process.Start(path);
        }

    }
}
