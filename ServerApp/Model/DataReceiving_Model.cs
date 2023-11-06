using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using ServerApp.View;
using System.Text.Json;
//using ClassLibrary;

namespace ServerApp.Model
{
    public class DataReceiving_Model : WebSocketBehavior
    {
        DeserializeReceivedData deserialize = new DeserializeReceivedData();

        private readonly IDataAccess _dataAccess;


        public DataReceiving_Model(IDataAccess dataAccess)
        {
            this._dataAccess = dataAccess;
            //deserialize.ReceivingData += Main_View.OnReceivingData;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            _dataAccess.SetData(e.Data);
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
