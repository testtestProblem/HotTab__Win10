using GlobalVar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win8Hottab_unknow;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using static CollectDataAP.HotkeyFunc;

namespace CollectDataAP
{
    class Connect2UWP
    {
        private AppServiceConnection connection = null;

        /// <summary>
        /// Open connection to UWP app service
        /// </summary>
        public async void InitializeAppServiceConnection()
        {
            connection = new AppServiceConnection();
            connection.AppServiceName = "SampleInteropService";
            connection.PackageFamilyName = Package.Current.Id.FamilyName;
            connection.RequestReceived += Connection_RequestReceived;
            connection.ServiceClosed += Connection_ServiceClosed;

            AppServiceConnectionStatus status = await connection.OpenAsync();
            if (status != AppServiceConnectionStatus.Success)
            {
                // something went wrong ...
                Console.WriteLine(status.ToString());
                Console.ReadLine();

                //In app, something went wrong ...
                //MessageBox.Show(status.ToString());
                //this.IsEnabled = false;
            }
        }

        /// <summary>
        /// Handles the event when the desktop process receives a request from the UWP app
        /// </summary>
        private async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        { 
            //Console.WriteLine("Connection_RequestReceived");

            foreach(object key in args.Request.Message.Keys)
            {
                if((string)key == "BasicInfo")
                {
                    Console.WriteLine("BasicInfo");

                    GetInformation getInformation = new GetInformation();
                    getInformation.InitializeWMIHandler();

                    if (getInformation.InitGlobalVariable() == 0)   //have error
                    {
                        // compose the response as ValueSet
                        ValueSet response = new ValueSet();

                        response.Add("BasicInfo2UWP", "Error! Please restart.");

                        // send the response back to the UWP
                        await args.Request.SendResponseAsync(response);
                    }
                    else
                    {
                        string basicInfo = getInformation.GetInfomation();
                        // compose the response as ValueSet
                        ValueSet response = new ValueSet();

                        response.Add("BasicInfo2UWP", basicInfo);

                        // send the response back to the UWP
                        await args.Request.SendResponseAsync(response);
                    }
                }
                else if ((string)key == "Battery")
                {
                    int? key1 = args.Request.Message["Battery"] as int?;

                    if (key1 != null )
                    {
                        Console.WriteLine((int)key1);

                        // compose the response as ValueSet
                        ValueSet response = new ValueSet();
                        response.Add("bat1", (uint)GetInfoBattery.getBatRelativeCharge(1));
                        response.Add("bat2", (uint)GetInfoBattery.getBatRelativeCharge(2));

                        // send the response back to the UWP
                        await args.Request.SendResponseAsync(response);
                    }
                }
                else if((string)key == "deviceConfig")
                {
                    DeviceState deviceState = new DeviceState();
                    uint? deviceCode;
                    
                    uint state = deviceState.GetDeviceStatePower();
                    deviceCode = args.Request.Message["deviceConfig"] as uint?;

                    if ((DeviceState.DeviceStatePower)deviceCode == DeviceState.DeviceStatePower.initAll)
                    {
                        //do nothing
                    }
                    else    
                    {   //config device state
                        try
                        {
                            foreach (uint device in Enum.GetValues(typeof(DeviceState.DeviceStatePower)))
                            {
                                if ((deviceCode & device) == device)
                                {
                                    state = state ^ (uint)deviceCode;
                                    deviceState.SetDeviceStatePower(state);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            //TODO: not verify
                            var dialog = new MessageDialog(e.Message);
                            await dialog.ShowAsync();
                        }
                    }
                    // compose the response as ValueSet
                    ValueSet response = new ValueSet();

                    state = deviceState.GetDeviceStatePower();
                    response.Add("res_deviceConfig", state);

                    // send the response back to the UWP
                    await args.Request.SendResponseAsync(response);
                }
                else if ((string)key == "HotKeyFunc")
                {
                    uint? func = args.Request.Message["HotKeyFunc"] as uint?;
                    uint? hotKeyState = args.Request.Message["HotKeyState"] as uint?;

                    if (func != null && hotKeyState != null)
                    {
                        HotkeyFunc.changeFuncName((HotkeyList)hotKeyState, (FunctionList)func);
                        
                       // IniFile inifile = new IniFile();
                        //inifile.path = "C:\\Program Files\\HotTab\\HottabCfg.ini";
                        //inifile.IniWriteValue("FunctionKey", ((HotkeyList2)hotKeyState).ToString(), ((FunctionList)func).ToString());
                        //inifile.IniWriteValue("SETTING", ((HotkeyList2)hotKeyState).ToString(), ((FunctionList)func).ToString());
                        //HotTabRegistry.WritePrivateProfileString("FunctionKey", ((HotkeyList2)hotKeyState).ToString(), ((FunctionList)func).ToString(), inifile.path);
                        ProcessStart.processStart_reg(((HotkeyList2)hotKeyState).ToString(), ((FunctionList)func).ToString());

                        Console.WriteLine("Hottey change function  " + ((HotkeyList2)hotKeyState).ToString() + "  " + ((FunctionList)func).ToString());
                        /*
                        //test registry
                        Console.WriteLine("RegistryWindows.getValue(\"noThisValue\") " + RegistryWindows.getValue("noThisValue"));
                        RegistryWindows.setValue("test1", "register test");
                        RegistryWindows.setValue("test123456", "register123456 test");
                        RegistryWindows.setValue("testtest123", "register testtest123456");
                        Console.WriteLine("RegistryWindows.getValue(\"test1\") " + RegistryWindows.getValue("test1"));
                        
                        Console.WriteLine("RegistryWindows.getValue(\"test123456\") " + RegistryWindows.getValue("test123456"));
                        Console.WriteLine("RegistryWindows.getValue(\"testtest123\") " + RegistryWindows.getValue("testtest123"));
                        */
                    }
                }
                else if ((string)key == "HotKeyFuncCustomize")
                {
                    string func = args.Request.Message["HotKeyFuncCustomize"] as string;
                    uint? hotKeyState = args.Request.Message["HotKeyState"] as uint?;
                    
                    if (func != null && hotKeyState != null)
                    {
                        HotkeyFunc.changeFuncName((HotkeyList)hotKeyState, (string)func);

                        //IniFile inifile = new IniFile();
                       // inifile.path = "C:\\Program Files\\HotTab\\HottabCfg.ini";
                        //HotTabRegistry.WritePrivateProfileString("FunctionKey", ((HotkeyList2)hotKeyState).ToString(), (string)func, inifile.path);
                        ProcessStart.processStart_reg(((HotkeyList2)hotKeyState).ToString(), (string)func);
                    }
                }
                //TODO: using index??
                else if ((string)key == "HotKeyFuncNow")
                {
                    uint? now = args.Request.Message["HotKeyFuncNow"] as uint?;

                    if (now != null)
                    {
                        // compose the response as ValueSet
                        ValueSet response = new ValueSet();
                        response.Add("f1Short_btn", HotkeyFunc.funcName[0]);
                        response.Add("f2Short_btn", HotkeyFunc.funcName[1]);
                        response.Add("f3Short_btn", HotkeyFunc.funcName[2]);
                        response.Add("f1Long_btn", HotkeyFunc.funcName[4]);
                        response.Add("f2Long_btn", HotkeyFunc.funcName[5]);
                        response.Add("f3Long_btn", HotkeyFunc.funcName[3]);

                        // send the response back to the UWP
                        await args.Request.SendResponseAsync(response);
                    }
                }
                else if ((string)key == "HotKeyFuncDefault")
                {
                    uint? now = args.Request.Message["HotKeyFuncDefault"] as uint?;

                    if (now != null)
                    {
                        HotkeyFunc.defaultHotketFunc();

                        //IniFile inifile = new IniFile();
                        //inifile.path = "C:\\Program Files\\HotTab\\HottabCfg.ini";
                        //HotTabRegistry.WritePrivateProfileString("FunctionKey", (HotkeyList2.F1S).ToString(), (FunctionList.volumeUp).ToString(), inifile.path);
                        //HotTabRegistry.WritePrivateProfileString("FunctionKey", (HotkeyList2.F2S).ToString(), (FunctionList.volumeDown).ToString(), inifile.path);
                        //HotTabRegistry.WritePrivateProfileString("FunctionKey", (HotkeyList2.F3S).ToString(), (FunctionList.backlight20).ToString(), inifile.path);
                        //HotTabRegistry.WritePrivateProfileString("FunctionKey", (HotkeyList2.F1L).ToString(), (FunctionList.Calc).ToString(), inifile.path);
                        //HotTabRegistry.WritePrivateProfileString("FunctionKey", (HotkeyList2.F2L).ToString(), (FunctionList.cmd).ToString(), inifile.path);
                        // HotTabRegistry.WritePrivateProfileString("FunctionKey", (HotkeyList2.F3L).ToString(), (FunctionList.backlight100).ToString(), inifile.path);

                        ProcessStart.processStart_reg((HotkeyList2.F1S).ToString(), (FunctionList.volumeUp).ToString());
                        ProcessStart.processStart_reg((HotkeyList2.F2S).ToString(), (FunctionList.volumeDown).ToString());
                        ProcessStart.processStart_reg((HotkeyList2.F3S).ToString(), (FunctionList.backlight20).ToString());
                        ProcessStart.processStart_reg((HotkeyList2.F1L).ToString(), (FunctionList.Calc).ToString());
                        ProcessStart.processStart_reg((HotkeyList2.F2L).ToString(), (FunctionList.cmd).ToString());
                        ProcessStart.processStart_reg((HotkeyList2.F3L).ToString(), (FunctionList.backlight100).ToString());


                    }
                }
                else if((string)key == "KEY1")
                {
                    int? key1 = args.Request.Message["KEY1"] as int?;
                    int? key2 = args.Request.Message["KEY2"] as int?;

                    if (key1 != null && key2 != null)
                    {
                        int ans = (int)key1 + (int)key2;
                        Console.WriteLine((int)key1 + " + " + (int)key2 + " = " + ans);

                        // compose the response as ValueSet
                        ValueSet response = new ValueSet();
                        response.Add("KEY3", ans);

                        // send the response back to the UWP
                        await args.Request.SendResponseAsync(response);
                    }
                }
            }
        }

        public async void SendData2UWP(uint data)
        {
            // ask the UWP to calculate d1 + d2
            ValueSet request = new ValueSet();
            request.Add("deviceStateAll", (uint)data);
            //request.Add("D2", (double)2);
            await connection.SendMessageAsync(request);
            //AppServiceResponse response = await connection.SendMessageAsync(request);
            //string result = (string)response.Message["RESULT"];
        }

        public async void SendData2UWP(string data)
        {
            // ask the UWP to calculate d1 + d2
            ValueSet request = new ValueSet();
            request.Add("deviceStateAll", (string)data);
            //request.Add("D2", (double)2);
            await connection.SendMessageAsync(request);
            //AppServiceResponse response = await connection.SendMessageAsync(request);
            //string result = (string)response.Message["RESULT"];
        }

        public async void Send2UWP_2(string a, string b)
        {
            // ask the UWP to calculate d1 + d2
            ValueSet request = new ValueSet();
            request.Add("s_a", a);
            request.Add("s_b", b);

            //start sending
            AppServiceResponse response = await connection.SendMessageAsync(request);
            //get response
            string result = response.Message["toConsole_result"] as string;

            Console.WriteLine("send data_a to UWP: " + a);
            Console.WriteLine("send data_b to UWP: " + b);
            Console.WriteLine("getting data from UWP: " + result);
        }

        /// <summary>
        /// Handles the event when the app service connection is closed
        /// </summary>
        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            //In console, connection to the UWP lost, so we shut down the desktop process
            Console.WriteLine("UWP Disconnect! Please restart APP!");
            //Console.ReadLine();

            //Due to should always run in background
            //Environment.Exit(0);

            //In app, connection to the UWP lost, so we shut down the desktop process
            //Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            //{
            //    Application.Current.Shutdown();
            //}));
        }
    }
}
