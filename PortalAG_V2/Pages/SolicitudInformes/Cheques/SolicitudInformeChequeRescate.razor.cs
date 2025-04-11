using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes.Solicitudes;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Models.Cheques;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;

namespace PortalAG_V2.Pages.SolicitudInformes.Cheques
{
    public partial class SolicitudInformeChequeRescate
    {
        
        SfGrid<ChequesModel> Grid;
        MainServices service;
        MudDatePicker? _DatePickerInicio;
        MudDatePicker? _DatePickerfin;
        DateTime? dateToday = DateTime.Today;
        DateTime? dateNull = null;
        private string fInicio;
        private string fFin;
        private int Tipo = 1;
        private bool _processing = false;
        private bool _validateData = true;
        string UrlHistoricos = "Cheques/ConsultaInformesCheque/";
        private List<ChequesModel> ChequesConsulta = new List<ChequesModel>();
        // private List<NCDetalleModel> Detalle = new List<NCDetalleModel>();
        private List<string> ButtonsGrid = new List<string>() { "ExcelExport" };
        private bool showCallAlert = true;
        bool Loading = false;
        public int SelectedOption { get; set; } = 1;


        public async Task CargarDatos(int tipoConsulta, string inicio, string fin)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlHistoricos}{tipoConsulta}/{Tipo}/0/{inicio}/{fin}/");
                if (lista.IsSuccessStatusCode)
                {
                    ChequesConsulta = JsonConvert.DeserializeObject<List<ChequesModel>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    ChequesConsulta = new List<ChequesModel>();
                    Loading = false;
                    snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                Loading = false;
                string mensaje = e.Message;
            }
        }
        public async Task CargarDatosVencimiento(int tipoConsulta, string inicio, string fin)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlHistoricos}{tipoConsulta}/1/0/{inicio}/{fin}/");
                if (lista.IsSuccessStatusCode)
                {
                    ChequesConsulta = JsonConvert.DeserializeObject<List<ChequesModel>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    ChequesConsulta = new List<ChequesModel>();
                    Loading = false;
                    snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                Loading = false;
                string mensaje = e.Message;
            }
        }


        async Task ProcessSomething()
        {
            _processing = true;
            ChequesConsulta = new List<ChequesModel>();
            await CargarDatos(2,fInicio, fFin);
            _processing = false;
        }
        async Task ProcessSomething2()
        {
            _processing = true;
            ChequesConsulta = new List<ChequesModel>();
            await CargarDatosVencimiento(1, fInicio, fFin);
            _processing = false;
        }

        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (ChequesConsulta.Count() >= 0)
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
      
        private void TipoConsultaChanged(int value)
        {
            Tipo = value;
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

