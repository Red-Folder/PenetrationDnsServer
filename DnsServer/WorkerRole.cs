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
using Microsoft.Owin.Hosting;

namespace DnsServer
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private PenTestingDnsServer server;
        private IDisposable _app = null;

        public override void Run()
        {
            Log.Information("DnsServer is running");

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

            Log.Information("DnsServer has been started");

            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["WebAPI"];
            string baseUri = String.Format("{0}://{1}",
                endpoint.Protocol, endpoint.IPEndpoint);

            Log.Information(String.Format("Starting OWIN at {0}", baseUri));

            _app = WebApp.Start<Startup>(new StartOptions(url: baseUri));

            // Start the server (by default it listents on port 53)
            var dnsEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["DnsService"].IPEndpoint;
#if DEBUG
            dnsEndpoint = new IPEndPoint(IPAddress.Any, 53);
#endif
            Log.Information(String.Format("Starting DNS Server at {0}", dnsEndpoint));
            server = PenTestingDnsServer.Instance;
            server.Listen(dnsEndpoint);

            return result;
        }

        public override void OnStop()
        {
            Log.Information("DnsServer is stopping");

            Log.Information("Server stopping");
            server.Close();

            if (_app != null)
            {
                _app.Dispose();
            }

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Log.Information("DnsServer has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            Log.Information("Server starting");

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(5000);
            }
        }
    }
}
