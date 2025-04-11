using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.StockCyber;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;

namespace PortalAG_V2.Pages.StockCyber
{
    public partial class InformeStockCyber
    {
        #region Variables
        MainServices service;
        SfGrid<StockCyberModel> Grid;
        private bool _processing = false;
        private List<string> ButtonsGrid = new List<string>() { "Actualizar", "ExcelExport", "Search" };
        private List<StockCyberModel> ResumenPedidos = new List<StockCyberModel>() { };
        bool Loading = false;
        string[] SpecificCols = { "Codigo", "Nombre" };

        #endregion

        #region URL's
        string URL = "Utilidades/seguimientoCodigos";
        #endregion
        public async Task CargarDatos()
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{URL}");
                if (lista.IsSuccessStatusCode)
                {
                    ResumenPedidos = JsonConvert.DeserializeObject<List<StockCyberModel>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    ResumenPedidos = new List<StockCyberModel>();
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
                    Loading = true;
                    await Grid.ExportToExcelAsync();
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    snakBarCreation("No hay datos para exportar", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                }
            }
            if (args.Item.Id == "Grid_Actualizar")
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

