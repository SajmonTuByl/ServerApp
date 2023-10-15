using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Model
{
    public class Main_Model : INotifyPropertyChanged
    {
        private BindingList<DeviceObj_Model> devicesList = new BindingList<DeviceObj_Model>();

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
            }

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}