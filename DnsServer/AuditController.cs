using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace DnsServer
{
    public class AuditController: ApiController
    {
        public List<string> Get()
        {
            return PenTestingDnsServer.Instance.Records;
            //return new HttpResponseMessage()
            //{
            //    Content = new StringContent("Hello from OWIN!")
            //};
        }
    }
}
