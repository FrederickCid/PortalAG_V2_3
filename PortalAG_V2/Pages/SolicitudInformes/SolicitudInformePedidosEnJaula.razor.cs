using MatBlazor;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Componentes.EstadoPedido;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;
using static PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaPedidosEnJaulaModel;

namespace PortalAG_V2.Pages.SolicitudInformes
{
    public partial class SolicitudInformePedidosEnJaula
    {
        MudTextField<string> MudTextIDVendedor;
        SfGrid<ConsultaPedidosEnJaulaModel> Grid;
        MainServices service;
        MudDatePicker? _DatePickerInicio;
        MudDatePicker? _DatePickerfin;
        DateTime? dateToday = DateTime.Today;
        DateTime? dateNull = null;
        private string IDVendedor;
        private bool _processing = false;
        private bool _validateData = true;
        string UrlHistoricos = "Solicitud/ConsultaPedidosEnJaula";
        private List<ConsultaPedidosEnJaulaModel> ListaJaula = new List<ConsultaPedidosEnJaulaModel>();
        private List<GrupocontableModel> ListaGrupocontable = new List<GrupocontableModel>();
        private List<string> ButtonsGrid = new List<string>() { "Actualizar",  "ExcelExport" };
        private bool showCallAlert = true;
        bool Loading = false;




        public async Task CargarDatosIDVendedor(string IDVendedor)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{UrlHistoricos}/{IDVendedor}");
                if (lista.IsSuccessStatusCode)
                {
                    ListaJaula = JsonConvert.DeserializeObject<List<ConsultaPedidosEnJaulaModel>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;                    
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    Loading = false;
                    ListaJaula = new List<ConsultaPedidosEnJaulaModel>();
                    ListaGrupocontable = new List<GrupocontableModel>();
                    snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }

        public async Task CargarDatos()
        {
            try
            {
                Loading = true;
                 service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{UrlHistoricos}/");
                if (lista.IsSuccessStatusCode)
                {
                    ListaJaula = JsonConvert.DeserializeObject<List<ConsultaPedidosEnJaulaModel>>(await lista.Content.ReadAsStringAsync());
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    Loading = false;
                }
                else
                {
                    ListaJaula = new List<ConsultaPedidosEnJaulaModel>();
                    snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
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
        async Task ProcessSomething()
        {
            _processing = true;
            MudTextIDVendedor.Disabled = true;
            ListaJaula = new List<ConsultaPedidosEnJaulaModel>();
            await CargarDatosIDVendedor(IDVendedor);
            MudTextIDVendedor.Disabled = false;
            _processing = false;
        }
        

        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (ListaJaula.Count() >= 0)
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
            if (args.Item.Id == "Grid_Actualizar")
            {

                await CargarDatos();

            }
        }

        #region Funcion Alerta
        private void CerrarAlerta()
        {
            showCallAlert = !showCallAlert;
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
    }
}
