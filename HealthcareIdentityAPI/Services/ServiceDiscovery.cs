using Consul;
using Newtonsoft.Json;
using System.Text;
using System.Threading;

namespace HealthcareIdentityAPI.Services
{
    public class ServiceDiscovery    {
        private readonly IConsulClient _consulClient;
        private readonly HttpClient _httpClient;

        public ServiceDiscovery(IConsulClient consulClient, HttpClient httpClient)
        {
            _consulClient = consulClient;
            _httpClient = httpClient;
        }
        public async Task<string> GetServiceUriAsync(string serviceName)
        {
            try
            {
                //var kv = _consulClient.KV;
                //var result = await kv.Get(serviceName);
                var queryResult = _consulClient.Health.Service(serviceName).Result;
                if(queryResult is not null
                    && queryResult.Response is not null
                    && queryResult.Response.Length > 0)
                {
                    var services = queryResult.Response;
                    return $"http://{services[0].Service.Address}:{services[0].Service.Port}"; 
                }
                    
                //if (result.Response != null)
                //{
                //    var data = Encoding.UTF8.GetString(result.Response.Value);
                //    var configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                //    return $"http://{configData["Address"]}:{configData["Port"]}";
                //}
                return "";
            }
            catch(Exception ex)
            {

            }
            return "";
        }
   
    }
    public class ConsulConfig
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceHost { get; set; }
        public int ServicePort { get; set; }
        public string HealthCheckUrl { get; set; }
        public int HealthCheckIntervalSeconds { get; set; }
        public int HealthCheckTimeoutSeconds { get; set; }
    }
}
