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

namespace ServerApp.View
{
    /// <summary>
    /// Logika interakcji dla klasy Main_View.xaml
    /// </summary>
    public partial class Main_View
    {
        WebSocketServer wssv;
        public static ObservableCollection<DeviceObj_Model> devicesList = new ObservableCollection<DeviceObj_Model>();
        

        public Main_View()
        {
            InitializeComponent();
            DataGrid_Sensors.DataContext = devicesList;

            DeserializeReceivedData data = new DeserializeReceivedData();
            data.ReceivingData += OnReceivingData;

            //this.Dispatcher.BeginInvoke(new Action(() => DataGrid_Sensors.DataContext = EnvironmentalVariables.Instance.DevicesList));
            //DataGrid_Sensors.ItemsSource = DataReceiving_Model.devicesList;

            GlobalVariables.serverIp = GetLocalIPAddress();

            // Inicjuję wartości domyślne dla adresu IP oraz numeru portu
            TextBox_IPAddress.Text = GlobalVariables.serverIp;
            TextBox_PortNo.Text = GlobalVariables.serverPort;
        }

        // Kliknięcie w przycisk "Connect" uruchamia nasłuch na danym porcie
        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.serverIp = TextBox_IPAddress.Text;
            GlobalVariables.serverPort = TextBox_PortNo.Text;

            
            
            wssv = new WebSocketServer("ws://" + GlobalVariables.serverIp + ":" + GlobalVariables.serverPort);
            wssv.AddWebSocketService<DataReceiving_Model>("/");
            wssv.Start();

            if (wssv.IsListening) GlobalVariables.serverStatus = "Working";
            else GlobalVariables.serverStatus = "Disconnected";
            Label_Status.Content = GlobalVariables.serverStatus; // To przerobić na singletona, aby można było dołożyć PropertyChanged

            
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
            /*
            foreach (var device in DataReceiving_Model.devicesList)
            {
                devices.Add(device);
            }
            */
            //DataGrid_Sensors.ItemsSource = devices;
            DataGrid_Sensors.Items.Refresh();
        }

        //To check if you're connected or not:
        //System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        public static void OnReceivingData(object source, ReceivingDataEventArgs args)
        {
            devicesList.Add(args.Device);
        //https://stackoverflow.com/questions/11142177/pass-by-value-in-c-sharp
        }
    }
}
