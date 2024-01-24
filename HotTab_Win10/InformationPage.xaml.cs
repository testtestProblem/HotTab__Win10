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
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HotTab_Win10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InformationPage : Page
    {
        public InformationPage()
        {
            this.InitializeComponent();

            //ApplicationView.PreferredLaunchViewSize = new Size(400, 400);
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
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
            }
            textBox.Text = "";
            textBox.Text = MainPage.BasicInformation;

            /*

            if (MainPage.BatteryInformation >= 100)
                image_battery.Source = new BitmapImage(new Uri("ms-appx:///Assets/battery/Rbat100.bmp"));
            else if (MainPage.BatteryInformation >= 75)
                image_battery.Source = new BitmapImage(new Uri("ms-appx:///Assets/battery/Rbat75.BMP"));
            else if (MainPage.BatteryInformation >= 50)
                image_battery.Source = new BitmapImage(new Uri("ms-appx:///Assets/battery/Rbat50.BMP"));
            else if (MainPage.BatteryInformation >= 25)
                image_battery.Source = new BitmapImage(new Uri("ms-appx:///Assets/battery/Rbat25.BMP"));
            else if (MainPage.BatteryInformation >= 10)
                image_battery.Source = new BitmapImage(new Uri("ms-appx:///Assets/battery/Rbat0.BMP"));
            else
                image_battery.Source = new BitmapImage(new Uri("ms-appx:///Assets/battery/Rbat.bmp"));
            */


            StartupTask startupTask = await StartupTask.GetAsync("MyStartupId"); // Pass the task ID you specified in the appxmanifest file
            switch (startupTask.State)
            {
                case StartupTaskState.Disabled:
                    // Task is disabled but can be enabled.
                    StartupTaskState newState = await startupTask.RequestEnableAsync(); // ensure that you are on a UI thread when you call RequestEnableAsync()
                   // textBox.Text = "Request to enable startup, result = " + newState;
                    break;
                case StartupTaskState.DisabledByUser:
                    // Task is disabled and user must enable it manually.
                    MessageDialog dialog = new MessageDialog(
                        "You have disabled this app's ability to run " +
                        "as soon as you sign in, but if you change your mind, " +
                        "you can enable this in the Startup tab in Task Manager.",
                        "TestStartup");
                    await dialog.ShowAsync();
                    break;
                case StartupTaskState.DisabledByPolicy:
                    TextBlock.Text = "Startup disabled by group policy, or not supported on this device";
                    break;
                case StartupTaskState.Enabled:
                    //textBox.Text = "Startup is enabled.";
                    break;
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

        private void return_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
            /*
            MainPage.startupHindCount++;
            if (MainPage.startupHindCount==3) MainPage.startupHindCount=10;
            else if (MainPage.startupHindCount == 7)
            {
                disableStartup_btn.Visibility = Visibility.Collapsed;
                enableStartup_btn.Visibility = Visibility.Collapsed;
                hindStartup_btn.Visibility = Visibility.Collapsed;
            }*/

        }

        private async void enableStartup_btn_Click(object sender, RoutedEventArgs e)
        {
            StartupTask startupTask = await StartupTask.GetAsync("MyStartupId"); // Pass the task ID you specified in the appxmanifest file
            switch (startupTask.State)
            {
                case StartupTaskState.Disabled:
                    // Task is disabled but can be enabled.
                    StartupTaskState newState = await startupTask.RequestEnableAsync(); // ensure that you are on a UI thread when you call RequestEnableAsync()
                    textBox.Text = "Request to enable startup, result = " + newState;
                    break;
                case StartupTaskState.DisabledByUser:
                    // Task is disabled and user must enable it manually.
                    MessageDialog dialog = new MessageDialog(
                        "You have disabled this app's ability to run " +
                        "as soon as you sign in, but if you change your mind, " +
                        "you can enable this in the Startup tab in Task Manager.",
                        "TestStartup");
                    await dialog.ShowAsync();
                    break;
                case StartupTaskState.DisabledByPolicy:
                    TextBlock.Text = "Startup disabled by group policy, or not supported on this device";
                    break;
                case StartupTaskState.Enabled:
                    textBox.Text = "Startup is enabled.";
                    break;
            }
        }

        private async void disableStartup_btn_Click(object sender, RoutedEventArgs e)
        {
            StartupTask startupTask = await StartupTask.GetAsync("MyStartupId"); // Pass the task ID you specified in the appxmanifest file
            switch (startupTask.State)
            {
                case StartupTaskState.Disabled:
                    // Task is disabled but can be enabled.
                    textBox.Text = "Startup has disabled.";
                    break;
                case StartupTaskState.DisabledByUser:
                    // Task is disabled and user must enable it manually.
                    MessageDialog dialog = new MessageDialog(
                        "You have disabled this app's ability to run " +
                        "as soon as you sign in, but if you change your mind, " +
                        "you can enable this in the Startup tab in Task Manager.",
                        "TestStartup");
                    await dialog.ShowAsync();
                    break;
                case StartupTaskState.DisabledByPolicy:
                    textBox.Text = "Startup disabled by group policy, or not supported on this device";
                    break;
                case StartupTaskState.Enabled:
                    // Task is disabled but can be enabled.
                    startupTask.Disable(); // ensure that you are on a UI thread when you call RequestEnableAsync()
                    textBox.Text = "Startup is disabled.";
                    break;
            }
        }

        private void hindStartup_btn_Click(object sender, RoutedEventArgs e)
        {
            disableStartup_btn.Visibility = Visibility.Collapsed;
            enableStartup_btn.Visibility = Visibility.Collapsed;
            hindStartup_btn.Visibility = Visibility.Collapsed;
        }
    }
}
