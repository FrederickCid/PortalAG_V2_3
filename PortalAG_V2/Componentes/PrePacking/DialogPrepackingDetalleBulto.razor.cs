using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.Formularios;
using PortalAG_V2.Shared.Model.Prepacking;
using PortalAG_V2.Shared.Services;
using System.Net;
using System.Runtime.CompilerServices;

namespace PortalAG_V2.Componentes.PrePacking
{
    public partial class DialogPrepackingDetalleBulto
    {
        public AppState appSatate { get; set; }
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public double idGuia { get; set; }
        [Parameter] public string idUsuario { get; set; }
        [Parameter] public List<ShowGuiaPrePackingDetalleBulto> DetalleGuiaReBu { get; set; }
        [Parameter] public ShowGuiaPrePackingDetalleDTO Detalle { get; set; }
        [Parameter] public string img { get; set; }
        [Parameter] public Func<Task> funcion { get; set; }
        [Parameter] public Func<Task> FuncionActualizarDetalle { get; set; }

        //void Submit() => MudDialog.Close(DialogResult.Ok(showGuiaDetalleBultos.FirstOrDefault().fa));
        void Cancel() => MudDialog.Cancel();


        MainServices service;
        private bool Loading = false;
        private MudDataGrid<ShowGuiaPrePackingDetalleBulto> GridHO;
        private List<ShowGuiaPrePackingDetalleBulto> showGuiaDetalleBultos = new List<ShowGuiaPrePackingDetalleBulto> { };
        public static string ObtenerActualizaDetalleBulto = "GuiaPrePackingList/DetalleBultoGuiaPrePacking/";
        private int bultos;
        private int uniXbultos;
        MudDialog myDialog;




        private async Task ObtenerActualizaDetalleBultoGuiaPrePacking(int idtipoguia, double idguia, int tipoConsulta, int linea, int lineaNum, string? idArticulo, int lineaBulto, int bultos, int unidadxBulto, string idUsuario)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.PrePacking.HttpClientInstance.GetAsync($"api/v1/{ObtenerActualizaDetalleBulto}{idtipoguia}/{idguia}/{tipoConsulta}/{linea}/{lineaNum}/{idArticulo}/{lineaBulto}/{bultos}/{unidadxBulto}/{idUsuario}");
                if (lista.IsSuccessStatusCode)
                {
                    showGuiaDetalleBultos = JsonConvert.DeserializeObject<List<ShowGuiaPrePackingDetalleBulto>>(await lista.Content.ReadAsStringAsync());
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



        #region SnackBar

        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion

        private async Task AgregarBulto(int bultos, int uni, int tipoConsulta) //2 Agregar bultos a unidad x bulto  || 22 Agregar unidad x bulto 1
        {
            showGuiaDetalleBultos = new List<ShowGuiaPrePackingDetalleBulto> { };
            await ObtenerActualizaDetalleBultoGuiaPrePacking(1, Detalle.IDGuia, tipoConsulta, Detalle.Linea, 0, Detalle.IDArticulo, 0, bultos, uni, idUsuario);
            if (showGuiaDetalleBultos.Count > 0)
            {
                if (showGuiaDetalleBultos.FirstOrDefault().msgResult == "OK")
                {
                    DetalleGuiaReBu = new List<ShowGuiaPrePackingDetalleBulto> { };
                    DetalleGuiaReBu = showGuiaDetalleBultos;
                    snakBarCreation(($"Agregado Correctamente"), Defaults.Classes.Position.BottomStart, Severity.Success, 1000);

                }
                else
                {
                    snakBarCreation(($"{showGuiaDetalleBultos.FirstOrDefault().msgError}"), Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);

                }
            }
            else
            {
                DetalleGuiaReBu = new List<ShowGuiaPrePackingDetalleBulto> { };
            }
            StateHasChanged();
        }

        private async Task EliminarBulto(ShowGuiaPrePackingDetalleBulto args)
        {
         
            showGuiaDetalleBultos = new List<ShowGuiaPrePackingDetalleBulto> { };
            await ObtenerActualizaDetalleBultoGuiaPrePacking(1, args.IDGuia, 3, args.Linea, 0, args.IDArticulo, args.LineaBulto, 0, 0, idUsuario);
            if (showGuiaDetalleBultos.Count > 0)
            {
                if (showGuiaDetalleBultos.FirstOrDefault().msgResult == "OK")
                {
                    DetalleGuiaReBu = new List<ShowGuiaPrePackingDetalleBulto> { };
                    DetalleGuiaReBu = showGuiaDetalleBultos;
                    snakBarCreation(($"Borrado Correctamente"), Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
            }
            else
            {
                DetalleGuiaReBu = new List<ShowGuiaPrePackingDetalleBulto> { };
            }            
        }
        private void listo()
        {
            MudDialog.Close(DialogResult.Ok(DetalleGuiaReBu.Count>0 ? DetalleGuiaReBu.FirstOrDefault().Faltan : -1));
        }
    }
}
