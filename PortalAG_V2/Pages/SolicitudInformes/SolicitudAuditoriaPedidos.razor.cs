using agDataAccess.Models.Solicitudes;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes.Solicitudes;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;

namespace PortalAG_V2.Pages.SolicitudInformes
{
    public partial class SolicitudAuditoriaPedidos
    {
        MainServices service;
        bool Loading = false;
        private List<AuditoriaPedidosModel> ListaAuditoria = new List<AuditoriaPedidosModel>();
        string UrlAuditoria = "Solicitud/Lineas/AuditoriaPedidos";
        SfGrid<AuditoriaPedidosModel> Grid;
        private List<string> ButtonsGrid = new List<string>() { "ExcelExport" };

        protected override async Task OnInitializedAsync()
        {
            await CargarDatos();
        }

        public async Task CargarDatos()
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{UrlAuditoria}");
                if (lista.IsSuccessStatusCode)
                {
                    ListaAuditoria = JsonConvert.DeserializeObject<List<AuditoriaPedidosModel>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    ListaAuditoria = new List<AuditoriaPedidosModel>();
                    Loading = false;
                    snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;

            }

        }
        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (ListaAuditoria.Count() >= 0)
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

        public void DobleClick(RowSelectEventArgs<AuditoriaPedidosModel> args)
        {
            var Detalle = new DialogParameters<ModalDetalleBulto> {
                { x => x.Detalle, args.Data.DetalleAuditoria }           };
            
            var nuevo = new MudBlazor.DialogOptions() { CloseButton = true, FullWidth = true, MaxWidth = MaxWidth.Medium };
            _dialogService.Show<ModalDetalleBulto>($"Pedido NRO: {args.Data.NroPedido}", Detalle, nuevo);
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
