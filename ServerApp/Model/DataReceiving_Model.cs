using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using ServerApp.View;
using static System.Net.Mime.MediaTypeNames;

namespace ServerApp.Model
{
    public class DataReceiving_Model : WebSocketBehavior
    {
        DeserializeReceivedData deserialize = new DeserializeReceivedData();
        bool eventAdded = false;

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            if (!eventAdded)
            {
                deserialize.ReceivingData += Main_View.OnReceivingData;
                eventAdded = true;
            }
            deserialize.DeserializeData(e.Data);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            GlobalVariables.numberOfDevices++;
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            GlobalVariables.numberOfDevices--;
        }

        
    }
}
