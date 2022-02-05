using System.Net.Http.Headers;
using System.Reflection;
using API.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;

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

        //Get the WB Api http client
        public HttpClient GetHttpClient2()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(WbKey2);

            return httpClient;
        }
        
        //Get full api url. Some of the parameters might be unrequired and might be null
        //So this function create url based on filled parameters
        public string GetFullUrl(string baseUrl, Object dto)
        {
            var kvps = new List<KeyValuePair<string, string>>();
            foreach(PropertyInfo propertyInfo in dto.GetType().GetProperties())
            {
                Object value = propertyInfo.GetValue(dto);
                if (Object.ReferenceEquals(null, value))
                    continue;
                string strValue = value.ToString(); 
                if(!String.IsNullOrEmpty(strValue))
                {
                    kvps.Add(new KeyValuePair<string, string>(propertyInfo.Name.ToLower(), strValue));
                }                
            }

            var query = new QueryBuilder(kvps).ToQueryString();
            var finalQuery = baseUrl + query;  

            return finalQuery;
        }

    }
}