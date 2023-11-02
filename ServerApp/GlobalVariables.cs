using ServerApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public static class GlobalVariables
    {
        public static string serverIp = "127.0.0.1";
        public static string serverPort = "11000";
        public static string serverStatus = "";

        public static string databaseIp = "127.0.0.1";
        public static string databasePort = "11000";
        public static string databaseStatus = "";

        public static int numberOfDevices = 0;
    }
}
