using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes;
using PortalAG_V2.Componentes.EstadoPedido;
using PortalAG_V2.Componentes.Pagos;
using PortalAG_V2.Pages.ConfirmacionDespacho;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.AvisoDePago;
using PortalAG_V2.Shared.Services;
using PortalAG_V2.Shared.Model.AvisoDePago;

using System.Net.Http.Json;


namespace PortalAG_V2.Pages.AvisoDePago
{
    public partial class AvisoPagoNotificacion
    {
        #region Variables
        [CascadingParameter]
        public AppState appSatate { get; set; }
        private ClientFactory conexion;
        private Timer? timer;
        Loading? loading;
        private bool _processing = false;
        private bool isDialogVisible = false;
        bool _color = false;
        string _IDUser = "";
        int operancion = 0;
        private int Count = 240;
        private HashSet<AvisoPagoModel> selectRow = new HashSet<AvisoPagoModel>();
        private List<AvisoPagoModel> _listConfirmacion = new List<AvisoPagoModel>();
        private List<AvisoPagoModel> _listConfirmacionDetalle = new List<AvisoPagoModel>();
        public List<BancoModel> Bancos = new List<BancoModel>();
        public List<BancoAndesModel> BancosAndes = new List<BancoAndesModel>();
        MainServices service;
        #endregion

        #region Endpoints
        private const string urlListado = "api/v2/AvisodePagos/ConsultaAvisoPagoCaja";
        private const string UrlListadoBancos = "api/v2/AvisodePagos/ConsultaBancos";
        private const string UrlListadoBancosAndes = "api/v2/AvisodePagos/ConsultaBancosAndes";
        #endregion

        #region al iniciar la pagina
        protected override async Task OnInitializedAsync()
        {
            StartCountdown();
            await ConsultarLista();
            await CargaBancos();
            await CargaBancosAndes();
        }
        #endregion

        #region Contador Reversa
        public void StartCountdown()
        {
            try
            {
                timer = new Timer(new TimerCallback(async (e) =>
                {
                    if (Count <= 0)
                    {
                        await Actualizar();
                        Count = 240;
                    }
                    else
                    {
                        Count--;
                    }
                    await InvokeAsync(StateHasChanged);
                }), null, 1000, 1000);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }
        #endregion

        #region consultar lista
        public async Task ConsultarLista()
        {
            var user = await _authenticationManager.CurrentUser();
            _IDUser = user.GetUserId();

            conexion = new MainServices().Formularios;
            var auxListado = await conexion.HttpClientInstance.GetAsync($"{urlListado}/{appSatate.IDUsuario}");
            if (auxListado.IsSuccessStatusCode)
            {
                try
                {
                    _listConfirmacion = JsonConvert.DeserializeObject<List<AvisoPagoModel>>(await auxListado.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error al deserealizar listado: " + ex.Message, Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar listado", Severity.Error);
            }
        }
        public async Task ConsultarListaDetalle(string idUsuario, int IDOperacion)
        {
            var user = await _authenticationManager.CurrentUser();
            _IDUser = user.GetUserId();

            conexion = new MainServices().Formularios;
            var auxListado = await conexion.HttpClientInstance.GetAsync($"{urlListado}/{idUsuario}/{IDOperacion}");
            if (auxListado.IsSuccessStatusCode)
            {
                try
                {
                    _listConfirmacionDetalle = JsonConvert.DeserializeObject<List<AvisoPagoModel>>(await auxListado.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error al deserealizar listado: " + ex.Message, Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar listado", Severity.Error);
            }
        }
        private async Task CargaBancos()
        {
            conexion = new MainServices().Formularios;
            var auxListado = await conexion.HttpClientInstance.GetAsync($"{UrlListadoBancos}");
            if (auxListado.IsSuccessStatusCode)
            {
                try
                {
                    Bancos = JsonConvert.DeserializeObject<List<BancoModel>>(await auxListado.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error al deserealizar listado: " + ex.Message, Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar listado", Severity.Error);
            }
        }
        private async Task CargaBancosAndes()
        {
            conexion = new MainServices().Formularios;
            var auxListado = await conexion.HttpClientInstance.GetAsync($"{UrlListadoBancosAndes}");
            if (auxListado.IsSuccessStatusCode)
            {
                try
                {
                    BancosAndes = JsonConvert.DeserializeObject<List<BancoAndesModel>>(await auxListado.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error al deserealizar listado: " + ex.Message, Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar listado", Severity.Error);
            }
        }



        #endregion

        #region color cuando se selecciona una row
        public string ColorSelect(AvisoPagoModel element, int rowNumber)
        {
            return selectRow.Contains(element) ? "selected" : string.Empty;
        }
        #endregion

        #region para llamar modal
        private async Task ClickRow(DataGridRowClickEventArgs<AvisoPagoModel> args)
        {
            await ConsultarListaDetalle(appSatate.IDUsuario, args.Item.IDOperacion);

            if (_listConfirmacionDetalle.Count > 0)
            {
                var parameters = new DialogParameters<DialogTransferencia>
                {
                    { x => x.idUsuario, appSatate.IDUsuario },
                    { x => x.funcion, ConsultarLista },
                    { x => x.Detalle, _listConfirmacionDetalle.FirstOrDefault() },
                    { x => x.imagenes, _listConfirmacionDetalle.FirstOrDefault().Imagenes },
                    { x => x.Bancos, Bancos },
                    { x => x.BancosAndes, BancosAndes }
                };

                var options = new MudBlazor.DialogOptions() { CloseButton = true, FullWidth = false, MaxWidth = MaxWidth.Medium };

                if (_listConfirmacionDetalle.FirstOrDefault().IDTipoPago == 13)
                {
                    DialogService.Show<DialogTransferencia>($"Transferencia", parameters, options);

                }
                else 
                {
                    DialogService.Show<DialogTransferencia>($"Deposito", parameters, options);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar el detalle", Severity.Error);
            }
        }

        #endregion

        #region funcion para actualizar

        public async Task Actualizar()
        {
            try
            {
                await ConsultarLista();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                loading?.Cerrar();
            }
        }
        #endregion
    }
}
