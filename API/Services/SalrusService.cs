using System.Net.Http.Headers;
using System.Reflection;
using API.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;

namespace API.Services
{
    public class SalrusService : ISalrusService
    {
        private readonly IConfiguration _config;

        public SalrusService(IConfiguration config)
        {
            _config = config;
        }

        //Get the Salrus Api http client
        public HttpClient GetHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
                if (value is bool boolValue) 
                {
                    value = boolValue  ? 1 : 0;
                }
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