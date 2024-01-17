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
using static HotTab_Device2.DeviceState;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HotTab_Win10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeviceControlPage : Page
    {
        public DeviceControlPage()
        {
            this.InitializeComponent();
            //ApplicationView.PreferredLaunchViewSize = new Size(400, 400);
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                App.AppServiceConnected += MainPage_AppServiceConnected;
                App.AppServiceDisconnected += MainPage_AppServiceDisconnected;
                //await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }


            //for inital UI
            //HasBeen_Click((uint)Modules.ExpandCOM);
            HasBeen_Click((uint)Modules.initAll, Modules.initAll);
            //btn_allLED.Background = new SolidColorBrush(Windows.UI.Colors.Orange); //for test

            //image_wifi.Source = new BitmapImage(new Uri(@"C:\Users\WIN10\source\repos\HotTabWin10_3\HotTab_Win10\HotTab_Win10\Assets\device\_3G.png", UriKind.Absolute));

            // Create source
            //BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            //myBitmapImage.BeginInit();
            //myBitmapImage.UriSource = new Uri(@"C:\Users\WIN10\source\repos\HotTabWin10_3\HotTab_Win10\HotTab_Win10\Assets\device\_3G.png", UriKind.Absolute);

            // To save significant application memory, set the DecodePixelWidth or
            // DecodePixelHeight of the BitmapImage value of the image source to the desired
            // height or width of the rendered image. If you don't do this, the application will
            // cache the image as though it were rendered as its normal size rather than just
            // the size that is displayed.
            // Note: In order to preserve aspect ratio, set DecodePixelWidth
            // or DecodePixelHeight but not both.
            //myBitmapImage.DecodePixelWidth = 200;
            //myBitmapImage.EndInit();

            //image_wifi.Source = myBitmapImage;


            //create a new bitmage image  
            //BitmapImage bitmapImage = new BitmapImage();
            //bitmapImage.UriSource = new Uri("ms-appx:///Assets/device/_3G.png");
            //image_wifi.Source = bitmapImage;

            //image_wifi.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/_3G.png"));

            //UWP will crash
            //HasBeen_Click(0);
        }

        // Raised when Button gains focus.
        // Changes the color of the Button to Red.
        private void OnGotFocusHandler(object sender, RoutedEventArgs e)
        {
            //btn_barcode.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
        }

        // Raised when Button losses focus.
        // Changes the color of the Button back to white.
        private void OnLostFocusHandler(object sender, RoutedEventArgs e)
        {
            // Button tb = e.Source as Button;
            //tb.Background = Brushes.White;
            //btn_barcode.Background = new SolidColorBrush(Windows.UI.Colors.Pink);
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
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CheckDeviceState_Button((uint)args.Request.Message["deviceStateAll"]);
            });
        }

        private void CheckDeviceState_Button(uint deviceStateCode)
        {
            if ((deviceStateCode & (uint)Modules.Wifi) == (uint)Modules.Wifi)
            {
                //btn_wifi.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                //image_wifi.Source = new BitmapImage(new Uri(@"/Assets/device/G_Wi-Fi.bmp", UriKind.Relative));
                image_wifi.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_Wi-Fi.bmp"));
            }
            else
            {
                //btn_wifi.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                image_wifi.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/Wi-Fi.bmp"));
            }

            if ((deviceStateCode & (uint)Modules.Gobi3G) == (uint)Modules.Gobi3G)
            {
                //btn_gobi3G.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                image_gobi3G.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_3G.png"));
            }
            else
            {
                //btn_gobi3G.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                image_gobi3G.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/_3G.png"));
            }

            if ((deviceStateCode & (uint)Modules.GPS) == (uint)Modules.GPS)
            {
                //btn_GPS.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                image_GPS.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_GPS.bmp"));
            }
            else
            {
                //btn_GPS.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                image_GPS.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/GPS.bmp"));
            }

            if ((deviceStateCode & (uint)Modules.Bluetooth) == (uint)Modules.Bluetooth)
            {
                //btn_bluetooth.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                image_bluetooth.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_blueTooth.bmp"));
            }
            else
            {
                //btn_bluetooth.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                image_bluetooth.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/blueTooth1.bmp"));
            }

            if ((deviceStateCode & (uint)Modules.WebCamRear) == (uint)Modules.WebCamRear)
            {
                //btn_webCamRear.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                image_webCamRear.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_Camera.bmp"));
            }
            else
            {
                //btn_webCamRear.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                image_webCamRear.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/Camera1.bmp"));
            }

            if ((deviceStateCode & (uint)Modules.AllLED) == (uint)Modules.AllLED)
            {
                //btn_allLED.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                image_allLED.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/gLCD.png"));
            }
            else
            {
                //btn_allLED.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                image_allLED.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/wLCD.png"));
            }

            if ((deviceStateCode & (uint)Modules.Barcode) == (uint)Modules.Barcode)
            {
                btn_barcode.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
            }
            else
            {
                btn_barcode.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            }

            if ((deviceStateCode & (uint)Modules.RFID) == (uint)Modules.RFID)
            {
                btn_RFID.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
            }
            else
            {
                btn_RFID.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            }

            if ((deviceStateCode & (uint)Modules.GPSAntenna) == (uint)Modules.GPSAntenna)
            {
                //btn_GPSAntenna.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                image_GPSAntenna.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/gANT_GPS.png"));
            }
            else
            {
                //btn_GPSAntenna.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                image_GPSAntenna.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/wANT_GPS.png"));
            }
        }

        private void CheckDeviceState_Button(uint deviceStateCode, Modules modules)
        {
            if (Modules.Wifi == modules || Modules.initAll== modules) {
                if ((deviceStateCode & (uint)Modules.Wifi) == (uint)Modules.Wifi)
                {
                    //btn_wifi.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                    //image_wifi.Source = new BitmapImage(new Uri(@"/Assets/device/G_Wi-Fi.bmp", UriKind.Relative));
                    image_wifi.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_Wi-Fi.bmp"));
                }
                else
                {
                    //btn_wifi.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                    image_wifi.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/Wi-Fi.bmp"));
                }
            }

            if (Modules.Gobi3G == modules || Modules.initAll == modules) 
            {
                if ((deviceStateCode & (uint)Modules.Gobi3G) == (uint)Modules.Gobi3G )
                {
                    //btn_gobi3G.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                    image_gobi3G.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_3G.png"));
                }
                else
                {
                    //btn_gobi3G.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                    image_gobi3G.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/_3G.png"));
                }
            }

            if (Modules.GPS == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.GPS) == (uint)Modules.GPS)
                {
                    //btn_GPS.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                    image_GPS.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_GPS.bmp"));
                }
                else
                {
                    //btn_GPS.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                    image_GPS.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/GPS.bmp"));
                }
            }

            if (Modules.Bluetooth == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.Bluetooth) == (uint)Modules.Bluetooth)
                {
                    //btn_bluetooth.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                    image_bluetooth.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_blueTooth.bmp"));
                }
                else
                {
                    //btn_bluetooth.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                    image_bluetooth.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/blueTooth1.bmp"));
                }
            }

            if (Modules.WebCamRear == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.WebCamRear) == (uint)Modules.WebCamRear)
                {
                    //btn_webCamRear.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                    image_webCamRear.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/G_Camera.bmp"));
                }
                else
                {
                    //btn_webCamRear.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                    image_webCamRear.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/Camera1.bmp"));
                }
            }

            if (Modules.AllLED == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.AllLED) == (uint)Modules.AllLED)
                {
                    //btn_allLED.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                    image_allLED.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/gLCD.png"));
                }
                else
                {
                    //btn_allLED.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                    image_allLED.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/wLCD.png"));
                }
            }

            if (Modules.Barcode == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.Barcode) == (uint)Modules.Barcode)
                {
                    btn_barcode.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                }
                else
                {
                    btn_barcode.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                }
            }

            if (Modules.WebCamFront == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.WebCamFront) == (uint)Modules.WebCamFront)
                {
                    btn_webCamFront.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                }
                else
                {
                    btn_webCamFront.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                }
            }


            if (Modules.RFID == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.RFID) == (uint)Modules.RFID)
                {
                    btn_RFID.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                }
                else
                {
                    btn_RFID.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                }
            }

            if (Modules.GPSAntenna == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.GPSAntenna) == (uint)Modules.GPSAntenna)
                {
                    //btn_GPSAntenna.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                    image_GPSAntenna.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/gANT_GPS.png"));
                }
                else
                {
                    //btn_GPSAntenna.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                    image_GPSAntenna.Source = new BitmapImage(new Uri("ms-appx:///Assets/device/wANT_GPS.png"));
                }
            }

            if (Modules.ExpandUSB == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.ExpandUSB) == (uint)Modules.ExpandUSB)
                {
                    btn_expandUSB.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                }
                else
                {
                    btn_expandUSB.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                }
            }

            if (Modules.ExpandCOM == modules || Modules.initAll == modules)
            {
                if ((deviceStateCode & (uint)Modules.ExpandCOM) == (uint)Modules.ExpandCOM)
                {
                    btn_expandCOM.Background = new SolidColorBrush(Windows.UI.Colors.Orange);
                }
                else
                {
                    btn_expandCOM.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
                }
            }
        }

        private async void HasBeen_Click(uint moduleName)
        {
            ValueSet request = new ValueSet();
            request.Add("deviceConfig", moduleName);
            AppServiceResponse response = await App.Connection.SendMessageAsync(request);

            if (response.Message["res_deviceConfig"] as uint? != null)
            {
                CheckDeviceState_Button((uint)response.Message["res_deviceConfig"]);
            }
        }

        private async void HasBeen_Click(uint moduleName, Modules modules)
        {
            ValueSet request = new ValueSet();
            request.Add("deviceConfig", moduleName);
            AppServiceResponse response = await App.Connection.SendMessageAsync(request);

            if (response.Message["res_deviceConfig"] as uint? != null)
            {
                CheckDeviceState_Button((uint)response.Message["res_deviceConfig"], modules);
            }
        }

        private void return_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void btn_wifi_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.Wifi, Modules.Wifi);

           // if (MainPage.startupHindCount >= 2) MainPage.startupHindCount += 2;
        }

        private void btn_gobi3G_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.Gobi3G, Modules.Gobi3G);
            //if (MainPage.startupHindCount == 4) MainPage.startupHindCount = 6;
            //else MainPage.startupHindCount = 10;
        }

        private void btn_GPS_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.GPS, Modules.GPS);
        }

        private void btn_bluetooth_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.Bluetooth, Modules.Bluetooth);
        }

        private void btn_webCamRear_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.WebCamRear, Modules.WebCamRear);
        }

        private void btn_allLED_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.AllLED, Modules.AllLED);
        }

        private void btn_barcode_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.Barcode, Modules.Barcode);
        }

        private void btn_webCamFront_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.WebCamFront, Modules.WebCamFront);
        }

        private void btn_RFID_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.RFID, Modules.RFID);
        }

        private void btn_GPSAntenna_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.GPSAntenna, Modules.GPSAntenna);
        }

        private void btn_expandUSB_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.ExpandUSB, Modules.ExpandUSB);
        }

        private void btn_expandCOM_Click(object sender, RoutedEventArgs e)
        {
            HasBeen_Click((uint)Modules.ExpandCOM, Modules.ExpandCOM);
        }
    }
}
