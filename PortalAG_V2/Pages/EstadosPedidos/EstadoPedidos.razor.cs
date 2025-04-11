using agDataAccess.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Componentes.EstadoPedido;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Services;



namespace PortalAG_V2.Pages.EstadosPedidos
{
    public partial class EstadoPedidos
    {
        public MainServices? service;

        bool _processing = false;
        bool _Loading = false;

        #region Varibles

        GridEstadoPedidos gridPedidos;

        int activeIndex = 0;

        string NroBusqueda;
        string? valorOpcion { get; set; } = "Cotizacion";

        private int Count = 200;

        private Timer timer;

        #endregion

        #region Listas

        public List<EstadoCantidadNoMOD>? listaRadzenTabsItem = new List<EstadoCantidadNoMOD>();
        public List<EstadoPedidosNoMOD>? ResultadoBusqueda = new List<EstadoPedidosNoMOD>();
        EstadoPedidosNoMOD Resultado;

        #endregion

        #region Url

        string urlTab = "GetEstadoCantidad/ObtenerEstadoCantidad2";

        string urlTabBusqueda = "GetEstadoPedidos/BusquedaFactura/";

        #endregion

        protected override async Task OnInitializedAsync()
        {

            await CargarTabEstadoCantidad();
            StartCountdown();
        }

        #region CargarTabEstadoCantidad

        private async Task CargarTabEstadoCantidad()
        {

            try
            {
                
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{urlTab}");
                if (lista.IsSuccessStatusCode)
                {
                    listaRadzenTabsItem = JsonConvert.DeserializeObject<List<EstadoCantidadNoMOD>>(await lista.Content.ReadAsStringAsync());
                    gridPedidos.opcion = valorOpcion;
                    _processing = true;
                    // gridPedidos?.CargarGridPedidos();
                    _processing = false;
                   

                }
                _Loading = false;
                StateHasChanged();

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;                
            }
        }

        #endregion

        #region CargarBusqueda

        private async Task CargarBusqueda(int NroBusqueda)
        {

            try
            {
                snakBarCreation("Cargando...", Defaults.Classes.Position.BottomStart, Severity.Info, 2000);
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{urlTabBusqueda}" + NroBusqueda);
                if (lista.IsSuccessStatusCode)
                {

                    ResultadoBusqueda = JsonConvert.DeserializeObject<List<EstadoPedidosNoMOD>>(await lista.Content.ReadAsStringAsync());
                    Resultado = ResultadoBusqueda.FirstOrDefault();
                    _processing = true;
                    // gridPedidos?.CargarGridPedidos();
                    _processing = false;
                    snakBarCreation($"Pedido encontrado: {NroBusqueda} ", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);

                }
                else
                {
                    ResultadoBusqueda = new List<EstadoPedidosNoMOD>();
                    Resultado = null;
                    

                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }

        #endregion

        #region OnChange

        void OnChange(MouseEventArgs args)
        {
            valorOpcion = "";
            switch (activeIndex)
            {
                case 0:
                    valorOpcion = "Cotizacion";
                    Count = 200;
                    break;
                case 1:
                    valorOpcion = "Picking";
                    Count = 200;
                    break;
                case 2:
                    valorOpcion = "Revision";
                    Count = 200;
                    break;
                case 3:
                    valorOpcion = "Facturar";
                    Count = 200;
                    break;
                case 4:
                    valorOpcion = "Facturado";
                    Count = 200;
                    break;
                case 5:
                    valorOpcion = "Liberado";
                    Count = 200;
                    break;
                case 6:
                    valorOpcion = "ENHR";
                    Count = 200;
                    break;
                case 7:
                    valorOpcion = "JAD";
                    Count = 200;
                    break;
                case 8:
                    valorOpcion = "ENCAMION";
                    Count = 200;
                    break;
                case 9:
                    valorOpcion = "ENRUTA";
                    Count = 200;
                    break;
                case 10:
                    valorOpcion = "ENTREGADO";
                    Count = 200;
                    break;

            }
            gridPedidos.opcion = valorOpcion;
            //gridPedidos?.CargarGridPedidos();
            StateHasChanged();
        }

        #endregion

        #region Actualizar
        public async Task Actualizar()
        {
            try
            {
                _Loading = true;
                await CargarTabEstadoCantidad();
                Count = 200;
                _Loading = false;

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }

        #endregion

        #region StartCountdown



        public void StartCountdown()
        {
            try
            {
                timer = new Timer(new TimerCallback(async (e) =>
                {
                    if (Count <= 0)
                    {
                        await Actualizar();
                        Count = 200;
                    }
                    else
                    {
                        Count--;
                    }
                    StateHasChanged();
                }), null, 1000, 1000);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }



        #endregion

        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion

        private async void onclickBusqueda()
        {
            await CargarBusqueda(Int32.Parse(NroBusqueda));
            if (Resultado != null)
            {
                var parameters = new DialogParameters<MudBlazorDialogCustom>
                {
                    { x => x.Nombre, $"{Resultado.Vendedor}" },
                    { x => x.Usuario, $"{Resultado.IDVendedor}" },
                    { x => x.RSocial, $"{Resultado.RazonSocial}" },
                    { x => x.RutRS, $"{Resultado.IDCliente}" },
                    { x => x.option, $"CUALQUIERA" },
                    { x => x.FechaSolicitud, $"{Resultado.FechaTerminoAutorizacion}" },
                    { x => x.Lista, Resultado }
                };
                var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = true, MaxWidth = MaxWidth.ExtraExtraLarge };
                DialogService.Show<MudBlazorDialogCustom>("", parameters, options);

            }
            else {
                snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 3000);
            }
        }

    }
}
