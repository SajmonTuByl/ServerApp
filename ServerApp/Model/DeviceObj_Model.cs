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
        private int deviceId;
        private string deviceName;
        private string deviceIpAddress;
        private string deviceStatus;
        private int deviceUpdateInterval; //miliseconds
        private int deviceBatteryLevel;

        private BindingList<SensorObj_Model> sensorsList = new BindingList<SensorObj_Model>();

        public int DeviceId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
                OnPropertyChanged(nameof(DeviceId));
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
                OnPropertyChanged(nameof(DeviceName));
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
                OnPropertyChanged(nameof(DeviceIpAddress));
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
                OnPropertyChanged(nameof(DeviceStatus));
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
                OnPropertyChanged(nameof(DeviceUpdateInterval));
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
                OnPropertyChanged(nameof(DeviceBatteryLevel));
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
