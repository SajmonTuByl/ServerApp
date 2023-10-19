using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Model
{
    public class Deserialization_Model
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIpAddress { get; set; }
        public string DeviceStatus { get; set; }
        public int DeviceUpdateInterval { get; set; }
        public int DeviceBatteryLevel { get; set; }

        public int[] SensorId { get; set; }
        public string[] SensorName { get; set; }
        public string[] SensorStatus { get; set; }
        public string[] SensorType { get; set; }
        public float[] SensorValue { get; set; }
        public string[] SensorUnit { get; set; }

    }
}
