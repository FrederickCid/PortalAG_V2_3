using agDataAccess.Models.Solicitudes;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Shared.Model.CargaMAsivaOfertas;
using PortalAG_V2.Shared.Model.StockCyber;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;

namespace PortalAG_V2.Pages.CargaMasiva.CargaMasivaOferta
{
    public partial class ListadoOfertas
    {
        public string NroBusqueda;
        public MainServices? service;
        SfGrid<OfertaSistemaModel> Grid;
        bool _processing = false;
        List<OfertaSistemaModel> OfertaLista = new() { };
        string url = "Ofertas/Listado";
        bool loading = false;
        private List<string> ButtonsGrid = new List<string>() { "Actualizar", "ExcelExport", "Search" };
        string[] SpecificCols = { "IDListaDescuento", "ListaDescuento" };
        protected override async Task OnInitializedAsync()
        {
            await GetListaOfertas();

        }
        private void onclickBusqueda()
        {
        }

        private async Task GetListaOfertas()
        {
            try
            {
                loading = true;
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{url}");
                if (lista.IsSuccessStatusCode)
                {
                    loading = false;
                    OfertaLista = JsonConvert.DeserializeObject<List<OfertaSistemaModel>>(await lista.Content.ReadAsStringAsync());
                    // snakBarCreation("Actualizado!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    loading = false;
                    //snakBarCreation("Sin Reposcionoes!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                loading = false;
                string mensaje = ex.Message;
            }

        }
        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (OfertaLista.Count() > 0)
                {
                    snakBarCreation("Exportando...", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    loading = true;
                    await Grid.ExportToExcelAsync();
                    loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    snakBarCreation("No hay datos para exportar", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                }
            }
            if (args.Item.Id == "Grid_Actualizar")
            {

                await GetListaOfertas();

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
