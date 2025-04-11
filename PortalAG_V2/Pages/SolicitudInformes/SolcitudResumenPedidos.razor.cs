using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;
namespace PortalAG_V2.Pages.SolicitudInformes
{
    public partial class SolcitudResumenPedidos
    {
        #region Variables
        MainServices service;
        SfGrid<EstadoPedidoResumidoModel> Grid;
        private bool _processing = false;
        private List<string> ButtonsGrid = new List<string>() { "Generar", "ExcelExport" };
        private List<EstadoPedidoResumidoModel> ResumenPedidos = new List<EstadoPedidoResumidoModel>() { };
        bool Loading = false;
        #endregion

        #region URL's
        string URL = "Solicitud/EstadoPedidosResumido";
        #endregion
        public async Task CargarDatos()
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{URL}");
                if (lista.IsSuccessStatusCode)
                {
                    ResumenPedidos = JsonConvert.DeserializeObject<List<EstadoPedidoResumidoModel>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    ResumenPedidos = new List<EstadoPedidoResumidoModel>();
                    Loading = false;

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await CargarDatos();
        }

        #region Procesar Btn
        async Task ProcessSomething()
        {
            _processing = true;            
            await CargarDatos();
            _processing = false;
        }
        #endregion

        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (ResumenPedidos.Count() > 0)
                {
                    snakBarCreation("Exportando...", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    _processing = true;
                    await Grid.ExportToExcelAsync();
                    _processing = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    snakBarCreation("No hay datos para exportar", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                }
            }
            if (args.Item.Id == "Grid_Generar")
            {

                await CargarDatos();

            }
        }

        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion
    }
}
