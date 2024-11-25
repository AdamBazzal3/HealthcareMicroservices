using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using HealthcareIdentityAPI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HealthcareIdentityAPI.Services;

namespace HealthcareUI.Services.RestService
{
    public class AuthRestService:IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ServiceDiscovery _discovery;

        public AuthRestService(HttpClient client, IConfiguration configuration, ServiceDiscovery discovery)
        {
            _httpClient = client;
            _configuration = configuration;
            _discovery = discovery;
            InitializeAsync().Wait();
        }

        public async Task InitializeAsync()
        {
            string origin = await _discovery.GetServiceUriAsync(AppConstants.IDENTITY_SERVICE_NAME.ToString());
            if (origin is null) throw new Exception("Service is not registered");
            if (string.IsNullOrEmpty(origin)) return;
            _httpClient.BaseAddress = new Uri($"{origin}/auth/"); ;
        }

        public async Task<LoginResponseModel> LoginAsync(string username, string password)
        {

            string jsonPayload = JsonSerializer.Serialize(new LoginRequestModel { Username = username, Password = password });
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("login", content);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = await httpResponseMessage.Content.ReadAsStringAsync();
                var responseModel = JsonSerializer.Deserialize<LoginResponseModel>(result);
                return responseModel;
            }
            else
            {
                return null;
            }
        }

        public async Task<ResponseResult> SignUpAsync(User user)
        {
            string jsonPayload = JsonSerializer.Serialize(user);
            StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("signup", content);


            if (response.IsSuccessStatusCode)
            {

                return new ResponseResult { IsSuccess = true };
            }
            else
            {
                return new ResponseResult { IsSuccess = false };
            }
        }
    }
}
