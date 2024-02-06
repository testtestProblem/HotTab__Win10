using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoPlayerController;

namespace CollectDataAP
{
    class HotKey
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        [DllImport(@"WMIO2.dll")]
        public static extern bool ModeOpen(uint uiMode);

        //Hook id
        private const int WH_KEYBOARD_LL = 13;                    //Type of Hook - Low Level Keyboard
        private const int WH_KEYBOARD = 2;                    //Type of Hook - Low Level Keyboard

        //Key id
        private const int WM_KEYDOWN = 0x0100;                    //Value passed on KeyDown
        private const int WM_SYSKEYDOWN = 0x0104;                  //Value passed on  KeyDown for menu (alt)
        private const int WM_KEYUP = 0x0101;                      //Value passed on KeyUp

        //Key flag for hotkey 
        private static bool menuUp = false;                 //Bool to use as a flag for control key
        private static bool controlUp = false;                 //Bool to use as a flag for control key
        private static bool shiftUp = false;                 //Bool to use as a flag for control key

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        private static IntPtr HWND_BROADCAST = (IntPtr)0xffff;

        private static LowLevelKeyboardProc _proc = HookCallback; //The function called when a key is pressed
        private static IntPtr _hookID = IntPtr.Zero;

        private static IntPtr handle;

        //start monitor key code
        public static void KeyCode()
        {
            //Brightness.InitializeClass();

            ModeOpen(2);    //choose hotkey mode 2

            //HotKey.handle = Process.GetCurrentProcess().Handle;
            //for sove package can not find handle problem
            HotKey.handle = Program.handle;

            _hookID = SetHook(_proc);    //Set our hook

            Application.Run();         //Start a standard application method loop 
        }


        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //SendMessageW(_hookID, WM_APPCOMMAND, _hookID, (IntPtr)APPCOMMAND_VOLUME_UP);
            //SendMessageW(HWND_BROADCAST, WM_APPCOMMAND, HWND_BROADCAST, (IntPtr)APPCOMMAND_VOLUME_UP);
            //SendMessageW(_hookID, WM_APPCOMMAND, IntPtr.Zero, (IntPtr)APPCOMMAND_VOLUME_UP);
            
            Process[] allProcess = Process.GetProcesses();
            //HotKey.handle = allProcess.First().Handle;

            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)) //A Key was pressed down
            {
                int vkCode = Marshal.ReadInt32(lParam);           //Get the keycode
                string theKey = ((Keys)vkCode).ToString();        //Name of the key
                Console.WriteLine("Key make " + theKey);
                /*
                if (theKey.Contains("A"))
                {
                    SendMessageW(handle, WM_APPCOMMAND, IntPtr.Zero, (IntPtr)APPCOMMAND_VOLUME_UP);
                }
                else if (theKey.Contains("B"))
                {
                    SendMessageW(handle, WM_APPCOMMAND, IntPtr.Zero, (IntPtr)APPCOMMAND_VOLUME_DOWN);
                }
                else if (theKey.Contains("C"))
                {
                    //Brightness.SetBrightness(126);
                    BacklightControl.SetBrightness(20);
                }
                else if (theKey.Contains("D"))
                {
                    //Brightness.SetBrightness(256);
                    BacklightControl.SetBrightness(100);
                }
                else if (theKey.Contains("E"))
                {
                    //Brightness.SetBrightness(10);
                    BacklightControl.SetBrightness(60);
                }
                else if (theKey.Contains("F"))
                {
                    System.Diagnostics.Process.Start("calc");
                }
                else if (theKey.Contains("G"))
                {
                    Process.Start("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe");
                }
                else if (theKey.Contains("H"))
                {
                    System.Diagnostics.Process.Start("cmd");
                }
                else if (theKey.Contains("I"))
                {
                    System.Diagnostics.Process.Start("Taskmgr");
                }
                else if (theKey.Contains("J"))
                {
                    System.Diagnostics.Process.Start("mspaint");
                }
                else if (theKey.Contains("K"))
                {
                    int vol = (int)AudioManager.GetMasterVolume();
                    AudioManager.SetMasterVolume((vol + 2) <= 100 ? vol + 2 : 100);
                }
                else if (theKey.Contains("L"))
                {
                    int vol = (int)AudioManager.GetMasterVolume();
                    AudioManager.SetMasterVolume((vol - 2) >= 0 ? vol - 2 : 0);
                }
                else if (theKey == "Escape")                           //If they press escape
                {
                    UnhookWindowsHookEx(_hookID);                 //Release our hook
                    Environment.Exit(0);                          //Exit our program
                }*/
            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)    //KeyUP
            {
                int vkCode = Marshal.ReadInt32(lParam);           //Get Keycode
                string theKey = ((Keys)vkCode).ToString();        //Get Key name
                Console.WriteLine("Key break " + theKey);

                if (menuUp == true)
                {
                    if (theKey.Contains("D0") || theKey.Contains("D3") || theKey.Contains("D4") || theKey.Contains("D5") || theKey.Contains("D6")
                        || theKey.Contains("D7") || theKey.Contains("D") || theKey.Contains("M") || theKey.Contains("RShiftKey") || theKey.Contains("RControlKey"))
                    {

                    }
                    else
                    {
                        menuUp = false;
                        controlUp = false;
                        shiftUp = false;
                    }
                }
                if (menuUp == true && controlUp == true && shiftUp == true)
                {
                    if (theKey.Contains("D0"))
                    {
                        Console.WriteLine("Menu Key Pressed");
                    }
                    else if (theKey.Contains("D3"))
                    {
                        Console.WriteLine("F1 Key Short Press");
                        //SendMessageW(handle, WM_APPCOMMAND, IntPtr.Zero, (IntPtr)APPCOMMAND_VOLUME_UP); 
                        //System.Diagnostics.Process.Start("Taskmgr");
                        HotkeyFunc.func(HotkeyFunc.funcName[0]);
                        
                        //for test
                        //BacklightControl.getBrighness2();
                    }
                    else if (theKey.Contains("D4"))
                    {
                        Console.WriteLine("F1 Key Long Press");
                        HotkeyFunc.func(HotkeyFunc.funcName[4]);
                    }
                    else if (theKey.Contains("D5"))
                    {
                        Console.WriteLine("F2 Key Short Press");
                        //SendMessageW(handle, WM_APPCOMMAND, IntPtr.Zero, (IntPtr)APPCOMMAND_VOLUME_DOWN);
                        //System.Diagnostics.Process.Start("mspaint");
                        HotkeyFunc.func(HotkeyFunc.funcName[1]);
                    }
                    else if (theKey.Contains("D6"))
                    {
                        Console.WriteLine("F2 Key Long Presss");
                        HotkeyFunc.func(HotkeyFunc.funcName[5]);
                    }
                    else if (theKey.Contains("D7"))
                    {
                        Console.WriteLine("F3 Key Short Press");
                        HotkeyFunc.func(HotkeyFunc.funcName[2]);
                    }
                    else if (theKey.Contains("D8"))
                    {
                        Console.WriteLine("F3 Key Long Presss");
                        HotkeyFunc.func(HotkeyFunc.funcName[3]);
                    }
                    else if (theKey.Contains("D"))
                    {
                        Console.WriteLine("Home Key Short Press");
                    }
                    else if (theKey.Contains("M"))
                    {
                        Console.WriteLine("Home Key Long Press");
                    }

                    menuUp = false;
                    controlUp = false;
                    shiftUp = false;
                }

                if (theKey.Contains("ShiftKey") || theKey.Contains("RShiftKey") || theKey.Contains("LShiftKey") && menuUp == true && controlUp == true)
                {
                    Console.WriteLine("HotTab ShiftHey Key Break");
                    shiftUp = true;
                }

                if (theKey.Contains("ControlKey") || theKey.Contains("RControlKey") || theKey.Contains("LControlKey") && menuUp == true)
                {
                    Console.WriteLine("HotTab ControlKey Key Break");
                    controlUp = true;
                }

                if (theKey.Contains("Menu") || theKey.Contains("RMenu") || theKey.Contains("LMenu"))
                {
                    Console.WriteLine("HotTab Menu Key Break");
                    menuUp = true;
                }

            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam); //Call the next hook
        }
    }
}
