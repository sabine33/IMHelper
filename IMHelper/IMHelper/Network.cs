using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Net.Wifi;
using Android.Locations;
using System.Threading.Tasks;

namespace IMHelper
{
    class Network
    {

        private static WifiManager wifi;
        public static List<string> WiFiNetworks;
 
        public static void GetWifiNetworks(Context context,Action<List<string>> wifis)
        {
            WiFiNetworks = new List<string>();

            // Get a handle to the Wifi
            wifi = (WifiManager)context.GetSystemService(Context.WifiService);

            // Start a scan and register the Broadcast receiver to get the list of Wifi Networks
           var wifiReceiver = new WifiReceiver((list) =>
            {
                wifis(list);
            });
            context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
            wifi.StartScan();
        }
        class WifiReceiver : BroadcastReceiver
        {
            Action<List<string>> wifis;
            public WifiReceiver(Action<List<string>> action)
            {
                this.wifis = action;
            }
            public override void OnReceive(Context context, Intent intent)
            {
                IList<ScanResult> scanwifinetworks = wifi.ScanResults;
                foreach (ScanResult wifinetwork in scanwifinetworks)
                {
                    WiFiNetworks.Add(wifinetwork.Ssid);
                }
                wifis(WiFiNetworks);

            }
        }

       
     

        }
  

}
