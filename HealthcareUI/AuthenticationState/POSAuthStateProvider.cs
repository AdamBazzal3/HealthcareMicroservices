using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using HealthcareIdentityAPI.Models;
using System.Security.Claims;

namespace HealthcareUI.AuthenticationState
{
    public class HealthcareAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        public HealthcareAuthStateProvider(ProtectedSessionStorage sessionStorage)
        {
           _sessionStorage = sessionStorage;
        }
        public override async Task<Microsoft.AspNetCore.Components.Authorization.AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                
                var userSessionStorageResult = await _sessionStorage.GetAsync<LoginResponseModel>("USER_SESSION");
                var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

                if (userSession == null)
                    return await Task.FromResult(new Microsoft.AspNetCore.Components.Authorization.AuthenticationState(_anonymous));
                
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.User.UserName),
                    new Claim(ClaimTypes.Role, ""+ userSession.User.Role)
                },
                "HealthcareAuth"));

                return await Task.FromResult(new Microsoft.AspNetCore.Components.Authorization.AuthenticationState(claimsPrincipal));
            }
            catch (Exception)
            {
                return await Task.FromResult(new Microsoft.AspNetCore.Components.Authorization.AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthState(LoginResponseModel userSession)
        {
                ClaimsPrincipal userPrincipal;
                if (userSession != null && !string.IsNullOrEmpty(userSession.Token))
                {
                    await _sessionStorage.SetAsync("USER_SESSION", userSession);
                    userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userSession.User.UserName),
                        new Claim(ClaimTypes.Role, ""+ userSession.User.Role)
                    }));
                }
                else
                {
                    await _sessionStorage.DeleteAsync("USER_SESSION");

                    userPrincipal = _anonymous;
                }

                NotifyAuthenticationStateChanged(Task.FromResult(new Microsoft.AspNetCore.Components.Authorization.AuthenticationState(userPrincipal)));
        }
    }
}
