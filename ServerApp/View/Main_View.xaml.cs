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
        public Main_ViewModel Main_ViewModel { get; set; }
        WebSocketServer wssv;

        public Main_View()
        {
            InitializeComponent();
            this.DataContext = Main_ViewModel;

            // To wrzucić do ViewModel
            GlobalVariables.serverIp = GetLocalIPAddress();
            TextBox_IPAddress.Text = GlobalVariables.serverIp;
            TextBox_PortNo.Text = GlobalVariables.serverPort;
        }

        // Kliknięcie w przycisk "Connect" uruchamia nasłuch na danym porcie
        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            // To wrzucić do ViewModel
            GlobalVariables.serverIp = TextBox_IPAddress.Text;
            GlobalVariables.serverPort = TextBox_PortNo.Text;

            StartServer();

            // To wrzucić do ViewModel
            Label_Status.Content = GlobalVariables.serverStatus;

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
        }

        public void StartServer()
        {
            wssv = new WebSocketServer("ws://" + GlobalVariables.serverIp + ":" + GlobalVariables.serverPort);
            wssv.AddWebSocketService<DataReceiving_Model>("/");
            wssv.Start();

            if (wssv.IsListening) GlobalVariables.serverStatus = "Working";
            else GlobalVariables.serverStatus = "Disconnected";
        }





        //To check if you're connected or not:
        //System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();



        /*
        public BindingList<DeviceObj_Model> DevicesList
        {
            get
            {
                return devicesList;
            }
            set
            {
                devicesList = value;
                OnPropertyChanged();
                UpdateSensorsList();
            }

        }
        public BindingList<SensorObj_Model> SensorsList
        {
            get
            {
                return sensorsList;
            }
            set
            {
                sensorsList = value;
                OnPropertyChanged();
            }

        }
        public void UpdateSensorsList()
        {
            SensorsList.Clear();
            foreach (var device in devicesList)
            {
                foreach (var sensor in device.SensorsList)
                {
                    SensorsList.Add(sensor);
                }
            }
        }
        */
    }

}
