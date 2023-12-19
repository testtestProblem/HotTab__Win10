using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HotTab_Win10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotkeyControlPage : Page
    {
        HotkeyList hotkeyList = HotkeyList.noValue;
        FunctionList functionList = FunctionList.noValue;

        Button button;

        public HotkeyControlPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                App.AppServiceConnected += MainPage_AppServiceConnected;
                App.AppServiceDisconnected += MainPage_AppServiceDisconnected;
                //await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }

            //send value to sideloade app
            ValueSet request = new ValueSet();
            request.Add("HotKeyFuncNow", (uint)2312);
            AppServiceResponse response = await App.Connection.SendMessageAsync(request);   //send data and get response 

            //display the response key/value pairs
            //tbResult.Text = "";
            foreach (string key in response.Message.Keys)
            {
                if (key == "f1Short_btn") f1Short_btn.Content = "Volume up";
                else if (key == "f2Short_btn") f2Short_btn.Content = "Volume down";
                else if (key == "f3Short_btn") f3Short_btn.Content = "BL 20";
                else if (key == "f1Long_btn") f1Long_btn.Content = "Calc";
                else if (key == "f2Short_btn") f2Short_btn.Content = "cmd";
                else if (key == "f3Short_btn") f3Short_btn.Content = "BL 100";
            }
        }

        /// <summary>
        /// When the desktop process is disconnected, reconnect if needed
        /// </summary>
        private async void MainPage_AppServiceDisconnected(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // disable UI to access the connection
                //btnRegKey.IsEnabled = false;

                // ask user if they want to reconnect
                Reconnect();
            });
        }

        /// <summary>
        /// Ask user if they want to reconnect to the desktop process
        /// </summary>
        private async void Reconnect()
        {
            if (App.IsForeground)
            {
                MessageDialog dlg = new MessageDialog("Connection to desktop process lost. Reconnect?");
                UICommand yesCommand = new UICommand("Yes", async (r) =>
                {
                    await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
                });
                dlg.Commands.Add(yesCommand);
                UICommand noCommand = new UICommand("No", (r) => { });
                dlg.Commands.Add(noCommand);
                await dlg.ShowAsync();
            }
        }


        /// <summary>
        /// When the desktop process is connected, get ready to send/receive requests
        /// </summary>
        private async void MainPage_AppServiceConnected(object sender, AppServiceTriggerDetails e)
        {
            App.Connection.RequestReceived += AppServiceConnection_RequestReceived;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //enable UI to access the connection
                //btnRegKey.IsEnabled = true;
            });

            


        }



        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>     //I don't know this code
            {
                
            });
        }

        public enum HotkeyList : uint
        {
            f1Short = 0x01,
            f2Short = 0x02,
            f3Short = 0x04,

            f1Long = 0x08,
            f2Long = 0x10,
            f3Long = 0x20,

            noValue =0x1000
        };

        public enum FunctionList : uint
        {
            volumeUp = 0x01,
            volumeDown = 0x02,

            backlight20 = 0x04,
            backlight100 = 0x08,

            Calc = 0x10,
            cmd = 0x20,

            defaultSetting = 0x80,

            noValue = 0x1000
        };

        private async void send2Console(HotkeyList hotkeyList, FunctionList functionList)
        {
            //send value to sideloade app
            ValueSet request = new ValueSet();
            request.Add("HotKeyState", (uint)hotkeyList);
            request.Add("HotKeyFunc", (uint)functionList);
            AppServiceResponse response = await App.Connection.SendMessageAsync(request);   //send data and get response 
        }

        private void clearDataList()
        {
            functionList = FunctionList.noValue;
            hotkeyList = HotkeyList.noValue;
        }

        private void return_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void f1Short_btn_Click(object sender, RoutedEventArgs e)
        {
            hotkeyList = HotkeyList.f1Short;
            button = this.f1Short_btn;
        }

        private void f2Short_btn_Click(object sender, RoutedEventArgs e)
        {
            hotkeyList = HotkeyList.f2Short;
            button = this.f2Short_btn;
        }

        private void f3Short_btn_Click(object sender, RoutedEventArgs e)
        {
            hotkeyList = HotkeyList.f3Short;
            button = this.f3Short_btn;
        }

        private void f1Long_btn_Click(object sender, RoutedEventArgs e)
        {
            hotkeyList = HotkeyList.f1Long;
            button = this.f1Long_btn;
        }

        private void f2Long_btn_Click(object sender, RoutedEventArgs e)
        {
            hotkeyList = HotkeyList.f2Long;
            button = this.f2Long_btn;
        }

        private void f3Long_btn_Click(object sender, RoutedEventArgs e)
        {
            hotkeyList = HotkeyList.f3Long;
            button = this.f3Long_btn;
        }

        private async void default_btn_Click(object sender, RoutedEventArgs e)
        {
            f1Short_btn.Content = func1_btn.Content;
            f2Short_btn.Content = func2_btn.Content;
            f3Short_btn.Content = func3_btn.Content;
            f1Long_btn.Content = func5_btn.Content;
            f2Long_btn.Content = func6_btn.Content;
            f3Long_btn.Content = func4_btn.Content;

            ValueSet request = new ValueSet();
            request.Add("HotKeyFuncDefault", (uint)123);
            AppServiceResponse response = await App.Connection.SendMessageAsync(request);   //send data and get response 
        }

        private void func1_btn_Click(object sender, RoutedEventArgs e)
        {
            if (hotkeyList != HotkeyList.noValue)
            {
                button.Content = func1_btn.Content;
                functionList = FunctionList.volumeUp;

                send2Console(hotkeyList, functionList);

                clearDataList();
            }
        }

        private void func2_btn_Click(object sender, RoutedEventArgs e)
        {
            if (hotkeyList != HotkeyList.noValue)
            {
                button.Content = func2_btn.Content;
                functionList = FunctionList.volumeDown;

                send2Console(hotkeyList, functionList);

                clearDataList();
            }
        }

        private void func3_btn_Click(object sender, RoutedEventArgs e)
        {
            if (hotkeyList != HotkeyList.noValue)
            {
                button.Content = func3_btn.Content;
                functionList = FunctionList.backlight20;

                send2Console(hotkeyList, functionList);

                clearDataList();
            }
        }

        private void func4_btn_Click(object sender, RoutedEventArgs e)
        {
            if (hotkeyList != HotkeyList.noValue)
            {
                button.Content = func4_btn.Content;
                functionList = FunctionList.backlight100;

                send2Console(hotkeyList, functionList);

                clearDataList();
            }
        }

        private void func5_btn_Click(object sender, RoutedEventArgs e)
        {
            if (hotkeyList != HotkeyList.noValue)
            {
                button.Content = func5_btn.Content;
                functionList = FunctionList.Calc;

                send2Console(hotkeyList, functionList);

                clearDataList();
            }
        }

        private void func6_btn_Click(object sender, RoutedEventArgs e)
        {
            if (hotkeyList != HotkeyList.noValue)
            {
                button.Content = func6_btn.Content;
                functionList = FunctionList.cmd;

                send2Console(hotkeyList, functionList);

                clearDataList();
            }
        }

        private void func7_btn_Click(object sender, RoutedEventArgs e)
        {
            if (hotkeyList != HotkeyList.noValue)
            {
                button.Content = func7_btn.Content;
                functionList = FunctionList.noValue;

                send2Console(hotkeyList, functionList);

                clearDataList();
            }
        }

        

        
    }
}
