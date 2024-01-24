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

        private string string2UIstring(string data)
        {
            if (data == "volumeUp") return "Volume up";
            else if (data == "volumeDown") return "Volume down";
            else if (data == "backlight20") return "BL down";
            else if (data == "backlight100") return "BL up";
            else if (data == "calculatorWin10" || data == "Calc") return "Calc";
            else if (data == "cmdWin10" || data == "cmd") return "cmd";
            else if (data == "" || data == "noValue") return "Null";
            else {
                string[] dataS = data.Split('\\');
                return dataS.Last();
            } 
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
                if (key == "f1Short_btn") f1Short_btn.Content = string2UIstring((string)response.Message[key]);
                else if (key == "f2Short_btn") f2Short_btn.Content = string2UIstring((string)response.Message[key]);
                else if (key == "f3Short_btn") f3Short_btn.Content = string2UIstring((string)response.Message[key]);
                else if (key == "f1Long_btn") f1Long_btn.Content = string2UIstring((string)response.Message[key]);
                else if (key == "f2Long_btn") f2Long_btn.Content = string2UIstring((string)response.Message[key]);
                else if (key == "f3Long_btn") f3Long_btn.Content = string2UIstring((string)response.Message[key]);
            }

            disableAllFuncKey();
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

        private void btnOrange(HotkeyList hotkeyList)
        {
            /*
            if (hotkeyList == HotkeyList.noValue) {
                f1Short_btn.Background = new SolidColorBrush(Windows.UI.Colors.Gray);
            }
            else if (hotkeyList == HotkeyList.f1Short) { f1Short_btn.Background = new SolidColorBrush(Windows.UI.Colors.LightGray); }
            else if (hotkeyList == HotkeyList.f2Short) { f2Short_btn.Background = new SolidColorBrush(Windows.UI.Colors.Orange); }
            else if (hotkeyList == HotkeyList.f3Short) { f3Short_btn.Background = new SolidColorBrush(Windows.UI.Colors.Orange); }
            else if (hotkeyList == HotkeyList.f1Long) { f1Long_btn.Background = new SolidColorBrush(Windows.UI.Colors.Orange); }
            else if (hotkeyList == HotkeyList.f2Long) { f2Long_btn.Background = new SolidColorBrush(Windows.UI.Colors.Orange); }
            else if (hotkeyList == HotkeyList.f3Long) { f3Long_btn.Background = new SolidColorBrush(Windows.UI.Colors.Orange); }
            */
            
            enableAllFuncKey();
        }

        private void disableAllFuncKey()
        {
            func1_btn.IsEnabled = false;
            func2_btn.IsEnabled = false;
            func3_btn.IsEnabled = false;
            func4_btn.IsEnabled = false;
            func5_btn.IsEnabled = false;
            func6_btn.IsEnabled = false;
            func7_btn.IsEnabled = false;
            funcCustomize_btn.IsEnabled = false;

            func1_btn.Opacity = 0;
            func2_btn.Opacity = 0;
            func3_btn.Opacity = 0;
            func4_btn.Opacity = 0;
            func5_btn.Opacity = 0;
            func6_btn.Opacity = 0;
            func7_btn.Opacity = 0;
            funcCustomize_btn.Opacity = 0;
        }

        private void enableAllFuncKey()
        {
            func1_btn.IsEnabled = true;
            func2_btn.IsEnabled = true;
            func3_btn.IsEnabled = true;
            func4_btn.IsEnabled = true;
            func5_btn.IsEnabled = true;
            func6_btn.IsEnabled = true;
            func7_btn.IsEnabled = true;
            funcCustomize_btn.IsEnabled = true;

            func1_btn.Opacity = 100;
            func2_btn.Opacity = 100;
            func3_btn.Opacity = 100;
            func4_btn.Opacity = 100;
            func5_btn.Opacity = 100;
            func6_btn.Opacity = 100;
            func7_btn.Opacity = 100;
            funcCustomize_btn.Opacity = 100;

            //func1_btn.Background=tr
        }

        private void f1Short_btn_Click(object sender, RoutedEventArgs e)
        {
            btnOrange(HotkeyList.f1Short);

            hotkeyList = HotkeyList.f1Short;
            button = this.f1Short_btn;
        }

        private void f2Short_btn_Click(object sender, RoutedEventArgs e)
        {
            btnOrange(HotkeyList.f2Short);

            hotkeyList = HotkeyList.f2Short;
            button = this.f2Short_btn;
        }

        private void f3Short_btn_Click(object sender, RoutedEventArgs e)
        {
            btnOrange(HotkeyList.f3Short);

            hotkeyList = HotkeyList.f3Short;
            button = this.f3Short_btn;
        }

        private void f1Long_btn_Click(object sender, RoutedEventArgs e)
        {
            btnOrange(HotkeyList.f1Long);

            hotkeyList = HotkeyList.f1Long;
            button = this.f1Long_btn;
        }

        private void f2Long_btn_Click(object sender, RoutedEventArgs e)
        {
            btnOrange(HotkeyList.f2Long);

            hotkeyList = HotkeyList.f2Long;
            button = this.f2Long_btn;
        }

        private void f3Long_btn_Click(object sender, RoutedEventArgs e)
        {
            btnOrange(HotkeyList.f3Long);

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

            hotkeyList = HotkeyList.noValue;

            disableAllFuncKey();

            ValueSet request = new ValueSet();
            request.Add("HotKeyFuncDefault", (uint)123);
            AppServiceResponse response = await App.Connection.SendMessageAsync(request);   //send data and get response 
        }

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            hotkeyList = HotkeyList.noValue;
            disableAllFuncKey();
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
            disableAllFuncKey();
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
            disableAllFuncKey();
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
            disableAllFuncKey();
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
            disableAllFuncKey();
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
            disableAllFuncKey();
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
            disableAllFuncKey();
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
            disableAllFuncKey();
        }

        private async void funcCustomize_btn_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            //PickAPhotoOutputTextBlock.Text = "";

            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            
            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            //var window = WindowHelper.GetWindowForElement(this);
            //var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            //WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            //openPicker.ViewMode = PickerViewMode.Thumbnail;
            //openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".exe");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();

            if (hotkeyList != HotkeyList.noValue)
            {
                if (file != null)
                {
                    button.Content = file.Name;

                    ValueSet request = new ValueSet();
                    request.Add("HotKeyFuncCustomize", (string)file.Path);
                    request.Add("HotKeyState", (uint)hotkeyList);
                    AppServiceResponse response = await App.Connection.SendMessageAsync(request);   //send data and get response 
                }
                else
                {
                    funcCustomize_btn.Content = "Operation cancelled.";
                }

                functionList = FunctionList.noValue;
                clearDataList();
            }

            disableAllFuncKey();
        }
    }
}
