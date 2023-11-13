using LiveCharts;
using ServerApp.ViewModel;
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

        private ChartValues<SensorSample> samples = new ChartValues<SensorSample>();

        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; OnPropertyChanged("ParentId"); }
        }
        public string ParentName
        {
            get { return parentName; }
            set { parentName = value; OnPropertyChanged("ParentName"); }
        }
        public int SensorId 
        {
            get { return sensorId; } 
            set { sensorId = value; OnPropertyChanged("SensorId"); }
        }
        public string SensorName
        {
            get { return sensorName; }
            set { sensorName = value; OnPropertyChanged("SensorName"); }
        }
        public string SensorStatus
        { 
            get { return sensorStatus; }
            set { sensorStatus = value; OnPropertyChanged("SensorStatus"); }
        }
        public string SensorType 
        {
            get { return sensorType; }
            set { sensorType = value; OnPropertyChanged("SensorType"); }
        }
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; OnPropertyChanged("TimeStamp"); }
        }

        public float SensorValue
        {
            get { return sensorValue; }
            set { sensorValue = value; OnPropertyChanged("SensorValue"); }
        }
        public string SensorUnit
        {
            get { return sensorUnit; }
            set { sensorUnit = value; OnPropertyChanged("SensorUnit"); }
        }

        public ChartValues<SensorSample> Samples
        {
            get { return samples; }
            set { samples = value; OnPropertyChanged("Samples"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
