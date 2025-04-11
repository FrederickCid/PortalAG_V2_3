using agDataAccess.Models.ConsultaLineasPickingPacking;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;

namespace PortalAG_V2.Pages.SolicitudInformes
{
    public partial class SolicitudInformePedidosSacados
    {
        SfGrid<PediDosSacadosModel> Grid;
        MainServices service;
        private List<PediDosSacadosModel> PedidosSacados = new List<PediDosSacadosModel>();
        private List<string> ButtonsGrid = new List<string>() { "ExcelExport", "Search" };
        string UrlPedidosSacados = "Lineas/PedidosSacados";
        string[] SpecificCols = { "NroDocumento", "IDOperacion", "IDCliente", "RazonSocial", "Vendedor" };
        private bool Loading = false;
        private bool _processing = false;
        private string Fecha;

        public async Task CargarDatosPedidosSacados(string Fecha)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{UrlPedidosSacados}/{Fecha}");
                if (lista.IsSuccessStatusCode)
                {
                    PedidosSacados = JsonConvert.DeserializeObject<List<PediDosSacadosModel>>(await lista.Content.ReadAsStringAsync());

                    Loading = false;
                }
                else
                {
                    PedidosSacados = new List<PediDosSacadosModel>();
                    Loading = false;
                    snakBarCreation("Error!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }
        public async Task BuscarPedidosSacados()
        {
            //accion con boton buscar
            PedidosSacados = new List<PediDosSacadosModel>();
            await CargarDatosPedidosSacados(Fecha);
        }

        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion

        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (PedidosSacados.Count() >= 0)
                {
                    snakBarCreation("Exportando...", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    //_processing = true;
                    await Grid.ExportToExcelAsync();
                    //_processing = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    snakBarCreation("No hay datos para exportar", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                }
            }
        }

    }


}
