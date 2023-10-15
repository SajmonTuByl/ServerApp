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
        private int parentSerialNo;
        private string parentName;
        private int sensorSerialNo;
        private string sensorName;
        private string sensorStatus;
        private string sensorType;

        private float sensorValue_1;
        private float sensorValue_2;
        private string sensorUnit_1;
        private string sensorUnit_2;

        public int ParentSerialNo
        {
            get { return parentSerialNo; }
            set { parentSerialNo = value; OnPropertyChanged(); }
        }
        public string ParentName
        {
            get { return parentName; }
            set { parentName = value; OnPropertyChanged(); }
        }
        public int SensorSerialNo 
        {
            get { return sensorSerialNo; } 
            set { sensorSerialNo = value; OnPropertyChanged(); }
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
        public float SensorValue_1 
        {
            get { return sensorValue_1; }
            set { sensorValue_1 = value; OnPropertyChanged(); }
        }
        public float SensorValue_2 
        {
            get { return sensorValue_2; }
            set { sensorValue_2 = value; OnPropertyChanged(); }
        }
        public string SensorUnit_1 
        {
            get { return sensorUnit_1; }
            set { sensorUnit_1 = value; OnPropertyChanged(); }
        }
        public string SensorUnit_2 
        {
            get { return sensorUnit_2; }
            set { sensorUnit_2 = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
