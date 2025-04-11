using agDataAccess.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes.Pagos;
using PortalAG_V2.Componentes.Solicitudes;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Model.HojaDeRuta;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaLineasPickingPacking;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;

namespace PortalAG_V2.Componentes.SolicitudesLineas
{
    public partial class LineasProcesadas
    {
        #region Variables
        [Inject] ExportService exportService { get; set; }
        [Inject] IJSRuntime js { get; set; }
        MainServices service;
        private bool _processing = false;
        private List<string> ButtonsGrid = new List<string>() { "Generar", "ExcelExport" };
        private List<LineasUsuarioModel> ResumenLineas = new() { };
        private List<Lineas> ListLineasPicking = new() { };
        private List<Lineas> ListLineasPacking = new() { };
        private List<Lineas> ListLineasDespacho = new() { };
        private List<Lineas> ListLineasReposicion = new() { };
        private List<Lineas> ListLineasDevoluciom = new() { };
        private string fInicio;
        private string fFin;
        DateTime? dateToday = DateTime.Today;
        DateTime? dateNull = null;
        bool Loading = false;
        MudDatePicker? _DatePickerInicio;
        MudDatePicker? _DatePickerfin;
        private bool showCallAlert = true;
        #endregion

        #region URL's
        string URL = "LineasUsuario/GetLineas";
        string TEST = "LineasUsuario/GetLineas/test";
        #endregion
        public async Task CargarDatos(string inicio, string fin)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                ResumenLineas = new() { };
                ListLineasPicking = new() { };
                ListLineasPacking = new() { };
                ListLineasDespacho = new() { };
                ListLineasReposicion = new() { };
                ListLineasDevoluciom = new();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{URL}/test/{inicio}/{fin}/1/1/1/1");
                if (lista.IsSuccessStatusCode)
                {

                    ResumenLineas = JsonConvert.DeserializeObject<List<LineasUsuarioModel>>(await lista.Content.ReadAsStringAsync());
                    if (ResumenLineas.Count() > 0)
                    {
                        if (ResumenLineas.FirstOrDefault().LineasPicking.Count > 0)
                        {
                            ListLineasPicking = ResumenLineas.FirstOrDefault().LineasPicking;
                            Loading = false;
                        }
                        else
                        {
                            ListLineasPicking = new() { };
                            Loading = false;
                        }
                        if (ResumenLineas.FirstOrDefault().LineasPacking.Count > 0)
                        {
                            ListLineasPacking = ResumenLineas.FirstOrDefault().LineasPacking;
                            Loading = false;
                        }
                        else
                        {
                            ListLineasPacking = new() { };
                            Loading = false;
                        }
                        if (ResumenLineas.FirstOrDefault().LineasReposicion.Count > 0)
                        {
                            ListLineasReposicion = ResumenLineas.FirstOrDefault().LineasReposicion;
                            Loading = false;
                        }
                        else
                        {
                            ListLineasReposicion = new() { };
                            Loading = false;
                        }
                        if (ResumenLineas.FirstOrDefault().LineasDevolucion.Count > 0)
                        {
                            ListLineasDevoluciom = ResumenLineas.FirstOrDefault().LineasDevolucion;
                            Loading = false;
                        }
                        else
                        {
                            ListLineasDevoluciom = new() { };
                            Loading = false;
                        }
                        //ListLineasDespacho = ResumenLineas.FirstOrDefault().lineasDespacho;
                        Loading = false;
                        snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    }
                    else
                    {
                        ListLineasPicking = new List<Lineas>() { };
                        ListLineasPacking = new List<Lineas>() { };
                        ListLineasDespacho = new List<Lineas>() { };
                        ListLineasReposicion = new List<Lineas>() { };
                        Loading = false;
                        snakBarCreation("Error!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    }
                }
                else
                {
                    ResumenLineas = new();
                    Loading = false;

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                Loading = false;

            }
        }

        protected override async Task OnInitializedAsync()
        {
            // await CargarDatos();
        }

        #region Procesar Btn
        async Task ProcessSomething()
        {
            _processing = true;
            ResumenLineas = new();
            await CargarDatos(fInicio, fFin);
            _processing = false;
        }
        #endregion


   

        #region Guardar PDF Hoja De Ruta
        public async Task GenerarPDFLineas()
        {
            using (MemoryStream memory = exportService.CreatePdfLineas(ListLineasPicking, ListLineasPacking, ListLineasReposicion, ListLineasDevoluciom))
            {
                await js.SaveAs($"Lienas {dateToday}.pdf", memory.ToArray());
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

        #region Funcion Alerta
        private void CerrarAlerta()
        {
            showCallAlert = !showCallAlert;
        }
        #endregion

        public async Task ProbarPDF()
        {
            await GenerarPDFLineas();
        }

        public void ModalAiura()
        {
            var parameters = new DialogParameters<AyudaLineasUsuarioModal>();
            var options = new MudBlazor.DialogOptions() { CloseButton = true, FullWidth = true, MaxWidth = MaxWidth.ExtraExtraLarge };
            DialogService.Show<AyudaLineasUsuarioModal>($"Ayuda", parameters, options);
        }
    }
}
