using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnsServer
{
    public class NotificationHub: Hub
    {
        public void SendMessage(string level, string message)
        {
            Trace.TraceInformation(String.Format("{0}: {1}", level, message));
            Clients.All.addMessage(level, message);
        }
    }
}
