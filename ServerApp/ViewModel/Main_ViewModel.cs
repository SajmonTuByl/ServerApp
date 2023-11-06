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
        public ObservableCollection<DeviceObj_Model> DevicesList { get; set; }

        public Main_ViewModel()
        {
        }
    }
}
