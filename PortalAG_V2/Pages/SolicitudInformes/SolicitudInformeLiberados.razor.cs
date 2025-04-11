using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Componentes.EstadoPedido;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Model.HojaDeRuta;
using PortalAG_V2.Shared.Model.NotaDeCredito;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;
using System.Net.Http.Json;

namespace PortalAG_V2.Pages.SolicitudInformes
{
    public partial class SolicitudInformeLiberados
    {
        SfGrid<ConsultaLiberadosLista> Grid;
        MainServices service;
        MudDatePicker? _DatePickerInicio;
        MudDatePicker? _DatePickerfin;
        DateTime? dateToday = DateTime.Today;
        DateTime? dateNull = null;
        private string fInicio;
        private string fFin;
        private bool _processing = false;
        private bool _validateData = true;
        string UrlLiberados = "Despacho/Liberados";
        private List<ConsultaLiberadosLista> responseConsulta = new List<ConsultaLiberadosLista>();
        private List<string> ButtonsGrid = new List<string>() { "ExcelExport" };
        private bool showCallAlert = true;
        bool Loading = false;
        public AppState appSatate { get; set; }
        private string responseBody;



        public async Task CargarDatos(string inicio, string fin)
        {
            try
            {
                ConsultaLiberados PostConsulta = new ConsultaLiberados()
                {                   
                    FechaInicio = inicio,
                    FechaFin = fin               
                    //IDUsuario = appSatate.IDUsuario.ToString()
                };
                Loading = true;
                service = new MainServices();

                var lista = await service.SolcitudInformes.HttpClientInstance.PostAsJsonAsync($"api/v2/{UrlLiberados}", PostConsulta);
                if (lista.IsSuccessStatusCode)
                {
                    responseBody = await lista.Content.ReadAsStringAsync();
                    responseConsulta = JsonConvert.DeserializeObject<List<ConsultaLiberadosLista>>(responseBody);
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    Loading = false;
                    
                    snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);

                }
                Loading = false;
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
            responseConsulta = new List<ConsultaLiberadosLista>();
            await CargarDatos(fInicio, fFin);
            _processing = false;
        }

        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (responseConsulta.Count() >= 0)
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
