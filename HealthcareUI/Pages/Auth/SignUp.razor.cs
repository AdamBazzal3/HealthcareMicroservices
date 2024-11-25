using Microsoft.AspNetCore.Components;
using HealthcareUI.Services.RestService;
using HealthcareIdentityAPI.Models;

namespace HealthcareUI.Pages.Auth
{
    public partial class SignUp : ComponentBase
    {
        [Inject]
        private IAuthService _authService { get; set; }

        public User User { get; set; } = new();
        public async Task SignUpAsync()
        {
            try
            {
                
                ResponseResult r = await _authService.SignUpAsync(User);
                if (r.IsSuccess)
                {
                    //return Redirect("/");
                }
                else;
                    //return Page();
            }
            catch(Exception)
            {
                //return Page();
            }
        }
    }
}
