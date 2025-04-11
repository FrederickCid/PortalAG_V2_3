using MudBlazor;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Settings;
using System.Net.NetworkInformation;

namespace PortalAG_V2.Shared
{
    partial class MainLayout
    {
        private string CurrentUserId { get; set; }
        private string ImageDataUrl { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }
        MudMessageBox mbox { get; set; }
        private string Localidad { get; set; }
        public AppState appState { get; set; }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                CurrentUserId = user.GetUserId();
                FirstName = user.GetFirstName();
                appState.IDUsuario = CurrentUserId;
                if (FirstName.Length > 0)
                {
                    FirstLetterOfName = FirstName[0];
                }
                SecondName = user.GetLastName();
                Email = user.GetEmail();

                // SOLICITUD DE IMAGEN A LA BASE
                //var imageResponse = await _accountManager.GetProfilePictureAsync(CurrentUserId);
                //if (imageResponse.Succeeded)
                //{
                //    ImageDataUrl = imageResponse.Data;
                //}

                //var currentUserResult = await _userManager.GetAsync(CurrentUserId);
                //if (!currentUserResult.Succeeded || currentUserResult.Data == null)
                //{
                //    _snackBar.Add("Estás desconectado porque el usuario con tu Token ha sido eliminado.", Severity.Error);
                //    await _authenticationManager.Logout();
                //}


            }
        }
        public string SelectedOptionLocalidad { get; set; }
        private async void OnButtonClicked()
        {
            bool? result = await mbox.Show();


            if (result != null)
            {
                Localidad = SelectedOptionLocalidad;
                await _authenticationManager.SetLocalidad(Localidad);
                appState = new AppState
                {
                    IDAllEmpresa = 2,
                    IDEmpresa = 2,
                    Localidad = Localidad.Equals("Internet") ? 2 : 1,
                    IDUsuario = CurrentUserId,
                    Dark = _currentTheme == BlazorHeroTheme.DarkTheme ? 1 : 0
                };
                //_navigationManager.NavigateTo("/");
                _snackBar.Add("Localidad modificada!", Severity.Success);
            }
            StateHasChanged();
        }


        private MudTheme _currentTheme;
        private bool _drawerOpen = true;
        private bool _rightToLeft = false;
        protected override async Task OnInitializedAsync()
        {
            _currentTheme = BlazorHeroTheme.DefaultTheme;
            _currentTheme = await _clientPreferenceManager.GetCurrentThemeAsync();
            var storageLocalidad = await _authenticationManager.GetLocalidad();

            if (String.IsNullOrEmpty(storageLocalidad))
            {
                Localidad = "CDA";
                await _authenticationManager.SetLocalidad(Localidad);
                appState = new AppState
                {
                    IDAllEmpresa = 2,
                    IDEmpresa = 2,
                    Localidad = 1,
                    IDUsuario = CurrentUserId,
                    Dark = _currentTheme == BlazorHeroTheme.DarkTheme ? 1 : 0
                };
            }
            else
            {
                Localidad = storageLocalidad;
                appState = new AppState
                {
                    IDAllEmpresa = 2,
                    IDEmpresa = 2,
                    Localidad = storageLocalidad.Equals("Internet") ? 2 : 1,
                    IDUsuario = CurrentUserId,
                    Dark = _currentTheme == BlazorHeroTheme.DarkTheme ? 1 : 0
                };
            }





        }


        private void Logout()
        {
            var parameters = new DialogParameters
            {
                {nameof(Shared.Logout.ContentText), $"Logout Confirmation"},
                {nameof(Shared.Logout.ButtonText), $"Logout"},
                {nameof(Shared.Logout.Color), Color.Error},
                {nameof(Shared.Logout.CurrentUserId), CurrentUserId}
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            _dialogService.Show<Shared.Logout>("Logout", parameters, options);
        }
        private async Task DarkMode()
        {
            bool isDarkMode = await _clientPreferenceManager.ToggleDarkModeAsync();
            _currentTheme = isDarkMode ? BlazorHeroTheme.DefaultTheme : BlazorHeroTheme.DarkTheme;
            appState = new AppState
            {
                IDAllEmpresa = 2,
                IDEmpresa = 2,
                Localidad = Localidad.Equals("Internet") ? 2 : 1,
                IDUsuario = CurrentUserId,
                Dark = _currentTheme == BlazorHeroTheme.DarkTheme ? 1 : 0
            };
        }

        private async Task RightToLeftToggle()
        {
            var isRtl = await _clientPreferenceManager.ToggleLayoutDirection();
            _rightToLeft = isRtl;
            _drawerOpen = false;
        }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
    }
}
