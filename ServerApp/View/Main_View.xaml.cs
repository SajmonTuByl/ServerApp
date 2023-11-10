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
using MySql.Data.MySqlClient;
using System.Data;

namespace ServerApp.View
{
    /// <summary>
    /// Logika interakcji dla klasy Main_View.xaml
    /// </summary>
    public partial class Main_View
    {
        public static Main_ViewModel Main_ViewModel { get; set; }

        WebSocketServer wssv;
        static MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;

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
                    foreach (var sensor1 in item.SensorsList)  // Tutaj z jakiegoś powodu pojawia się zduplikowany sensor
                    {
                        if (Main_ViewModel.SensorsList.Count != 0)
                        {
                            if (conn != null && conn.State == ConnectionState.Open) AddSensorToDb(sensor1.ParentId, sensor1.SensorId, sensor1.TimeStamp, sensor1.SensorValue, sensor1.SensorUnit, conn);
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
            StopServer();
        }

        public void StartServer()
        {
            Main_ViewModel.ServerStatus = "Uruchamiam...";
            wssv = new WebSocketServer("ws://" + Main_ViewModel.ServerIp + ":" + Main_ViewModel.ServerPort);
            wssv.AddWebSocketService<DataReceiving_Model>("/");
            try
            {
                Button_Connect.IsEnabled = false;
                Button_Disconnect.IsEnabled = true;
                TextBox_IPAddress.IsEnabled = false;
                TextBox_PortNo.IsEnabled = false;
                wssv.Start();
                Main_ViewModel.ServerStatus = "Uruchomiony";
            }
            catch (Exception ex)
            {
                Main_ViewModel.ServerStatus = ex.Message;
                Button_Connect.IsEnabled = true;
                Button_Disconnect.IsEnabled = false;
                TextBox_IPAddress.IsEnabled = true;
                TextBox_PortNo.IsEnabled = true;
            }
        }
        public void StopServer()
        {
            try
            {
                wssv.Stop();
                TextBox_IPAddress.IsEnabled = true;
                TextBox_PortNo.IsEnabled = true;
                Button_Connect.IsEnabled = true;
                Button_Disconnect.IsEnabled = false;
                Main_ViewModel.ServerStatus = "Nieuruchomiony";
            } catch (Exception ex)
            {
                Main_ViewModel.ServerStatus = ex.Message;
            }
            
        }

        public void StartDb()
        {
            Main_ViewModel.DbStatus = "Łączę...";
            string myConnectionString;

            myConnectionString = "server=" + Main_ViewModel.DbIp + ";port=" + Main_ViewModel.DbPort + ";uid=TestUser;pwd=TestPassword;database=TestDb";

            try
            {
                Button_ConnectDb.IsEnabled = false;
                Button_DisconnectDb.IsEnabled = true;
                TextBox_DbIPAddress.IsEnabled = false;
                TextBox_DbPortNo.IsEnabled = false;

                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                cmd = new MySql.Data.MySqlClient.MySqlCommand();

                Main_ViewModel.DbStatus = "Podłączony";
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Main_ViewModel.DbStatus = ex.Message;
                Button_ConnectDb.IsEnabled = true;
                Button_DisconnectDb.IsEnabled = false;
                TextBox_DbIPAddress.IsEnabled = true;
                TextBox_DbPortNo.IsEnabled = true;

                /*
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                */
            }          
        }
        public void StopDb()
        {
            try
            {
                conn.Close();
                Main_ViewModel.DbStatus = "Niepodłączony";
                Button_ConnectDb.IsEnabled = true;
                Button_DisconnectDb.IsEnabled = false;
                TextBox_DbIPAddress.IsEnabled = true;
                TextBox_DbPortNo.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Main_ViewModel.DbStatus = ex.Message;
            }
        }

        public static void AddSensorToDb(int parentId, int sensorId, DateTime date, float value, string unit, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            //https://dev.mysql.com/doc/connector-net/en/connector-net-programming-prepared.html
            try
            {
                string tableName = parentId.ToString() + "_" + sensorId.ToString();
                
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conn;

                cmd.CommandText = "INSERT INTO " + tableName + " VALUES('" + date.ToString() + "', '" + value + "', '" + unit + "')";
                cmd.ExecuteNonQuery();

                /*
                for (int i = 1; i <= 1000; i++)
                {
                    cmd.Parameters["@number"].Value = i;
                    cmd.Parameters["@text"].Value = "A string value";

                    cmd.ExecuteNonQuery();
                }
                */
            }
            catch(Exception ex)
            {
                CreateTable(parentId, sensorId, conn);
            }
        }

        public static void CreateTable(int parentId, int sensorId, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            try
            {
                string tableName = parentId.ToString() + "_" + sensorId.ToString();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conn;
                cmd.CommandText =
                    "CREATE TABLE " + tableName + " (Date VARCHAR(20), Value FLOAT, Unit VARCHAR(10));";

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddDeviceToDb(
            int deviceId, 
            string deviceName, 
            string deviceIpAddress, 
            string deviceStatus, 
            int deviceUpdateInterval, 
            int deviceBatteryLevel)
        {
           
        }

        private void Button_DisconnectDb_Click(object sender, RoutedEventArgs e)
        {
            StopDb();
        }

        private void Button_ConnectDb_Click(object sender, RoutedEventArgs e)
        {
            StartDb();
        }

        //To check if you're connected or not:
        //System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
    }

}
