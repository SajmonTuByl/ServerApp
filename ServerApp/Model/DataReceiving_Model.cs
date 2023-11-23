using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using ServerApp.View;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace ServerApp.Model
{
    public class DataReceiving_Model : WebSocketBehavior
    {
        DeviceObj_Model Device { get; set; }

        public DataReceiving_Model()
        {
            Device = new DeviceObj_Model();
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            DeserializeData(e.Data);

            Main_View.GetNewDevice(Device);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }

        public void DeserializeData(string data)
        {
            Deserialization_Model? deserialization_Model = JsonSerializer.Deserialize<Deserialization_Model>(data);

            Device.DeviceId = (int)(deserialization_Model.deviceId != null ? deserialization_Model.deviceId : 0);
            Device.DeviceName = deserialization_Model.deviceName != null ? deserialization_Model.deviceName : "";
            Device.DeviceIpAddress = deserialization_Model.deviceIpAddress != null ? deserialization_Model.deviceIpAddress : "";
            Device.DeviceStatus = deserialization_Model.deviceStatus != null ? deserialization_Model.deviceStatus : "";
            Device.DeviceUpdateInterval = (int)(deserialization_Model.deviceUpdateInterval != null ? deserialization_Model.deviceUpdateInterval : 0);
            Device.DeviceBatteryLevel = (int)(deserialization_Model.deviceBatteryLevel != null ? deserialization_Model.deviceBatteryLevel : 0);

            int counter = 0;
            foreach (var item in deserialization_Model.sensorStatus)
            {
                if (item != null && item != "")
                    counter++;
            }
            Device.SensorsList.Clear();
            for (int i = 0; i < counter; i++)
            {
                SensorObj_Model sensorObj_Model = new SensorObj_Model();
                sensorObj_Model.ParentId = (int)deserialization_Model.deviceId;
                sensorObj_Model.ParentName = deserialization_Model.deviceName;
                sensorObj_Model.SensorId = deserialization_Model.sensorId[i];
                sensorObj_Model.SensorName = deserialization_Model.sensorName[i];
                sensorObj_Model.SensorStatus = deserialization_Model.sensorStatus[i];
                sensorObj_Model.SensorType = deserialization_Model.sensorType[i];

                sensorObj_Model.TimeStamp = DateTime.Now;

                sensorObj_Model.SensorValue = deserialization_Model.sensorValue[i];
                sensorObj_Model.SensorUnit = deserialization_Model.sensorUnit[i];

                Device.SensorsList.Add(sensorObj_Model);
            }
        }
    }
    
}
