using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;


namespace DnsServer
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private PenTestingDnsServer server;

        public override void Run()
        {
            Trace.TraceInformation("DnsServer is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("DnsServer has been started");

            // Proxy to google's DNS
            server = new PenTestingDnsServer();

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("DnsServer is stopping");

            Trace.TraceInformation("Server stopping");
            server.Close();

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("DnsServer has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            Trace.TraceInformation("Server starting");
            // Start the server (by default it listents on port 53)
            server.Listen();

            //// TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                // Trace.TraceInformation("Working");
                await Task.Delay(5000);
            }
        }
    }
}
