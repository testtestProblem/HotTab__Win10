using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HotTab_Win10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static public string BasicInformation;
        static public uint? BatteryInformation;
        //static public int startupHindCount=0;

        public MainPage()
        {
            this.InitializeComponent();
            
            //ApplicationView.PreferredLaunchViewSize = new Size(400, 400);
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                //for connect or disconnect event
                App.AppServiceConnected += MainPage_AppServiceConnected;
                App.AppServiceDisconnected += MainPage_AppServiceDisconnected;

                //ApplicationData.Current.LocalSettings.Values["parameters"] = "test";

                //for sideload app
                //await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();

                //for sideload app
                if (App.fulltrustIsRun == 0)
                {
                    App.fulltrustIsRun = 1;
                    MessageDialog dlg;
                    //Mutex.OpenExisting
                    //if (!App.createdNew)
                    //{
                        //MessageBox.Show(appName + " is already running! Exiting the application.");
                    //    dlg = new MessageDialog("appName" + " is already running!Exiting the application.");
                        //Console.ReadKey();
                    //    return;
                    //}
                    //else
                    //{
                        //MessageBox.Show(appName + "Start is running!");
                    //    dlg = new MessageDialog("appName" + "Start is running!");
                        await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
                    //}
                    //await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
                    /*
                    UICommand yesCommand = new UICommand("Yes", async (r) =>
                    {
                    });
                    dlg.Commands.Add(yesCommand);
                    UICommand noCommand = new UICommand("No", (r) => { });
                    dlg.Commands.Add(noCommand);
                    await dlg.ShowAsync();*/

                }

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
                // enable UI to access  the connection
                //btnRegKey.IsEnabled = true;
            });
        }

        /// <summary>
        /// Handle calculation request from desktop process
        /// (dummy scenario to show that connection is bi-directional)
        /// </summary>
        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            //get value from sideload app
            string s_a = (string)args.Request.Message["s_a"];
            string s_b = (string)args.Request.Message["s_b"];
            //double result = d1 + d2;

            //send "have get data" to sideload app
            ValueSet response = new ValueSet();
            response.Add("toConsole_result", "Hello! sideload app");
            await args.Request.SendResponseAsync(response);

            //textBox.Text = ""; //it will error
            //log the getting value in the UI for demo purposes
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>     //I don't know this code
            {
                textBox.Text += string.Format("Request(getting data from sideload app):\ndata 1: {0} \ndata 2: {1}", s_a, s_b);
            });
        }


        /// <summary>
        /// When the desktop process is disconnected, reconnect if needed
        /// </summary>
        private async void MainPage_AppServiceDisconnected(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
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

        private async void calc_btn_Click(object sender, RoutedEventArgs e)
        {
            //send value to sideloade app
            ValueSet request = new ValueSet();
            request.Add("KEY1", int.Parse(a_textBlock.Text));
            request.Add("KEY2", int.Parse(b_textBlock.Text));

            try
            {
                AppServiceResponse response = await App.Connection.SendMessageAsync(request);   //send data and get response 

                //display the response key/value pairs
                //tbResult.Text = "";
                foreach (string key in response.Message.Keys)
                {
                    ans_textBox.Text = "sended by sideload app\nkey: " + key + "\nvalue: " + response.Message[key];
                }
            }
            catch 
            {
                //Do Nothing
            }
        }

        private void deviceControl_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DeviceControlPage));
        }

        private async void information_btn_Click(object sender, RoutedEventArgs e)
        {
            ValueSet request = new ValueSet();
            request.Add("BasicInfo", "basicInfo");
            AppServiceResponse response = await App.Connection.SendMessageAsync(request);

            BasicInformation = response.Message["BasicInfo2UWP"] as string;

            ValueSet request2 = new ValueSet();
            request2.Add("Battery", (int)1357);
            AppServiceResponse response2 = await App.Connection.SendMessageAsync(request2);
            BatteryInformation = response2.Message["bat1"] as uint?;

            this.Frame.Navigate(typeof(InformationPage));
        }

        private void hotkeyControl_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HotkeyControlPage));
        }
    }
}
