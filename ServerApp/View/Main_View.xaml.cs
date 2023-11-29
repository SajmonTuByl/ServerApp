using System;
using System.Net.Sockets;
using System.Net;
using System.Windows;
using ServerApp.Model;
using WebSocketSharp.Server;
using ServerApp.ViewModel;
using MySql.Data.MySqlClient;
using System.Data;
using CsvHelper;
using System.IO;
using System.Globalization;
using Microsoft.Win32;

namespace ServerApp.View
{
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
                bool foundDevice = false;
                foreach (var item in Main_ViewModel.DevicesList)
                {
                    if (item.DeviceId == device.DeviceId)
                    {
                        Main_ViewModel.DevicesList.Remove(item);
                        Main_ViewModel.DevicesList.Add(device);
                        foundDevice = true;
                        break;
                    }
                }
                if (!foundDevice) Main_ViewModel.DevicesList.Add(device);

                // Dodawanie sensorów do SensorsLIst
                foreach (var item in Main_ViewModel.DevicesList)
                {
                    foreach (var receivedSensor in item.SensorsList)
                    {
                        if (conn != null && conn.State == ConnectionState.Open) AddSensorToDb(receivedSensor.ParentId, receivedSensor.SensorId, receivedSensor.TimeStamp, receivedSensor.SensorValue, conn);
                            bool foundSensor = false;
                            foreach (var sensor in Main_ViewModel.SensorsList)
                            {
                                // Jeżeli SensorId i ParentId nowego są takie same jak już istniejącego
                                if (receivedSensor.SensorId == sensor.SensorId && receivedSensor.ParentId == sensor.ParentId)
                                {
                                    // to zauktualizuj tylko wartość i znacznik czasu
                                    sensor.SensorValue = receivedSensor.SensorValue;
                                    sensor.TimeStamp = receivedSensor.TimeStamp;

                                    // i dodaj te wartości również do zbioru punktów do rysowania wykresu
                                    // ale najpierw sprawdź, czy auto update jest true
                                    // i czy nie ma za dużo próbek do rysowania wykresu, tj. więcej niż Main_ViewModel.SamplesNo
                                    if (Main_ViewModel.ChartAutoUpdate == true)
                                    {
                                        int oldSamples = sensor.Samples.Count - Main_ViewModel.SamplesCount;
                                        for (int i = 0; i < oldSamples; i++) sensor.Samples.RemoveAt(0);

                                        sensor.Samples.Add(new SensorSample { DateTime = receivedSensor.TimeStamp, Value = receivedSensor.SensorValue });
                                    }
                                    foundSensor = true;
                                    break;
                                }
                            }
                            if (!foundSensor) Main_ViewModel.SensorsList.Add(receivedSensor); ;
                    }
                }
            });
        }

        // Kliknięcie w przycisk "Connect" uruchamia nasłuch na danym porcie
        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            StartServer();
        }

        private void Button_Disconnect_Click(object sender, RoutedEventArgs e)
        {
            StopServer();
        }

        private void Button_DisconnectDb_Click(object sender, RoutedEventArgs e)
        {
            StopDb();
        }

        private void Button_ConnectDb_Click(object sender, RoutedEventArgs e)
        {
            StartDb();
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
            }
            catch (Exception ex)
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
                getDataFromDb.IsEnabled = true;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Main_ViewModel.DbStatus = ex.Message;
                Button_ConnectDb.IsEnabled = true;
                Button_DisconnectDb.IsEnabled = false;
                TextBox_DbIPAddress.IsEnabled = true;
                TextBox_DbPortNo.IsEnabled = true;
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
                getDataFromDb.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Main_ViewModel.DbStatus = ex.Message;
            }
        }

        public static void AddSensorToDb(int parentId, int sensorId, DateTime date, float value, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            try
            {
                string tableName = parentId.ToString() + "_" + sensorId.ToString();
                
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = conn;

                cmd.CommandText = "INSERT INTO " + tableName + " VALUES('" + date.ToString("s") + "', '" + value.ToString().Replace(",", ".") + "')";
                cmd.ExecuteNonQuery();
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
                    "CREATE TABLE " + tableName + " (DateStamp DateTime, Value DECIMAL(12,2))";

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetDataFromDb(int parentId, int sensorId, DateTime dateFrom, DateTime dateTo, MySql.Data.MySqlClient.MySqlConnection conn)
        { 
            try
            {
                string tableName = parentId.ToString() + "_" + sensorId.ToString();
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = "SELECT * FROM " + tableName + " WHERE DateStamp BETWEEN '" + 
                    dateFrom.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + 
                    dateTo.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DateTime date = reader.GetDateTime(reader.GetOrdinal("DateStamp"));
                    float value = reader.GetFloat(reader.GetOrdinal("Value"));
                    Main_ViewModel.SelectedSensor.Samples.Add(new SensorSample { DateTime = date, Value = value });
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ExportData()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Wartości rozdzielone przecinkiem|*.csv|Plik tekstowy|*.txt";
            saveFileDialog1.Title = "Zapisz dane pomiarowe";
            saveFileDialog1.ShowDialog();

            using var writer = new StreamWriter(saveFileDialog1.OpenFile());
            using var csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csvWriter.WriteHeader<SensorSample>();
            csvWriter.NextRecord();
            csvWriter.WriteRecords(Main_ViewModel.SelectedSensor.Samples);
        }

        private void exportData_Click(object sender, RoutedEventArgs e)
        {
            Main_ViewModel.ChartAutoUpdate = false;
            ExportData();
        }

        private void cleanGraph_Click(object sender, RoutedEventArgs e)
        {
            foreach (var sensor in Main_ViewModel.SensorsList)
            {
                if (sensor.ParentId == Main_ViewModel.SelectedSensor.ParentId && sensor.SensorId == Main_ViewModel.SelectedSensor.SensorId)
                    sensor.Samples.Clear();
            }
        }

        private void getDataFromDb_Click(object sender, RoutedEventArgs e)
        {
            Main_ViewModel.ChartAutoUpdate = false;
            Main_ViewModel.SelectedSensor.Samples.Clear();
            GetDataFromDb(
                Main_ViewModel.SelectedSensor.ParentId,
                Main_ViewModel.SelectedSensor.SensorId,
                Main_ViewModel.SelectedDateRange.GetFromDateTime(),
                Main_ViewModel.SelectedDateRange.GetToDateTime(),
                conn);

        }
    }
}
