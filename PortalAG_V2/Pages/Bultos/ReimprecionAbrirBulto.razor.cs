using agDataAccess.Models.Solicitudes;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Pages.Bultos
{
    public partial class ReimprecionAbrirBulto
    {
        [CascadingParameter]
        private AppState? appSatate { get; set; }
        MainServices servicio;
        private string urlReimpresion = "/api/v2/ImpresionDoc/GenerarReImpresioEtiquetaBulto";

        bool _processing = false;
        string NroPedido;
        string NroBulto;
        string Usuario;

        protected override async Task OnInitializedAsync()
        {
           
            var user = await _authenticationManager.CurrentUser();
            var IDuse = user.GetFirstName();
            Usuario = user.GetUserId();
            StateHasChanged();
        }

        private async void Buscar() {

            if (!string.IsNullOrEmpty(NroPedido))
            {
                if (!string.IsNullOrEmpty(NroBulto))
                {
                    servicio = new MainServices();
                    servicio.ConectionService.HttpClientInstance.DefaultRequestHeaders.Add("ID", Usuario);
                    var aux = await servicio.ConectionService.HttpClientInstance.GetAsync($"{urlReimpresion}/{NroPedido}/{NroBulto}/1/1");
                    if (aux.IsSuccessStatusCode)
                    {
                        // correcto
                        _snackBar.Add("Impresion generada", Severity.Success);
                        NroBulto = "";
                        NroPedido = "";
                        StateHasChanged();
                          
                    }
                }
                else
                {
                    //Error
                    _snackBar.Add("Nro de Bulto no puede ser vacio", Severity.Error);
                }
            }
            else
            {
                //Error
                _snackBar.Add("Nro de Pedido no puede ser vacio", Severity.Error);
            }
        }
        
    }
}
