

using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace HealthcareIdentityAPI.Models
{
    [Index(nameof(UserName),IsUnique = true)]
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }
        [JsonPropertyName("lastname")]
        public string Lastname { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("roles")]
        public string? Role { get; set; }
    }
}
