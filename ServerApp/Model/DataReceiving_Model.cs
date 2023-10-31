using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerApp.Model
{
    public class DataReceiving_Model : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            DeserializeReceivedData(e.Data);
        }

        public void DeserializeReceivedData(string data)
        {

        }
    }
}
