using System.Net.Http.Headers;
using API.Interfaces;

namespace API.Services
{
    public class WBService : IWBService
    {
        public string WbKey1 { get; set; }
        public string WbKey2 { get; set; }
        private readonly IConfiguration _config;

        public WBService(IConfiguration config)
        {
            _config = config;
            WbKey1 = _config["WBApiKey1"];
            WbKey2 = _config["WBApiKey2"];
        }

        public string GetWBKey()
        {
            return WbKey2;
        }

        public HttpClient getHttpClient2()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(WbKey2);

            return httpClient;
        }
    }
}