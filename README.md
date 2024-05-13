# HotTab Win10
* Function description: HotTab can turn on/off device power; can show hotkey function also can change it; can show battery remain power and others information.  
* Requirement: Above windows10 version 1809; X64 architecture  
  Should put KillProcess_forHotTab.exe in C:\Program Files\HotTab    
  Should put RegistryKey.exe in C:\Program Files\HotTab  
  Should put HottabCfg.ini in C:\Program Files\HotTab  
* UI illustration

-- Main  
<img width="601" alt="hottabMain" src="https://github.com/testtestProblem/HotTab_Win10/assets/107662393/a1445ed4-4a2b-4803-bb50-83c7652c6f7c">

-- Device state  
<img width="601" alt="hottabDevice" src="https://github.com/testtestProblem/HotTab_Win10/assets/107662393/715e8763-b49b-4a55-aa8e-383f0a449fae">  

-- Hotkey no change function  
<img width="601" alt="hottabHotkeyNoChoose" src="https://github.com/testtestProblem/HotTab_Win10/assets/107662393/21fde1ec-dd6d-4b51-b08c-0e97701a5182">  

-- Hotkey change function  
<img width="601" alt="hottabHotkeyChoose" src="https://github.com/testtestProblem/HotTab_Win10/assets/107662393/c91c3a68-f4ad-44e9-a98e-05d3fac46e34">  

-- Startup  
<img width="511" alt="enableStartup" src="https://github.com/testtestProblem/HotTab_Win10/assets/107662393/29951430-991a-4e7d-a807-aabc3b479cb9">  

-- Information  
<img width="601" alt="hottabInformation" src="https://github.com/testtestProblem/HotTab_Win10/assets/107662393/47b9d6d3-7624-48e7-8111-b4c7ff34a489">  

# Process start
* Start calculator which win10 default app ```Process.Start("calc");```, default path is```C:\Windows\System32```
* Start chrome which using app path to start ```Process.Start("C:\ProgramData\Microsoft\Windows\Start Menu\Programs");``` 


# Win32 allow administrator
* Due to UWP will limmit access some resource, Using fullTrustlouncher WIN32 can sove it.  
* First. Make console as administrator.   
Add Application Manifest File(Windows only), modify execution level to ```<requestedExecutionLevel level="highestAvailable" uiAccess="false" />```.   
I don't know why ```<requestedExecutionLevel  level="requireAdministrator" uiAccess="false" />``` will show error  
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/151571fe-8f75-4b43-a235-ff41f2e96bce)  

* Change WapProj Package.appxmanifest  
```xml
<Package
  xmlns="http://schemas.microsoft.com/appx/manifest/foundation/windows10"
  xmlns:uap="http://schemas.microsoft.com/appx/manifest/uap/windows10"
  xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities"
  
  xmlns:mp="http://schemas.microsoft.com/appx/2014/phone/manifest"
  xmlns:desktop="http://schemas.microsoft.com/appx/manifest/desktop/windows10"
  xmlns:uap5="http://schemas.microsoft.com/appx/manifest/uap/windows10/5"
	
  xmlns:uap3="http://schemas.microsoft.com/appx/manifest/uap/windows10/3"
  IgnorableNamespaces="uap mp desktop rescap uap3">
```  
```xml
<uap:Extension Category="windows.appService">
        <uap:AppService Name="SampleInteropService" />
</uap:Extension>
						
<desktop:Extension Category="windows.fullTrustProcess" Executable="CollectDataAP\CollectDataAP.exe">
        <desktop:FullTrustProcess>
                <desktop:ParameterGroup GroupId="User" Parameters="/user" />
                <desktop:ParameterGroup GroupId="Admin" Parameters="/admin" />
        </desktop:FullTrustProcess>
</desktop:Extension>
```  
-- allow Elevation  
```xml  
<Capabilities>
    <Capability Name="internetClient" />
    <rescap:Capability Name="runFullTrust" />
    <rescap:Capability Name="allowElevation" />
</Capabilities>
```
# CMD Kill process
* Get current user process: ```tasklist | more```
* Kill process: ```taskkill /IM “process name” /F```
* Kill multiple process: ```taskkill /IM "Process Name" /IM "Process Name" /F```; using PID ```PID taskkill /PID PID  /PID PID /F``` 



# Install appx to all users and new users
* ```PowerShell C:\> Add-AppxProvisionedPackage -Online -FolderPath "c:\Appx"``` This will get error(no applicable main package was found for this platform). I don't know how to sove it.
* ```Add-AppxProvisionedPackage -Online -DependencyPackagePath "all_dependency_package_path","","" -PackagePath "appxbundle_or_msixbundle_path" -SkipLicense``` It can successfully install appx to all users and new users
* Reference: https://stackoverflow.com/questions/57266123/can-i-use-add-appxprovisionedpackage-to-install-a-denpendency-appx-pacakge  

# Object type tranform to other type
* Error code: specified cast is not valid
* Due to force transform Object type to int will cause some unknow error. Using function like ```Convert.ToInt32(item.Value)``` will sove this problem.  
* Reference: https://blog.csdn.net/u011644138/article/details/106006732


# Detect S3 and sign in/out
* This class provides access to system event notifications. This class cannot be inherited.
* Because this is a static event, you must detach your event handlers when your application is disposed, or memory leaks will result.
* Reference: https://learn.microsoft.com/en-us/dotnet/api/microsoft.win32.systemevents?view=dotnet-plat-ext-8.0

* Detect sleep mode  
Add event  
```C#
SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
```
```C#
static public void SystemEvents_PowerModeChanged(object sender, EventArgs e)
        {
            Trace.WriteLine("SimpleService.PowerModeChanged", "Power mode changed; time: " +
                DateTime.Now.ToLongTimeString());

            HotKey.ModeOpen(2);    //choose hotkey mode 2
        }
```

* Detect sign in/out  
```C#
SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
```
```C#
static public void SystemEvents_SessionSwitch(object sender, EventArgs e)
        {
           // Trace.WriteLine("SimpleService.PowerModeChanged", "Power mode changed; time: " +
            //    DateTime.Now.ToLongTimeString());

            HotKey.ModeOpen(2);    //choose hotkey mode 2
        }
```


# Windows register
* The class Register is static
* If use UWP can not write register, because it permissions has been limmit.
* ```Registry.GetValue(keyPath, key, "noValue")``` In UWP, When first run app, because the path not exit, it will get null and create path. If have path exist but no value, will get "noValue". 
* ```Registry.SetValue(keyPath, key, value);``` If no path or value, it will create it.
```C#
class RegistryWindows
    {
        //private Registry registryKey;
        private const string userRoot = "HKEY_CURRENT_USER";
        private const string subkey = @"SOFTWARE\HotTabTest1";
        private static string keyPath = userRoot + "\\" + subkey;

        public static void setValue(string key, object value)
        {
            Registry.SetValue(keyPath, key, value);
        }

        public static string getValue(string key)
        {
            return (string)Registry.GetValue(keyPath, key, "noValue");
        }
    }
```
> [!WARNING]
> If restart app, the value not exist, the path maybe exist. Because of UWP sideload, the permission is not enough.

* RegisterKey represents a key-level node in the Windows registry. 

# XAML UWP button background while focuing will disappear 
* Qustion: While using mounce fouce button, the background picture will disappear.
* Soultion 1: This have a problem. It will apply all button. 
```xml
<Page.Resources>
    <StaticResource x:Key="ButtonBackground" ResourceKey="MyMyImageBrush" />
    <StaticResource x:Key="ButtonBackgroundPointerOver" ResourceKey="MyMyImageBrush" />
    <StaticResource x:Key="ButtonBackgroundPressed" ResourceKey="SystemControlBackgroundBaseMediumLowBrush" />
    <ImageBrush x:Key="MyMyImageBrush" ImageSource="ms-appx:///assets/Button.png" />
</Page.Resources>
```
* Solution 2: Disable mounce fouce event
```xml
    <Page.Resources>
        <Style TargetType="Button">
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <ContentPresenter x:Name="ContentPresenter"
                                              Padding="{TemplateBinding Padding}"
                                              HorizontalContentAlignment="{TemplateBinding HorizontalContentAlignment}"
                                              VerticalContentAlignment="{TemplateBinding VerticalContentAlignment}"
                                              AutomationProperties.AccessibilityView="Raw"
                                              Background="{TemplateBinding Background}"
                                              BorderBrush="{TemplateBinding BorderBrush}"
                                              BorderThickness="{TemplateBinding BorderThickness}"
                                              Content="{TemplateBinding Content}"
                                              ContentTemplate="{TemplateBinding ContentTemplate}"
                                              ContentTransitions="{TemplateBinding ContentTransitions}"
                                              CornerRadius="{TemplateBinding CornerRadius}">
                            <VisualStateManager.VisualStateGroups>
                                <VisualStateGroup x:Name="CommonStates">
                                    <VisualState x:Name="Normal">
                                        <Storyboard>
                                            <PointerUpThemeAnimation Storyboard.TargetName="ContentPresenter" />
                                            <ObjectAnimationUsingKeyFrames Storyboard.TargetName="ContentPresenter" Storyboard.TargetProperty="Opacity">
                                                <DiscreteObjectKeyFrame KeyTime="0" Value="1" />
                                            </ObjectAnimationUsingKeyFrames>
                                        </Storyboard>
                                    </VisualState>
                                    <VisualState x:Name="PointerOver">
                                        <Storyboard>
                                            <!--<ObjectAnimationUsingKeyFrames Storyboard.TargetName="ContentPresenter" Storyboard.TargetProperty="Background">
                                                <DiscreteObjectKeyFrame KeyTime="0" Value="{ThemeResource ButtonForeground}" />
                                            </ObjectAnimationUsingKeyFrames>-->
                                            <ObjectAnimationUsingKeyFrames Storyboard.TargetName="ContentPresenter" Storyboard.TargetProperty="BorderBrush">
                                                <DiscreteObjectKeyFrame KeyTime="0" Value="{ThemeResource ButtonForeground}" />
                                            </ObjectAnimationUsingKeyFrames>
                                            <!--<ObjectAnimationUsingKeyFrames Storyboard.TargetName="ContentPresenter" Storyboard.TargetProperty="Foreground">
                                                <DiscreteObjectKeyFrame KeyTime="0" Value="{ThemeResource ButtonForeground}" />
                                            </ObjectAnimationUsingKeyFrames>-->
                                            <PointerUpThemeAnimation Storyboard.TargetName="ContentPresenter" />
                                        </Storyboard>
                                    </VisualState>
                                    <VisualState x:Name="Pressed">
                                        <Storyboard>
                                            <ObjectAnimationUsingKeyFrames Storyboard.TargetName="ContentPresenter" Storyboard.TargetProperty="Background">
                                                <DiscreteObjectKeyFrame KeyTime="0" Value="{ThemeResource ButtonBackgroundPressed}" />
                                            </ObjectAnimationUsingKeyFrames>
                                            <ObjectAnimationUsingKeyFrames Storyboard.TargetName="ContentPresenter" Storyboard.TargetProperty="BorderBrush">
                                                <DiscreteObjectKeyFrame KeyTime="0" Value="{ThemeResource ButtonBorderBrushPressed}" />
                                            </ObjectAnimationUsingKeyFrames>
                                            <ObjectAnimationUsingKeyFrames Storyboard.TargetName="ContentPresenter" Storyboard.TargetProperty="Foreground">
                                                <DiscreteObjectKeyFrame KeyTime="0" Value="{ThemeResource ButtonForegroundPressed}" />
                                            </ObjectAnimationUsingKeyFrames>
                                            <PointerDownThemeAnimation Storyboard.TargetName="ContentPresenter" />
                                        </Storyboard>
                                    </VisualState>
                                    <VisualState x:Name="Disabled">
                                        <Storyboard>
                                            <ObjectAnimationUsingKeyFrames Storyboard.TargetName="ContentPresenter" Storyboard.TargetProperty="Opacity">
                                                <DiscreteObjectKeyFrame KeyTime="0" Value="0.65" />
                                            </ObjectAnimationUsingKeyFrames>
                                            <ObjectAnimationUsingKeyFrames Storyboard.TargetName="ContentPresenter" Storyboard.TargetProperty="BorderBrush">
                                                <DiscreteObjectKeyFrame KeyTime="0" Value="{ThemeResource ButtonBorderBrushDisabled}" />
                                            </ObjectAnimationUsingKeyFrames>
                                        </Storyboard>
                                    </VisualState>
                                </VisualStateGroup>
                            </VisualStateManager.VisualStateGroups>
                        </ContentPresenter>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
    </Page.Resources>
```



# Relative xmal layout
* RelativePanel: Elements are arranged in relation to the edge or center of the panel, and in relation to each other.  
* Relative xmal layout example
```xml
<RelativePanel >
        <Button x:Name="return_btn" Content="&#xea5c;" FontFamily="{StaticResource ResourceKey=CustomIconFont}" Margin="0,0,0,0" Height="75" Width="75" Click="return_btn_Click" FontSize="50"/>

        <Button x:Name="btn_wifi" Width="70" Height="70" Margin="-100,-500,0,0"  RelativePanel.AlignVerticalCenterWithPanel="True" RelativePanel.AlignHorizontalCenterWithPanel="True" Click="btn_wifi_Click" >
        </Button>
</RelativePanel>
```  

# Absoluty xmal layout
* Grid: Elements are arranged in rows and columns using Grid.Row and Grid.Column attached properties.
* This is default layout

# SplashScreen UWP (UWP startup picture)
* To turn off full screen while starting UWP.
* Package.appxmanifest file from UWP and add ```a:Optional="true"```
```XML
<uap:SplashScreen Image="Assets\SplashScreen.png" a:Optional="true" xmlns:a="http://schemas.microsoft.com/appx/manifest/uap/windows10/5" />
```
> [!TIP]
> If Optional set true, the splash screen will not be shown if the app can launch fast enough.
> If there is a delay in the app launch time, the splash screen will be shown.
> If false, the splash screen will always be shown.

# Read write ini
* The format of ini is often used in Windows  
> [!IMPORTANT]  
> If not get administrator, do not put file in C:\xxxxx.ini that need supervisor permission
* Read write function
```C#
[DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section,
          string key, string val, string filePath);

[DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section,
          string key, string def, StringBuilder retVal,
          int size, string filePath);
```
* Data should continue in one section. Can not use new line to seperate it
* Example format of ini
```
[section]
key=value
key2=value

[section2]
```

# Kill process
* Because while close UWP and open UWP can not re-connect console, the older console will become zombie. I don't know how to reconnect it, therefore should terminate older console.
* The example show how to terminate older console
```C#
static void KillAllNotepadProcesses()
        {
            System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName("CollectDataAP", "."); // use "." for this machine
            foreach (var proc in procs)
                if(proc.Id != Process.GetCurrentProcess().Id) proc.Kill();
        }
```

# Unsafe code config
* CS0227:只有在編譯時指定了 /unsafe，才會出現 unsafe 程式碼  
* In console, choose configuration >> Release; choose: Allow unsafe code  
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/86ed2877-f3f8-4eda-af25-be58ec5d15a2)


# Brightness control
* By changing gamma rate to adjust Win10 brightness
* Reference: https://www.codeproject.com/Articles/47355/Setting-Screen-Brightness-in-C  

# Backlight control
* Reference: https://github.com/JeroenvO/screen-brightness/blob/master/BrightnessConsoleJvO/Class1.cs  
* Add System.Management in Reference  
![image](https://github.com/testtestProblem/HotTab_Win10/assets/107662393/095fc4c0-fd4a-4396-8c70-73c02c2ed826)  
> [!IMPORTANT]  
> Should not enable "night mode", "auto brightness change"



# Startup
refrence: https://learn.microsoft.com/en-us/uwp/api/windows.applicationmodel.startuptask?view=winrt-22621

# Brife express class
```C#
namespace abcd{     //using difference name space to avoid naming confict
    class test{
        public test(){    //constructor
        }
        public string test123(){get;set;}    //properties
        private int test123123;              //field
        public int GetTest(){                //method
            return 1;
        }
    }
}
```

# Array of Delegates
* To create an array of delegates, declare a normal array as we have done so far. You can initialize each member using its index and calling the corresponding method. This can be done as follows:   
```C#
using System;

delegate double Measure(double R);

public class Circle
{
    const double PI = 3.14159;

    public double Diameter(double Radius)
    {
        return Radius * 2;
    }

    public double Circumference(double Radius)
    {
        return Diameter(Radius) * PI;
    }

    public double Area(double Radius)
    {
        return Radius * Radius * PI;
    }
}

public static class Program
{
    static int Main()
    {
        double R = 12.55;
        Circle circ = new Circle();
        Measure[] Calc = new Measure[3];

        Calc[0] = new Measure(circ.Diameter);
        double D = Calc[0](R);
        Calc[1] = new Measure(circ.Circumference);
        double C = Calc[1](R);
        Calc[2] = new Measure(circ.Area);
        double A = Calc[2](R);

        Console.WriteLine("Circle Characteristics");
        Console.WriteLine("Diameter:      {0}", D);
        Console.WriteLine("Circumference: {0}", C);
        Console.WriteLine("Area:          {0}\n", A);

        return 0;
    }
}
```
This would produce:  

Circle Characteristics  
Diameter:      25.1  
Circumference: 78.8539  
Area:          494.808  


# Reduce cpu usage rate 
* Using ```Application.Run();``` is batter than ```while (Console.ReadLine() != "0")```  
* It's usually use in hook  

# Restrict one instance
* UWP have restricted one process in default  
> [!IMPORTANT]  
> Relative variable should be static, public, and domain in groble
* Console must use ```Mutex``` to restricted one process  
```C#
//should be groble verable
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
  Attention: Should add ```CallNextHookEx``` in the end, or will error
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
* Using delagate to start hook function
```C#
private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
private static LowLevelKeyboardProc _proc = HookCallback; //The function called when a key is pressed
```
* Start monitor function which can be call by other function 
> [!CAUTION]
> Sould use administrator to install, and avoid open highest level app in background or foreground. Or it will show unexpectedly problem 

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

An accessibility application can use SendMessage to send WM_APPCOMMAND messages to the shell to launch applications.
```C#
        private const int WM_APPCOMMAND = 0x319;
```
application command
```C#
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
```
volume up example
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


