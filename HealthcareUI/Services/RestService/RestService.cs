
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

namespace HealthcareUI.Services.RestService
{
    public class RestService<T> : IDataService<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ServiceDiscovery _discovery;
        private string _token;
        public RestService(HttpClient client, IConfiguration configuration, ServiceDiscovery discovery)
        {
            _httpClient = client;
            _configuration = configuration;
            _discovery = discovery;

        }
        public async Task InitializeAsync()
        {
            string origin = await _discovery.GetServiceUriAsync(AppConstants.PATIENT_SERVICE_NAME.ToString());
            if (origin is null) throw new Exception("Service is not registed");
            _httpClient.BaseAddress = new Uri($"{origin}"); ;
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
        public async Task<List<T>> GetRecordsAsync()
        {

            using HttpResponseMessage response = await _httpClient.GetAsync("/api/records");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var records = JsonSerializer.Deserialize<List<T>>(result);
                return records;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<T>> GetAvailableDoctors()
        {

            throw new NotImplementedException();
        }


        public async Task<ResponseResult> InsertRecordAsync(T product)
        {
            // Serialize the object to JSON
            string jsonPayload = JsonSerializer.Serialize(product);
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync("/api/records", content);

            if (response.IsSuccessStatusCode)
            {
                return new ResponseResult { IsSuccess = true };
            }
            else
            {
                return new ResponseResult { IsSuccess = false, Message= await response.Content.ReadAsStringAsync() };
            }
        }

        public async Task<ResponseResult> UpdateRecordAsync(T patient)
        {
            // Serialize the object to JSON
            string jsonPayload = JsonSerializer.Serialize(patient);
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.PutAsync("/api/records", content);

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


            using HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/records/{id}");

            if (response.IsSuccessStatusCode)
            {
                return new ResponseResult { IsSuccess = true };
            }
            else
            {
                return new ResponseResult { IsSuccess = false, Message = await response.Content.ReadAsStringAsync() };
            }
        }

        public Task<(bool, List<T>)> GetPatientMedicalRecords(string patientId)
        {
            throw new NotImplementedException();
        }
    }
}
