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
        /*
        public delegate void HotkeyFuncList();
        public delegate void HotkeyFuncCustomiz(string a);

        static public HotkeyFuncList[] hotkeyFuncLists = new HotkeyFuncList[6];
        public HotkeyFuncCustomiz hotkeyFuncCustomiz;

        public HotkeyFunc()
        {
            hotkeyFuncLists[0] = volumeUp;
            hotkeyFuncLists[1] = volumeDown;
            hotkeyFuncLists[2] = backlight20;
            hotkeyFuncLists[3] = backlight100;
            hotkeyFuncLists[4] = calculatorWin10;
            hotkeyFuncLists[5] = cmdWin10;
        }
        */
        //static public HotkeyFuncList[] hotkeyFuncLists = new HotkeyFuncList[]
        //{
        //    new hotkeyFuncLists=volumeUp();
        //}

        public enum HotkeyList : uint
        {
            f1Short = 0x01,
            f2Short = 0x02,
            f3Short = 0x04,

            f1Long = 0x08,
            f2Long = 0x10,
            f3Long = 0x20,

            noValue = 0x1000
        };

        public enum HotkeyList2 : uint
        {
            F1S = 0x01,
            F2S = 0x02,
            F3S = 0x04,

            F1L = 0x08,
            F2L = 0x10,
            F3L = 0x20,

            noValue = 0x1000
        };

        public enum FunctionList : uint
        {
            volumeUp = 0x01,
            volumeDown = 0x02,

            backlight20 = 0x04,
            backlight100 = 0x08,

            Calc = 0x10,
            cmd = 0x20,

            noValue = 0x1000
        };

        static public string[] funcName = {
            "volumeUp", "volumeDown", "backlight20",
            "backlight100", "calculatorWin10", "cmdWin10" };

        static private string[] funcNameDefault = {
            "volumeUp", "volumeDown", "backlight20",
            "backlight100", "calculatorWin10", "cmdWin10" };

        static public void defaultHotketFunc()
        {            
            funcName[0] = funcNameDefault[0];
            funcName[1] = funcNameDefault[1];
            funcName[2] = funcNameDefault[2];
            funcName[3] = funcNameDefault[3];
            funcName[4] = funcNameDefault[4];
            funcName[5] = funcNameDefault[5];
        }

        static public void changeFuncName(HotkeyList key, FunctionList func)
        {
            Dictionary<uint, string> funcNameDefault2 = new Dictionary<uint, string>();
            funcNameDefault2.Add(0x01, "volumeUp");
            funcNameDefault2.Add(0x02, "volumeDown");
            funcNameDefault2.Add(0x04, "backlight20");
            funcNameDefault2.Add(0x08, "backlight100");
            funcNameDefault2.Add(0x10, "calculatorWin10");
            funcNameDefault2.Add(0x20, "cmdWin10");
            funcNameDefault2.Add(0x1000, "");

            Dictionary<uint, uint> hotkeyState = new Dictionary<uint, uint>();
            hotkeyState.Add(0x01, 0);   //f1Short
            hotkeyState.Add(0x02, 1);   //f2Short
            hotkeyState.Add(0x04, 2);   //f3Short
            hotkeyState.Add(0x08, 4);   //f1Long
            hotkeyState.Add(0x10, 5);   //f2Long
            hotkeyState.Add(0x20, 3);   //f3Long

            funcName[hotkeyState[(uint)key]] = funcNameDefault2[(uint)func];
        }

        static public void changeFuncName(HotkeyList key, string func)
        {
            Dictionary<uint, string> funcNameDefault2 = new Dictionary<uint, string>();
            funcNameDefault2.Add(0x01, "volumeUp");
            funcNameDefault2.Add(0x02, "volumeDown");
            funcNameDefault2.Add(0x04, "backlight20");
            funcNameDefault2.Add(0x08, "backlight100");
            funcNameDefault2.Add(0x10, "calculatorWin10");
            funcNameDefault2.Add(0x20, "cmdWin10");
            funcNameDefault2.Add(0x1000, "");

            Dictionary<uint, uint> hotkeyState = new Dictionary<uint, uint>();
            hotkeyState.Add(0x01, 0);   //f1Short
            hotkeyState.Add(0x02, 1);   //f2Short
            hotkeyState.Add(0x04, 2);   //f3Short
            hotkeyState.Add(0x08, 4);   //f1Long
            hotkeyState.Add(0x10, 5);   //f2Long
            hotkeyState.Add(0x20, 3);   //f3Long

            funcName[hotkeyState[(uint)key]] = func;
        }

        //TODO: Using delegate
        static public void func(string s)
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

                case "Calc":
                case "calculatorWin10":
                    calculatorWin10();
                    break;

                case "cmd":
                case "cmdWin10":
                    cmdWin10();
                    break;

                case "noValue":
                case "":
                    break;

                default:
                    customizeApp(s);
                    break;
            }
        }

        static private void volumeUp()
        {
            int vol = (int)AudioManager.GetMasterVolume();
            AudioManager.SetMasterVolume((vol + 5) <= 100 ? vol + 5 : 100);
        }

        static private void volumeDown()
        {
            int vol = (int)AudioManager.GetMasterVolume();
            AudioManager.SetMasterVolume((vol - 5) >= 0 ? vol - 5 : 0);
        }

        static private void backlight20()
        {
            //string[] a = { "1", "-" };
            //BacklightControl bl = new BacklightControl(a);
            /*
            byte[] b = BacklightControl.GetBrightnessLevels();
            BacklightControl.SetBrightness((byte)((b[0] - 2) >= 0 ? b[0] - 2 : 0));

            Console.WriteLine("BacklightControl.GetBrightness() -> "+ b[0]);
            */

            //BacklightControl.SetBrightness(20);
            int bl=0;
            bl=BacklightControl.getBrighness2();
            BacklightControl.SetBrightness((byte)((bl - 5) >= 0 ? bl - 5 : 0));
        }

        static private void backlight100()
        {
            //string[] a = { "1", "+" };
            //BacklightControl bl = new BacklightControl(a);

            /*byte[] b = BacklightControl.GetBrightnessLevels();
            BacklightControl.SetBrightness((byte)((b[0] + 2) <= 1000 ? b[0] + 2 : 1000));
          
            Console.WriteLine("BacklightControl.GetBrightness() -> " + b[0]);
        */
            //BacklightControl.SetBrightness(100);

            int bl = 0;
            bl = BacklightControl.getBrighness2();
            BacklightControl.SetBrightness((byte)((bl + 5) <= 100 ? bl + 5 : 100));
        }

        static private void calculatorWin10()
        {
            System.Diagnostics.Process.Start("calc");
        }

        static private void cmdWin10()
        {
            System.Diagnostics.Process.Start("cmd");
        }

        static private void customizeApp(string path)
        {
            Process.Start(path);
        } 
    }
}
