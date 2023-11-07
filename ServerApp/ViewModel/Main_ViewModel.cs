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
    public class Main_ViewModel : INotifyPropertyChanged
    {
        private string serverIp;
        private string serverPort;
        private string serverStatus;
        private string dbStatus;

        private int selectedSensorParentId;
        private int selectedSensorSensorId;

        public ObservableCollection<DeviceObj_Model> DevicesList { get; set; }
        public ObservableCollection<SensorObj_Model> SensorsList { get; set; }

        public string ServerIp
        {
            get => serverIp;
            set
            {
                serverIp = value;
                OnPropertyChanged("ServerIp");
            }
        }
        public string ServerPort
        {
            get => serverPort;
            set
            {
                serverPort = value;
                OnPropertyChanged("ServerPort");
            }
        }
        public string ServerStatus
        {
            get => serverStatus;
            set
            {
                serverStatus = value;
                OnPropertyChanged("ServerStatus");
            }
        }

        public string DbStatus
        {
            get => dbStatus;
            set
            {
                dbStatus = value;
                OnPropertyChanged("DbStatus");
            }
        }

        public int SelectedSensorParentId
        {
            get => selectedSensorParentId;
            set
            {
                selectedSensorParentId = value;
                OnPropertyChanged("SelectedSensorParentId");
            }
        }
        public int SelectedSensorSensorId
        {
            get => selectedSensorSensorId;
            set
            {
                selectedSensorSensorId = value;
                OnPropertyChanged("SelectedSensorSensorId");
            }
        }

        public Main_ViewModel()
        {
            DevicesList = new ObservableCollection<DeviceObj_Model>();
            SensorsList = new ObservableCollection<SensorObj_Model>();

            ServerIp = "127.0.0.1";
            ServerPort = "11000";
            ServerStatus = "test";
            DbStatus = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
