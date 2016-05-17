using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;

namespace DnsServer
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //HttpConfiguration config = new HttpConfiguration();
            //config.Routes.MapHttpRoute(
            //    "Default",
            //    "{controller}/{id}",
            //    new { id = RouteParameter.Optional });

            //app.UseWebApi(config);

            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
