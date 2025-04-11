using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes.EstadoPedido;
using PortalAG_V2.Componentes.Solicitudes;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;

namespace PortalAG_V2.Pages.SolicitudInformes
{
    public partial class SolicitudInformeFactura
    {
        SfGrid<informeFacturaModel> Grid;
        MainServices service;
        MudDatePicker? _DatePickerInicio;
        MudDatePicker? _DatePickerfin;
        DateTime? dateToday = DateTime.Today;
        DateTime? dateNull = null;
        private string fInicio;
        private string fFin;        
        private bool _processing = false;
        private bool _validateData = true;
        string UrlHistoricos = "informeFactura";
        private List<informeFacturaModel> InformeFacturasLista = new List<informeFacturaModel>();
        private List<string> ButtonsGrid = new List<string>() { "ExcelExport" };
        private bool showCallAlert = true;
        bool Loading = false;


        public async Task CargarDatos(string inicio, string fin)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{UrlHistoricos}/{inicio}/{fin}/");
                if (lista.IsSuccessStatusCode)
                {
                    InformeFacturasLista = JsonConvert.DeserializeObject<List<informeFacturaModel>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }               
                else
                {
                    InformeFacturasLista = new List<informeFacturaModel>();
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


        async Task ProcessSomething()
        {
            _processing = true;
            InformeFacturasLista = new List<informeFacturaModel>();
            await CargarDatos(fInicio, fFin);
            _processing = false;
        }

        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (InformeFacturasLista.Count() >= 0)
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
