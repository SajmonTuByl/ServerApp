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
        public int? deviceId { get; set; }
        public string? deviceName { get; set; }
        public string? deviceIpAddress { get; set; }
        public string? deviceStatus { get; set; }
        public int? deviceUpdateInterval { get; set; }
        public int? deviceBatteryLevel { get; set; }

        public int[]? sensorId { get; set; }
        public string[]? sensorName { get; set; }
        public string[]? sensorStatus { get; set; }
        public string[]? sensorType { get; set; }
        public float[]? sensorValue { get; set; }
        public string[]? sensorUnit { get; set; }

    }
}
