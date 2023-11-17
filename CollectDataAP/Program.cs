using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace CollectDataAP
{
    class Program
    { 
        static void Main(string[] args)
        {
            Connect2UWP connect2UWP = new Connect2UWP();

            string s_res;

            connect2UWP.InitializeAppServiceConnection();

            Console.WriteLine("[1] send data to UWP");
            while ((s_res = Console.ReadLine()) != "")
            {
                int i_res=int.Parse(s_res);

                if (i_res == 1)
                {
                    connect2UWP.Send2UWP_2("Hi!", "UWP");
                }
            }
        }
    }
}
