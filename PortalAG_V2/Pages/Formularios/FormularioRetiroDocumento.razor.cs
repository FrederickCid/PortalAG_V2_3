using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.Formularios;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;

namespace PortalAG_V2.Pages.Formularios
{
    public partial class FormularioRetiroDocumento
    {
        #region Variables
        public MainServices? service;
        MudForm form;
        MudButton limpiarButton;
        MudTextField<string> MudTextNroCliente;
        MudSelect<string> Direcciones;
        int solicitud;
        bool success = true;
        string[] errors = { };
        string _NroCliente = "";
        string _NombreCliente = "";
        string _Celular = "";
        string _Correo = "";
        string _Observaciones = "";
        string _Direccion ;
        int CantidadCheques;
        private bool showCallAlert = true;
        private List<FormularioModel> Campos = new List<FormularioModel>();
        private List<DireccionesModel> Direccion = new List<DireccionesModel>();
        int _Nrofactura = 0;
        int _Tipo = 3;
        #endregion



        string urlFormulario = "Formulario/GetFormulario";
        string urlFormularioPost = "Formulario/ActualizarFormulario/";


        public async Task CargarDatos(string NroCliente)
        {
            try
            {
                service = new MainServices();
                var lista = await service.Formulario.HttpClientInstance.GetAsync($"api/v2/{urlFormulario}/{_Tipo}/{_Nrofactura}/{NroCliente}");
                if (lista.IsSuccessStatusCode)
                {
                    Campos = JsonConvert.DeserializeObject<List<FormularioModel>>(await lista.Content.ReadAsStringAsync());
                }
                else
                {
                    Campos = new List<FormularioModel>();
                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }



        #region Funcion Alerta
        private void CerrarAlerta()
        {
            showCallAlert = !showCallAlert;
        }
        #endregion

        #region Funcion Botones
        private void limpiar()
        {

            if (_NroCliente.Length > 0)
            {
                _NroCliente = "";
                _NombreCliente = "";
                _Celular = "";
                _Correo = "";
                _Observaciones = "";
                CantidadCheques = 0;
                Direcciones.Disabled = true;
                snakBarCreation("Se ha limpiado el formulario", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                Campos = new List<FormularioModel>();
                Direccion = new List<DireccionesModel>();
            }
            else
            {
                snakBarCreation("Nada que limpiar", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
            }

        }
        private void limpiarAlBuscar()
        {

            if (_NroCliente.Length > 0)
            {
              
                _NombreCliente = "";
                _Celular = "";
                _Correo = "";
                _Observaciones = "";
                CantidadCheques = 0;
                Direcciones.Disabled = true;
                Campos = new List<FormularioModel>();
                Direccion = new List<DireccionesModel>();
            }

        }
        
        async Task EnviarSolicitud()
        {
            bool prompt = await DialogService.ConfirmAsync("¿Esta seguro del retiro de documento?");
            if (prompt)
            {
                DocumentoRequestActuaCheque FormularioEnvioPost = new DocumentoRequestActuaCheque()
                {
                    IDDireccion = _Direccion,
                    Observacion = _Observaciones,
                    TotalCheques = CantidadCheques,
                    NroSolicitud = solicitud
                };
                service = new MainServices();     
               
                var formulario = await service.Formulario.HttpClientInstance.PostAsJsonAsync($"api/v2/{urlFormularioPost}", FormularioEnvioPost);
                if (formulario.IsSuccessStatusCode)
                {
                    snakBarCreation("Eviado", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);
                    _NroCliente = "";
                    _NombreCliente = "";
                    _Celular = "";
                    _Correo = "";
                    _Observaciones = "";
                    _Direccion = "";
                    CantidadCheques = 0;
                    Direcciones.Disabled = true;
                    Direccion = new List<DireccionesModel>();

                }
                else
                {
                    snakBarCreation("Error", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);
                }
                
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

        #region Buscar Cliente
        private async void onEnterPress(KeyboardEventArgs args)
        {
            if (args.Key == "Enter" && _NroCliente.Length > 0)
            {
                limpiarAlBuscar();
                await CargarDatos(_NroCliente);
                MudTextNroCliente.Disabled = true;
                snakBarCreation("Buscando...", Defaults.Classes.Position.BottomStart, Severity.Info, 2000);
                await Task.Delay(2000);
                StateHasChanged();
                if (Campos.FirstOrDefault().msgResult.ToUpper() == "OK")
                {
                    snakBarCreation("Cliente Encontrado", Defaults.Classes.Position.BottomStart, Severity.Success, 2000);
                    MudTextNroCliente.Disabled = true;
                    solicitud = Campos.FirstOrDefault().NroSolicitud;
                    _NombreCliente = Campos.FirstOrDefault().RazonSocial;
                    _Celular = Campos.FirstOrDefault().Telefono;
                    _Correo = Campos.FirstOrDefault().Correo;
                    foreach (DireccionesModel item in Campos.FirstOrDefault().Direcciones)
                    {
                        if (item.TipoDireccion.Trim().Contains("B")) 
                        { 
                            Direccion.Add(item);
                        }
                    }
                    MudTextNroCliente.Disabled = false;
                    StateHasChanged();
                }
                else
                {
                    snakBarCreation("Cliente no encontrado", Defaults.Classes.Position.BottomStart, Severity.Error, 6000);
                    _NroCliente = "";
                    _NombreCliente = "";
                    _Celular = "";
                    _Correo = "";
                    _Observaciones = "";
                    CantidadCheques = 0;
                    _Direccion = "";
                    Direcciones.Disabled = true;
                    Direccion = new List<DireccionesModel>();
                    MudTextNroCliente.Disabled = false;
                    Campos = new List<FormularioModel>();
                    StateHasChanged();
                }
            }
        }
        //private async void onBlurBuscar(FocusEventArgs args)
        //{
        //    if (_NroCliente.Length > 2)
        //    {
        //        MudTextNroCliente.Disabled = true;
        //        StateHasChanged();
        //        snakBarCreation("Buscando...", Defaults.Classes.Position.BottomStart, Severity.Info, 6000);
        //        if (_NroCliente == "123123123")
        //        {
        //            await Task.Delay(3000);
        //            snakBarCreation("Cliente encontrado", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);
        //            _NombreCliente = "Cliente 1";
        //            _Celular = "+56912345678";
        //            _Correo = "cliente1@prueba.cl";
        //            MudTextNroCliente.Disabled = false;
        //            StateHasChanged();

        //        }
        //        else
        //        {
        //            await Task.Delay(6000);
        //            snakBarCreation("Cliente no encontrado", Defaults.Classes.Position.BottomStart, Severity.Error, 6000);
        //            _NroCliente = "";
        //            _NombreCliente = "";
        //            _Celular = "";
        //            _Correo = "";
        //            _Observaciones = "";
        //            CantidadCheques = 0;
        //            MudTextNroCliente.Disabled = false;
        //            StateHasChanged();
        //        }
        //    }
        //}
        #endregion




    }

}
