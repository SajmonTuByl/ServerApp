using ServerApp.Model;
using ServerApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerApp
{
    public class ReceivingDataEventArgs : EventArgs
    {
        public DeviceObj_Model Device { get; set; }
    }

    public class DeserializeReceivedData
    {
        public delegate void ReceivingDataEventHandler(object source, ReceivingDataEventArgs args);
        public event ReceivingDataEventHandler ReceivingData;

        static DeviceObj_Model device = new DeviceObj_Model();
        public void DeserializeData(string data)
        {
            Deserialization_Model? deserialization_Model = JsonSerializer.Deserialize<Deserialization_Model>(data);
            SensorObj_Model sensorObj_Model = new SensorObj_Model();

            device.DeviceId = (int)(deserialization_Model.deviceId != null ? deserialization_Model.deviceId : 0);
            device.DeviceName = deserialization_Model.deviceName != null ? deserialization_Model.deviceName : "";
            device.DeviceIpAddress = deserialization_Model.deviceIpAddress != null ? deserialization_Model.deviceIpAddress : "";
            device.DeviceStatus = deserialization_Model.deviceStatus != null ? deserialization_Model.deviceStatus : "";
            device.DeviceUpdateInterval = (int)(deserialization_Model.deviceUpdateInterval != null ? deserialization_Model.deviceUpdateInterval : 0);
            device.DeviceBatteryLevel = (int)(deserialization_Model.deviceBatteryLevel != null ? deserialization_Model.deviceBatteryLevel : 0);

            int counter = 0;
            foreach (var item in deserialization_Model.sensorStatus)
            {
                if (item != null && item != "")
                    counter++;
            }

            for (int i = 0; i < counter; i++)
            {
                sensorObj_Model.ParentId = (int)deserialization_Model.deviceId;
                sensorObj_Model.ParentName = deserialization_Model.deviceName;
                sensorObj_Model.SensorId = deserialization_Model.sensorId[i];
                sensorObj_Model.SensorName = deserialization_Model.sensorName[i];
                sensorObj_Model.SensorStatus = deserialization_Model.sensorStatus[i];
                sensorObj_Model.SensorType = deserialization_Model.sensorType[i];

                sensorObj_Model.TimeStamp = DateTime.Now;

                sensorObj_Model.SensorValue = deserialization_Model.sensorValue[i];
                sensorObj_Model.SensorUnit = deserialization_Model.sensorUnit[i];

                device.SensorsList.Add(sensorObj_Model);
            }
            OnReceivingData(device);
        }

        public virtual void OnReceivingData(DeviceObj_Model Device)
        {
            if (ReceivingData != null)
                ReceivingData(this, new ReceivingDataEventArgs() { Device = device });
        }
    }
}
