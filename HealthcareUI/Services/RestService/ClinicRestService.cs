
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

using HealthcareIdentityAPI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using HealthcareUI.Models;
using HealthcareIdentityAPI.Services;
using Polly.Retry;
using Polly;
using Polly.CircuitBreaker;
using System.Net.NetworkInformation;
using Polly.Wrap;

namespace HealthcareUI.Services.RestService
{
    public class ClinicRestService<T> : IDataService<T> where T : class {


        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ServiceDiscovery _discovery;
        private string _token;
        private string _endpoint;
        public ClinicRestService(HttpClient client, IConfiguration configuration, ServiceDiscovery discovery)
        {
            _httpClient = client;
            _configuration = configuration;
            _discovery = discovery;


        }


        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token ?? "");
            }
        }
        public async Task<List<T>> GetAvailableDoctors()
        {

            using HttpResponseMessage response = await _httpClient.GetAsync($"/api/doctors/available");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var doctors = JsonSerializer.Deserialize<List<T>>(result);
                return doctors;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<T>> GetRecordsAsync()
        {

            using HttpResponseMessage response = await _httpClient.GetAsync($"/api/{typeof(T).Name.ToLower()}s");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var doctors = JsonSerializer.Deserialize<List<T>>(result);
                return doctors;
            }
            else
            {
                return null;
            }
        }



        public async Task<ResponseResult> InsertRecordAsync(T product)
        {
            // Serialize the object to JSON
            string jsonPayload = JsonSerializer.Serialize(product);
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync($"/api/{typeof(T).Name.ToLower()}s", content);

            if (response.IsSuccessStatusCode)
            {
                return new ResponseResult { IsSuccess = true };
            }
            else
            {
                return new ResponseResult { IsSuccess = false, Message = await response.Content.ReadAsStringAsync() };
            }
        }

        public async Task<ResponseResult> UpdateRecordAsync(T patient)
        {
            // Serialize the object to JSON
            string jsonPayload = JsonSerializer.Serialize(patient);
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PutAsync($"/api/{typeof(T).Name.ToLower()}s", content);

            if (response.IsSuccessStatusCode)
            {
                return new ResponseResult { IsSuccess = true };
            }
            else
            {
                return new ResponseResult { IsSuccess = false, Message = await response.Content.ReadAsStringAsync() };
            }
        }

        public async Task<ResponseResult> DeleteRecordAsync(string id)
        {


            using HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/{typeof(T).Name.ToLower()}s/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ResponseResult { IsSuccess = true };
            }
            else
            {
                return new ResponseResult { IsSuccess = false, Message = await response.Content.ReadAsStringAsync() };
            }
        }
        private static readonly AsyncRetryPolicy<HttpResponseMessage> asyncRetryPolicy =
           Polly.Policy.HandleResult<HttpResponseMessage>(resp => {
                return resp.StatusCode == System.Net.HttpStatusCode.BadRequest;
            })
           .WaitAndRetryAsync(4, retryAttempt =>
           {
               Console.WriteLine($"Attempt {retryAttempt} - Retrying due to an error.");
               return TimeSpan.FromSeconds(5 + retryAttempt);
           });
        private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> circuitBreakerPolicy =
           Polly.Policy.HandleResult<HttpResponseMessage>(resp => {
               return resp.StatusCode == System.Net.HttpStatusCode.BadRequest;
               })
           .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
        private static  readonly AsyncPolicyWrap<HttpResponseMessage> combinedPolicy = 
            Policy.WrapAsync(asyncRetryPolicy, circuitBreakerPolicy);

        public async Task<(bool, List<T>)> GetPatientMedicalRecords(string patientId)
        {
            using HttpResponseMessage response = await  combinedPolicy.ExecuteAsync(async()=>
                    await _httpClient.GetAsync($"/api/medicalRecords/{patientId}")
            );

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var records = JsonSerializer.Deserialize<List<T>>(result);
                return (true,records);
            }
            else
            {
                return (false,null);
            }
        }
    }
}
