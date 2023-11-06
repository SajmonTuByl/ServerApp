using ServerApp.Model;
using ServerApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ServerApp.ViewModel
{
    public class Main_ViewModel
    {
        public Main_View Main_View { get; set; }
        
        public Main_ViewModel()
        {
            Main_View = new Main_View();
            DeserializeReceivedData data = new DeserializeReceivedData();
            data.ReceivingData += OnReceivingData;
        }

        public void OnReceivingData(object source, ReceivingDataEventArgs args)
        {
            Main_View.DevicesList.Add(args.Device);
        }
    }
}
