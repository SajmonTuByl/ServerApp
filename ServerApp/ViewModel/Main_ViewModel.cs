using ServerApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.ViewModel
{
    public class Main_ViewModel : INotifyPropertyChanged
    {
        /*
        private BindingList<DeviceObj_Model> devicesList = new BindingList<DeviceObj_Model>();
        private BindingList<SensorObj_Model> sensorsList = new BindingList<SensorObj_Model>();

        //DataGrid_Devices.DataContext = main_Model.DevicesList;
        //DataGrid_Devices.DataContext = GlobalVariables.DevicesList;
        // Trzeba powiązać DataContext w XAMLu do tej klasy!!!

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
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //public event PropertyChangedEventHandler? PropertyChanged;
    }
}
