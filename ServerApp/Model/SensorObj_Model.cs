using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Model
{
    public class SensorObj_Model : INotifyPropertyChanged
    {
        private int parentId;
        private string parentName;
        private int sensorId;
        private string sensorName;
        private string sensorStatus;
        private string sensorType;
        private DateTime timeStamp;

        private float sensorValue;
        private string sensorUnit;

        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; OnPropertyChanged(); }
        }
        public string ParentName
        {
            get { return parentName; }
            set { parentName = value; OnPropertyChanged(); }
        }
        public int SensorId 
        {
            get { return sensorId; } 
            set { sensorId = value; OnPropertyChanged(); }
        }
        public string SensorName
        {
            get { return sensorName; }
            set { sensorName = value; OnPropertyChanged(); }
        }
        public string SensorStatus
        { 
            get { return sensorStatus; }
            set { sensorStatus = value; OnPropertyChanged(); }
        }
        public string SensorType 
        {
            get { return sensorType; }
            set { sensorType = value; OnPropertyChanged(); }
        }
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; OnPropertyChanged(); }
        }

        public float SensorValue
        {
            get { return sensorValue; }
            set { sensorValue = value; OnPropertyChanged(); }
        }
        public string SensorUnit
        {
            get { return sensorUnit; }
            set { sensorUnit = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
