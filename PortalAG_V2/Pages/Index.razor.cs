using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Componentes.ChartIndex;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.ChartsModel;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaLineasPickingPacking;
using PortalAG_V2.Shared.Services;
using System;
using System.Collections.Generic;


namespace PortalAG_V2.Pages
{
    public partial class Index
    {
        MainServices service;

        MudBlazor.Anchor anchor;

        public bool VisiblePicking1 { get; set; } = true;
        public bool VisiblePicking2 { get; set; } = true;
        public bool VisiblePicking3 { get; set; } = true;
        public bool VisiblePacking { get; set; } = true;
        public bool VisibleReposiciones { get; set; } = true;
        public bool VisiblePickeadasNoPickeadas { get; set; } = true;
        public bool VisibleLineasPendientes { get; set; } = true;
        public bool VisibleLineasPendientes2 { get; set; } = true;
        public bool VisibleLineasPendientes3 { get; set; } = true;
        public bool VisibleLineasPendientes4 { get; set; } = true;

        public bool setVisiblePicking1 = false;
        public bool setVisiblePicking2 = false;
        public bool setVisiblePicking3 = false;
        public bool setVisiblePacking = false;
        public bool setVisibleReposiciones = false;
        public bool setVisiblePickeadasNoPickeadas = false;
        public bool setVisibleLineasPendientes = false;
        public bool setVisibleLineasPendientes2 = false;
        public bool setVisibleLineasPendientes3 = false;
        public bool setVisibleLineasPendientes4 = false;

        private int Count = 60;
        private Timer timer;

        bool open;
        bool Loaing = false;
        bool setting = false;
        bool ApplyVisible = false;

        private List<LineasTodas> ResumenLineas = new List<LineasTodas>() { };
        private List<lineasPickingModel> ListLineasPicking = new List<lineasPickingModel>() { };
        private List<lineasPickingModel> ListLineasPicking2 = new List<lineasPickingModel>() { };
        private List<lineasPickingModel> ListLineasPicking3 = new List<lineasPickingModel>() { };
        private List<lineasPackingModel> ListLineasPacking = new List<lineasPackingModel>() { };
        private List<lineasDespachoModel> ListLineasDespacho = new List<lineasDespachoModel>() { };
        private List<lineasReposicionModel> ListLineasReposicion = new List<lineasReposicionModel>() { };
        private List<LineasPickeadasNoPickeadasModel> ListLineasPickeadasNoPickeadas = new List<LineasPickeadasNoPickeadasModel>() { };
        private LineasPendientesModel ListLineasPendientes = new LineasPendientesModel() { };
        private List<LineasPendientesModelDTO> ListaLineasPendientesDTO = new();
        private List<LineasPendientesModelDTO> ListaLineasPendientesDTO2 = new();
        private List<LineasPendientesModelDTO> ListaLineasPendientesDTO3 = new();
        private List<LineasPendientesModelDTO> ListaLineasPendientesDTO4 = new();

        ChartPieComponent ComponentPicking = new ChartPieComponent();
        ChartPieComponent ComponentPicking2 = new ChartPieComponent();
        ChartPieComponent ComponentPicking3 = new ChartPieComponent();
        ChartPieComponent ComponentPacking = new ChartPieComponent();
        ChartPieComponent ComponentReposicio = new ChartPieComponent();
        ChartLineasPickeadasNoPickeadas ChartLineasPickeadasNoPickeadasComponent = new ChartLineasPickeadasNoPickeadas();
        ChartLineasPendientes ChartListLineasPendientes = new ChartLineasPendientes();
        ChartLineasPendientes ChartListLineasPendientes2 = new ChartLineasPendientes();
        ChartLineasPendientes ChartListLineasPendientes3 = new ChartLineasPendientes();
        ChartLineasPendientes ChartListLineasPendientes4 = new ChartLineasPendientes();



        protected override async Task OnInitializedAsync()
        {
            try
            {
                Loaing = true;
                setting = false;
                await ReadCookies();
                await WriteCookies();
                SetVisibleVAriable();
                DateTime fechaActual = DateTime.Now;
                string fechaFormateada = fechaActual.ToString("dd-MM-yyyy");
                DateTime fechaSemanaAtras = fechaActual.AddDays(-7);
//                DateTime fechaSemanaAtras = new DateTime(fechaActual.Year, fechaActual.Month, 1); inicio de mes
                string fechaSemanaAtrasFormateada = fechaSemanaAtras.ToString("dd-MM-yyyy");
             
                    await CargarDatosLineasPickeadas(fechaSemanaAtrasFormateada, fechaFormateada);
             
                    await CargarLineasPickeadasNopickeadas();
             
                    ListaLineasPendientesDTO = await CargarLineasPendientes(1);
              
                    ListaLineasPendientesDTO2 = await CargarLineasPendientes(2);
               
               
                    ListaLineasPendientesDTO3 = await CargarLineasPendientes(3);
               
                    ListaLineasPendientesDTO4 = await CargarLineasPendientes(4);
                
                Loaing = false;
                setting = true;
                StartCountdown();
                await UpdateChartSeries();
                await base.OnInitializedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ListLineasPicking = new List<lineasPickingModel>() { };
                ListLineasPicking2 = new List<lineasPickingModel>() { };
                ListLineasPicking3 = new List<lineasPickingModel>() { };
                ListLineasPacking = new List<lineasPackingModel>() { };
                ListLineasDespacho = new List<lineasDespachoModel>() { };
                ListLineasReposicion = new List<lineasReposicionModel>() { };
                ListLineasPickeadasNoPickeadas = new List<LineasPickeadasNoPickeadasModel>() { };
                ListLineasPendientes = new LineasPendientesModel() { };
            }

        }
        #region Carga de Datos

        public async Task CargarDatosLineasPickeadas(string inicio, string fin)
        {
            try
            {
                service = new MainServices();
                ResumenLineas = new List<LineasTodas>() { };
                ListLineasPicking = new List<lineasPickingModel>() { };
                ListLineasPicking2 = new List<lineasPickingModel>() { };
                ListLineasPicking3 = new List<lineasPickingModel>() { };
                ListLineasPacking = new List<lineasPackingModel>() { };
                ListLineasDespacho = new List<lineasDespachoModel>() { };
                ListLineasReposicion = new List<lineasReposicionModel>() { };

                string URL = "LineasUsuario/GetLineas";

                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{URL}/{inicio}/{fin}/1/1/1");
                if (lista.IsSuccessStatusCode)
                {

                    ResumenLineas = JsonConvert.DeserializeObject<List<LineasTodas>>(await lista.Content.ReadAsStringAsync());
                    if (ResumenLineas.Count() > 0)
                    {
                        if (ResumenLineas.FirstOrDefault().lineasPicking.Count > 0)
                        {
                            ListLineasPicking = ResumenLineas.FirstOrDefault().lineasPicking;
                            ListLineasPicking2 = ResumenLineas.FirstOrDefault().lineasPicking;
                            ListLineasPicking3 = ResumenLineas.FirstOrDefault().lineasPicking;
                        }
                        else
                        {
                            ListLineasPicking = new List<lineasPickingModel>() { };
                            ListLineasPicking2 = new List<lineasPickingModel>() { };
                            ListLineasPicking3 = new List<lineasPickingModel>() { };
                        }
                        if (ResumenLineas.FirstOrDefault().lineasPacking.Count > 0)
                        {

                            ListLineasPacking = ResumenLineas.FirstOrDefault().lineasPacking;
                        }
                        else
                        {
                            ListLineasPacking = new List<lineasPackingModel>() { };
                        }
                        if (ResumenLineas.FirstOrDefault().lineasReposicion.Count > 0)
                        {
                            ListLineasReposicion = ResumenLineas.FirstOrDefault().lineasReposicion;
                        }
                        else
                        {
                            ListLineasReposicion = new List<lineasReposicionModel>() { };
                        }
                    }
                    else
                    {
                        ListLineasPicking = new List<lineasPickingModel>() { };
                        ListLineasPicking2 = new List<lineasPickingModel>() { };
                        ListLineasPicking3 = new List<lineasPickingModel>() { };
                        ListLineasPacking = new List<lineasPackingModel>() { };
                        ListLineasDespacho = new List<lineasDespachoModel>() { };
                        ListLineasReposicion = new List<lineasReposicionModel>() { };
                    }
                }
                else
                {
                    ResumenLineas = new List<LineasTodas>();
                    ListLineasPicking = new List<lineasPickingModel>() { };
                    ListLineasPicking2 = new List<lineasPickingModel>() { };
                    ListLineasPicking3 = new List<lineasPickingModel>() { };
                    ListLineasPacking = new List<lineasPackingModel>() { };
                    ListLineasDespacho = new List<lineasDespachoModel>() { };
                    ListLineasReposicion = new List<lineasReposicionModel>() { };

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                Console.WriteLine(mensaje);
                ListLineasPicking = new List<lineasPickingModel>() { };
                ListLineasPicking2 = new List<lineasPickingModel>() { };
                ListLineasPicking3 = new List<lineasPickingModel>() { };
                ListLineasPacking = new List<lineasPackingModel>() { };
                ListLineasDespacho = new List<lineasDespachoModel>() { };
                ListLineasReposicion = new List<lineasReposicionModel>() { };

            }
        }
        public async Task CargarLineasPickeadasNopickeadas()
        {
            string URL = "Solicitud/Lineas/PickeadasNoPickeadas";
            service = new MainServices();
            ListLineasPickeadasNoPickeadas = new List<LineasPickeadasNoPickeadasModel>();
            var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{URL}/");
            if (lista.IsSuccessStatusCode)
            {
                ListLineasPickeadasNoPickeadas = JsonConvert.DeserializeObject<List<LineasPickeadasNoPickeadasModel>>(await lista.Content.ReadAsStringAsync());
            }
            else
            {
                ListLineasPickeadasNoPickeadas = new List<LineasPickeadasNoPickeadasModel>() { };
            }

        }
        public async Task<List<LineasPendientesModelDTO>> CargarLineasPendientes(int tipo)
        {
            try
            {
                string URL = "Lineas/PendientePicking";
                List<LineasPendientesModelDTO> ListaLineasPendientesDTOreturn = new();
                service = new MainServices();
                ListLineasPendientes = new();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{URL}/{tipo}");
                if (lista.IsSuccessStatusCode)
                {
                    ListLineasPendientes = JsonConvert.DeserializeObject<LineasPendientesModel>(await lista.Content.ReadAsStringAsync());
                    if (ListLineasPendientes != null)
                    {
                        ListaLineasPendientesDTOreturn = new List<LineasPendientesModelDTO>
                    {
                        new LineasPendientesModelDTO{label="Total", Cantidad=ListLineasPendientes.TotalPicking },
                        new LineasPendientesModelDTO{label="Total Verde", Cantidad=ListLineasPendientes.TotalVerdePicking},
                        new LineasPendientesModelDTO{label="Total Rojo", Cantidad=ListLineasPendientes.TotalRojoPicking},
                    };
                        return ListaLineasPendientesDTOreturn;
                    }
                    return ListaLineasPendientesDTOreturn = new();
                }
                else
                {
                    return ListaLineasPendientesDTOreturn = new();
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }

        }



        #endregion

        #region Actualiza Charts al momento de la carga de datos
        private async Task UpdateChartSeries()
        {
            try
            {
                await ComponentPicking.chartPickeadas.UpdateOptionsAsync(true, true, false);
                await ComponentPicking2.chartPickeadas2.UpdateOptionsAsync(true, true, false);
                await ComponentPicking3.chartPickeadas3.UpdateOptionsAsync(true, true, false);
                await ComponentPacking.chartPacking.UpdateOptionsAsync(true, true, false);
                await ComponentReposicio.chartReposicion.UpdateOptionsAsync(true, true, false);
                await ChartLineasPickeadasNoPickeadasComponent.chartPickeadasNoPickeadas.UpdateOptionsAsync(true, true, false);
                await ChartListLineasPendientes.chartLineasPendientes.UpdateOptionsAsync(true, true, false);
                await ChartListLineasPendientes2.chartLineasPendientes.UpdateOptionsAsync(true, true, false);
                await ChartListLineasPendientes3.chartLineasPendientes.UpdateOptionsAsync(true, true, false);
                await ChartListLineasPendientes4.chartLineasPendientes.UpdateOptionsAsync(true, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task UpdateChartSeriesCountDown()
        {
            try
            {
                ListaLineasPendientesDTO = await CargarLineasPendientes(1);
                ListaLineasPendientesDTO2 = await CargarLineasPendientes(2);
                ListaLineasPendientesDTO3 = await CargarLineasPendientes(3);
                ListaLineasPendientesDTO4 = await CargarLineasPendientes(4);
                await ChartListLineasPendientes.chartLineasPendientes.UpdateSeriesAsync(true);
                await ChartListLineasPendientes2.chartLineasPendientes.UpdateSeriesAsync(true);
                await ChartListLineasPendientes3.chartLineasPendientes.UpdateSeriesAsync(true);
                await ChartListLineasPendientes4.chartLineasPendientes.UpdateSeriesAsync(true);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region SetCookies
        void OpenDrawer(MudBlazor.Anchor anchor)
        {
            open = true;
            this.anchor = anchor;
        }
        protected async Task WriteCookies()
        {
            await javaScript.WriteCookie("VisiblePicking1", VisiblePicking1.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
            await javaScript.WriteCookie("VisiblePicking2", VisiblePicking2.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
            await javaScript.WriteCookie("VisiblePicking3", VisiblePicking3.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
            await javaScript.WriteCookie("VisiblePacking", VisiblePacking.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
            await javaScript.WriteCookie("VisibleReposiciones", VisibleReposiciones.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
            await javaScript.WriteCookie("VisiblePickeadasNoPickeadas", VisiblePickeadasNoPickeadas.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
            await javaScript.WriteCookie("VisibleLineasPendientes", VisibleLineasPendientes.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
            await javaScript.WriteCookie("VisibleLineasPendientes2", VisibleLineasPendientes2.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
            await javaScript.WriteCookie("VisibleLineasPendientes3", VisibleLineasPendientes3.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
            await javaScript.WriteCookie("VisibleLineasPendientes4", VisibleLineasPendientes4.ToString(), DateTime.Now.AddMinutes(30 * 24 * 30));
        }
        protected async Task ReadCookies()
        {
            VisiblePicking1 = await javaScript.ReadCookie("VisiblePicking1") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisiblePicking1"));
            VisiblePicking2 = await javaScript.ReadCookie("VisiblePicking2") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisiblePicking2"));
            VisiblePicking3 = await javaScript.ReadCookie("VisiblePicking3") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisiblePicking3"));
            VisiblePacking = await javaScript.ReadCookie("VisiblePacking") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisiblePacking"));
            VisibleReposiciones = await javaScript.ReadCookie("VisibleReposiciones") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisibleReposiciones"));
            VisiblePickeadasNoPickeadas = await javaScript.ReadCookie("VisiblePickeadasNoPickeadas") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisiblePickeadasNoPickeadas"));
            VisibleLineasPendientes = await javaScript.ReadCookie("VisibleLineasPendientes") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisibleLineasPendientes"));
            VisibleLineasPendientes2 = await javaScript.ReadCookie("VisibleLineasPendientes2") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisibleLineasPendientes2"));
            VisibleLineasPendientes3 = await javaScript.ReadCookie("VisibleLineasPendientes3") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisibleLineasPendientes3"));
            VisibleLineasPendientes4 = await javaScript.ReadCookie("VisibleLineasPendientes4") == "" ? true : bool.Parse(await javaScript.ReadCookie("VisibleLineasPendientes4"));
        }
        bool checkedChanged(bool x)
        {
            return x = !x;
        }
        void SetVisibleVAriable()
        {
            setVisiblePicking1 = VisiblePicking1;
            setVisiblePicking2 = VisiblePicking2;
            setVisiblePicking3 = VisiblePicking3;
            setVisiblePacking = VisiblePacking;
            setVisibleReposiciones = VisibleReposiciones;
            setVisiblePickeadasNoPickeadas = VisiblePickeadasNoPickeadas;
            setVisibleLineasPendientes = VisibleLineasPendientes;
            setVisibleLineasPendientes2 = VisibleLineasPendientes2;
            setVisibleLineasPendientes3 = VisibleLineasPendientes3;
            setVisibleLineasPendientes4 = VisibleLineasPendientes4;
        }
        async void SetVisibleApply()
        {
            if (setVisiblePicking1 = VisiblePicking1) setVisiblePicking1 = !setVisiblePicking1;
            if (setVisiblePicking2 = VisiblePicking2) setVisiblePicking2 = !setVisiblePicking2;
            if (setVisiblePicking3 = VisiblePicking3) setVisiblePicking3 = !setVisiblePicking3;
            if (setVisiblePacking = VisiblePacking) setVisiblePacking = !setVisiblePacking;
            if (setVisibleReposiciones = VisibleReposiciones) setVisibleReposiciones = !setVisiblePickeadasNoPickeadas;
            if (setVisiblePickeadasNoPickeadas = VisiblePickeadasNoPickeadas) setVisiblePickeadasNoPickeadas = !setVisiblePickeadasNoPickeadas;
            if (setVisibleLineasPendientes = VisibleLineasPendientes) setVisibleLineasPendientes = !setVisibleLineasPendientes;
            if (setVisibleLineasPendientes2 = VisibleLineasPendientes2) setVisibleLineasPendientes2 = !setVisibleLineasPendientes2;
            if (setVisibleLineasPendientes3 = VisibleLineasPendientes3) setVisibleLineasPendientes3 = !setVisibleLineasPendientes3;
            if (setVisibleLineasPendientes4 = VisibleLineasPendientes4) setVisibleLineasPendientes4 = !setVisibleLineasPendientes4;
            open = false;
            SetVisibleVAriable();
            await WriteCookies();
            StateHasChanged();
        }
        #endregion

        public async Task Actualizar()
        {
            try
            {

                await UpdateChartSeriesCountDown();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }
        public void StartCountdown()
        {
            try
            {
                timer = new Timer(new TimerCallback(async (e) =>
                {
                    if (Count <= 0)
                    {
                        await Actualizar();
                        Count = 60;
                    }
                    else
                    {
                        Count--;
                        //Console.WriteLine(Count);
                    }
                }), null, 1000, 1000);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }
    }
}
