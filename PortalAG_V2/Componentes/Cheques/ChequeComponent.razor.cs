using Microsoft.AspNetCore.Components;
using MudBlazor;
using PortalAG_V2.Auth;
using PortalAG_V2.Pages.Cheques;
using PortalAG_V2.Shared.Model.Cheques;
using PortalAG_V2.Shared.Model.FacturaPorServicio;
using PortalAG_V2.Shared.Model.Pagos;
using PortalAG_V2.Shared.Models.Cheques;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;
using System.Runtime.Intrinsics.X86;

namespace PortalAG_V2.Componentes.Cheques
{
    public partial class ChequeComponent
    {
        public string NroSerie = " 2008KK ";
        public int NroSerie2 = 2524582;
        public string Serial = "0XX-XXXXXXXXX";
        public int Monto = 0;
        public string RazonSocial = "NOMBRE APELLIDO";
        public string NombreCheque = "";

        [Parameter]
        public int Tipo { get; set; } = 1;
        [Parameter]
        public ChequesModel Cheque { get; set; } = new ChequesModel();
        [Parameter]
        public int dia { get; set; } = 0;
        [Parameter]
        public string mes { get; set; } = "";
        [Parameter]
        public int anno { get; set; } = 0;
        [Parameter]
        public Func<Task> ShowChage { get; set; }

        string UlrPostChequesProtesto = "Cheques/ActualizarCheque/Protesto";
        string UlrPostChequesRescate = "Cheques/ActualizarCheque/Rescate";
        string UlrPostChequesDeposito = "Cheques/ActualizarCheque/Deposito";
        string UlrPostChequesProrroga = "Cheques/ActualizarCheque/Prorroga";
        public ChequePostModel ChequePost = new ChequePostModel();
        MainServices service;

        #region Post De Datos Funciones
        private async Task PostChequeProtesto(string Respuesta, string Fecha)
        {
            try
            {
                service = new MainServices();

                ChequePost.IDOperacion = Cheque.IDOperacion;
                ChequePost.AnnoProceso = Cheque.AnnoProceso;
                ChequePost.FechaCancelacion = Fecha;
                ChequePost.NroCtaCteBanco = Cheque.NroCtaCteBanco;
                ChequePost.NroComprobante = Cheque.NroComprobante;
                ChequePost.NumeroSerie = Cheque.NumeroSerie;
                ChequePost.IDCliente = Cheque.IDCliente;
                ChequePost.RazonSocial = Cheque.RazonSocial;
                ChequePost.Monto = Cheque.Monto;
                ChequePost.FechaVencimiento = Cheque.FechaVencimiento;
                ChequePost.IDBanco = Cheque.IDBanco;
                ChequePost.Banco = Cheque.Banco;
                ChequePost.DocEntry = Cheque.DocEntry;
                ChequePost.Comentario = Respuesta;

                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostChequesProtesto}", ChequePost);
                if (Response.IsSuccessStatusCode)
                {
                    var Data = Response.Content.ReadFromJsonAsync<ChequesModel>();
                    snakBarCreation($"ingresado son exito", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);

                }
                else
                {
                    snakBarCreation($"Error Al envíar", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

                }
            }
            catch (Exception e)
            {
                snakBarCreation($"Error - {e.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

            }
        }
        private async Task PostChequeRescate(string Respuesta)
        {
            try
            {
                service = new MainServices();

                ChequePost.IDOperacion = Cheque.IDOperacion;
                ChequePost.AnnoProceso = Cheque.AnnoProceso;
                ChequePost.FechaCancelacion = Cheque.FechaCancelacion;
                ChequePost.NroCtaCteBanco = Cheque.NroCtaCteBanco;
                ChequePost.NroComprobante = Cheque.NroComprobante;
                ChequePost.NumeroSerie = Cheque.NumeroSerie;
                ChequePost.IDCliente = Cheque.IDCliente;
                ChequePost.RazonSocial = Cheque.RazonSocial;
                ChequePost.Monto = Cheque.Monto;
                ChequePost.FechaVencimiento = Cheque.FechaVencimiento;
                ChequePost.IDBanco = Cheque.IDBanco;
                ChequePost.Banco = Cheque.Banco;
                ChequePost.DocEntry = Cheque.DocEntry;
                ChequePost.Comentario = Respuesta;

                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostChequesRescate}", ChequePost);
                if (Response.IsSuccessStatusCode)
                {
                    var Data = Response.Content.ReadFromJsonAsync<ChequesModel>();
                    snakBarCreation($"ingresado son exito", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);

                }
                else
                {
                    snakBarCreation($"Error Al envíar", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

                }
            }
            catch (Exception e)
            {
                snakBarCreation($"Error - {e.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

            }
        }
        private async Task PostChequeDeposito(string Respuesta)
        {
            try
            {
                service = new MainServices();

                ChequePost.IDOperacion = Cheque.IDOperacion;
                ChequePost.AnnoProceso = Cheque.AnnoProceso;
                ChequePost.FechaCancelacion = Cheque.FechaCancelacion;
                ChequePost.NroCtaCteBanco = Cheque.NroCtaCteBanco;
                ChequePost.NroComprobante = Cheque.NroComprobante;
                ChequePost.NumeroSerie = Cheque.NumeroSerie;
                ChequePost.IDCliente = Cheque.IDCliente;
                ChequePost.RazonSocial = Cheque.RazonSocial;
                ChequePost.Monto = Cheque.Monto;
                ChequePost.FechaVencimiento = Cheque.FechaVencimiento;
                ChequePost.IDBanco = Cheque.IDBanco;
                ChequePost.Banco = Cheque.Banco;
                ChequePost.DocEntry = Cheque.DocEntry;
                ChequePost.Comentario = Respuesta;

                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostChequesDeposito}", ChequePost);
                if (Response.IsSuccessStatusCode)
                {
                    var Data = Response.Content.ReadFromJsonAsync<ChequesModel>();
                    snakBarCreation($"ingresado son exito", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);

                }
                else
                {
                    snakBarCreation($"Error Al envíar", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

                }
            }
            catch (Exception e)
            {
                snakBarCreation($"Error - {e.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

            }
        }
        private async Task PostChequeProrroga(string Respuesta, string Fecha)
        {
            try
            {
                service = new MainServices();

                ChequePost.IDOperacion = Cheque.IDOperacion;
                ChequePost.AnnoProceso = Cheque.AnnoProceso;
                ChequePost.FechaCancelacion = Cheque.FechaCancelacion;
                ChequePost.NroCtaCteBanco = Cheque.NroCtaCteBanco;
                ChequePost.NroComprobante = Cheque.NroComprobante;
                ChequePost.NumeroSerie = Cheque.NumeroSerie;
                ChequePost.IDCliente = Cheque.IDCliente;
                ChequePost.RazonSocial = Cheque.RazonSocial;
                ChequePost.Monto = Cheque.Monto;
                ChequePost.FechaVencimiento = Fecha;
                ChequePost.IDBanco = Cheque.IDBanco;
                ChequePost.Banco = Cheque.Banco;
                ChequePost.DocEntry = Cheque.DocEntry;
                ChequePost.Comentario = Respuesta;

                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostChequesProrroga}", ChequePost);
                if (Response.IsSuccessStatusCode)
                {
                    var Data = Response.Content.ReadFromJsonAsync<ChequesModel>();
                    snakBarCreation($"ingresado son exito", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);

                }
                else
                {
                    snakBarCreation($"Error Al envíar", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

                }
            }
            catch (Exception e)
            {
                snakBarCreation($"Error - {e.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

            }
        }
        #endregion

        #region Funcionalidades Boton 
        public async Task Protestar()
        {
            try
            {
                if (Cheque.Si_Failed == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Protestado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Closed == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Rescatado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Paid == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Depositado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Cancelled == 1) { snakBarCreation($"Con Prorroga!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }

                var parameters = new DialogParameters<DialogProtesto> { };
                var options = new MudBlazor.DialogOptions() { CloseButton = false, DisableBackdropClick = true, };
                var dialog = await DialogService.ShowAsync<DialogProtesto>("Question", parameters, options);
                var result = await dialog.Result;
                string[] Data = (string[])result.Data;
                if (Data.Length > 0)
                {
                    await PostChequeProtesto(Data[0], Data[1]);
                    await Limpiar();
                    snakBarCreation($"Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    //snakBarCreation($"{Data[0]} - Mensaje Protestar {Data[1]}  ", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    snakBarCreation("Operacion Cancelada", Defaults.Classes.Position.BottomStart, Severity.Info, 1000);

                }
            }
            catch (Exception e)
            {
                snakBarCreation(e.Message, Defaults.Classes.Position.BottomStart, Severity.Info, 1000);
            }
        }
        public async void Rescate()
        {
            try
            {
                if (Cheque.Si_Failed == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Protestado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Closed == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Rescatado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Paid == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Depositado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Cancelled == 1) { snakBarCreation($"Con Prorroga!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                var parameters = new DialogParameters<DialogRescate> { };

                var options = new MudBlazor.DialogOptions() { CloseButton = false, DisableBackdropClick = true, };
                var dialog = await DialogService.ShowAsync<DialogRescate>("Question", parameters, options);
                var result = await dialog.Result;
                if (!string.IsNullOrEmpty((string)result.Data))
                {
                    await PostChequeRescate((string)result.Data);
                    snakBarCreation($"Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    //snakBarCreation($"{(string)result.Data}  - Mensaje Rescate ", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    await Limpiar();

                }
                else
                {
                    snakBarCreation("Operacion Cancelada", Defaults.Classes.Position.BottomStart, Severity.Info, 1000);

                }
            }
            catch (Exception e) { snakBarCreation(e.Message, Defaults.Classes.Position.BottomStart, Severity.Info, 1000); }
        }
        public async void Prorroga()
        {
            try
            {
                if (Cheque.Si_Failed == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Protestado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Closed == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Rescatado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Paid == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Depositado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Cancelled == 1) { snakBarCreation($"Con Prorroga!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                var parameters = new DialogParameters<DialogProrroga> { };
                var options = new MudBlazor.DialogOptions() { CloseButton = false, DisableBackdropClick = true, };
                var dialog = await DialogService.ShowAsync<DialogProrroga>("Question", parameters, options);
                var result = await dialog.Result;
                string[] Data = (string[])result.Data;

                if (Data.Length > 0)
                {
                    await PostChequeProrroga(Data[0], Data[1]);
                    snakBarCreation($"Listo! ", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    //snakBarCreation($"{Data[0]} - {Data[1]} ", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    await Limpiar();
                }
                else
                {
                    snakBarCreation("Operacion Cancelada", Defaults.Classes.Position.BottomStart, Severity.Info, 1000);

                }
            }
            catch (Exception e)
            {
                snakBarCreation(e.Message, Defaults.Classes.Position.BottomStart, Severity.Info, 1000);
            }
        }
        public async void Deposito()
        {
            try
            {
                if (Cheque.Si_Failed == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Protestado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Closed == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Rescatado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Paid == 1 && Cheque.Si_Cancelled == 1) { snakBarCreation($"Ya Depositado!", Defaults.Classes.Position.BottomStart, Severity.Info, 1000); return; }
                if (Cheque.Si_Cancelled == 1) { snakBarCreation($"Con Prorroga!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000); return; }
                var parameters = new DialogParameters<DialogDeposito> { };
                var options = new MudBlazor.DialogOptions() { CloseButton = false, DisableBackdropClick = true, };
                var dialog = await DialogService.ShowAsync<DialogDeposito>("Question", parameters, options);

                var result = await dialog.Result;
                if (!string.IsNullOrEmpty((string)result.Data))
                {
                    await PostChequeDeposito((string)result.Data);
                    snakBarCreation($"Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    //snakBarCreation($"{(string)result.Data}  - Mensaje Deposito ", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    await Limpiar();

                }
                else
                {
                    snakBarCreation("Operacion Cancelada", Defaults.Classes.Position.BottomStart, Severity.Info, 1000);

                }
            }
            catch (Exception e) { snakBarCreation(e.Message, Defaults.Classes.Position.BottomStart, Severity.Info, 1000); }
        }
        private async Task Limpiar()
        {
            Cheque = new();
            await ShowChage();
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
