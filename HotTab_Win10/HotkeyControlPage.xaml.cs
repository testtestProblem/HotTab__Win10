using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        HotkeyList hotkeyList;
        FunctionList functionList;

        public enum HotkeyList : uint
        {
            f1Short = 0x1,
            f2Short = 0x2,
            f3Short = 0x4,

            f1Long = 0x8,
            f2Long = 0x10,
            f3Long = 0x20
        };

        public enum FunctionList : uint
        {
            volumeUp = 0x1,
            volumeDown = 0x2,

            backlight20 = 0x4,
            backlight100 = 0x8,

            Calc = 0x10,
            cmd = 0x20
        };

        public HotkeyControlPage()
        {
            this.InitializeComponent();
        }

        private void return_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void f1Short_btn_Click(object sender, RoutedEventArgs e)
        {
            hotkeyList = HotkeyList.f1Short;
        }

        private void func2_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
