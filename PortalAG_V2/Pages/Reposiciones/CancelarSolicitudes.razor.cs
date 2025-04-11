
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.Reposicones;
using PortalAG_V2.Shared.Services;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using System.Security.Claims;
using PortalAG_V2.Auth;
using System.Net.Http.Json;
using PortalAG_V2.Componentes;

namespace PortalAG_V2.Pages.Reposiciones
{
    partial class CancelarSolicitudes
    {
        private bool Loading = false;
        MainServices mainServices;
        ClaimsPrincipal user;
        List<EliminarReposicionesModel> eliminarReposiciones = new List<EliminarReposicionesModel>();
        MudDataGrid<EliminarReposicionesModel> dataGrid;
        string searchString = null;


        private string Url = "api/v2/Reposiciones/ConsultarEliminarRepo";
        private string UrlTraspasoSAP = "api/v2/MovimientoBodegas/EnvioSAP";
        private string _searchString;

        protected override async Task OnInitializedAsync()
        {
            user = await _authenticationManager.CurrentUser();
            await CargarDatos();
           //return base.OnInitializedAsync();
        }

        private Func<EliminarReposicionesModel, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.idSolicitud.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.idArticulo.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            

            return false;
        };

   

        private async Task CargarDatos()
        {
            mainServices = new MainServices();
            var reponse = await mainServices.ConectionService.HttpClientInstance.GetAsync($"{Url}/1/0/0/{user.GetUserId()}");
            if (reponse.IsSuccessStatusCode)
            {
                eliminarReposiciones = JsonConvert.DeserializeObject<List<EliminarReposicionesModel>>(await reponse.Content.ReadAsStringAsync());
                if (eliminarReposiciones != null)
                {
                    eliminarReposiciones = eliminarReposiciones.Where(x =>
                    {
                        if (string.IsNullOrWhiteSpace(searchString))
                            return true;
                        if (x.idSolicitud.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                            return true;
                        if (x.nroPallet.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                            return true;
                        if (x.idArticulo.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                            return true;
                        return false;
                    }).ToList();
                }

                Console.WriteLine(eliminarReposiciones.ToString());
            }
            else { }
        }

        public async Task Cancelar(EliminarReposicionesModel data)
        {
            var parameters = new DialogParameters<DialogConfirmacion> {
                { x => x.TextDialog, "¿Esta seguro que desea cancelar la solicitud?" },
                { x => x.Titulo, "Cancelar reposición" },
                { x => x.nombreBoton, "Ok" }
            };

            var dialog = await DialogService.ShowAsync<DialogConfirmacion>("Question", parameters);
            var result = await dialog.Result;

            if ((bool)result.Data)
            {
                mainServices = new MainServices();
                var reponse = await mainServices.ConectionService.HttpClientInstance.GetAsync($"{Url}/2/{data.idSolicitud}/{data.idOperacion}/{user.GetUserId()}");
                if (reponse.IsSuccessStatusCode)
                {
                    // ENVIO A SAP
                    List<ItemsGrilla> listaDetalleSAP = new List<ItemsGrilla>();
                    ItemsGrilla itemsGrilla = new ItemsGrilla
                    {
                        ItemCode = data.idArticulo,
                        Quantity = (data.unidadPorBulto * data.cantidad).ToString(),
                        WarehouseCode = data.desde
                    };
                    listaDetalleSAP.Add(itemsGrilla);


                    EnvioTraspaso envioTraspaso = new EnvioTraspaso
                    {
                        Comments = "Anulacion de reposicion",
                        JournalMemo = $"Solicitud reposicion anulada x: {user.GetUserId()}",
                        FromWarehouse = data.hasta,
                        ToWarehouse = data.desde,
                        StockTransferLines = listaDetalleSAP
                    };
                    var respuesta = await mainServices.SAP.HttpClientInstance.PostAsJsonAsync<EnvioTraspaso>(UrlTraspasoSAP, envioTraspaso);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        snakBarCreation("Proceso realizado con existo", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    }
                    else
                    {

                        snakBarCreation("Error al enviar a SAP", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    }


                }
                else
                {
                    snakBarCreation("Falla al cancelar - Consultar a TI", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                }

                await CargarDatos();
            }

            
        }

        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }

    }
}
