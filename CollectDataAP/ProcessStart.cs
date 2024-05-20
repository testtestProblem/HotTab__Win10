using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollectDataAP
{
    class ProcessStart
    {
        public static void processStart_reg(string key, string value)
        {
            string path = "C:\\Program Files\\HotTab\\RegistryKey.exe";
            if (!File.Exists(path))
            {
                MessageBox.Show("Can not find " + path, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;

                        string data = key + " " + value;
                        myProcess.StartInfo.Arguments = data;

                        // You can start any process, HelloWorld is a do-nothing example.
                        //myProcess.StartInfo.FileName = @"C:\Users\WIN10\source\repos\RegistryKey\RegistryKey\bin\Debug\RegistryKey.exe";
                        myProcess.StartInfo.FileName = path;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                        // This code assumes the process you are starting will terminate itself.
                        // Given that it is started without a window so you cannot terminate it
                        // on the desktop, it must terminate itself or you can do it programmatically
                        // from this application using the Kill method.
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void processStart_KillProcesses(string name)
        {
            string path = "C:\\Program Files\\HotTab\\KillProcess_forHotTab.exe";
            if (!File.Exists(path))
            {
                MessageBox.Show("Can not find " + path, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;

                        myProcess.StartInfo.Arguments = name;

                        // You can start any process, HelloWorld is a do-nothing example.
                        //myProcess.StartInfo.FileName = @"C:\Users\WIN10\source\repos\RegistryKey\RegistryKey\bin\Debug\RegistryKey.exe";
                        myProcess.StartInfo.FileName = path;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                        // This code assumes the process you are starting will terminate itself.
                        // Given that it is started without a window so you cannot terminate it
                        // on the desktop, it must terminate itself or you can do it programmatically
                        // from this application using the Kill method.
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
