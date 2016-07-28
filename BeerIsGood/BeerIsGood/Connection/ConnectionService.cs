using System;
using System.Net.Http;

namespace BeerIsGood.Connection
{
    public partial class ConnectionService
    {
        public HttpClient Client;

        public ConnectionService()
        {
            Client = new HttpClient();

            // max buffer size: 10MB = 10485760B
            Client.MaxResponseContentBufferSize = 10485760;

            // Timeout: 0 Stunden, 1 Minuten, 0 Sekunden
            Client.Timeout = new TimeSpan(0, 1, 0); 
        }        
    }
}
