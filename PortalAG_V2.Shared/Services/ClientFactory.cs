using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PortalAG_V2.Shared.Services
{
    public class ClientFactory
    {
        public HttpClient HttpClientInstance { get; set; }

        public ClientFactory(string Uri)
        {
            HttpClientInstance = new HttpClient
            {
                BaseAddress = new Uri(Uri)
            };

        }

        public static implicit operator ClientFactory(MainServices v)
        {
            throw new NotImplementedException();
        }
    }
}
