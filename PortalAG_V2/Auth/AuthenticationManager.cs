using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PortalAG_V2.Client.Auth;
using PortalAG_V2.Shared.Requests.User;
using PortalAG_V2.Shared.Services;
using PortalAG_V2.Shared.Storage;
using PortalAG_V2.Shared.Wrapper;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace PortalAG_V2.Auth
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private ClientFactory servicio;

        private string UrlLogin = "api/v2/Login/login";

        public AuthenticationManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            //_localizer = localizer;

        }
        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var satate = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return satate.User;
        }

        public async Task<IResult> Login(TokenRequest model)
        {
            servicio = new MainServices().ConectionService;
            var repuetaDetalle = await servicio.HttpClientInstance.PostAsJsonAsync<TokenRequest>($"{UrlLogin}", model);
            if (repuetaDetalle.IsSuccessStatusCode)
            {
                TokenResponse result = JsonConvert.DeserializeObject<TokenResponse>(await repuetaDetalle.Content.ReadAsStringAsync());

                if (result.isSuccess)
                {
                    var token = result.Token;
                    var refreshToken = result.RefreshToken;
                    var userImageURL = result.UserImageURL;
                    await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
                    await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
                    if (!string.IsNullOrEmpty(userImageURL))
                    {
                        await _localStorage.SetItemAsync(StorageConstants.Local.UserImageURL, userImageURL);
                    }

                    ((ProveedorAutenticacionJWT)this._authenticationStateProvider).MarkUserAsAuthenticated(model.Email);


                    ///_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    return await Result.SuccessAsync();
                }
                else
                {
                    return await Result.FailAsync("Error");
                }
            }
            else
            {
                return await Result.FailAsync("Error");
            }
        }

        public async Task<IResult> Logout()
        {
            await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.UserImageURL);
            ((ProveedorAutenticacionJWT)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return await Result.SuccessAsync();
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
            var refreshToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);
            return token;
        }

        public async Task<string> TryForceRefreshToken()
        {
            var availableToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);
            if (string.IsNullOrEmpty(availableToken)) return string.Empty;
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            var timeUTC = DateTime.UtcNow;
            var diff = expTime - timeUTC;
            if (diff.TotalMinutes <= 1)
                return await RefreshToken();
            return string.Empty;
        }

        public async Task<string> TryRefreshToken()
        {
            return await RefreshToken();
        }

        public async Task SetLocalidad(string localidad)
        {
            await _localStorage.SetItemAsync(StorageConstants.Local.Localidad, localidad);
        }
        public async Task<string> GetLocalidad()
        {
            var storageLocalidad = await _localStorage.GetItemAsync<string>(StorageConstants.Local.Localidad);
            if (string.IsNullOrEmpty(storageLocalidad)) return string.Empty;

            return storageLocalidad;
        }
    }
}
