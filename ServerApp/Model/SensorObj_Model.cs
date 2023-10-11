using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Model
{
    class SensorObj_Model
    {
        private int sensorSerialNo;
        private string sensorName;
        private string sensorStatus;
        private string sensorType;

        private float sensorValue_1;
        private float sensorValue_2;
        private string sensorUnit_1;
        private string sensorUnit_2;

        public int SensorSerialNo { get => sensorSerialNo; set => sensorSerialNo = value; }
        public string SensorName { get => sensorName; set => sensorName = value; }
        public string SensorStatus { get => sensorStatus; set => sensorStatus = value; }
        public string SensorType { get => sensorType; set => sensorType = value; }
        public float SensorValue_1 { get => sensorValue_1; set => sensorValue_1 = value; }
        public float SensorValue_2 { get => sensorValue_2; set => sensorValue_2 = value; }
        public string SensorUnit_1 { get => sensorUnit_1; set => sensorUnit_1 = value; }
        public string SensorUnit_2 { get => sensorUnit_2; set => sensorUnit_2 = value; }
    }
}
