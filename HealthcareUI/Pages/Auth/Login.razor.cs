using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using HealthcareUI.AuthenticationState;
using HealthcareUI.Services.RestService;
using HealthcareIdentityAPI.Models;

namespace HealthcareUI.Pages.Auth
{
    public partial class Login : ComponentBase
    {
        [Inject]
        private IAuthService _authService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        private AuthenticationStateProvider _authStateProvider { get; set; }
        private LoginRequestModel Model = new();
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        public async Task LoginAsync()
        {
            try
            {
                string username = Model.Username.Trim();
                string password = Model.Password.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    return;
                LoginResponseModel r = await _authService.LoginAsync(username, password);
                var authStateProvider = (HealthcareAuthStateProvider)_authStateProvider;

                await authStateProvider.UpdateAuthState(r);

                _navigationManager.NavigateTo("/", true);
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}
