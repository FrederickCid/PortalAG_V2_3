using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.Pagos;
using PortalAG_V2.Shared.Services;
using Radzen.Blazor.Rendering;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortalAG_V2.Pages.Pagos
{
    partial class FormasPagos
    {
        private ClientFactory conexion;
        private const string urlBancos = "api/v2/Pagos/Bancos";
        private const string urlTarjetas = "api/v2/Pagos/Tarjeta";
        private const string urlFromasPago = "api/v2/Pagos/FormaDePago";
        private const string urlBancoAndes = "api/v2/AvisodePagos/ConsultaBancosAndes";

        public string idCliente { get; set; }
        public string razonSocial { get; set; }
        public string montoPagar { get; set; } 
        public int montoLimite { get; set; } 
        public string TextValue { get; set; }

        public bool _tarjetas = false;
        public bool _cheques = false;
        public bool _transferencia = false;
        public bool _deposito = false;
        DateTime? date1 = DateTime.Today;
        DateTime current_year = DateTime.Now;
        public int year { get; set; } = 0;


        public IEnumerable<PagosDTO> Elements = new List<PagosDTO>();
        public PagosDTO _searchString = new PagosDTO();
        public List<BancosDTO> listBancos = new List<BancosDTO>();
        public List<BancosAndesDTO> listBancosAndes = new List<BancosAndesDTO>();
        public List<TarjetasDTO> listTarjetas = new List<TarjetasDTO>();
        public List<FormaPagoDTO> listFormaPago = new List<FormaPagoDTO>();

        // Efectivo
        public int _monto { get; set; }
        DateTime? fechaEfectivo = DateTime.Today;

        // Transferencia
        DateTime? fechaTransferencia = DateTime.Today;
        public BancosDTO bancoSelect = new BancosDTO();
        public BancosAndesDTO bancoAndesSelect = new BancosAndesDTO();
        public int _montoTransferencia { get; set; } = 0;
        public int _nroCuentaTransferencia { get; set; } = 0;
        public int _nroComprobanteTransferencia { get; set; } = 0;
        public List<TransferenciaDTO> _listTransferencia = new List<TransferenciaDTO>();

        // Depositos
        DateTime? fechaDeposito = DateTime.Today;
        public BancosDTO bancoSelectDeposito = new BancosDTO();
        public int _montoDeposito { get; set; } = 0;
        public int _nroCuentaDeposito { get; set; } = 0;
        public int _nroComprobanteDeposito { get; set; } = 0;
        public List<TransferenciaDTO> _listDeposito = new List<TransferenciaDTO>();

        // Tarjetas
        public TarjetasDTO tarjetaSelect = new TarjetasDTO();
        public FormaPagoDTO formaPagoSelect = new FormaPagoDTO();
        DateTime? fechaValidoHasta = DateTime.Today;
        public string _nroCuentaMayor { get; set; } = "";
        public string _nroTarjeta { get; set; } = "";
        public int _nroID { get; set; } = 0;
        public string _nroTelefono { get; set; } = "";
        public string _certificado { get; set; } = "";
        public int _importeVencido { get; set; } = 0;
        public int _cantidadPagos { get; set; } = 0;
        public int _primerPago { get; set; } = 0;
        public int _pagoAdicional { get; set; } = 0;
        public List<string> _listClaseOperacion = new List<string>();
        public string claseOperacionSelect { get; set; } = "";

        // Cheques
        List<CuentaDTO> listCuentas = new List<CuentaDTO>();
        CuentaDTO cuentaSelect = new CuentaDTO();
        DateTime? fechaVencimientoCheque = DateTime.Today;
        public int _importe { get; set; } = 0;
        public int _nrocuentaCheque { get; set; } = 0;
        public int _nroCheque { get; set; } = 0;
        public string _sucursal { get; set; } = "";
        BancosDTO bancoChqueSelect = new BancosDTO();
        public List<ChequeDTO> _listCheques = new List<ChequeDTO>();

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public AppState appSatate { get; set; }
        [Parameter]
        public PagosClienteDTO args { get; set; }
        [Parameter]
        public PagosIngresadosDTO pagosIngresadosEntrada { get; set; }
        [Parameter]
        public bool pagoAnticipado { get; set; }

        public PagosIngresadosDTO pagosIngresadosSalida { get; set; } = new PagosIngresadosDTO();

        protected override async Task OnInitializedAsync()
        {
            conexion = new MainServices().Pagos;
            year = current_year.Year;
            // Datos que viajan entre popUp y page
            if(pagosIngresadosEntrada != null)
            {
                pagosIngresadosSalida = pagosIngresadosEntrada;
                if(pagosIngresadosEntrada.Efectivo != null)
                {
                    if (pagosIngresadosEntrada.Efectivo.MontoEfectivo > 0)
                    {
                        //_tarjetas = true;
                        //_cheques = true;
                        _monto = pagosIngresadosEntrada.Efectivo.MontoEfectivo;
                    }
                   
                }
                if (pagosIngresadosEntrada.Transferencias != null)
                {
                    if (pagosIngresadosEntrada.Transferencias.Count > 0)
                    {
                        _listTransferencia = pagosIngresadosEntrada.Transferencias;
                        //_tarjetas = true;
                        //_cheques = true;
                        //_deposito = true;
                    }
                }
                if (pagosIngresadosEntrada.Depositos != null)
                {
                    if (pagosIngresadosEntrada.Depositos.Count > 0)
                    {
                        _listDeposito = pagosIngresadosEntrada.Depositos;
                        //_tarjetas = true;
                        //_cheques = true;
                        //_transferencia = true;
                    }
                }
                if (pagosIngresadosEntrada.Tarjeta != null)
                {

                }
                if (pagosIngresadosEntrada.Cheques != null)
                {
                    if (pagosIngresadosEntrada.Cheques.Count > 0)
                    {
                        _listCheques = pagosIngresadosEntrada.Cheques;
                    }
                }
                
            }
            else
            {
                pagosIngresadosSalida = new PagosIngresadosDTO();
            }

            if (pagoAnticipado)
            {
                _tarjetas = true;
                _cheques = true;
            }

            // Cabecera
            idCliente = args.IDCliente.ToString();
            razonSocial = args.RazonSocial.ToString();
            montoPagar =  args.SumaImporte.ToString("n0");
            montoLimite = Convert.ToInt32(Convert.ToDouble(montoPagar));
            _importeVencido = montoLimite;

            // Bancos
            bancoSelect = new BancosDTO();
            var auxBancos = await conexion.HttpClientInstance.GetAsync($"{urlBancos}");
            if(auxBancos.IsSuccessStatusCode)
            {
                listBancos = JsonConvert.DeserializeObject<List<BancosDTO>>(await auxBancos.Content.ReadAsStringAsync());
            }

            bancoAndesSelect = new BancosAndesDTO();
            var auxBancosAndes = await conexion.HttpClientInstance.GetAsync($"{urlBancoAndes}");
            if (auxBancosAndes.IsSuccessStatusCode)
            {
                listBancosAndes = JsonConvert.DeserializeObject<List<BancosAndesDTO>>(await auxBancosAndes.Content.ReadAsStringAsync());
            }

            // Tarjetas
            tarjetaSelect = new TarjetasDTO();
            var auxTarjeta = await conexion.HttpClientInstance.GetAsync($"{urlTarjetas}");
            if(auxTarjeta.IsSuccessStatusCode)
            {
                listTarjetas = JsonConvert.DeserializeObject<List<TarjetasDTO>>(await auxTarjeta.Content.ReadAsStringAsync());
            }

            // Formas de pagos
            formaPagoSelect = new FormaPagoDTO();
            var auxFormaPagos = await conexion.HttpClientInstance.GetAsync($"{urlFromasPago}");
            if (auxFormaPagos.IsSuccessStatusCode)
            {
                listFormaPago = JsonConvert.DeserializeObject<List<FormaPagoDTO>>(await auxFormaPagos.Content.ReadAsStringAsync());
            }

            _listClaseOperacion = new List<string>() { "Regular", "Pago por Internet", "Otro medio de pago"};

            // Cheques 
            listCuentas = new List<CuentaDTO>();
            listCuentas.Add(new CuentaDTO(11010301, "BCO SANTANDER"));
            listCuentas.Add(new CuentaDTO(11010302, "BCO SCOTIABANK"));

        }

        private void IngresoMontoEfectivo()
        {
            if(_monto > 0)
            {
                //_tarjetas = true;
                //_cheques = true;
                if(pagosIngresadosSalida.Efectivo != null)
                {
                    if(pagosIngresadosSalida.Efectivo.MontoEfectivo > 0)
                    {
                        pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago - pagosIngresadosSalida.Efectivo.MontoEfectivo;
                    }
                }
                pagosIngresadosSalida.Efectivo = new EfectivoPago();
                pagosIngresadosSalida.Efectivo.MontoEfectivo = _monto;
                pagosIngresadosSalida.Efectivo.FechaEfectivo = fechaEfectivo.Value.ToString();
                pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago + _monto;
                //args.TotalPago = args.TotalPago + _monto;
            }
            else
            {
                //_tarjetas = false;
                //_cheques = false;
                pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago - pagosIngresadosSalida.Efectivo.MontoEfectivo;
                pagosIngresadosSalida.Efectivo = null;
                
                //args.TotalPago = args.TotalPago - _monto;

            }

        }

        public async Task SeleccionBanco(BancosDTO data)
        {
            bancoSelect = data;

        }

        public async Task SeleccionBancoAndes(BancosAndesDTO data)
        {
            bancoAndesSelect = data;

        }

        public async Task SeleccionBancoDeposito(BancosDTO data)
        {
            bancoSelectDeposito = data;

        }

        public async Task SeleccionTarjeta(TarjetasDTO data)
        {
            tarjetaSelect = data;
            _nroCuentaMayor = data.acctCode;
            //_deposito = true;
            //_cheques = true;
            //_transferencia = true;
        }

        public void AgregarTransferencia()
        {
            if(bancoSelect != null && bancoSelect.descripcion != "" )
            {
                if (_montoTransferencia != 0)
                {
                    if (_nroCuentaTransferencia != 0)
                    {
                        if (_nroComprobanteTransferencia != 0)
                        {
                            if(bancoAndesSelect != null && bancoAndesSelect.descripcion != "")
                            {
                                TransferenciaDTO transferencia = new TransferenciaDTO();
                                transferencia.lineas = _listTransferencia.Count + 1;
                                transferencia.fecha = fechaTransferencia.Value;
                                transferencia.idBanco = bancoSelect.idBanco;
                                transferencia.banco = bancoSelect.descripcion;
                                transferencia.montoTarjeta = _montoTransferencia;
                                transferencia.numeroCuenta = _nroCuentaTransferencia;
                                transferencia.numeroComprobante = _nroComprobanteTransferencia;
                                transferencia.idBancoAndes = bancoAndesSelect.idBanco;
                                transferencia.bancoAndes = bancoAndesSelect.descripcion;
                                transferencia.cuentaAndes = bancoAndesSelect.cuentaCorriete;

                                _listTransferencia.Add(transferencia);
                                pagosIngresadosSalida.Transferencias = _listTransferencia;
                                pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago + Convert.ToInt32(transferencia.montoTarjeta);
                                //args.TotalPago = args.TotalPago + _montoTransferencia;
                                bancoSelect = new BancosDTO();
                                _montoTransferencia = 0;
                                _nroComprobanteTransferencia = 0;
                                _nroCuentaTransferencia = 0;
                                //_tarjetas = true;
                                //_cheques = true;
                                //_deposito = true;
                                
                            }
                            else
                            {
                                _snackBar.Add("Debe selecionar banco andes", Severity.Warning);
                            }
                            

                        }
                        else
                        {
                            _snackBar.Add("Debe ingresar nro de comprobante", Severity.Warning);
                        }
                    }
                    else
                    {
                        _snackBar.Add("Debe ingresar nro de cuenta", Severity.Warning);
                    }
                }
                else
                {
                    _snackBar.Add("Debe ingresar un monto", Severity.Warning);
                }
            }
            else
            {
                _snackBar.Add("Debeselecionar un banco", Severity.Warning);
            }
        }

        public void BorrarTransferencia(TransferenciaDTO data)
        {
            if (data != null)
            {
                if(_listTransferencia.Exists(x => x.numeroComprobante == data.numeroComprobante))
                {
                    _listTransferencia.Remove(data);
                    pagosIngresadosSalida.Transferencias = _listTransferencia;
                    pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago - Convert.ToInt32(data.montoTarjeta);

                    //args.TotalPago = args.TotalPago - Convert.ToInt32(data.montoTarjeta);
                    StateHasChanged();
                }
            }
        }

        public void AgregarDeposito()
        {
            if (bancoSelectDeposito != null && bancoSelectDeposito.descripcion != "")
            {
                if (_montoDeposito != 0)
                {
                    if (_nroCuentaDeposito != 0)
                    {
                        if (_nroComprobanteDeposito != 0)
                        {
                            if (bancoAndesSelect != null && bancoAndesSelect.descripcion != "")
                            {
                                TransferenciaDTO transferencia = new TransferenciaDTO();
                                transferencia.lineas = _listDeposito.Count + 1;
                                transferencia.fecha = fechaDeposito.Value;
                                transferencia.idBanco = bancoSelectDeposito.idBanco;
                                transferencia.banco = bancoSelectDeposito.descripcion;
                                transferencia.montoTarjeta = _montoDeposito;
                                transferencia.numeroCuenta = _nroCuentaDeposito;
                                transferencia.numeroComprobante = _nroComprobanteDeposito;
                                transferencia.idBancoAndes = bancoAndesSelect.idBanco;
                                transferencia.bancoAndes = bancoAndesSelect.descripcion;
                                transferencia.cuentaAndes = bancoAndesSelect.cuentaCorriete;

                                _listDeposito.Add(transferencia);
                                pagosIngresadosSalida.Depositos = _listDeposito;
                                pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago + Convert.ToInt32(transferencia.montoTarjeta);
                                //args.TotalPago = args.TotalPago + _montoDeposito;
                                bancoSelectDeposito = new BancosDTO();
                                _montoDeposito = 0;
                                _nroCuentaDeposito = 0;
                                _nroComprobanteDeposito = 0;
                                //_tarjetas = true;
                                //_cheques = true;
                                //_transferencia = true;
                               
                            }
                            else
                            {
                                _snackBar.Add("Debe selecionar banco andes", Severity.Warning);
                            }


                        }
                        else
                        {
                            _snackBar.Add("Debe ingresar nro de comprobante", Severity.Warning);
                        }
                    }
                    else
                    {
                        _snackBar.Add("Debe ingresar nro de cuenta", Severity.Warning);
                    }
                }
                else
                {
                    _snackBar.Add("Debe ingresar un monto", Severity.Warning);
                }
            }
            else
            {
                _snackBar.Add("Debeselecionar un banco", Severity.Warning);
            }
        }

        public void BorrarDeposito(TransferenciaDTO data)
        {
            if (data != null)
            {
                if (_listDeposito.Exists(x => x.idBanco == data.idBanco))
                {
                    _listDeposito.Remove(data);
                    pagosIngresadosSalida.Depositos = _listDeposito;
                    pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago - Convert.ToInt32(data.montoTarjeta);
                    //args.TotalPago = args.TotalPago - Convert.ToInt32(data.montoTarjeta);
                    StateHasChanged();
                }
            }
        }

        public void CalculoPagosParciales()
        {
            if(_importeVencido > 0)
            {
                if (_cantidadPagos > 0)
                {
                    _primerPago = (_importeVencido/_cantidadPagos);
                    pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago + _importeVencido;
                    //args.TotalPago = args.TotalPago - _importeVencido;
                }
                else
                {
                    _snackBar.Add("Cantidad de pagos no valido", Severity.Warning);
                }
            }
            else
            {
                _snackBar.Add("Importe vencido no valido",Severity.Warning);
            }
        }

        public void SeleccionClaseOperacion(string data)
        {
            claseOperacionSelect = data;
        }

        public void SeleccionCuenta(CuentaDTO data)
        {
            cuentaSelect = data;
        }

        public async Task SeleccionBancoCheque(BancosDTO data)
        {
            bancoChqueSelect = data;

        }

        public void AgregarCheque()
        {
            if (cuentaSelect != null)
            {
                if (_importe != 0)
                {
                    if (bancoChqueSelect != null)
                    {
                        if (_nrocuentaCheque != 0)
                        {
                            if (_nroCheque != 0)
                            {

                                ChequeDTO cheque = new ChequeDTO();
                                cheque.numeroCheuqe = _nroCheque;
                                cheque.fechaVencimiento = fechaVencimientoCheque.Value;
                                cheque.importe = _importe;
                                cheque.sucursal = _sucursal;
                                cheque.cuenta = _nrocuentaCheque.ToString();
                                cheque.banco = bancoChqueSelect.descripcion;
                                cheque.idBanco = bancoChqueSelect.idBanco;

                                _listCheques.Add(cheque);
                                pagosIngresadosSalida.Cheques = _listCheques;
                                pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago + Convert.ToInt32(cheque.importe);
                                //args.TotalPago = args.TotalPago + _importe;
                                //_nrocuentaCheque = 0;
                                _nroCheque = _nroCheque + 1;
                                //_importe = 0;
                                //_sucursal = "";
                                //_nrocuentaCheque = 0;
                                //bancoChqueSelect = new BancosDTO();
                                fechaVencimientoCheque = fechaVencimientoCheque.Value.AddMonths(1);
                                

                            }
                            else
                            {
                                _snackBar.Add("Debe ingresar nro de cheque", Severity.Warning);
                            }
                                

                        }
                        else
                        {
                            _snackBar.Add("Debe ingresar nro de cuenta", Severity.Warning);
                        }
                    }
                    else
                    {
                        _snackBar.Add("Debe selecionar un banco", Severity.Warning);
                    }
                }
                else
                {
                    _snackBar.Add("Debe ingresar importe", Severity.Warning);
                }
            }
            else
            {
                _snackBar.Add("Debe selecionar cuenta mayor ", Severity.Warning);
            }
        }

        public void BorrarCheque(ChequeDTO data)
        {
            if (data != null)
            {
                if (_listCheques.Exists(x => x.numeroCheuqe == data.numeroCheuqe))
                {
                   _listCheques.Remove(data);
                    pagosIngresadosSalida.Cheques = _listCheques;
                    pagosIngresadosSalida.TotalPago = pagosIngresadosSalida.TotalPago - Convert.ToInt32(data.importe);
                    StateHasChanged();
                }
            }
        }
        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void Aceptar()
        {

            if (pagosIngresadosSalida.TotalPago < args.SumaImporte)
            {
                _snackBar.Add("El suma total de importes no puede ser menor al monto a pagar ", Severity.Error);
            }
            else
            {
                //Confirmacion datos tarjeta
                if (_primerPago > 0)
                {
                    if (_nroCuentaMayor != "")
                    {
                        if (_nroTarjeta != "")
                        {
                            if (_nroID > 0)
                            {
                                if (_cantidadPagos > 0)
                                {
                                    pagosIngresadosSalida.Tarjeta = new TarjetasPago();

                                    pagosIngresadosSalida.Tarjeta.tarjetas = tarjetaSelect;
                                    pagosIngresadosSalida.Tarjeta.NroTarjeta = Convert.ToInt32(_nroCuentaMayor);
                                    pagosIngresadosSalida.Tarjeta.FechaValidoHasta = fechaValidoHasta.Value;
                                    pagosIngresadosSalida.Tarjeta.NroID = _nroID;
                                    pagosIngresadosSalida.Tarjeta.NroTelefono = _nroTelefono;
                                    pagosIngresadosSalida.Tarjeta.ImporteVencido = _importeVencido;
                                    pagosIngresadosSalida.Tarjeta.CantidadPagos = _cantidadPagos;
                                    pagosIngresadosSalida.Tarjeta.PrimerPago = _primerPago;
                                    pagosIngresadosSalida.Tarjeta.PagoAdicional = _pagoAdicional;
                                    pagosIngresadosSalida.Tarjeta.Certificado = _certificado;
                                    pagosIngresadosSalida.Tarjeta.ClaseOperacionSelect = claseOperacionSelect;
                                    _snackBar.Add("Bien", Severity.Success);
                                    MudDialog.Close(DialogResult.Ok(pagosIngresadosSalida));

                                }
                                else
                                {
                                    _snackBar.Add("Debe ingresar cantidades de pago", Severity.Error);
                                }
                            }
                            else
                            {
                                _snackBar.Add("Debe ingresar numero ID de tarjeta", Severity.Error);
                            }
                        }
                        else
                        {
                            _snackBar.Add("Debe ingresar numero de tarjeta", Severity.Error);
                        }
                    }
                    else
                    {
                        _snackBar.Add("Debe selecionar un tipo de tarjeta", Severity.Error);
                    }
                }
                else
                {
                    _snackBar.Add("Bien", Severity.Success);
                    MudDialog.Close(DialogResult.Ok(pagosIngresadosSalida));
                }
                
               
            }
        }

        Func<BancosDTO, string> converterBancos = p => p.descripcion;
        Func<BancosAndesDTO, string> converterBancosAndes = p => p.descripcion + p.cuentaCorriete;
        Func<TarjetasDTO, string> converterTarjeta = p => p.cardName;
        Func<CuentaDTO, string> converterCuenta = p => p.Descripcion;
    }
}
