using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes.PrePacking;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.Prepacking;
using PortalAG_V2.Shared.Services;


namespace PortalAG_V2.Pages.PrePacking
{
    public partial class Prepacking
    {
        #region Variables
        [CascadingParameter]
        public AppState appSatate { get; set; }
        MainServices service;
        private bool Click = true;
        private bool ClickPendiente = true;
        private bool Loading = false;
        private string _searchString;
        private List<ShowGuiaPrePacking> GuiasPrePackingDisponibles = new List<ShowGuiaPrePacking> { };
        private List<ShowGuiaPrePacking> GuiasPrePackingDisponiblesPendientes = new List<ShowGuiaPrePacking> { };
        private List<ShowGuiaPrePackingDetalleBulto> showGuiaDetalleBultos = new List<ShowGuiaPrePackingDetalleBulto> { };
        private List<ShowGuiaPrePackingDetalle> DetalleGuia = new List<ShowGuiaPrePackingDetalle> { };
        private MudDataGrid<ShowGuiaPrePacking> GridHO;
        private string NroBusqueda = "A-";
        private string NroBusquedaPendietes = "A-";

        //Para generar pdf
        [Inject] ExportService exportService { get; set; }
        [Inject] IJSRuntime js { get; set; }

        #endregion

        #region Url
        //POST
        public static string UrlObtenerGuiaPrePacking = "GuiaPrePackingList/GuiaPrePacking/";
        public static string UrlCargarDetalleGuiaPrePacking = "GuiaPrePackingList/DetalleGuiaPrePacking/";
        public static string IngresarGuiaPrepackingURl = "GuiaPrePackingList/IngresarGuiaPrePacking/";

        //GET

        #endregion Url
        protected async override void OnInitialized()
        {
            await ObtenerGuiaPrePackingPendientes(1, 0, 1, "");
        }
        #region consultals
        private async Task ObtenerGuiaPrePacking(int idtipoguia, double idguia, int tipoConsulta, string Search)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.PrePacking.HttpClientInstance.GetAsync($"api/v1/{UrlObtenerGuiaPrePacking}{idtipoguia}/{idguia}/{tipoConsulta}/{Search}");
                if (lista.IsSuccessStatusCode)
                {
                    GuiasPrePackingDisponibles = JsonConvert.DeserializeObject<List<ShowGuiaPrePacking>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    StateHasChanged();
                }
                else
                {
                    GuiasPrePackingDisponibles = new List<ShowGuiaPrePacking>();
                    Loading = false;
                    snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
                Loading = false;
                StateHasChanged();
            }
            Loading = false;
        }
        private async Task ObtenerGuiaPrePackingPendientes(int idtipoguia, double idguia, int tipoConsulta, string Search)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.PrePacking.HttpClientInstance.GetAsync($"api/v1/{UrlObtenerGuiaPrePacking}{idtipoguia}/{idguia}/{tipoConsulta}/{Search}");
                if (lista.IsSuccessStatusCode)
                {
                    GuiasPrePackingDisponiblesPendientes = JsonConvert.DeserializeObject<List<ShowGuiaPrePacking>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    StateHasChanged();
                }
                else
                {
                    GuiasPrePackingDisponiblesPendientes = new List<ShowGuiaPrePacking>();
                    Loading = false;
                    snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
                Loading = false;
                StateHasChanged();
            }
            Loading = false;
        }
        private async Task ObtenerDetalleGuiaPrePacking(int idtipoguia, double idguia, int tipoConsulta, string Search)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.PrePacking.HttpClientInstance.GetAsync($"api/v1/{UrlCargarDetalleGuiaPrePacking}{idtipoguia}/{idguia}/{tipoConsulta}/{Search}");
                if (lista.IsSuccessStatusCode)
                {
                    DetalleGuia = JsonConvert.DeserializeObject<List<ShowGuiaPrePackingDetalle>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    StateHasChanged();
                }
                else
                {
                    DetalleGuia = new List<ShowGuiaPrePackingDetalle> { };
                    Loading = false;
                    snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
                Loading = false;
                StateHasChanged();
            }
            Loading = false;
        }

        private async Task IngresarGuiaPrepacking(int idtipoguia, double docEntry, double docNum, string idusuario)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.PrePacking.HttpClientInstance.GetAsync($"api/v1/{IngresarGuiaPrepackingURl}{idtipoguia}/{docEntry}/{docNum}/{idusuario}");
                if (lista.IsSuccessStatusCode)
                {
                    Loading = false;
                    StateHasChanged();
                }
                else
                {
                    showGuiaDetalleBultos = new List<ShowGuiaPrePackingDetalleBulto> { };
                    Loading = false;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                Console.WriteLine(mensaje);
                Loading = false;
                StateHasChanged();
            }
            Loading = false;
        }


        #endregion

        private void onClickCancelar()
        {
            Click = true;
            Clear();
        }
        public  void onClickCancelarPendientes()
        {
            ClickPendiente = true;
            Clear();
        }
        private async void onClickCrear()
        {
            Click = false;
            await ObtenerGuiaPrePacking(1, 0, 0, NroBusqueda);
        }
        private async void onClickCrearPendientes()
        {
            ClickPendiente = false;
            await ObtenerGuiaPrePackingPendientes(1, 0, 1, NroBusqueda);
        }


        #region Funciones Grilla

        private async Task clickDesde(DataGridRowClickEventArgs<ShowGuiaPrePacking> args)
        {
            try
            {
                await IngresarGuiaPrepacking(1, args.Item.DocEntryEM, args.Item.DocNumEM, appSatate.IDUsuario);
                await ObtenerGuiaPrePacking(1, 0, 1, args.Item.ReferenciaEM);
                if (GuiasPrePackingDisponibles.Count > 0)
                {
                    await ObtenerDetalleGuiaPrePacking(1, GuiasPrePackingDisponibles.Find(x => x.IDGuia == args.Item.IDGuia).IDGuia, 21, "a");
                    if (DetalleGuia is not null)
                    {
                        var parameters = new DialogParameters<DialogPrepackingDetalle>
                        {
                            { x => x.idGuia, GuiasPrePackingDisponibles.FirstOrDefault().IDGuia },
                            { x => x.DetalleGuiaRe, DetalleGuia },
                            { x => x.idUsuario, appSatate.IDUsuario },
                            { x => x.DocEntryEM, args.Item.DocEntryEM },
                            { x => x.ReferenciaEM, args.Item.ReferenciaEM },
                            { x => x.DocNumEM, args.Item.DocNumEM },
                            { x => x.FuncionActualizar, Actualizar }
                        };

                        var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = false, MaxWidth = MaxWidth.ExtraExtraLarge, DisableBackdropClick = true, };
                        DialogService.Show<DialogPrepackingDetalle>($"Guia N°:{GuiasPrePackingDisponibles.FirstOrDefault().IDGuia}", parameters, options);
                    }
                    else
                    {
                        snakBarCreation("Guia No contiene Detalle!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task clickDesdePendientes(DataGridRowClickEventArgs<ShowGuiaPrePacking> args)
        {
            try
            {
                //await IngresarGuiaPrepacking(1, args.Item.DocEntryEM, args.Item.DocNumEM, appSatate.IDUsuario);
                await ObtenerGuiaPrePacking(1, 0, 1, args.Item.ReferenciaEM);
                if (GuiasPrePackingDisponibles.Count > 0)
                {
                    await ObtenerDetalleGuiaPrePacking(1, GuiasPrePackingDisponibles.Find(x => x.IDGuia == args.Item.IDGuia).IDGuia, 21, "a");                    

                    if (DetalleGuia is not null)
                    {
                        var parameters = new DialogParameters<DialogPrepackingDetallePendientes>();
                        parameters.Add(x => x.idGuia, GuiasPrePackingDisponibles.Find(x=> x.IDGuia == args.Item.IDGuia).IDGuia);
                        parameters.Add(x => x.DetalleGuiaRe, DetalleGuia);
                        parameters.Add(x => x.idUsuario, appSatate.IDUsuario);
                        parameters.Add(x => x.DocEntryEM, args.Item.DocEntryEM);
                        parameters.Add(x => x.ReferenciaEM, args.Item.ReferenciaEM);
                        parameters.Add(x => x.DocNumEM, args.Item.DocNumEM);
                        parameters.Add(x => x.FuncionActualizar, ActualizarPendiente);
                        parameters.Add(x => x.guia, args.Item);

                        var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = false, MaxWidth = MaxWidth.ExtraExtraLarge, DisableBackdropClick = true, };
                        DialogService.Show<DialogPrepackingDetallePendientes>($"Guia N°:{GuiasPrePackingDisponibles.Find(x=> x.IDGuia == args.Item.IDGuia).IDGuia}", parameters, options);
                    }
                    else
                    {
                        snakBarCreation("Guia No contiene Detalle!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        async Task Actualizar()
        {
            await ObtenerGuiaPrePacking(1, 0, 0, NroBusqueda);
        }

        async Task ActualizarPendiente()
        {
            await ObtenerGuiaPrePackingPendientes(1, 0, 1, NroBusquedaPendietes);
        }

        private void RemoveItem(ShowGuiaPrePacking args)
        {

        }
        #endregion

        #region SnackBar


        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }

        #endregion SnackBar

        #region Limpiar Campos
        private void Clear()
        {
            GuiasPrePackingDisponibles = new List<ShowGuiaPrePacking> { };
            GuiasPrePackingDisponiblesPendientes = new List<ShowGuiaPrePacking> { };

        }
        #endregion

        #region Filtro Input Text
        private Func<ShowGuiaPrePacking, bool> QuickFilter => x =>
        {
            //si no encuetra nada no manda nada
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            //Buscar por NRO de Documento 
            if (x.DocNumEM.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Buscar Por CLiente
            if (x.NumeroGuia.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Busqueda mas amplia de los dos anterirore y agregando al vendedor, para mostrar multiples
            if ($"{x.IDProveedor} {x.Proveedor}".Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
        #endregion

        public async void Buscar()
        {
            await ObtenerGuiaPrePacking(1, 0, 1, NroBusqueda);
        } 
        
     

    }
}