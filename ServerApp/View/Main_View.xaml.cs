using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using ServerApp.Model;

namespace ServerApp.View
{
    /// <summary>
    /// Logika interakcji dla klasy Main_View.xaml
    /// </summary>
    public partial class Main_View
    {
        Socket listeningSocket;
        Socket clientSocket;
        public Main_Model main_Model = new Main_Model();

        public Main_View()
        {
            InitializeComponent();
            DataGrid_Devices.DataContext = main_Model;



            //--Obszar testowy
            SensorObj_Model testSensor = new SensorObj_Model();
            testSensor.SensorSerialNo = 111;
            testSensor.SensorName = "Czujnik testowy";
            testSensor.SensorStatus = "Działa";
            testSensor.SensorType = "temperature";

            testSensor.SensorValue_1 = 66;
            testSensor.SensorValue_2 = -15;
            testSensor.SensorUnit_1 = "oC";
            testSensor.SensorUnit_2 = "oC";

            DeviceObj_Model testDevice = new DeviceObj_Model();
            testDevice.DeviceSerialNo = 222;
            testDevice.DeviceName = "ArduinoModule_1";
            testDevice.DeviceIpAddress = "192.168.1.10";
            testDevice.DeviceStatus = "Ok";
            testDevice.DeviceUpdateInterval = 100; //miliseconds
            testDevice.DeviceBatteryLevel = 101;

            testDevice.SensorsList.Add(testSensor);

            main_Model.DevicesList.Add(testDevice);
            //--Obszar testowy - koniec

            
            //DataGrid_Devices.ItemsSource = DevicesList;
            // Inicjuję wartości domyślne dla adresu IP oraz numeru portu
            TextBox_IPAddress.Text = "127.0.0.1";
            TextBox_PortNo.Text = "11000";
        }

        // Kliknięcie w przycisk "Connect" uruchamia nasłuch na danym porcie

        // W momencie, gdy nowe urządzenie będzie chciało podłączyć się do serwera,
        // to tworzony jest nowy wątek, w którym uruchamiamy funkcję "NewClient".
        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Label_Status.Content = "Connecting...";
                IPAddress ip_address = IPAddress.Parse(TextBox_IPAddress.Text);
                int port = int.Parse(TextBox_PortNo.Text);
                IPEndPoint ep = new IPEndPoint(ip_address, port);

                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listeningSocket.Bind(ep);

                listeningSocket.Listen(10);

                Label_Status.Content = "Connected";
                Button_Connect.IsEnabled = false;
                Button_Disconnect.IsEnabled = true;

                Thread t = new Thread(this.NewClient);
                t.Start();

            }
            catch (SocketException)
            {
                Label_Status.Content = "Error. SocketException";
            }
            catch (FormatException)
            {
                Label_Status.Content = "Error. FormatException";
            }
            catch (ArgumentOutOfRangeException)
            {
                Label_Status.Content = "Error. ArgumentOutOfRangeException";
            }
        }

        // Funkcja wywoływana w trakcie podłączania się nowego urządzenia do serwera
        // Zadaniem funkcji jest stworzenie obiektu urządzenia oraz obiektów jego czujników, a następnie dodanie ich do listy dostępnych urządzeń/czujników
        public void NewClient()
        {
            while (true)
            {
                clientSocket = listeningSocket.Accept();
                Thread t = new Thread(StartReceiving);
                t.Start();
                string ip = ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString(); // Adres IP podłączonego urządzenia
                // new Thread(() => { new Form2(clientSocket).ShowDialog(); }).Start();
            }
        }
        private void StartReceiving()
        {
            while (clientSocket.Connected)
            {
                try
                {
                    byte[] bytes = new byte[1024];
                    int counter = clientSocket.Receive(bytes);
                    string data = Encoding.ASCII.GetString(bytes, 0, counter);
                    //richTextBox_MessageWindow.AppendText("Client:\n" + data + "\n");
                }
                catch (SocketException)
                {
                    //"Client is disconnected!";
                    break;
                }
                catch (ObjectDisposedException)
                {
                    MessageBox.Show("Error. ObjectDisposedException");
                    break;
                }
            }
            //label_Status.Text = "Client is disconnected!";
        }

        /*
        private void button_Send_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox_MessageWindow.AppendText("Server:\n" + textBox_Message.Text + "\n");
                byte[] message = Encoding.ASCII.GetBytes(textBox_Message.Text);
                clientSocket.Send(message);
                textBox_Message.Text = "";
            }
            catch (SocketException)
            {
                MessageBox.Show("Error. SocketException");

                clientSocket.Close();
                label_Status.Text = "Error. Disconnected";
                textBox_Message.Enabled = false;
                button_Send.Enabled = false;
            }
        }
        */
    }
}
