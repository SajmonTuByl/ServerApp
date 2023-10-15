using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Model
{
    public class DeviceObj_Model : INotifyPropertyChanged
    {
        private int deviceSerialNo;
        private string deviceName;
        private string deviceIpAddress;
        private string deviceStatus;
        private int deviceUpdateInterval; //miliseconds
        private int deviceBatteryLevel;

        private BindingList<SensorObj_Model> sensorsList = new BindingList<SensorObj_Model>();

        public int DeviceSerialNo
        {
            get
            {
                return deviceSerialNo;
            }
            set
            {
                deviceSerialNo = value;
                OnPropertyChanged();
            }
        }

        public string DeviceName
        {
            get
            {
                return deviceName;
            }
            set
            {
                deviceName = value;
                OnPropertyChanged();
            }
        }

        public string DeviceIpAddress
        {
            get
            {
                return deviceIpAddress;
            }
            set
            {
                deviceIpAddress = value;
                OnPropertyChanged();
            }
        }

        public string DeviceStatus
        {
            get
            {
                return deviceStatus;
            }
            set
            {
                deviceStatus = value;
                OnPropertyChanged();
            }
        }

        public int DeviceUpdateInterval
        {
            get
            {
                return deviceUpdateInterval;
            }
            set
            {
                deviceUpdateInterval = value;
                OnPropertyChanged();
            }
        }
        public int DeviceBatteryLevel
        {
            get
            {
                return deviceBatteryLevel;
            }
            set
            {
                deviceBatteryLevel = value;
                OnPropertyChanged();
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
