using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using ServerApp.Model;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Text.Json;
using ServerApp.ViewModel;

namespace ServerApp.View
{
    /// <summary>
    /// Logika interakcji dla klasy Main_View.xaml
    /// </summary>
    public partial class Main_View
    {
        public static Main_ViewModel Main_ViewModel { get; set; }
        WebSocketServer wssv;

        public Main_View()
        {
            Main_ViewModel = new Main_ViewModel();

            InitializeComponent();
            this.DataContext = Main_ViewModel;
            Main_ViewModel.ServerIp = GetLocalIPAddress();
        }

        public static void GetNewDevice(DeviceObj_Model device)
        {
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                // Dodawanie urządzeń do DevicesList
                if (Main_ViewModel.DevicesList.Count == 0) Main_ViewModel.DevicesList.Add(device);
                foreach (var item in Main_ViewModel.DevicesList)
                {
                    if (item.DeviceId == device.DeviceId)
                    {
                        Main_ViewModel.DevicesList.Remove(item);
                        Main_ViewModel.DevicesList.Add(device);
                        break;
                    }
                }

                // Dodawanie sensorów do SensorsLIst
                foreach (var item in Main_ViewModel.DevicesList)
                {
                    foreach (var sensor1 in item.SensorsList)
                    {
                        if (Main_ViewModel.SensorsList.Count != 0)
                        {
                            foreach (var sensor2 in Main_ViewModel.SensorsList)
                            {
                                if (sensor1.SensorId == sensor2.SensorId && sensor1.ParentId == sensor2.ParentId)
                                {
                                    Main_ViewModel.SensorsList.Remove(sensor2);
                                    Main_ViewModel.SensorsList.Add(sensor1);
                                    break;
                                }
                            }
                        }
                        else Main_ViewModel.SensorsList.Add(sensor1);
                    }
                }
            });
        }

        // Kliknięcie w przycisk "Connect" uruchamia nasłuch na danym porcie
        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            StartServer();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void Button_Disconnect_Click(object sender, RoutedEventArgs e)
        {
            DataGrid_Sensors.Items.Refresh();
            Label_Status.UpdateLayout();
        }

        public void StartServer()
        {
            wssv = new WebSocketServer("ws://" + Main_ViewModel.ServerIp + ":" + Main_ViewModel.ServerPort);
            wssv.AddWebSocketService<DataReceiving_Model>("/");
            wssv.Start();

            if (wssv.IsListening) Main_ViewModel.ServerStatus = "Working";
            else Main_ViewModel.ServerStatus = "Disconnected";
        }
        //To check if you're connected or not:
        //System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
    }

}
