# Relative layout
* Defines an area within which you can position and align child objects in relation to each other or the parent panel.  
* This example show button always at central  
```xml
<Page
    x:Class="HotTab_Win10.HotkeyControlPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:HotTab_Win10"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d"
    Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">

    <RelativePanel>
        <Button x:Name="information_btn" Content="Information" Margin="0,0,0,0" RelativePanel.AlignVerticalCenterWithPanel="True" RelativePanel.AlignHorizontalCenterWithPanel="True" Height="70" Width="120" FontSize="15"/>
    </RelativePanel>
</Page>
```  
# Absolutely layout
* This is the default layout. Here are call grib, align by row and colume.  
  
# Volume control
* Reference: https://gist.github.com/sverrirs/d099b34b7f72bb4fb386  

# Backlight control
* Reference: https://github.com/JeroenvO/screen-brightness/blob/master/BrightnessConsoleJvO/Class1.cs  

# Brightness control
* Changing graam to control brightness.  

# Reduce cpu usage rate 
* Using ```Application.Run();``` is batter than ```while (Console.ReadLine() != "0")```  

# Restrict one instance
* UWP have restricted one process in default  
* Console must use ```Mutex``` to restricted one process  
```C#
const string appName = "MyAppName";
            bool createdNew;

            Mutex mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                Console.WriteLine(appName + " is already running! Exiting the application.");
                //Console.ReadKey();
                return;
            }
```

# Hide console
* Set output type to Windows Application   
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/e4def112-6824-4b09-8e0c-6313fca53b27)
 

# Monitor key code
* About hook  
Hook can monitor thread. There are two part hook, one is local, another is global.  
When setting hook need to do three part, one is install hook, another is install hook, the other is goto next hook.  
```C#
[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);
```
* monitor key code globol hook
```C#
//Hook id
        private const int WH_KEYBOARD_LL = 13;           //Type of Hook - Low Level Keyboard
```
* set hook
```C#
private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
```
* key event
```C#
//Key id
        private const int WM_KEYDOWN = 0x0100;                    //Value passed on KeyDown
        private const int WM_SYSKEYDOWN = 0x0104;                  //Value passed on  KeyDown for menu (alt)
        private const int WM_KEYUP = 0x0101;                      //Value passed on KeyUp
```
* hook funcion
```C#
private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //SendMessageW(_hookID, WM_APPCOMMAND, _hookID, (IntPtr)APPCOMMAND_VOLUME_UP);
            // SendMessageW(HWND_BROADCAST, WM_APPCOMMAND, HWND_BROADCAST, (IntPtr)APPCOMMAND_VOLUME_UP);

            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)) //A Key was pressed down
            {
                    ...
            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)    //KeyUP
            {
                    ...
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam); //Call the next hook

        }
            
```


```


# Create thread
Create new thread can not make others function stop by using ```Application.Run(); ```  

* About making thread  
```C#
        new Thread(() => t_KeyCode()).Start();
```
```C#
private static void t_KeyCode()
        {
            HotKey.KeyCode();
        }
```

# Adjust volume
* send message  
There are two type message, one is SendMessage(......), the other is PostMessage(......).  
The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message
PostMessage is that post a message to a thread's message queue and return immediately  

* Disadvantage: Need a handle, but need a lot of CPU utilization

* An accessibility application can use SendMessage to send WM_APPCOMMAND messages to the shell to launch applications.
```C#
        private const int WM_APPCOMMAND = 0x319;
```
* application command
```C#
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
```
* volume up example
```C#
        SendMessageW(handle, WM_APPCOMMAND, IntPtr.Zero, (IntPtr)APPCOMMAND_VOLUME_UP);
```

# Launch other app
Default file from ```C:\WINDOWS\System32```
```C#
System.Diagnostics.Process.Start("calc");
System.Diagnostics.Process.Start("Taskmgr");

Process.Start("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe");
```

# Make font icon  
* Download font icon  
Reference - https://icofont.com/  
* Find out unicode in svg file 
```XML
<glyph glyph-name="ui-bluetooth" horiz-adv-x="1000" unicode="&#xec42;" d="M594 348c54-55 108-108 162-162 32-32 32-57-1-90-76-76-151-152-227-227-30-30-73-24-87 12-4 10-4 21-4 32 0 83 0 166 0 249v28c-8-8-14-12-18-16l-102-102c-30-30-60-34-83-10-23 24-20 52 10 82l178 177c22 22 22 33 0 55-61 62-123 123-184 185-30 30-22 74 16 87 22 7 40 0 55-15 36-36 71-71 106-107 6-5 12-10 20-18 1 9 2 14 2 20 0 81 0 161 0 242 0 7-1 14-2 21-3 24 7 42 27 53 21 10 41 8 58-9 10-9 19-18 29-28 70-70 140-140 211-211 27-27 27-54 0-81-49-50-99-99-148-148-6-6-12-12-18-19z m-55 100c37 37 75 74 107 106-35 35-72 72-107 108v-214z m0-418c38 39 76 76 109 109l-109 110v-219z" />
```
Or, changing to HTML for easyer search what you want  
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/0683dd86-1df5-442b-8196-c65a9656e9e4)

* install ttf  
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/91afe5c5-2b9c-4f8c-8c1b-140dbabecf50)

* Put ttf into assets  
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/73c07b2a-4b03-46de-bfcd-04b9028ff3c8)

* ttf propertice set copy always  
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/16204720-2d8d-4239-b874-e15dd2a88bf5)

* Add this into app.xmal  
Attention - Filename should defined  
```XAML
<Application
    x:Class="HotTab_Win10.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:local="using:HotTab_Win10">

    <Application.Resources>
        <FontFamily x:Key="CustomIconFont">Assets/icofont.ttf#icofont
        </FontFamily>
    </Application.Resources>

</Application>
```


* Add icon font into MainPage.xaml
```
        <Button x:Name="btn_webCamRear" Content="&#xeecf;" FontFamily="{StaticResource ResourceKey=CustomIconFont}" Margin="28,178,0,0" VerticalAlignment="Top" Width="45" Height="35" FontSize="25" Click="btn_webCamRear_Click">
        </Button>
```


