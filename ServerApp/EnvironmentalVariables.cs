using ServerApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public sealed class EnvironmentalVariables : INotifyPropertyChanged
    {
        private static EnvironmentalVariables instance = new EnvironmentalVariables();
        public static EnvironmentalVariables Instance
        {
            get
            {
                return instance;
            }
        }

        private EnvironmentalVariables() { }

        private ObservableCollection<DeviceObj_Model> devicesList = new ObservableCollection<DeviceObj_Model>();

        public ObservableCollection<DeviceObj_Model> DevicesList
        {
            get
            {
                return devicesList;
            }
            set
            {
                if (devicesList != value)
                {
                    devicesList = value;
                    this.RaiseNotifyPropertyChanged("DevicesList");
                }
            }
        }

        private void RaiseNotifyPropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
