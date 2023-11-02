using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using ServerApp.View;

namespace ServerApp.Model
{
    public class DataReceiving_Model : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            GlobalVariables.DeserializeReceivedData(e.Data);
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
