using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.Pagos;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using PortalAG_V2.Shared.Services;
using System;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.Intrinsics.X86;
using static MudBlazor.CategoryTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortalAG_V2.Pages.Pagos
{
    partial class NuevoPago
    {
        [CascadingParameter] private AppState appSatate { get; set; }

        private ClientFactory conexion;
        private const string urlPedidoPendientes = "api/v2/Pagos/PagosIdCliente";
        private const string urlIngresaPagos = "api/v2/Pagos/PagoEnviado";

        public string idCliente { get; set; }
        public string razonSocial { get; set; }
        public string deudaTotal { get; set; }
        public string referencia { get; set; }
        public string nroCobranza { get; set; }
        public string comentarios { get; set; }
        public string sumaImporte { get; set; }
        public string totalPago { get; set; }
        public string saldoPendiente { get; set; }
        

        DateTime? dateContabilizacion = DateTime.Today;
        DateTime? dateVencimiento = DateTime.Today;
        DateTime? dateDocumento = DateTime.Today;

        public PagosClienteDTO clienteDTO = new PagosClienteDTO();
        public IList<PedidosDTO> pedidosPendientePago = new List<PedidosDTO>();
        public List<PedidosDTO> pedidosPendientePagoSelected = new List<PedidosDTO>();
        public IList<Documeto> facturaDTO = new List<Documeto>();
        public List<SaldosFavorDTO> saldoFavor = new List<SaldosFavorDTO>();
        public List<SaldosFavorDTO> saldoFavorSelect = new List<SaldosFavorDTO>();

        public bool checkPago { get; set; } = false;
        public bool _procesarPago { get; set; } = false;
        private PagosIngresadosDTO pagosIngresadosPopUp { get; set; } = new PagosIngresadosDTO();
        string _IDUser = "";
        protected override async Task OnInitializedAsync()
        {
            pedidosPendientePago = new List<PedidosDTO>();
            pedidosPendientePagoSelected = new List<PedidosDTO>();
            saldoFavor = new List<SaldosFavorDTO>();

            var user = await _authenticationManager.CurrentUser();
            var IDuse = user.GetFirstName();
            _IDUser = user.GetUserId();

            conexion = new MainServices().Pagos;

        }

        private void Regresar()
        {
            _navigationManager.NavigateTo("/pagos");
        }
        private async void IrAPagar()
        {
            //_navigationManager.NavigateTo("/formaspagos");
            if (Convert.ToDouble(sumaImporte) > 0)
            {
                var parameters = new DialogParameters
                {
                    {nameof(FormasPagos.args), clienteDTO},
                    {nameof(FormasPagos.appSatate), appSatate},
                    {nameof(FormasPagos.pagosIngresadosEntrada), pagosIngresadosPopUp},
                    {nameof(FormasPagos.pagoAnticipado), false}
                };

                var options = new DialogOptions
                {
                    ClassBackground = "my-custom-class",
                    FullWidth = true,
                    MaxWidth = MaxWidth.ExtraLarge,
                    CloseButton = true,
                    DisableBackdropClick = true,
                };
                var dialogo = _dialogService.Show<FormasPagos>("Formas de pago", parameters, options);
                var result = await dialogo.Result;
                if (!result.Cancelled)
                {
                    var data = (PagosIngresadosDTO)result.Data;
                    pagosIngresadosPopUp = data;
                    totalPago = pagosIngresadosPopUp.TotalPago.ToString("n0");

                    CalculoSaldoPendiente();
                    StateHasChanged();
                }
            }
            else if(idCliente != "" && idCliente != null)
            {
                _snackBar.Add("Esta por generar un pago anticiapdo",Severity.Warning);
                var parameters = new DialogParameters
                {
                    {nameof(FormasPagos.args), clienteDTO},
                    {nameof(FormasPagos.appSatate), appSatate},
                    {nameof(FormasPagos.pagosIngresadosEntrada), pagosIngresadosPopUp},
                    {nameof(FormasPagos.pagoAnticipado), true}
                };

                var options = new DialogOptions
                {
                    ClassBackground = "my-custom-class",
                    FullWidth = true,
                    MaxWidth = MaxWidth.ExtraLarge,
                    CloseButton = true,
                    DisableBackdropClick = true,
                };
                var dialogo = _dialogService.Show<FormasPagos>("Formas de pago", parameters, options);
                var result = await dialogo.Result;
                if (!result.Cancelled)
                {
                    var data = (PagosIngresadosDTO)result.Data;
                    pagosIngresadosPopUp = data;
                    totalPago = pagosIngresadosPopUp.TotalPago.ToString("n0");

                    CalculoSaldoPendiente();
                    StateHasChanged();
                }
            }
            else
            {
                _snackBar.Add("Ingrese codigo del cliente", Severity.Error);
            }
            
        }

        private async void AceptarPago()
        {
            var parameters = new DialogParameters
            {
                {nameof(DialogConfirmacionPago.pedidosPendientePagoSelected), pedidosPendientePagoSelected}
            };

            var options = new DialogOptions
            {
                ClassBackground = "my-custom-class",
                FullWidth = true,
                MaxWidth = MaxWidth.Medium,
                CloseButton = true,
                DisableBackdropClick = true,
            };
            var dialogo = _dialogService.Show<DialogConfirmacionPago>("Confirmacion de proceso", parameters, options);
            var result = await dialogo.Result;
            if (!result.Cancelled)
            {
                var data = (bool)result.Data;
                CrearObjetoParaSAP();
                //_snackBar.Add("Pago y liberacion procesada",Severity.Success);
            }
        }

        private async Task CrearObjetoParaSAP()
        {
            ProcesarParaSap _paraSAP = new ProcesarParaSap();

            try
            {
                // CABECERA
                _paraSAP.idCliente = idCliente;
                _paraSAP.razonSocial = razonSocial;
                _paraSAP.numeroCobranza = 0;
                _paraSAP.fechaContabilizacion = dateContabilizacion.Value;
                _paraSAP.fechaVencimiento = dateVencimiento.Value;
                _paraSAP.fechaDocumento = dateDocumento.Value;
                _paraSAP.referenciaPago = referencia;
                _paraSAP.comentarios = comentarios;
                _paraSAP.sumaImporte = Convert.ToInt32(sumaImporte.Replace(".", string.Empty));
                _paraSAP.saldoPendiente = Convert.ToInt32(saldoPendiente.Replace(".", string.Empty)); ;
                _paraSAP.totalPago = Convert.ToInt32(totalPago.Replace(".", string.Empty)); ;
                _paraSAP.deudaCliente = int.Parse(deudaTotal.Replace(".", string.Empty).Replace("CLP",string.Empty));

                //Pedidos a pagar y/o liberar
                _paraSAP.ventasPagoPedidos = new List<Ventaspagopedido>();
                if (pedidosPendientePagoSelected != null)
                {
                    foreach (PedidosDTO pedidos in pedidosPendientePagoSelected)
                    {
                        Ventaspagopedido pagoPedidos = new Ventaspagopedido();

                        pagoPedidos.idOperacionPedido = pedidos.IDOperacion;
                        pagoPedidos.idTipoOperacion = 0;
                        pagoPedidos.nroDocumento = pedidos.NroDocumento;
                        pagoPedidos.docEntry = 0;
                        pagoPedidos.montoPedido = Convert.ToInt32(pedidos.Monto);
                        pagoPedidos.pagadoAFecha = Convert.ToInt32(pedidos.PagadoAFecha);
                        pagoPedidos.saldoAFecha = Convert.ToInt32(pedidos.SaldoAFecha);
                        pagoPedidos.valorAPagar = Convert.ToInt32(pedidos.ValorAPagar);
                        pagoPedidos.saldoNuevo = Convert.ToInt32(pedidos.SaldoNuevo);
                        pagoPedidos.siAutorizaLiberacion = pedidos.Check ? 1 : 0;
                        pagoPedidos.fechaAutorizaLiberacion = DateTime.Now;
                        pagoPedidos.idUsuarioAutorizaLiberacion = _IDUser;
                        pagoPedidos.siProgramaDespacho = 0;
                        pagoPedidos.fechaProgramarLiberacion = DateTime.Now;
                        pagoPedidos.idUsuarioProgramarLiberacion = "";
                        pagoPedidos.notaAutorizacionLiberacion = "";

                        _paraSAP.ventasPagoPedidos.Add(pagoPedidos);
                    }
                }

                // Efectivo
                _paraSAP.efectivo = new Efectivo();
                if (pagosIngresadosPopUp.Efectivo != null)
                {
                    _paraSAP.efectivo.fechaEfectivo = pagosIngresadosPopUp.Efectivo.FechaEfectivo;
                    _paraSAP.efectivo.cuentaContable = "";
                    _paraSAP.efectivo.montoEfectivo = pagosIngresadosPopUp.Efectivo.MontoEfectivo;
                }
                    

                // Transferencia
                _paraSAP.transferencias = new List<Transferencia>();
                if (pagosIngresadosPopUp.Transferencias != null)
                {
                    foreach (TransferenciaDTO transferencia in pagosIngresadosPopUp.Transferencias)
                    {
                        Transferencia data = new Transferencia();

                        data.lineas = transferencia.lineas;
                        data.fecha = transferencia.fecha;
                        data.numeroCuenta = transferencia.numeroCuenta;
                        data.montoTarjeta = Convert.ToInt32(transferencia.montoTarjeta);
                        data.numeroComprobante = transferencia.numeroComprobante;
                        data.idBanco = transferencia.idBanco;
                        data.banco = transferencia.banco;
                        data.idBancoAndes = transferencia.idBancoAndes;
                        data.bancoAndes = transferencia.bancoAndes;
                        data.cuentaAndes = transferencia.cuentaAndes;
                        data.idAvisoPago = 0;
                        _paraSAP.transferencias.Add(data);
                    }
                }

                // Deposito
                _paraSAP.depositos = new List<Deposito>();
                if (pagosIngresadosPopUp.Depositos != null)
                {
                    foreach (TransferenciaDTO transferencia in pagosIngresadosPopUp.Depositos)
                    {
                        Deposito data = new Deposito();

                        data.lineas = transferencia.lineas;
                        data.fecha = transferencia.fecha;
                        data.numeroCuenta = transferencia.numeroCuenta;
                        data.montoTarjeta = Convert.ToInt32(transferencia.montoTarjeta);
                        data.numeroComprobante = transferencia.numeroComprobante;
                        data.idBanco = transferencia.idBanco;
                        data.banco = transferencia.banco;
                        data.idBancoAndes = transferencia.idBancoAndes;
                        data.bancoAndes = transferencia.bancoAndes;
                        data.cuentaAndes = transferencia.cuentaAndes;
                        data.idAvisoPago = 0;
                        _paraSAP.depositos.Add(data);
                    }
                }

                // Tarjetas
                _paraSAP.tarjeta = new Tarjeta();
                _paraSAP.tarjeta.tarjetas = new Tarjetas();
                if (pagosIngresadosPopUp.Tarjeta != null)
                {
                    _paraSAP.tarjeta.tarjetas.creditCard = pagosIngresadosPopUp.Tarjeta.tarjetas.creditCard;
                    _paraSAP.tarjeta.tarjetas.cardName = pagosIngresadosPopUp.Tarjeta.tarjetas.cardName;
                    _paraSAP.tarjeta.tarjetas.acctCode = pagosIngresadosPopUp.Tarjeta.tarjetas.acctCode;
                    _paraSAP.tarjeta.tarjetas.phone = "";// pagosIngresadosPopUp.Tarjeta.tarjetas.phone.ToString();
                    _paraSAP.tarjeta.tarjetas.companyId = "";// pagosIngresadosPopUp.Tarjeta.tarjetas.companyId.ToString();

                    _paraSAP.tarjeta.nroTarjeta = pagosIngresadosPopUp.Tarjeta.NroTarjeta;
                    _paraSAP.tarjeta.fechaValidoHasta = pagosIngresadosPopUp.Tarjeta.FechaValidoHasta;
                    _paraSAP.tarjeta.nroID = pagosIngresadosPopUp.Tarjeta.NroID;
                    _paraSAP.tarjeta.nroTelefono = pagosIngresadosPopUp.Tarjeta.NroTelefono;
                    _paraSAP.tarjeta.importeVencido = pagosIngresadosPopUp.Tarjeta.ImporteVencido;
                    _paraSAP.tarjeta.cantidadPagos = pagosIngresadosPopUp.Tarjeta.CantidadPagos;
                    _paraSAP.tarjeta.primerPago = pagosIngresadosPopUp.Tarjeta.PrimerPago;
                    _paraSAP.tarjeta.pagoAdicional = pagosIngresadosPopUp.Tarjeta.PagoAdicional;
                    _paraSAP.tarjeta.certificado = pagosIngresadosPopUp.Tarjeta.Certificado;
                    _paraSAP.tarjeta.claseOperacionSelect = pagosIngresadosPopUp.Tarjeta.ClaseOperacionSelect;
                }
                

                // Cheques
                _paraSAP.cheques = new List<Cheque>();
                if (pagosIngresadosPopUp.Cheques != null)
                {
                    foreach (ChequeDTO data in pagosIngresadosPopUp.Cheques)
                    {
                        Cheque cheque = new Cheque();

                        cheque.numeroCheque = data.numeroCheuqe;
                        cheque.fechaVencimiento = data.fechaVencimiento;
                        cheque.importe = Convert.ToInt32(data.importe);
                        cheque.sucursal = data.sucursal;
                        cheque.cuenta = data.cuenta;
                        cheque.idBanco = data.idBanco;
                        cheque.banco = data.banco;

                        _paraSAP.cheques.Add(cheque);
                    }
                }

                // Saldo a favor
                _paraSAP.saldos_A_Favor = new List<Saldos_A_Favor>();
                if (saldoFavorSelect != null)
                {
                    foreach (SaldosFavorDTO data in saldoFavorSelect)
                    {
                        Saldos_A_Favor saldo = new Saldos_A_Favor();

                        saldo.linea = data.linea;
                        saldo.annoProceso = data.annoProceso;
                        saldo.idOperacion = Convert.ToInt32(data.idOperacion);
                        saldo.tipoOperacionSaldo = data.tipoDocumento.ToString();
                        saldo.nroDocumentoSaldo = data.nroDocumentoSaldo.ToString();
                        saldo.fechaDocumentoSaldo = data.fechaDocumentoSaldo;
                        saldo.montoDocumentoSaldo = Convert.ToInt32(data.montoDocumentoSaldo);
                        saldo.pagarSaldo = Convert.ToInt32(data.pagarSaldo);
                        saldo.saldo = Convert.ToInt32(data.saldo);
                        saldo.check = data.check;

                        _paraSAP.saldos_A_Favor.Add(saldo);


                    }
                }

                var auxIngresaPagos = await conexion.HttpClientInstance.PostAsJsonAsync<ProcesarParaSap>($"{urlIngresaPagos}", _paraSAP);
                if (auxIngresaPagos.IsSuccessStatusCode)
                {
                    _snackBar.Add("PAGO INGRESADO", Severity.Success);
                    LimpiarTodo();
                    await BuscarCliente();
                    StateHasChanged();
                }
                else
                {
                    _snackBar.Add("ERROR - PAGO NO INGRESADO", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                _snackBar.Add($"{ex.Message}", Severity.Error);
            }
            

        }


        private async Task BuscarCliente()
        {
            var id = idCliente;
            PagosDTO pagoEntrada = new PagosDTO();
            pagoEntrada.iDCliente = id;
            var auxPendientes = await conexion.HttpClientInstance.PostAsJsonAsync<PagosDTO>($"{urlPedidoPendientes}",pagoEntrada);
            if (auxPendientes.IsSuccessStatusCode)
            {
                try
                {       
                    List<PagosClienteDTO> pagosClienteDTO = new List<PagosClienteDTO>();
                    pagosClienteDTO = JsonConvert.DeserializeObject<List<PagosClienteDTO>>(await auxPendientes.Content.ReadAsStringAsync());

                    clienteDTO = pagosClienteDTO.FirstOrDefault();  // Informacion del cliente
                    pedidosPendientePago = pagosClienteDTO.FirstOrDefault().Pedidos == null ? new List<PedidosDTO>() : pagosClienteDTO.FirstOrDefault().Pedidos; // Pedidos pendiente de pago
                    var suma = 0;
                    if(pedidosPendientePago.Count > 0)
                    {
                        foreach (var data in pedidosPendientePago)
                        {
                            suma = suma + Convert.ToInt32(data.SaldoAFecha);
                            deudaTotal = suma.ToString("n0") + " CLP";
                            if (data.SiAutorizaDespacho == 1)
                            {
                                data.Check = true;
                            }
                        }

                        facturaDTO = pedidosPendientePago.FirstOrDefault().Documetos; //  Facturas/Boletas 
                    }
                    

                    
                    saldoFavor = pagosClienteDTO.FirstOrDefault().Saldos_A_Favors == null ? new List<SaldosFavorDTO>() : pagosClienteDTO.FirstOrDefault().Saldos_A_Favors; // Saldos a favor

                    // Fechas 
                    dateContabilizacion = DateTime.Parse(clienteDTO.FechaContabilizacion);
                    dateVencimiento = DateTime.Parse(clienteDTO.FechaVencimiento);
                    dateDocumento = DateTime.Parse(clienteDTO.FechaDocumento);

                    //Razon Social
                    razonSocial = clienteDTO.RazonSocial;

                    // Limpar variables -> Posible funcion
                    pagosIngresadosPopUp = new PagosIngresadosDTO(); // Lista de tipos de pagos ingresados
                    pedidosPendientePagoSelected = new List<PedidosDTO>(); // Lista de pedidos para liberar y/o pagar
                    saldoFavorSelect = new List<SaldosFavorDTO>(); // Lista de saldos a favor selecionados
                    nroCobranza = "";
                    referencia = "";
                    comentarios = "";
                    sumaImporte = "0";
                    totalPago = "0";
                    saldoPendiente = "0";

                    

                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error deserealizar lista de pagos", Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar lista de pagos", Severity.Error);
            }
            StateHasChanged();
        }

        // Seleccionar para despachar
        public void RowCellClicked(PedidosDTO data)
        {
            if(data.SiAutorizaDespacho == 0)
            {
                data.Check = !data.Check;

                if (data.Check)
                {
                    if (pedidosPendientePagoSelected.Exists(x => x.IDOperacion == data.IDOperacion))
                    {
                        var index = pedidosPendientePagoSelected.FindIndex(x => x.IDOperacion == data.IDOperacion);
                        if (index > -1)
                        {
                            pedidosPendientePagoSelected[index].Check = true;
                        }
                    }
                    else
                    {
                        pedidosPendientePagoSelected.Add(data);
                    }

                }
                else
                {
                    if (pedidosPendientePagoSelected.Exists(x => x.IDOperacion == data.IDOperacion))
                    {
                        var index = pedidosPendientePagoSelected.FindIndex(x => x.IDOperacion == data.IDOperacion);
                        if (index > -1)
                        {
                            if (pedidosPendientePagoSelected[index].ValorAPagar > 0)
                            {
                                pedidosPendientePagoSelected[index].Check = false;
                            }
                            else
                            {
                                pedidosPendientePagoSelected.Remove(data);
                            }

                        }

                    }

                }
            }
                        
        }
        // Ingresa montos a pagar
        private void CommittedItemChanges(PedidosDTO item)
        {
            

            if (item.ValorAPagar > item.SaldoAFecha)
            {
                item.ValorAPagar = 0;
                _snackBar.Add("El monto no puede superar el total del pedido");
            }
            else if (item.ValorAPagar < 0) {
                item.ValorAPagar = 0;
                _snackBar.Add("El monto no puede ser negativo");
            }
            else
            {
                if (pedidosPendientePagoSelected.Exists(x => x.IDOperacion == item.IDOperacion))
                {
                    pedidosPendientePagoSelected.Remove(item);
                }
                var aux = item.ValorAPagar;
                item.SaldoNuevo = (item.SaldoAFecha - aux);

                pedidosPendientePagoSelected.Add(item);


                // Calculo Importe
                CalculoImporte();
                
            }
            
        }


        // Seleccionar para usar
        public void RowCellClickedSaldoFavor(SaldosFavorDTO data)
        {
            data.check = !data.check;
            if (data.check)
            {
                saldoFavorSelect.Add(data);
            }
            else
            {   
                if (saldoFavorSelect.Exists(x => x.idOperacion == data.idOperacion))
                {
                    saldoFavorSelect.Remove(data);
                    
                }

                if(saldoFavor.Exists(x => x.idOperacion == data.idOperacion))
                {
                    var index = saldoFavor.FindIndex(x => x.idOperacion == data.idOperacion);
                    if (index > -1)
                    {
                        saldoFavor[index].pagarSaldo = 0;
                        switch (saldoFavor[index].tipoDocumento)
                        {
                            case 56:
                                saldoFavor[index].saldo = saldoFavor[index].debe;
                                break;
                            case 61:
                                saldoFavor[index].saldo = saldoFavor[index].haber;
                                break;
                            case 11:
                                saldoFavor[index].saldo = saldoFavor[index].haber;
                                break;
                            case 14:
                                saldoFavor[index].saldo = saldoFavor[index].debe;
                                break;
                            case 8:
                                saldoFavor[index].saldo = saldoFavor[index].haber;
                                break;

                        }
                        
                    }
                }
            }
            StateHasChanged();
            CalculoImporte();
        }
        // Ingresar monto a usar 
        private void CommittedItemChangesFavor(SaldosFavorDTO item)
        {
            if (item.pagarSaldo < 0)
            {
                item.pagarSaldo = 0;
                _snackBar.Add("El monto no puede ser negativo");
            }
            else
            {
                switch (item.tipoDocumento)
                {
                    case 56:    // Debe
                        if (item.pagarSaldo > item.debe)
                        {
                            item.pagarSaldo = 0;
                            item.saldo = item.debe;
                            _snackBar.Add("El monto no puede superar el total del documento");
                        }
                        else
                        {
                            var aux = item.pagarSaldo;
                            item.saldo = (item.debe - aux); // SUMA A LA DEUDA 

                            //  Calculo Importe
                            CalculoImporte();
                        }
                        break;
                    case 61:    // Haber
                        if (item.pagarSaldo > item.haber)
                        {
                            item.pagarSaldo = 0;
                            item.saldo = item.haber;
                            _snackBar.Add("El monto no puede superar el total del documento");
                        }
                        else
                        {
                            var aux = item.pagarSaldo;
                            item.saldo = (item.haber - aux); // SUMA A LA DEUDA 

                            //  Calculo Importe
                            CalculoImporte();
                        }
                        break;

                    case 11:    // Haber
                        if (item.pagarSaldo > item.haber)
                        {
                            item.pagarSaldo = 0;
                            item.saldo = item.haber;
                            _snackBar.Add("El monto no puede superar el total del documento");
                        }
                        else
                        {
                            var aux = item.pagarSaldo;
                            item.saldo = (item.haber - aux); // SUMA A LA DEUDA 

                            //  Calculo Importe
                            CalculoImporte();
                        }
                        break;
                    case 14:    // Debe
                        if (item.pagarSaldo > item.debe)
                        {
                            item.pagarSaldo = 0;
                            item.saldo = item.debe;
                            _snackBar.Add("El monto no puede superar el total del documento");
                        }
                        else
                        {
                            var aux = item.pagarSaldo;
                            item.saldo = (item.debe - aux); // SUMA A LA DEUDA 

                            //  Calculo Importe
                            CalculoImporte();
                        }
                        break;
                    case 8:     // Haber
                        if (item.pagarSaldo > item.haber)
                        {
                            item.pagarSaldo = 0;
                            item.saldo = item.haber;
                            _snackBar.Add("El monto no puede superar el total del documento");
                        }
                        else
                        {
                            var aux = item.pagarSaldo;
                            item.saldo = (item.haber - aux); // SUMA A LA DEUDA 

                            //  Calculo Importe
                            CalculoImporte();
                        }
                        break;

                }

                CalculoImporte();
            }
            
           

        }

        private void CalculoImporte()
        {
            int calculoSaldoFavorDebe = 0;
            int calculoSaldoFavorHaber = 0;
            int calculoImporte = 0;

            foreach (SaldosFavorDTO data in saldoFavorSelect)
            {
                if (data.tipoDocumento == 56 || data.tipoDocumento ==  14)
                {
                    calculoSaldoFavorDebe = calculoSaldoFavorDebe + Convert.ToInt32(data.pagarSaldo);
                }
                if (data.tipoDocumento == 61 || data.tipoDocumento == 11 | data.tipoDocumento == 8)
                {
                    calculoSaldoFavorHaber = calculoSaldoFavorHaber + Convert.ToInt32(data.pagarSaldo);
                }
                //calculoSaldoFavor = calculoSaldoFavor + Convert.ToInt32(data.pagarSaldo);
            }

            foreach (PedidosDTO data in pedidosPendientePago)
            {
                calculoImporte = calculoImporte + Convert.ToInt32(data.ValorAPagar);
            }

            sumaImporte = ((calculoImporte + calculoSaldoFavorDebe) - calculoSaldoFavorHaber).ToString("n0");
            clienteDTO.SumaImporte = ((calculoImporte + calculoSaldoFavorDebe) - calculoSaldoFavorHaber);
            CalculoSaldoPendiente();
        }
        
        private void CalculoSaldoPendiente()
        {

            saldoPendiente = (Convert.ToDouble(totalPago) - Convert.ToDouble(sumaImporte)).ToString("n0");
            _procesarPago = true;
            if (Convert.ToDouble(saldoPendiente) >= 0)
            {
                _procesarPago = false;
            }
        }


        private void LimpiarTodo()
        {
            pagosIngresadosPopUp = new PagosIngresadosDTO();
            razonSocial     = "";
            deudaTotal      = "";
            referencia      = "";
            nroCobranza     = "";
            comentarios     = "";
            sumaImporte     = "";
            totalPago       = "";
            saldoPendiente  = "";

            clienteDTO = new PagosClienteDTO();
            pedidosPendientePago = new List<PedidosDTO>();
            pedidosPendientePagoSelected = new List<PedidosDTO>();
            facturaDTO = new List<Documeto>();
            saldoFavor = new List<SaldosFavorDTO>();
            saldoFavorSelect = new List<SaldosFavorDTO>();

            checkPago = false;
            _procesarPago = false;

        }

    }
}


//private void SelectedItemsChanged(HashSet<PedidosDTO> items)
//{
//    IEnumerable<PedidosDTO> firstDiffSecond = pedidosPendientePago.Except(items.ToList());
//    List<PedidosDTO> lista = new List<PedidosDTO>();
//    lista = firstDiffSecond.ToList();

//    foreach (PedidosDTO data in lista)
//    {
//        var index = pedidosPendientePago.IndexOf(data);
//        if (Convert.ToDouble(sumaImporte) > 0)
//        {
//            sumaImporte = (Convert.ToDouble(sumaImporte) - pedidosPendientePago[index].ValorAPagar).ToString("n0");
//            clienteDTO.SumaImporte = Convert.ToInt32(Convert.ToDouble(sumaImporte));
//        }
//        pedidosPendientePago[index].ValorAPagar = 0;

//    }
//    saldoPendiente = (Convert.ToDouble(pagosIngresadosPopUp.TotalPago) - Convert.ToDouble(sumaImporte)).ToString("n0");
//    pedidosPendientePagoSelected = new List<PedidosDTO>();
//    pedidosPendientePagoSelected = items.ToList();

//}

//private void SelectedItemsChangedFavor(HashSet<SaldosFavorDTO> items)
//{
//    IEnumerable<SaldosFavorDTO> firstDiffSecond = saldoFavor.Except(items.ToList());
//    List<SaldosFavorDTO> lista = new List<SaldosFavorDTO>();
//    lista = firstDiffSecond.ToList();

//    foreach (SaldosFavorDTO data in lista)
//    {
//        var index = saldoFavor.IndexOf(data);
//        if (Convert.ToDouble(sumaImporte) > 0)
//        {
//            sumaImporte = (Convert.ToDouble(sumaImporte) + saldoFavor[index].pagarSaldo).ToString("n0");
//            clienteDTO.SumaImporte = Convert.ToInt32(Convert.ToDouble(sumaImporte));
//        }
//        saldoFavor[index].pagarSaldo = 0;
//    }
//    saldoPendiente = (Convert.ToDouble(pagosIngresadosPopUp.TotalPago) + Convert.ToDouble(sumaImporte)).ToString("n0");
//    saldoFavorSelect = new List<SaldosFavorDTO>();
//    saldoFavorSelect = items.ToList();

//}
