using System;
using System.Net.Http;

namespace DemoApp.Core.Rest
{
    public static class HttpClientFactory
    {
        public static Func<HttpClient> Get { get; set; }

        static HttpClientFactory()
        {
            Get = (() => new HttpClient());
        }
    }
}
