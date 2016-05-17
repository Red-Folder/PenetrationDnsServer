using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnsServer
{
    public class Log
    {
        public static void Information(string message)
        {
            AddMessage("Information", message);
        }

        public static void Warning(string message)
        {
            AddMessage("Warning", message);
        }
        public static void Error(string message)
        {
            AddMessage("Error", message);
        }
        public static void Debug(string message)
        {
            AddMessage("Debug", message);
        }

        private static void AddMessage(string level, string message)
        {
            Trace.TraceInformation(String.Format("{0}: {1}", level, message));

            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.addMessage(level, message);
        }
    }
}
