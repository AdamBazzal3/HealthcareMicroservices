
using HealthcareIdentityAPI.Models;
using System.Net.Http;
using System.Text;

namespace HealthcareUI.Services.RestService
{
    public interface IAuthService
    {
        Task<LoginResponseModel> LoginAsync(string username, string password);

        Task<ResponseResult> SignUpAsync(User user);
        Task InitializeAsync();
    }
}
