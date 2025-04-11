using Microsoft.JSInterop;
using PortalAG_V2.Services;

namespace PortalAG_V2.Componentes
{
    public partial class UpdateAvailableDetector
    {
        private bool _newVersionAvailable = false;
        private async Task Reload()
        {
            await _jsRuntime.ReloadWasm();
        }

        protected override async Task OnInitializedAsync()
        {
            await RegisterForUpdateAvailableNotification();
        }

        private async Task RegisterForUpdateAvailableNotification()
        {
            await _jsRuntime.InvokeAsync<object>(
                identifier: "registerForUpdateAvailableNotification",
                DotNetObjectReference.Create(this),
                nameof(OnUpdateAvailable));
        }

        [JSInvokable(nameof(OnUpdateAvailable))]
        public Task OnUpdateAvailable()
        {
            _newVersionAvailable = true;

            StateHasChanged();

            return Task.CompletedTask;
        }
    }
}
