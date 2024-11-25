using System.Text.Json.Serialization;

namespace HealthcareIdentityAPI.Models
{
    public class LoginResponseModel
    {
        [JsonPropertyName("user")]
        public User User { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
