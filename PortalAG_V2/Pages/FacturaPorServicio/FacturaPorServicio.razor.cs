using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;
using DocumentFormat.OpenXml.Office2013.Excel;
using Microsoft.Extensions.Logging.Abstractions;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes;
using PortalAG_V2.Pages.Pagos;
using PortalAG_V2.Shared.Model.FacturaPorServicio;
using PortalAG_V2.Shared.Model.FacturaPorServicio.Response;
using PortalAG_V2.Shared.Model.Formularios;
using PortalAG_V2.Shared.Model.NotaDeCredito;
using PortalAG_V2.Shared.Model.Pagos;
using PortalAG_V2.Shared.Services;
using SAPB1_Class.RequestSB1;
using SAPB1_Class.ResponseSB1;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Json;
using Documentline = SAPB1_Class.RequestSB1.InvoicesRequest.Documentline;

namespace PortalAG_V2.Pages.FacturaPorServicio
{
    public partial class FacturaPorServicio
    {

        Loading? loading;
        MudForm? form;
        bool success = true;
        string[] errors = { };

        public MudRadio<string>? Factura;
        public MudRadio<string>? Boleta;

        public string? formaPago;
        public string? condicionPago;

        MainServices service;

        #region Variables Globales

        public string? CodigoCliente { get; set; }
        //public string? Digito { get; set; }
        public string? RazonSocial { get; set; }
        public string? TextoUno { get; set; }
        public string? TextoDos { get; set; }
        public string? TextoTres { get; set; }
        public int CuentaContable { get; set; } = 41010999;

        public string? CondicionPagoCombo { get; set; }
        public string? FormaPagoCombo { get; set; }
        public int cantidadLetras { get; set; }
        public string? Localidad { get; set; } = "";
        public string? NroOC { get; set; } = "";
        public string? NroFactura { get; set; } = "0";
        public string? OptionFacturaBoleta { get; set; } = "Factura";
        public DateTime? FechaVencimiento { get; set; } = DateTime.Now;

        public double Neto { get; set; }
        public double DCto { get; set; }
        public double Iva { get; set; }
        public double Total { get; set; }
        public double SubTotal { get; set; }

        public string? porcentaje { get; set; }
        public double valorPorcentaje { get; set; }
        public DateTime? Fecha { get; set; } = DateTime.Now;
        public bool Visibility { get; set; } = false;
        bool Termino = false;
        public int CuentaContableRemplazo { get; set; }

        public int nroDocPop = 0;

        public bool facturaDisable = false;
        public bool busquedaNro = false;


        #endregion

        DialogOptions dialogOptions = new() { FullWidth = true };

        ConsultaFormaPagoModel FormaPagos = new ConsultaFormaPagoModel();
        List<ConsultaFormaPagoModel> listaFormaPagos = new List<ConsultaFormaPagoModel>();
        CondicionPagoVentasModel CondicionPagos = new CondicionPagoVentasModel();
        List<CondicionPagoVentasModel> listaCondicionPagos = new List<CondicionPagoVentasModel>();
        ClienteFacturaPorServicioModel cliente = new ClienteFacturaPorServicioModel();
        private List<RequestDetalleFS> Detalle = new List<RequestDetalleFS>();

        #region Url

        private string UrlCrearCabeceraFS = "api/v2/FacturaPorServicio/CrearCabeceraFS/";
        private string UrlCrearDetalleFS = "api/v2/FacturaPorServicio/CrearDetalleFS/";
        string UrlObtenerCliente = "api/v2/FacturaPorServicio/ObtenerClienteFacturaServico";
        private string UrlFormaDePago = "api/v1/Cliente/listaFormaPago";
        private string UrlCondicionDePago = "api/v1/Cliente/listaCondicionDePago";

        private string urlConsultarNroFactura = "api/v2/FacturaPorServicio/BusquedaNroFactura";
        private string urlGenerarFactura = "api/v2/FacturaPorServicio/EnvioSAP";
        private string urlActualizaCampoDocEntry = "api/v2/FacturaPorServicio/ActualizaCampoDocEntry";
        private string urlImpresionDTE = "api/v2/FacturaPorServicio/ImpresionDTE";
        #endregion

        protected override async Task OnInitializedAsync()
        {
            service = new MainServices();
            await ComboboxFormaDePago();
            await ComboboxCondicionDePago();
            await Task.Delay(0);
        }

        #region Procesar Factura por Servicio

        private async Task ProcesarFacturaPorServicio()
        {
            loading?.Abrir();
            var user = await _authenticationManager.CurrentUser();

            if (RazonSocial != "")
            {
                if (CondicionPagos.descripcion != "" && FormaPagos.descripcion != "")
                {
                    if (TextoUno != "")
                    {
                        if (SubTotal > 0)
                        {
                            RequestCabeceraFS cabecera = new RequestCabeceraFS
                            {
                                AnnoProceso = DateTime.Now.Year,
                                Correlativo = 1,
                                IDTipoOperacion = OptionFacturaBoleta!.Equals("Factura") ? 33 : 39,
                                Fecha = Fecha!.Value.ToString("yyyyMMdd"),
                                IDEstado = 1,
                                IDCliente = CodigoCliente,
                                Neto = Neto,
                                IVAFecha = 19,
                                IVA = Iva,
                                Total = Total,
                                TotalFactura = Total,
                                FechaDocumento = Fecha!.Value.ToString("yyyyMMdd"),
                                IDFormaPago = FormaPagos.idFormaPago,
                                FormaPago = FormaPagos.descripcion,
                                IDVendedor = 0,
                                Vendedor = user.GetUserId(),
                                IDTransporte = 119,
                                Transporte = "DESPACHO ANDES",
                                CantidadFormaPago = cantidadLetras,
                                IDUsuario = user.GetUserId(),
                                SiEmbalaje = 0,
                                FechaVencimiento = FechaVencimiento!.Value.ToString("yyyyMMdd"),
                                Observacion = "Pago Servicio",
                                IDFacturador = 1,
                                Facturador = "Sandra Jara",
                                IDCondicionPago = CondicionPagos.idCondicionPago,
                                CondicionPago = CondicionPagos.descripcion,
                                Comentarios = "Pago Servicio",
                                NumeroOC = NroOC,
                                Localidad = Localidad
                            };

                            RequestDetalleFS detalle = new RequestDetalleFS
                            {
                                AnnoProceso = DateTime.Now.Year,
                                IDOperacion = 0,
                                IDTipoOperacion = OptionFacturaBoleta!.Equals("Factura") ? 33 : 39,
                                Fecha = Fecha!.Value.ToString("yyyyMMdd"),
                                CorrelativoArticulo = 0,
                                Nombre = "",
                                CorrelativoBulto = 1,
                                Bulto = 1,
                                CantidadBulto = 1,
                                Cantidad = 1,
                                IDUsuario = user.GetUserId(),
                                Nota = "Pago Servicio"
                            };

                            try
                            {
                                service = new MainServices();
                                var getResultCabecera = await service.FacturaPorServicioLocal.HttpClientInstance.PostAsJsonAsync<RequestCabeceraFS>(UrlCrearCabeceraFS, cabecera);
                                if (getResultCabecera.IsSuccessStatusCode)
                                {
                                    var ResultID = JsonConvert.DeserializeObject<ResponseCabeceraFS>(await getResultCabecera.Content.ReadAsStringAsync());

                                    detalle.IDOperacion = ResultID.IDOperacion;

                                    int nroDocPop = ResultID.NroDocumento;

                                    if (ResultID.msgError == "OK")
                                    {

                                        if (!string.IsNullOrEmpty(TextoUno))
                                        {
                                            detalle.Nombre = TextoUno;

                                            var getResultDetalle = await service.FacturaPorServicioLocal.HttpClientInstance.PostAsJsonAsync<RequestDetalleFS>(UrlCrearDetalleFS, detalle);
                                            if (getResultDetalle.IsSuccessStatusCode)
                                            {
                                                try
                                                {
                                                    Documentline items = new Documentline
                                                    {
                                                        LineNum = 0,
                                                        ItemDescription = TextoUno,
                                                        Quantity = 0,
                                                        UnitPrice = Convert.ToInt64(Neto),
                                                        LineTotal = Convert.ToInt64(Neto),
                                                        TaxCode = "IVA",
                                                        AccountCode = String.IsNullOrEmpty(CuentaContable.ToString()) ? "41010999" : CuentaContable.ToString(),

                                                    };
                                                    List<Documentline> listItems = new List<Documentline>();
                                                    listItems.Add(items);

                                                    InvoicesRequest invoices = new InvoicesRequest
                                                    {
                                                        CardCode = CodigoCliente,
                                                        CardName = RazonSocial,
                                                        HandWritten = "tNO",
                                                        Indicator = OptionFacturaBoleta!.Equals("Factura") ? "33" : "39",
                                                        DocType = "dDocument_Service",
                                                        DocumentSubType = OptionFacturaBoleta!.Equals("Factura") ? "bod_None" : "bod_Bill",
                                                        FolioPrefixString = OptionFacturaBoleta!.Equals("Factura") ? "FV" : "BL",
                                                        DocDate = Fecha!.Value.ToString("yyyyMMdd"),
                                                        DocDueDate = FechaVencimiento!.Value.ToString("yyyyMMdd"),
                                                        TaxDate = Fecha!.Value.ToString("yyyyMMdd"),
                                                        //  ShipToCode = "BILL TO",
                                                        PayToCode = "BILL TO",
                                                        //Address = "BILL TO",
                                                        //Address2 = "SHIP TO",
                                                        DiscountPercent = Convert.ToInt64(valorPorcentaje),
                                                        // FederalTaxID = "IVA",
                                                        DocTotal = Convert.ToInt64(Total),
                                                        SalesPersonCode = 10,
                                                        Comments = "Basado en Pedido " + nroDocPop.ToString().Trim() + (NroOC.Trim().Length == 0 ? "" : " - Nro. OC/" + NroOC.Trim()),
                                                        U_PREPARADO_POR = "Oficina",
                                                        U_REVISADO_POR = "Oficina",
                                                        U_FACTURADO_POR = "Sandra Jara",
                                                        PaymentGroupCode = -1,
                                                        FolioNumber = nroDocPop,
                                                        DocumentLines = listItems
                                                    };

                                                    var responseFactura = await service.FacturaPorServicioLocal.HttpClientInstance.PostAsJsonAsync<InvoicesRequest>(urlGenerarFactura, invoices);
                                                    if (responseFactura.IsSuccessStatusCode)
                                                    {
                                                        var invoieResponse = JsonConvert.DeserializeObject<InvoicesResponse>(await responseFactura.Content.ReadAsStringAsync());

                                                        ActualizarDocEntryDTO datoActualizar = new ActualizarDocEntryDTO
                                                        {
                                                            AnnoProceso = DateTime.Now.Year,
                                                            IDOperacion = ResultID.IDOperacion,
                                                            Correlativo = 1,
                                                            IDTipoOperacion = OptionFacturaBoleta!.Equals("Factura") ? 33 : 39,
                                                            NroFactura = invoieResponse.FolioNumber,
                                                            DocEntry = invoieResponse.DocEntry,
                                                            SiError = 0,
                                                            Error = "",
                                                            IDUsuario = "sjara"
                                                        };
                                                        var responseActualiza = await service.FacturaPorServicioLocal.HttpClientInstance.PostAsJsonAsync<ActualizarDocEntryDTO>(urlActualizaCampoDocEntry, datoActualizar);
                                                        if (responseActualiza.IsSuccessStatusCode)
                                                        {
                                                            try
                                                            {
                                                                var responseImpresion = await service.FacturaPorServicioLocal.HttpClientInstance.GetAsync($"{urlImpresionDTE}/{DateTime.Now.Year}/{ResultID.IDOperacion}/{1}");
                                                                var respuestaMensaje = JsonConvert.DeserializeObject<Respuestas>(await responseImpresion.Content.ReadAsStringAsync());
                                                                if (respuestaMensaje.Respuesta.FirstOrDefault().MsgResult.ToLower().Equals("ok"))
                                                                {
                                                                    _snackBar.Add("Txt Generado", Severity.Info);
                                                                    loading?.Cerrar();

                                                                }
                                                                else
                                                                {
                                                                    _snackBar.Add("Error al generar txt", Severity.Error);
                                                                    loading?.Cerrar();
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                _snackBar.Add("Error al generar txt: " + ex.Message.ToString(), Severity.Error);
                                                                loading?.Cerrar();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            _snackBar.Add("Error al consultar API actualizaDocEntry", Severity.Error);
                                                            loading?.Cerrar();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var invoieResponse = JsonConvert.DeserializeObject<InvoicesResponse>(await responseFactura.Content.ReadAsStringAsync());
                                                        _snackBar.Add("Error al generar factura " + invoieResponse.Comentario, Severity.Error);
                                                        loading?.Cerrar();
                                                    }
                                                }

                                                catch (Exception ex)
                                                {
                                                    _snackBar.Add("Error en envio SAP " + ex.Message, Severity.Error);
                                                    loading?.Cerrar();
                                                }

                                                if (!string.IsNullOrEmpty(TextoDos))
                                                {
                                                    detalle.Nombre = TextoDos;

                                                    var getResultDetalle1 = await service.FacturaPorServicioLocal.HttpClientInstance.PostAsJsonAsync<RequestDetalleFS>(UrlCrearDetalleFS, detalle);
                                                    if (getResultDetalle1.IsSuccessStatusCode)
                                                    {
                                                        if (!string.IsNullOrEmpty(TextoTres))
                                                        {
                                                            detalle.Nombre = TextoTres;

                                                            var getResultDetalle2 = await service.FacturaPorServicioLocal.HttpClientInstance.PostAsJsonAsync<RequestDetalleFS>(UrlCrearDetalleFS, detalle);
                                                            if (!getResultDetalle2.IsSuccessStatusCode)
                                                            {
                                                                _snackBar.Add("Error al ingresar detalle", Severity.Error);
                                                                loading?.Cerrar();
                                                            }
                                                        }
                                                    }

                                                }
                                            }

                                            CodigoCliente = "";
                                            RazonSocial = "";
                                            TextoUno = "";
                                            TextoDos = "";
                                            TextoTres = "";
                                            CuentaContable = 41010999;
                                            CondicionPagos.descripcion = "";
                                            FormaPagos.descripcion = "";
                                            cantidadLetras = 0;
                                            Localidad = "";
                                            NroOC = "";
                                            OptionFacturaBoleta = "Factura";
                                            Neto = 0;
                                            DCto = 0;
                                            Iva = 0;
                                            Total = 0;
                                            SubTotal = 0;
                                            porcentaje = "0";
                                            valorPorcentaje = 0;

                                            loading?.Cerrar();

                                            snakBarMensaje("Factura creada exitosamente! - NroDocumento: " + nroDocPop, Defaults.Classes.Position.TopCenter, Severity.Success, 6000);

                                        }
                                        else
                                        {
                                            _snackBar.Add("Error al ingresar detalle", Severity.Error);
                                            loading?.Cerrar();
                                        }
                                    }
                                    else
                                    {
                                        _snackBar.Add("Error al crear cabecera", Severity.Error);
                                        loading?.Cerrar();
                                    }
                                }
                                else
                                {
                                    _snackBar.Add("Error al consultar API", Severity.Error);
                                    loading?.Cerrar();
                                }
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add(ex.Message, Severity.Error);
                                loading?.Cerrar();
                            }
                        }
                        else
                        {
                            _snackBar.Add("Por Favor ingresar monto positivo!", Severity.Error);
                            loading?.Cerrar();
                        }
                        
                    }
                    else
                    {
                        _snackBar.Add("Por Favor ingresar detalle!", Severity.Error);
                        loading?.Cerrar();
                    }
                }
                else
                {
                    _snackBar.Add("Por Favor ingresar condicion de pago o forma de pago!", Severity.Error);
                    loading?.Cerrar();
                }
                
            }
            else
            {
                _snackBar.Add("Por Favor ingresar cliente!", Severity.Error);
                loading?.Cerrar();
            }
            
            
        }

        #endregion

        #region Cargar Combobox

        private async Task ComboboxFormaDePago()
        {
            try
            {
                var lista = await service.BackOffice.HttpClientInstance.GetAsync(UrlFormaDePago);
                if (lista.IsSuccessStatusCode)
                {
                    var getResult = JsonConvert.DeserializeObject<TuplaFS>(await lista.Content.ReadAsStringAsync());
                    listaFormaPagos = getResult.item1;
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }
        private async Task OnChangeFormaDePago(ConsultaFormaPagoModel args)
        {
            try
            {
                formaPago = args.descripcion;
            }
            catch (Exception ex)
            {
                await Task.Delay(0);
                _snackBar.Add(ex.Message, Severity.Error);
            }
        }

        private async Task ComboboxCondicionDePago()
        {
            try
            {
                var lista = await service.BackOffice.HttpClientInstance.GetAsync(UrlCondicionDePago);
                if (lista.IsSuccessStatusCode)
                {
                    var getResult = JsonConvert.DeserializeObject<TuplaFS1>(await lista.Content.ReadAsStringAsync());
                    listaCondicionPagos = getResult.item1;
                }
            }
            catch (Exception ex)
            {
                _snackBar.Add(ex.Message, Severity.Error);
            }
        }
        private async Task OnChangeCondicionDePago(CondicionPagoVentasModel args)
        {
            try
            {
                condicionPago = args.descripcion;
            }
            catch (Exception ex)
            {
                await Task.Delay(0);
                _snackBar.Add(ex.Message, Severity.Error);
            }
        }

        #endregion

        #region ObtenerClienete

        public async Task ObtenerClienete()
        {
            loading?.Abrir();
            try
            {
                if (!string.IsNullOrEmpty(CodigoCliente))
                {
                    service = new MainServices();
                    cliente.Rut = CodigoCliente.Trim();
                    var getResultCliente = await service.FacturaPorServicioLocal.HttpClientInstance.GetAsync($"{UrlObtenerCliente}/{CodigoCliente}");
                    if (getResultCliente.IsSuccessStatusCode)
                    {
                        var getResult = JsonConvert.DeserializeObject<List<ClienteFacturaPorServicioModel>>(await getResultCliente.Content.ReadAsStringAsync());
                        RazonSocial = getResult![0].Nombre;
                        snakBarMensaje("Consulta Exitosa!", Defaults.Classes.Position.TopCenter, Severity.Success, 5000);
                        loading?.Cerrar();
                    }
                    else
                    {
                        RazonSocial = "";
                        snakBarMensaje("No se encontraron Datos!", Defaults.Classes.Position.TopCenter, Severity.Warning, 5000);
                        loading?.Cerrar();
                    }
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                snakBarMensaje(ex.Message, Defaults.Classes.Position.TopCenter, Severity.Error, 5000);
                loading?.Cerrar();
            }
            await Task.Delay(0);
        }

        #endregion

        #region Calculos

        private async Task CalculosPesos()
        {
            try
            {
                if (valorPorcentaje == 0)
                {
                    if (SubTotal > 0)
                    {
                        Neto = Math.Round(SubTotal - DCto);
                        Iva = Math.Round(Neto * 19 / 100);
                        Total = Math.Round(Neto + Iva);

                        if (DCto > 0)
                        {
                            double result;
                            result = Math.Round(DCto * 100);
                            double resulTotal = Math.Round(result / SubTotal);
                            porcentaje = $"{resulTotal}";
                            valorPorcentaje = Convert.ToDouble(porcentaje);
                        }
                        await Task.Delay(0);
                    }
                    else if (SubTotal == 0 && DCto == 0 && Total == 0)
                    {
                        Neto = 0;
                        Iva = 0;
                        Total = 0;
                    }
                }
                
            }
            catch (Exception ex)
            {
                snakBarMensaje(ex.Message, Defaults.Classes.Position.TopCenter, Severity.Error, 5000);
            }
            await Task.Delay(0);
        }

        private async Task CalculosPorcentual()
        {
            try
            {
                if (DCto == 0)
                {
                    if (SubTotal > 0)
                    {
                        Neto = Math.Round(SubTotal - (SubTotal * valorPorcentaje / 100));
                        Iva = Math.Round(Neto * 19 / 100);
                        Total = Math.Round(Neto + Iva);

                        if (valorPorcentaje != 0)
                        {
                            porcentaje = $"{valorPorcentaje}";
                            DCto = (SubTotal * valorPorcentaje / 100);
                        }
                        await Task.Delay(0);
                    }
                }
            }
            catch (Exception ex)
            {
                snakBarMensaje(ex.Message, Defaults.Classes.Position.TopCenter, Severity.Error, 5000);
            }
            await Task.Delay(0);
        }

        private async Task CalculoInverso()
        {

            if (SubTotal == 0 && DCto == 0 && Total > 0)
            {
                Neto = Math.Round(Total / 1.19);
                Iva = Math.Round(Neto * 19 / 100);
                SubTotal = Neto;
            }
            await Task.Delay(0);
        }

        #endregion

        #region snakBarMensaje

        private void snakBarMensaje(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }

        #endregion

        #region Métodos PopUp

        public async void AbrirPopUp()
        {
            if (!Visibility)
            {
                Visibility = true;
                CuentaContableRemplazo = 0;
            }
            await Task.Delay(0);
        }

        public async void CambiarCuentaContable()
        {
            if (!CuentaContableRemplazo!.Equals(""))
            {
                CuentaContable = CuentaContableRemplazo;
            }
            Visibility = false;
            StateHasChanged();
            await Task.Delay(0);
        }

        #endregion

        #region Nueva factura
        public async Task NuevaFactura()
        {
            facturaDisable = false;
            busquedaNro = false;
            NroFactura = "0";
            CodigoCliente = "";
            RazonSocial = "";
            TextoUno = "";
            TextoDos = "";
            TextoTres = "";
            CuentaContable = 41010999;
            CondicionPagos.descripcion = "";
            FormaPagos.descripcion = "";
            cantidadLetras = 0;
            Localidad = "";
            NroOC = "";
            OptionFacturaBoleta = "Factura";
            Neto = 0;
            DCto = 0;
            Iva = 0;
            Total = 0;
            SubTotal = 0;
            porcentaje = "0";
            valorPorcentaje = 0;
            Fecha = DateTime.Now;
            FechaVencimiento = DateTime.Now;
        }
        #endregion

        #region Busqueda atras/adelante y por nrofactura
        public async Task BusquedaAtras()
        {
            loading?.Abrir();
            service = new MainServices();
            var getResultAtras = await service.FacturaPorServicioLocal.HttpClientInstance.GetAsync($"{urlConsultarNroFactura}/{NroFactura}/{1}");
            if (getResultAtras.IsSuccessStatusCode)
            {
                try
                {
                    var resultFactura = JsonConvert.DeserializeObject<List<BusquedaNroFactura>>(await getResultAtras.Content.ReadAsStringAsync());


                    facturaDisable = true;
                    busquedaNro = true;
                    CodigoCliente = resultFactura.First().IDCliente;
                    RazonSocial = resultFactura.First().RazonSocial;
                    CondicionPagos.descripcion = resultFactura.First().CondicionPago.ToUpper();
                    FormaPagos.descripcion = resultFactura.First().FormaPago.ToUpper();
                    Localidad = resultFactura.First().Localidad;
                    NroOC = resultFactura.First().NumeroOC;
                    SubTotal = (resultFactura.First().Neto + resultFactura.First().Descuentos);
                    Neto = resultFactura.First().Neto;
                    Iva = resultFactura.First().IVA;
                    Total = resultFactura.First().Total;
                    NroFactura = resultFactura.First().NroDocumento.ToString();
                    OptionFacturaBoleta = resultFactura.First().IDTipoOperacion.Equals(33) ? "FACTURA" : "BOLETA";

                    var detalleList = resultFactura.First().Detalle;
                    var i = 0;

                    foreach (var detalle in detalleList)
                    {
                        if (i == 0) TextoUno = detalle.Nombre;
                        if (i == 1) TextoDos = detalle.Nombre;
                        if (i == 2) TextoTres = detalle.Nombre;

                        i++;
                    }

                    Fecha = DateTime.ParseExact(resultFactura.First().Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    FechaVencimiento = DateTime.ParseExact(resultFactura.First().FechaVencimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);



                    snakBarMensaje("Datos encontrados!", Defaults.Classes.Position.TopCenter, Severity.Success, 500);
                    loading?.Cerrar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    loading?.Cerrar();
                }
            }
            loading?.Cerrar();
        }
        public async Task BusquedaAdelante()
        {
            loading?.Abrir();
            service = new MainServices();
            var getResultAtras = await service.FacturaPorServicioLocal.HttpClientInstance.GetAsync($"{urlConsultarNroFactura}/{NroFactura}/{2}");
            if (getResultAtras.IsSuccessStatusCode)
            {
                try
                {
                    var resultFactura = JsonConvert.DeserializeObject<List<BusquedaNroFactura>>(await getResultAtras.Content.ReadAsStringAsync());


                    facturaDisable = true;
                    busquedaNro = true;
                    CodigoCliente = resultFactura.First().IDCliente;
                    RazonSocial = resultFactura.First().RazonSocial;
                    CondicionPagos.descripcion = resultFactura.First().CondicionPago.ToUpper();
                    FormaPagos.descripcion = resultFactura.First().FormaPago.ToUpper();
                    Localidad = resultFactura.First().Localidad;
                    NroOC = resultFactura.First().NumeroOC;
                    SubTotal = (resultFactura.First().Neto + resultFactura.First().Descuentos);
                    Neto = resultFactura.First().Neto;
                    Iva = resultFactura.First().IVA;
                    Total = resultFactura.First().Total;
                    NroFactura = resultFactura.First().NroDocumento.ToString();
                    OptionFacturaBoleta = resultFactura.First().IDTipoOperacion.Equals(33) ? "FACTURA" : "BOLETA";

                    var detalleList = resultFactura.First().Detalle;
                    var i = 0;

                    foreach (var detalle in detalleList)
                    {
                        if (i == 0) TextoUno = detalle.Nombre;
                        if (i == 1) TextoDos = detalle.Nombre;
                        if (i == 2) TextoTres = detalle.Nombre;

                        i++;
                    }

                    Fecha = DateTime.ParseExact(resultFactura.First().Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    FechaVencimiento = DateTime.ParseExact(resultFactura.First().FechaVencimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);



                    snakBarMensaje("Datos encontrados!", Defaults.Classes.Position.TopCenter, Severity.Success, 500);
                    loading?.Cerrar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    loading?.Cerrar();
                }
            }
            loading?.Cerrar();
        }
        public async Task BusquedaFactura()
        {
            loading?.Abrir();
            service = new MainServices();
            var getResultAtras = await service.FacturaPorServicioLocal.HttpClientInstance.GetAsync($"{urlConsultarNroFactura}/{NroFactura}/{3}");
            if (getResultAtras.IsSuccessStatusCode)
            {
                try
                {
                    var resultFactura = JsonConvert.DeserializeObject<List<BusquedaNroFactura>>(await getResultAtras.Content.ReadAsStringAsync());

                    facturaDisable = true;
                    busquedaNro = true;
                    CodigoCliente = resultFactura.First().IDCliente;
                    RazonSocial = resultFactura.First().RazonSocial;
                    CondicionPagos.descripcion = resultFactura.First().CondicionPago.ToUpper();
                    FormaPagos.descripcion = resultFactura.First().FormaPago.ToUpper();
                    Localidad = resultFactura.First().Localidad;
                    NroOC = resultFactura.First().NumeroOC;
                    SubTotal = (resultFactura.First().Neto + resultFactura.First().Descuentos);
                    Neto = resultFactura.First().Neto;
                    Iva = resultFactura.First().IVA;
                    Total = resultFactura.First().Total;
                    OptionFacturaBoleta = resultFactura.First().IDTipoOperacion.Equals(33) ? "FACTURA" : "BOLETA";

                    var detalleList = resultFactura.First().Detalle;
                    var i = 0;

                    foreach (var detalle in detalleList)
                    {
                        if (i == 0) TextoUno = detalle.Nombre;
                        if (i == 1) TextoDos = detalle.Nombre;
                        if (i == 2) TextoTres = detalle.Nombre;

                        i++;
                    }

                    Fecha = DateTime.ParseExact(resultFactura.First().Fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    FechaVencimiento = DateTime.ParseExact(resultFactura.First().FechaVencimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);


                    snakBarMensaje("Datos encontrados!", Defaults.Classes.Position.TopCenter, Severity.Success, 500);
                    loading?.Cerrar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    loading?.Cerrar();
                }
            }
            loading?.Cerrar();
        }
        #endregion
    }
}


