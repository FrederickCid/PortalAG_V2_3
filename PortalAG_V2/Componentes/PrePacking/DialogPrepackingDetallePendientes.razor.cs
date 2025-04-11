using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Model.HojaDeRuta;
using PortalAG_V2.Shared.Model.Prepacking;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Componentes.PrePacking
{
    public partial class DialogPrepackingDetallePendientes
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public double idGuia { get; set; }
        [Parameter] public string idUsuario { get; set; }
        [Parameter] public List<ShowGuiaPrePackingDetalle> DetalleGuiaRe { get; set; }
        [Parameter] public ShowGuiaPrePacking guia { get; set; }
        [Parameter] public int DocEntryEM { get; set; }
        [Parameter] public string ReferenciaEM { get; set; }
        [Parameter] public int DocNumEM { get; set; }
        [Parameter] public Func<Task> FuncionActualizar { get; set; }

        DialogPrepackingDetalleBulto dialogbultos;
        [Inject] IJSRuntime js { get; set; }
        MainServices service;
        [Inject] ExportService exportService { get; set; }

        private string _searchString;
        private bool Loading = false;
        private bool _processing = false;
        private MudDataGrid<ShowGuiaPrePackingDetalleDTO> GridHO;
        private List<ShowGuiaPrePackingDetalleBulto> showGuiaDetalleBultos = new List<ShowGuiaPrePackingDetalleBulto> { };
        public List<ShowGuiaPrePackingDetalleDTO> DetalleGuiaReDTO = new List<ShowGuiaPrePackingDetalleDTO> { };
        public List<ShowGuiaPrePackingDetalleDTOPDF> DetalleGuiaReDTOPDF = new List<ShowGuiaPrePackingDetalleDTOPDF> { };
        public static string ObtenerActualizaDetalleBulto = "GuiaPrePackingList/DetalleBultoGuiaPrePacking/";
        public static string ActualizarEstadoGuiaPrePackingURl = "GuiaPrePackingList/ActualizarEstadoGuiaPrePacking/";
        public static string IngresarGuiaPrepackingURl = "GuiaPrePackingList/IngresarGuiaPrePacking/";

        protected override async Task OnInitializedAsync()
        {
            await cargarFaltantes();
        }

        private async Task ObtenerActualizaDetalleBultoGuiaPrePacking(int idtipoguia, double idguia, int tipoConsulta, int linea, int lineaNum, string? idArticulo, int lineaBulto, int bultos, int unidadxBulto, string idUsuario)
        {
            try
            {
                service = new MainServices();
                var lista = await service.PrePacking.HttpClientInstance.GetAsync($"api/v1/{ObtenerActualizaDetalleBulto}{idtipoguia}/{idguia}/{tipoConsulta}/{linea}/{lineaNum}/{idArticulo}/{lineaBulto}/{bultos}/{unidadxBulto}/{idUsuario}");
                if (lista.IsSuccessStatusCode)
                {
                    showGuiaDetalleBultos = JsonConvert.DeserializeObject<List<ShowGuiaPrePackingDetalleBulto>>(await lista.Content.ReadAsStringAsync());
                    DetalleGuiaReDTO.Add(new ShowGuiaPrePackingDetalleDTO
                    {
                        CantidadEM = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).CantidadEM,
                        IDGuia = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).IDGuia,
                        LineNum = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).LineNum,
                        IDArticulo = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).IDArticulo,
                        Nombre = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).Nombre,
                        Imagen = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).Imagen,
                        IDEstado = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).IDEstado,
                        Fecha = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).Fecha,
                        IDUsuario = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).IDUsuario,
                        Linea = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).Linea,
                        NroParte = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).NroParte,
                        Faltan = showGuiaDetalleBultos.Count > 0 ? showGuiaDetalleBultos.FirstOrDefault().Faltan : DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).CantidadEM,
                    });
                    DetalleGuiaReDTOPDF.Add(new ShowGuiaPrePackingDetalleDTOPDF
                    {
                        CantidadEM = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).CantidadEM,
                        IDGuia = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).IDGuia,
                        LineNum = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).LineNum,
                        IDArticulo = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).IDArticulo,
                        Nombre = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).Nombre,
                        Imagen = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).Imagen,
                        IDEstado = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).IDEstado,
                        Fecha = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).Fecha,
                        IDUsuario = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).IDUsuario,
                        Linea = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).Linea,
                        Faltan = showGuiaDetalleBultos.Count > 0 ? showGuiaDetalleBultos.FirstOrDefault().Faltan : DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).CantidadEM,
                        NroParte = DetalleGuiaRe.Find(x => x.IDArticulo == idArticulo && x.Linea == linea).NroParte,
                        ListaBultos = showGuiaDetalleBultos,
                    });
                    //snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    StateHasChanged();
                }
                else
                {
                    showGuiaDetalleBultos = new List<ShowGuiaPrePackingDetalleBulto> { };
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


        private async Task ActualizarEstadoGuiaPrePacking(int idtipoguia, double idguia, int tipoConsulta, int idEtapa, int idEstado, string observacion, string idUsuario)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.PrePacking.HttpClientInstance.GetAsync($"api/v1/{ActualizarEstadoGuiaPrePackingURl}{idtipoguia}/{idguia}/{tipoConsulta}/{idEtapa}/{idEstado}/{observacion}/{idUsuario}");
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

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();


        #region SnackBar

        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion SnackBar


        private async Task clickDesde(DataGridRowClickEventArgs<ShowGuiaPrePackingDetalleDTO> args)
        {
            await ObtenerActualizaDetalleBultoGuiaPrePacking(1, args.Item.IDGuia, 1, args.Item.Linea, 0, args.Item.IDArticulo, 0, 0, 0, idUsuario);
            DetalleGuiaReDTO.RemoveAt(DetalleGuiaReDTO.Count() - 1);
            var parameters = new DialogParameters<DialogPrepackingDetalleBulto>
            {
                { x => x.idGuia, args.Item.IDGuia },
                { x => x.DetalleGuiaReBu, showGuiaDetalleBultos },
                { x => x.Detalle, args.Item },
                { x => x.idUsuario, idUsuario },
                { x => x.img, args.Item.Imagen },
                //{ x => x.FuncionActualizarDetalle, cargarFaltantes }
            };

            var options = new DialogOptions() { CloseButton = false, FullWidth = false, MaxWidth = MaxWidth.ExtraExtraLarge, DisableBackdropClick = true, };
            var dialog = await DialogService.ShowAsync<DialogPrepackingDetalleBulto>($"Articulo: {args.Item.IDArticulo}", parameters, options);
            var result = await dialog.Result;
            if ((double)result.Data >= 0)
            {
                DetalleGuiaReDTO.Find(x => x.IDArticulo == args.Item.IDArticulo && x.Linea == args.Item.Linea).Faltan = (double)result.Data;
            }
            else
            {
                DetalleGuiaReDTO.Find(x => x.IDArticulo == args.Item.IDArticulo && x.Linea == args.Item.Linea).Faltan = DetalleGuiaReDTO.Find(x => x.IDArticulo == args.Item.IDArticulo).CantidadEM;
            }
        }

        private async void ListoGuia()
        {
            bool prompt = await DialogServicesf.ConfirmAsync("¿Esta seguro que esta listo el Pre-Packing?");
            if (prompt)
            {
                Submit();
                await FuncionActualizar();
                await IngresarGuiaPrepacking(1, DocEntryEM, DocNumEM, idUsuario);
                await ActualizarEstadoGuiaPrePacking(1, Convert.ToDouble(idGuia), 1, 1, 5, "OK", idUsuario);
                await GenerarPDFPrePacking();
                snakBarCreation("Guía actualizada con éxito!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
            }
        }
        private async void ACtualizar()
        {
            bool prompt = await DialogServicesf.ConfirmAsync("¿Esta seguro que desea cancelar el Pre-Packing?");
            if (prompt)
            {
                Submit();
                await FuncionActualizar();
            }
        }

        private async Task cargarFaltantes()
        {
            DetalleGuiaReDTO = new List<ShowGuiaPrePackingDetalleDTO> { };
            foreach (ShowGuiaPrePackingDetalle item in DetalleGuiaRe)
            {
                await ObtenerActualizaDetalleBultoGuiaPrePacking(1, item.IDGuia, 1, item.Linea, 0, item.IDArticulo, 0, 0, 0, idUsuario);

            }

        }

        private async Task cargarFaltantesDespues()
        {
            Loading = true;
            foreach (ShowGuiaPrePackingDetalleDTO item in DetalleGuiaReDTO)
            {
                await ObtenerActualizaDetalleBultoGuiaPrePacking(1, item.IDGuia, 1, item.Linea, 0, item.IDArticulo, 0, 0, 0, idUsuario);

            }
            Loading = false;
        }

        private Func<ShowGuiaPrePackingDetalleDTO, bool> QuickFilter => x =>
        {
            //si no encuetra nada no manda nada
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            //Buscar por NRO de Documento 
            if (x.IDArticulo.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Buscar Por CLiente
            if (x.Nombre.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        };

        public async Task GenerarPDFPrePacking()
        {
            try
            {
                using (MemoryStream memory = exportService.CreatePdfPrePacking(DetalleGuiaReDTOPDF, idGuia, idUsuario, DocEntryEM, DocNumEM, ReferenciaEM))
                {
                    await js.SaveAs($"Guia Prepacking {DetalleGuiaRe.FirstOrDefault().IDGuia}.pdf", memory.ToArray());

                }
            }
            catch (Exception e) 
            {
                snakBarCreation("Error - " + e.Message, Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
               
            }
        }
        private async Task ImprimirPDF()
        {
            try
            {
                await GenerarPDFPrePacking();
            }
            catch (Exception e)
            {
                snakBarCreation("Error - " + e.Message, Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
              
            }
        }

    }
}
