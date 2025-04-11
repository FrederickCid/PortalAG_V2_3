using agDataAccess.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.AvisoDePago;
using PortalAG_V2.Shared.Services;
using PortalAG_V2.Shared.Model.AvisoDePago;
using Syncfusion.Blazor.Popups;
using System;


namespace PortalAG_V2.Componentes.Pagos
{
    public partial class DialogTransferencia
    {

        
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string idUsuario { get; set; }
        [Parameter] public AvisoPagoModel Detalle { get; set; } = new AvisoPagoModel();
        [Parameter] public List<BancoModel> Bancos { get; set; }
        [Parameter] public List<BancoAndesModel> BancosAndes { get; set; }
        [Parameter] public List<Archivo> imagenes { get; set; }
        [Parameter] public Func<Task> funcion { get; set; }

        public List<AvisoPagoModel> Detalled = new List<AvisoPagoModel>();
        private ClientFactory conexion;

        

        private const string urlListado = "api/v2/AvisodePagos/ActualizaEstado";

       


        public async Task EnviarConsulta(string idUsuario, int IDOperacion)
        {
            var user = await _authenticationManager.CurrentUser();
            

            conexion = new MainServices().Formularios;
            var auxListado = await conexion.HttpClientInstance.GetAsync($"{urlListado}/{idUsuario}/{IDOperacion}");
            if (auxListado.IsSuccessStatusCode)
            {
                _snackBar.Add("Listo", Severity.Success);
                
            }
            else
            {
                _snackBar.Add("Error al consultar listado", Severity.Error);
            }
        }

        async Task Abrir(string imagen, string nombre)
        {
            var parameters = new DialogParameters<ImagenCompleta>();
            parameters.Add(x => x.imagen, imagen);
            var options = new MudBlazor.DialogOptions() { CloseButton = true, FullWidth = false, MaxWidth = MaxWidth.ExtraExtraLarge };

            DialogService.Show<ImagenCompleta>($"{nombre}", parameters, options);
        }

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel(); 

        public async Task Pagado()
        {
            bool prompt = await DialogServicesf.ConfirmAsync("¿Esta ingresado en SAP?");
            if (prompt)
            {
                await EnviarConsulta(idUsuario, Detalle.IDOperacion);
                await funcion();
                Cancel();
            }
        }

        private async Task OnCancelar() { 
             MudDialog.Cancel();
        }

        private void Rechazar() {
            var parameters = new DialogParameters<RechazarPago>();
            parameters.Add(x => x.idUsuario, idUsuario);
            parameters.Add(x => x.FuncCloseDialog, OnCancelar);
            parameters.Add(x => x.funcRecargar, funcion);
            parameters.Add(x => x.idOperacion, Detalle.IDOperacion);
            var options = new MudBlazor.DialogOptions() { CloseButton = true, FullWidth = false, MaxWidth = MaxWidth.Medium };
            DialogService.Show<RechazarPago>($"Rachazar?", parameters, options);
        }

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

