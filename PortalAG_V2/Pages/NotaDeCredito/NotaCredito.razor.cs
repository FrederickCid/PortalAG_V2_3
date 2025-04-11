using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using PortalAG_V2.Shared.Services;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Linq;
using System.Net.Http;
using PortalAG_V2.Shared.Model.NotaDeCredito;
using PortalAG_V2.Shared.Helpers;
using static PortalAG_V2.Pages.Movimientos.Dialog123;
using PortalAG_V2.Auth;
using DocumentFormat.OpenXml.Office2016.Excel;
using PortalAG_V2.Componentes;
using SheriffDataAccess.Models.SheriffModel;
using static PortalAG_V2.Shared.Model.NotaDeCredito.ModeloProcesarNCsap;

namespace PortalAG_V2.Pages.NotaDeCredito
{

    partial class NotaCredito
    {
        [Inject] ExportService exportService { get; set; }
        [Inject] IJSRuntime js { get; set; }
        public string Enter = "Enter";
        [CascadingParameter]
        private AppState? appSatate { get; set; }
        private ClientFactory servicio;
        private int IDOperacionGlobalNC { get; set; }
        private int UltimoNroDocumentoNC { get; set; }

        //-------------------------------------------

        public class Elemento
        {
            public int Linea { get; set; }
            public string? Codigo { get; set; }
            public string? Nombre { get; set; }
            public double Precio { get; set; }
            public double PorcentajeDsto { get; set; }
            public double Descuento { get; set; }
            public double PrecioDescuento { get; set; }
            public double Cantidad { get; set; }
            public double Total { get; set; }
        }

        //-------------------------------------------

        private string _codBarras { get; set; } = "";
        private string NroDocumento { get; set; } = "";
        private string _docReferencia { get; set; } = "";
        private string _fechaDocRef { get; set; } = "";
        private string _montoDocRef { get; set; } = "";
        private string _rutRef { get; set; } = "";
        private string _razonSocialRef { get; set; } = "";

        //-----------------------------------------------

        private MudSelect<string>? _refTipoNC;
        private MudSelect<string>? _refTipoDevolucion;
        private string _fechaProceso { get; set; } = "";
        private string _nroNotaCredito { get; set; } = "";
        private string _comentario { get; set; } = "";
        private DetalleClienteNCDTO infoDocRef = new DetalleClienteNCDTO();

        //------------------------------------------------

        private int _totalDoc { get; set; } = 0;
        private double _dsctoPorcentajeDoc { get; set; } = 0;
        private int _netoDoc { get; set; } = 0;
        private int _descuentoDoc { get; set; } = 0;
        private int _ivaDoc { get; set; } = 0;
        private int _subtotalDoc { get; set; } = 0;
        private bool _boolTipoNC { get; set; } = false;
        private bool _boolTipoDevolucion { get; set; } = false;
        private bool _boolComentario { get; set; } = false;
        private string _descuentoText = "";

        //---------------------------------------------------

        private MudTable<ProductoNCDTODevolver>? _tableDetalle;
        private List<string> listTiposNC = new List<string>()
        {
            "Documento completo",
            "Por devolucion",
            "Valor concepto"
        };
        private List<string> listTiposDevolucion = new List<string>();
        private string? _tipoDocumentoSelect { get; set; }
        private string? _tipoDevolucionSelect { get; set; }

        //--------------------------------------------------

        private string _idArticulo { get; set; } = "";
        private string _nombreArticulo { get; set; } = "";
        private double _totalPorArticulo { get; set; } = 0;
        private bool _disableManual = false;
        private bool _errorValidacion = true;
        private bool _soloAnulacion = false;

        //------------------------------------------------

        private string _motivoDev { get; set; } = "";
        private double _netoVC { get; set; } = 0;
        private double _descuentoVC { get; set; } = 0;
        private double _ivaVC { get; set; } = 0;
        private double _totalVC { get; set; } = 0;
        public string? ConceptoReferencia_NC;

        //------------------------------------------------

        string fechaProceso2 = DateTime.Now.ToString("dd/MM/yyyy");
        private List<ProductosNCDTO> _productosDocRef = new List<ProductosNCDTO>();
        private List<ProductoNCDTODevolver> _listDetalleNC = new List<ProductoNCDTODevolver>();
        public static RequestNCDetalle ObjRequest = new RequestNCDetalle();
        public static List<ProductosNCDTO> listaProductosEnviada = new List<ProductosNCDTO>();

        private RequestIngresaSolicitudNC DatosNC = new RequestIngresaSolicitudNC();

        private string urlBuscaDatosDocRef = "/api/v2/NotasDeCredito/GetDetalleCliente";
        private string urlBuscaDetalleDocRef = "/api/v2/NotasDeCredito/GetProductos";
        private string urlPostVentasEstadoSolicitudNC = "/api/v2/NotasDeCredito/IngresaSolicitudNC";
        private string urlGuardarDetalleNC = "/api/v2/NotasDeCredito/GuardarDetallePorDocumento";
        private string urlEstadoNCDetalle = "/api/v2/NotasDeCredito/IngresaSolicitudNCDetalle";
        private string urlMontosNC = "/api/v2/NotasDeCredito/IngresaMontosDatosNC";
        //new
        private string urlGenerarNC = "/api/v2/NotaDeCredito/GenerarNC";
        private string urlFacturaXServicio = "/api/v2/NotasDeCredito/ObtenerFacturaServicio";
        private string Datos = "";

        public ModeloProcesarNCsap Respuesta = new();

        //-------------------------------------------------

        private List<Elemento> Elements = new List<Elemento>();
        public List<DetalleClienteNCDTO> clientes = new List<DetalleClienteNCDTO>();
        public ObservableCollection<ProductoNCDTODevolver> ProductosADevolver = new ObservableCollection<ProductoNCDTODevolver>();
        public List<ProductoNCDTODevolver> ProductosADevolverValorConcepto = new List<ProductoNCDTODevolver>();
        SetDatosPDF dataPDF = new SetDatosPDF();
        public int FacturaServicio { get; set; } = 0;

        string _IDUser = "";
        //-------------------------------------------------
        protected override async Task OnInitializedAsync()
        {
            var user = await _authenticationManager.CurrentUser();
            var IDuse = user.GetFirstName();
            _IDUser = user.GetUserId();
            servicio = new MainServices().ConectionServiceNotaCredito;
            //servicio.HttpClientInstance.DefaultRequestHeaders.Add("ID", appSatate.IDUsuario);
            _fechaProceso = DateTime.Today.ToString("dd/MM/yyyy");
            StateHasChanged();
        }
        public async Task GetGenerarNC(int IdOperacion)
        {
            try
            {
                var lista = await servicio.HttpClientInstance.GetAsync($"{urlGenerarNC}/{IdOperacion}");
                if (lista.IsSuccessStatusCode)
                {
                    Respuesta = JsonConvert.DeserializeObject<ModeloProcesarNCsap>(await lista.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            StateHasChanged();

        }
        public async Task BuscarFactServicio() 
        {

            var lista = await servicio.HttpClientInstance.GetAsync($"{urlFacturaXServicio}/{NroDocumento}");
            if (lista.IsSuccessStatusCode)
            {
                FacturaServicoModel Respuesta = JsonConvert.DeserializeObject<FacturaServicoModel>(await lista.Content.ReadAsStringAsync());

                _codBarras = $"F{Respuesta.AnnoProceso}{Respuesta.IDOperacion}00{Respuesta.Correlativo}";
                await BuscarDocumento();
            }
            else {
                _snackBar.Add("Factura Por Servicio No Encontrada", Severity.Error);
            }

        }

        public async Task BuscarDocumento()
        {
            //IsLoading = true;
            try
            {
                LimpiarValoresNCDoc();

                var responseC = await GetDetalleCliente();
                if (responseC.IsSuccess)
                {
                    _snackBar.Add("Datos encontrados", Severity.Success);
                    _fechaProceso = DateTime.Today.ToString("dd/MM/yyyy");

                }
                else
                {
                    _snackBar.Add("Error al buscar documento base", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                _snackBar.Add(ex.Message, Severity.Error);
            }
        }
        string anoOpera { get; set; }
        string refencia { get; set; }
        string year { get; set; }
        string idOperacion { get; set; }
        string correlativo { get; set; }
        public async Task<Response> GetDetalleCliente() // Busca la factura
        {
            try
            {
                //_codBarras = "AQUI VA EL CODIGO DE BARRA";
                anoOpera = _codBarras.Substring(1, 4);
                refencia = _codBarras.Substring(7, 7);
                year = _codBarras.Substring(1, 4);
                idOperacion = _codBarras.Substring(5, 9);
                correlativo = _codBarras.Substring(_codBarras.Length - 3);

                //anoOpera = "2022";
                //refencia = "1";
                //year = "2022";
                //idOperacion = "2";
                //correlativo = "1";


                RequestNCDetalle request = new RequestNCDetalle
                {
                    AnnoProceso = Convert.ToInt32(anoOpera),
                    Correlativo = Convert.ToInt32(correlativo),
                    IDAllGestEmpresa = 2,
                    IDEmpresa = 1,
                    IDOperacion = Convert.ToInt32(idOperacion),
                    IDTipoOperacion = 33 // No afecta para la primera busqueda
                };
                //Barcode2 = "C" + request.AnnoProceso.ToString() + request.IDOperacion.ToString() + request.Correlativo;
                //Cosulta todos los datos de la factura
                var response = await servicio.HttpClientInstance.PostAsJsonAsync<RequestNCDetalle>($"{urlBuscaDatosDocRef}", request);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al buscar datos de documento referencia",
                        Result = null
                    };
                }
                // Deserealiza los datos de la factura/boleta
                var DocRef = JsonConvert.DeserializeObject<DetalleClienteNCDTO>(await response.Content.ReadAsStringAsync());
                clientes.Clear();
                clientes.Add(DocRef);
                infoDocRef = DocRef;
                _docReferencia = infoDocRef.IDTipoOperacion == 33 ? "F-" + infoDocRef.NroDocumento.ToString() : "B-" + infoDocRef.NroDocumento.ToString();
                _fechaDocRef = infoDocRef.FechaDocumento.ToString();
                _montoDocRef = infoDocRef.TotalFactura.ToString("n0");
                _rutRef = infoDocRef.Rut!.ToString();
                _razonSocialRef = infoDocRef.RazonSocial!;
                _totalDoc = infoDocRef.TotalFactura;
                _dsctoPorcentajeDoc = infoDocRef.OtrosValores;
                _netoDoc = infoDocRef.Neto;
                _descuentoDoc = infoDocRef.Descuentos;
                _ivaDoc = infoDocRef.IVA;
                _subtotalDoc = _netoDoc + _descuentoDoc;
                _descuentoText = _dsctoPorcentajeDoc <= 0 ? "Sin Descuento" : $"Descuento {_dsctoPorcentajeDoc}%";

                // si el documento no es del mes o del año se bloquea valor concepto
                //DateTime fechaProcesoRef = Convert.ToDateTime(infoDocRef.FechaActualizacion);
                //if (fechaProcesoRef.Month > DateTime.Now.AddMonths(-1).Month || fechaProcesoRef.Year != DateTime.Now.Year)
                //{
                //    // Solo anulacion
                //    _soloAnulacion = true;
                //    listTiposNC = new List<string>()
                //    {
                //        "Por devolucion",
                //    };
                //}
                //else
                //{
                //    // Todo tipo
                //    if (_IDUser.Equals("vfuentes"))
                //    {
                //        listTiposNC = new List<string>()
                //        {
                //            "Documento completo",
                //            "Por devolucion"
                //        };
                //    }
                //    else
                //    {
                //        listTiposNC = new List<string>()
                //        {
                //            "Documento completo",
                //            "Por devolucion",
                //            "Valor concepto"
                //        };
                //    }

                //}

                // Consulta los articulos de la factura o boleta
                var responseP = await servicio.HttpClientInstance.PostAsJsonAsync<RequestNCDetalle>($"{urlBuscaDetalleDocRef}", request);
                if (!responseP.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al obtener detalle",
                        Result = null
                    };
                }

                // Deserializa la lista de articulos de la factura
                var listaProductos = JsonConvert.DeserializeObject<List<ProductosNCDTO>>(await responseP.Content.ReadAsStringAsync());
                _productosDocRef = listaProductos!;

                StateHasChanged();
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = null
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }

        }


        private async void ChangeTipoDocumento(string select)
        {
            _tipoDocumentoSelect = select;
            _netoVC = 0;
            _descuentoVC = 0;
            _ivaVC = 0;
            _totalVC = 0;
            _listDetalleNC.Clear();
            switch (select)
            {
                case "Documento completo":
                    _snackBar.Add("Completo", Severity.Normal);
                    _boolTipoNC = false;
                    _boolTipoDevolucion = false;
                    if (_IDUser.Equals("vfuentes"))
                    {
                        listTiposDevolucion = new List<string>
                        {
                            "Con devolucion fisica"
                        };
                    }
                    else
                    {
                        listTiposDevolucion = new List<string>
                        {
                            "Con devolucion fisica",
                            "Sin devolucion fisica"
                        };
                    }

                    break;
                case "Por devolucion":
                    _snackBar.Add("Devolucion", Severity.Normal);
                    _boolTipoNC = false;
                    _boolTipoDevolucion = false;

                    if (_IDUser.Equals("vfuentes"))
                    {
                        listTiposDevolucion = new List<string>
                        {
                            "Recepción Ventas",
                            "Producto Dañado",
                            "Devolución de producto"
                        };
                    }
                    else
                    {
                        listTiposDevolucion = new List<string>
                        {
                            "Recepción Ventas",
                            "Devolución de producto",
                            "Producto Dañado",
                            "Anulación"
                        };
                    }


                    break;
                case "Valor concepto":
                    _snackBar.Add("Concepto", Severity.Normal);
                    _boolTipoNC = true;
                    _boolTipoDevolucion = true;
                    _boolComentario = true;
                    listTiposDevolucion.Clear();
                    _refTipoDevolucion.Reset();
                    break;
            }
            _tipoDevolucionSelect = "";
        }

        private async void ChangeTipoDevolucion(string select)
        {
            bool _mostrarDetalle = false;
            _tipoDevolucionSelect = select;
            _disableManual = false;
            _netoVC = 0;
            _descuentoVC = 0;
            _ivaVC = 0;
            _totalVC = 0;
            //LimpiarValoresNC();
            _listDetalleNC.Clear();
            switch (_tipoDocumentoSelect)
            {
                case "Documento completo":
                    _mostrarDetalle = true;
                    switch (_tipoDevolucionSelect)
                    {
                        case "Con devolucion fisica":
                            _listDetalleNC.Clear();
                            _snackBar.Add("Con devolucion fisica", Severity.Info);
                            break;
                        case "Sin devolucion fisica":
                            _listDetalleNC.Clear();
                            _snackBar.Add("Sin devolucion fisica", Severity.Info);
                            break;
                    }
                    break;
                case "Por devolucion":
                    _listDetalleNC.Clear();
                    switch (_tipoDevolucionSelect)
                    {
                        case "Recepción Ventas":
                            _mostrarDetalle = true;
                            _snackBar.Add("Recepción Ventas", Severity.Info);
                            break;
                        case "Devolución de producto":
                            _disableManual = true;
                            _snackBar.Add("Devolución de producto", Severity.Info);
                            break;
                        case "Producto Dañado":
                            _disableManual = true;
                            _snackBar.Add("Producto Dañado", Severity.Info);
                            break;
                        case "Anulación":
                            _mostrarDetalle = true;
                            _snackBar.Add("Anulación", Severity.Info);
                            break;
                    }
                    break;
                case "Valor concepto":
                    _listDetalleNC.Clear();
                    _snackBar.Add("Concepto", Severity.Info);
                    break;
            }

            if (_mostrarDetalle)
            {
                foreach (ProductosNCDTO var in _productosDocRef)
                {
                    ProductoNCDTODevolver auxVar = new ProductoNCDTODevolver
                    {
                        Linea = var.Linea,
                        IDArticulo = var.IDArticulo,
                        Cantidad = var.Cantidad,
                        Nombre = var.Nombre,
                        PrecioVenta = Convert.ToInt32(var.PrecioVenta),
                        CantidadADevolver = var.Cantidad
                    };

                    _listDetalleNC.Add(auxVar);
                }

                _netoVC = infoDocRef.Neto;
                _descuentoVC = infoDocRef.Descuentos;
                _ivaVC = infoDocRef.IVA;
                _totalVC = infoDocRef.TotalFactura;
            }
            StateHasChanged();
        }

        private async Task BuscarIDArticulo(KeyboardEventArgs args)
        {
            if (args.Key == Enter)
            {
                if (String.IsNullOrWhiteSpace(_idArticulo) || String.IsNullOrEmpty(_idArticulo))
                {
                    _snackBar.Add("Campo ID Cliente es requerido", Severity.Error);
                }
                else
                {
                    if (_productosDocRef.Exists(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower()))
                    {
                        _nombreArticulo = _productosDocRef.Find(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower())!.Nombre!;
                    }
                    else
                    {
                        _snackBar.Add("Este articulo no existe en el documento base", Severity.Error);
                    }
                }
            }

        }

        private async Task BuscarIDArticuloFocus(FocusEventArgs args)
        {
            if (String.IsNullOrWhiteSpace(_idArticulo) || String.IsNullOrEmpty(_idArticulo))
            {
                _snackBar.Add("Campo ID Cliente es requerido", Severity.Error);
            }
            else
            {
                if (_productosDocRef.Exists(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower()))
                {
                    _nombreArticulo = _productosDocRef.Find(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower())!.Nombre!;
                }
                else
                {
                    _snackBar.Add("Este articulo no existe en el documento base", Severity.Error);
                }
            }
        }

        private async Task VerificarTotalArticulo(KeyboardEventArgs args)
        {
            if (args.Key == Enter)
            {
                if (_totalPorArticulo == 0)
                {
                    _snackBar.Add("Cantidad articulo no puede ser 0", Severity.Error);
                }
                else
                {
                    if (_productosDocRef.Exists(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower()))
                    {
                        var cantidadBase = _productosDocRef.Find(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower())!.Cantidad!;
                        if (_totalPorArticulo > cantidadBase)
                        {
                            _snackBar.Add("La cantidad no a devolver no puede superar la cantidad del documento base", Severity.Error);
                        }
                        else
                        {
                            _errorValidacion = false;
                        }
                    }
                    else
                    {
                        _snackBar.Add("Este articulo no existe en el documento base", Severity.Error);
                    }
                }
            }
        }

        private async Task VerificarTotalArticuloFocus(FocusEventArgs args)
        {
            if (_totalPorArticulo == 0)
            {
                _snackBar.Add("Cantidad articulo no puede ser 0", Severity.Error);
            }
            else
            {
                if (_productosDocRef.Exists(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower()))
                {
                    var cantidadBase = _productosDocRef.Find(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower())!.Cantidad!;
                    if (_totalPorArticulo > cantidadBase)
                    {
                        _snackBar.Add("La cantidad no a devolver no puede superar la cantidad del documento base", Severity.Error);
                    }
                    else
                    {
                        _errorValidacion = false;
                    }
                }
                else
                {
                    _snackBar.Add("Este articulo no existe en el documento base", Severity.Error);
                }
            }
        }

        private async Task AgregarArticulo()
        {

            var cantidadBase = _productosDocRef.Find(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower())!.Cantidad!;
            if (_totalPorArticulo > cantidadBase)
            {
                _snackBar.Add("La cantidad no a devolver no puede superar la cantidad del documento base", Severity.Error);
            }
            else
            {
                if (_listDetalleNC.Exists(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower()))
                    _listDetalleNC.Remove(_listDetalleNC.Find(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower())!);

                ProductoNCDTODevolver auxVar = new ProductoNCDTODevolver
                {
                    Linea = _productosDocRef.Find(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower())!.Linea!,
                    IDArticulo = _idArticulo,
                    Cantidad = Convert.ToInt32(_productosDocRef.Find(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower())!.Cantidad!),
                    Nombre = _nombreArticulo,
                    PrecioVenta = Convert.ToInt32(_productosDocRef.Find(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower())!.PrecioVenta!),
                    CantidadADevolver = Convert.ToInt32(_totalPorArticulo)
                };

                _listDetalleNC.Add(auxVar);
                _errorValidacion = true;
                CalcularMontosNC();
                StateHasChanged();
            }

        }

        private async Task EliminarArticulo()
        {
            if (_listDetalleNC.Exists(x => x.IDArticulo!.ToLower() == _idArticulo.ToLower()))
            {
                _listDetalleNC.Remove(_listDetalleNC.Find(x => x.IDArticulo!.ToLower() == _idArticulo!.ToLower())!);
            }
            CalcularMontosNC();
            StateHasChanged();
        }

        public void NuevoDetalle()
        {
            _idArticulo = "";
            _nombreArticulo = "";
            _totalPorArticulo = 0;
        }

        public void CalcularMontosNC()
        {
            double _totalAux = 0;
            foreach (ProductoNCDTODevolver var in _listDetalleNC)
            {
                _totalAux += (var.CantidadADevolver * var.PrecioVenta);
            }
            double _subTotalVC = _totalAux;
            _descuentoVC = (_totalAux * infoDocRef.OtrosValores) / 100;
            _netoVC = (_totalAux - _descuentoVC);
            _ivaVC = (_netoVC * 19) / 100;
            _totalVC = (_netoVC + _ivaVC);
        }

        private async Task CalcularMontosValorConcepto(FocusEventArgs args)
        {
            _descuentoVC = 0;
            _netoVC = _netoVC;
            _ivaVC = (_netoVC * 19) / 100;
            _totalVC = (_netoVC + _ivaVC);
        }
        private async Task CalcularMontosValorConcepto(KeyboardEventArgs args)
        {
            if (args.Key == Enter)
            {
                _descuentoVC = 0;
                _netoVC = _netoVC;
                _ivaVC = (_netoVC * 19) / 100;
                _totalVC = (_netoVC + _ivaVC);
            }

        }

        ResponseDetalleNotaCreditoDTO ingresaDetalleNCResponse = new ResponseDetalleNotaCreditoDTO();


        public void CargarTodo()
        {
            _listDetalleNC.Clear();
            foreach (ProductosNCDTO var in _productosDocRef)
            {
                ProductoNCDTODevolver auxVar = new ProductoNCDTODevolver
                {
                    Linea = var.Linea,
                    IDArticulo = var.IDArticulo,
                    Cantidad = var.Cantidad,
                    Nombre = var.Nombre,
                    PrecioVenta = Convert.ToInt32(var.PrecioVenta),
                    CantidadADevolver = var.Cantidad
                };

                _listDetalleNC.Add(auxVar);
            }

            _netoVC = infoDocRef.Neto;
            _descuentoVC = infoDocRef.Descuentos;
            _ivaVC = infoDocRef.IVA;
            _totalVC = infoDocRef.TotalFactura;
        }

        private void LimpiarValoresNC()
        {
            _listDetalleNC.Clear();
            _productosDocRef.Clear();
            DatosNC = new RequestIngresaSolicitudNC();
            infoDocRef = new DetalleClienteNCDTO();
            _codBarras = "";
            _docReferencia = "";
            _fechaDocRef = "";
            _montoDocRef = "";
            _rutRef = "";
            _razonSocialRef = "";
            _tipoDocumentoSelect = "";
            _tipoDevolucionSelect = "";
            _soloAnulacion = false;
            _fechaProceso = "";
            _nroNotaCredito = "";
            _comentario = "";
            _netoVC = 0;
            _descuentoVC = 0;
            _ivaVC = 0;
            _totalVC = 0;
            _idArticulo = "";
            _nombreArticulo = "";
            _totalPorArticulo = 0;
            _motivoDev = "";
        }
        private void LimpiarValoresNCDoc()
        {
            _listDetalleNC.Clear();
            _productosDocRef.Clear();
            DatosNC = new RequestIngresaSolicitudNC();
            infoDocRef = new DetalleClienteNCDTO();

            _docReferencia = "";
            _fechaDocRef = "";
            _montoDocRef = "";
            _rutRef = "";
            _razonSocialRef = "";
            _tipoDocumentoSelect = "";
            _tipoDevolucionSelect = "";
            _soloAnulacion = false;
            _fechaProceso = "";
            _nroNotaCredito = "";
            _comentario = "";
            _netoVC = 0;
            _descuentoVC = 0;
            _ivaVC = 0;
            _totalVC = 0;
            _idArticulo = "";
            _nombreArticulo = "";
            _totalPorArticulo = 0;
            _motivoDev = "";
        }

        private async Task ProcesarNC()
        {
            if (!string.IsNullOrEmpty(_tipoDocumentoSelect) || !string.IsNullOrEmpty(_tipoDevolucionSelect)) // Procesar por primera vez
            {
                DatosNC = new RequestIngresaSolicitudNC();

                DatosNC.IDAllGestEmpresa = 2;
                DatosNC.IDEmpresa = 1;
                DatosNC.AnnoProceso = DateTime.Now.Year;
                DatosNC.IDOperacion = 0;
                DatosNC.Correlativo = 1;
                DatosNC.TipoOperacion = 61;
                DatosNC.IDTipo = _tipoDocumentoSelect == "Documento completo" ? 1 : _tipoDocumentoSelect == "Por devolucion" ? 3 : 5;
                DatosNC.IDTipoSiDevolucion = (_tipoDevolucionSelect == "Con devolucion fisica" ? 1 : _tipoDevolucionSelect == "Sin devolucion fisica" ? 0 : _tipoDevolucionSelect == "Recepción Ventas" ?
                    9 : _tipoDevolucionSelect == "Devolución de producto" ? 5 : _tipoDevolucionSelect == "Producto Dañado" ? 3 : _tipoDevolucionSelect == "Anulación" ? 1 : 1);
                DatosNC.IDCliente = infoDocRef.IDCliente;
                DatosNC.RazonSocial = infoDocRef.RazonSocial;
                DatosNC.Subtotal = Convert.ToInt32(_netoVC) + Convert.ToInt32(_descuentoVC);
                DatosNC.Neto = Convert.ToInt32(_netoVC);
                DatosNC.Descto = Convert.ToInt32(_descuentoVC);
                DatosNC.IVA = Convert.ToInt32(_ivaVC);
                DatosNC.Total = Convert.ToInt32(_totalVC);
                DatosNC.AnnoProcesoRef = Convert.ToInt32(year);
                DatosNC.IDOperacionRef = Convert.ToInt32(idOperacion);
                DatosNC.CorrelativoRef = infoDocRef.Correlativo;
                DatosNC.TipoOperacionRef = infoDocRef.IDTipoOperacion;
                DatosNC.NroFacturaRef = infoDocRef.NroDocumento;
                DatosNC.FechaFacturaRef = infoDocRef.FechaDocumento;
                DatosNC.MontoFacturaRef = infoDocRef.TotalFactura;
                DatosNC.IDGuia = 0;
                DatosNC.IDOperacionNuevo = 0;
                DatosNC.IDEstado = infoDocRef.IDEstado;
                DatosNC.IDUsuario = string.IsNullOrEmpty(appSatate.IDUsuario) ? "ti" : appSatate.IDUsuario;//infoDocRef.IDUsuario;
                DatosNC.TipoConsulta = 1;

                // Se guarda el encabezado para cualquier tipo de NC y cualquier tipo de devolucion
                var postEstadoSolicitud = await servicio.HttpClientInstance.PostAsJsonAsync<RequestIngresaSolicitudNC>($"{urlPostVentasEstadoSolicitudNC}", DatosNC);
                if (!postEstadoSolicitud.IsSuccessStatusCode)
                {
                    _snackBar.Add("Error al guardar datos de encabezado", Severity.Error);
                }
                else
                {
                    var responseBD = JsonConvert.DeserializeObject<ResponseBD2>(await postEstadoSolicitud.Content.ReadAsStringAsync());
                    IDOperacionGlobalNC = responseBD.IDOperacion;
                    UltimoNroDocumentoNC = responseBD.UltimoNroDocumentoNC;
                    _nroNotaCredito = responseBD.UltimoNroDocumentoNC.ToString();


                    //---------------------------Si es Documento completo o Por devolucion y Anulacion--------------------------//
                    if (_tipoDocumentoSelect == "Documento completo" || (_tipoDocumentoSelect == "Por devolucion" && (_tipoDevolucionSelect == "Anulación")))
                    {
                        //Guardar el detalle de la NC
                        RequestNCDetalle requestDatosNC = new RequestNCDetalle
                        {
                            AnnoProceso = DateTime.Now.Year,
                            Correlativo = 1,
                            IDAllGestEmpresa = 2,
                            IDEmpresa = 1,
                            IDOperacion = IDOperacionGlobalNC,
                            IDTipoOperacion = 61
                        };

                        ProductosADevolver.Clear();
                        foreach (ProductoNCDTODevolver var in _listDetalleNC)
                        {

                            ProductoNCDTODevolver prod = new ProductoNCDTODevolver();
                            prod.Cantidad = var.Cantidad;
                            prod.CantidadADevolver = var.CantidadADevolver;
                            prod.IDArticulo = var.IDArticulo;
                            prod.Linea = var.Linea;
                            prod.Nombre = var.Nombre;
                            prod.PrecioVenta = var.PrecioVenta;
                            prod.Total = (var.CantidadADevolver * var.PrecioVenta);
                            ProductosADevolver.Add(prod);
                        }

                        dataPDF = new SetDatosPDF(
                            Convert.ToDateTime(fechaProceso2), UltimoNroDocumentoNC, IDOperacionGlobalNC, true, true, _tipoDevolucionSelect,
                            _descuentoText, _subtotalDoc, _descuentoDoc, _netoDoc, _ivaDoc, _totalDoc, _comentario);

                        foreach (ProductosNCDTO data in _productosDocRef)
                        {
                            if (String.IsNullOrEmpty(data.DescripcionUbicacion))
                            {
                                _productosDocRef.Find(x => x.IDArticulo == data.IDArticulo).DescripcionUbicacion = "";
                            }

                        }
                        ResponseListaDetallePorDocumentoNC ObjEnviado = new ResponseListaDetallePorDocumentoNC(requestDatosNC, _productosDocRef);
                        var responseP = await servicio.HttpClientInstance.PostAsJsonAsync<ResponseListaDetallePorDocumentoNC>(urlGuardarDetalleNC, ObjEnviado);
                        if (!responseP.IsSuccessStatusCode)
                        {
                            _snackBar.Add("Error al crear solicitud", Severity.Error);
                        }
                        else
                        {
                            MontosNCDTO montosNC = new MontosNCDTO();
                            montosNC.IDAllGestEmpresa = 2;
                            montosNC.IDEmpresa = 1;
                            montosNC.AnnoProceso = DatosNC.AnnoProceso;
                            montosNC.IDOperacion = IDOperacionGlobalNC;
                            montosNC.Subtotal = _subtotalDoc;
                            montosNC.Neto = _netoDoc;
                            montosNC.PorcentajeDesc = Convert.ToInt32(_dsctoPorcentajeDoc);
                            montosNC.Descto = _descuentoDoc;
                            montosNC.IVA = _ivaDoc;
                            montosNC.Total = _totalDoc;
                            montosNC.Nota = _comentario == "" ? _tipoDocumentoSelect + " (" + _tipoDevolucionSelect + ")" : _comentario;
                            var postEstadoSolicitudActualizar = await servicio.HttpClientInstance.PostAsJsonAsync<MontosNCDTO>($"{urlMontosNC}", montosNC);
                            if (!postEstadoSolicitud.IsSuccessStatusCode)
                            {
                                _snackBar.Add("Error al crear NC. Consultar a TI", Severity.Error);
                            }
                            else
                            {
                                _snackBar.Add("Nota de credito solicitada", Severity.Success);
                                await GenerarPDF(SetDatos());
                                //Genera NC SAP y SII Automatico----------------
                                await GetGenerarNC(IDOperacionGlobalNC);
                                //----------------------------------------------
                                await GenerarPDFPorValorConcepto(SetDatos());
                                RespuestaProcesarNCsap RespuestaNCConsulta = new();
                                RespuestaNCConsulta = Respuesta.Respuesta.FirstOrDefault();
                                if (RespuestaNCConsulta.msgResult.Length > 0 && RespuestaNCConsulta.msgResult != null)
                                {
                                    var parameters = new DialogParameters<DialogConfirmacionNC> {
                                        { x => x.TextDialog, RespuestaNCConsulta.msgMessage },
                                        { x => x.Titulo,  $"{RespuestaNCConsulta.msgValor} - { RespuestaNCConsulta.msgResult}"}  ,
                                        { x => x.nombreBoton, "OK" },
                                        { x => x.ocultarcancelar, false}
                                        };
                                    var dialog = await DialogServices.ShowAsync<DialogConfirmacionNC>("Question", parameters);
                                    LimpiarValoresNC();
                                }
                                LimpiarValoresNC();
                            }
                        }

                    }



                    if (_tipoDocumentoSelect == "Por devolucion" && (_tipoDevolucionSelect == "Devolución de producto" || _tipoDevolucionSelect == "Producto Dañado" || _tipoDevolucionSelect == "Recepción Ventas"))
                    {

                        //Guardar el detalle de la NC
                        RequestNCDetalle requestDatosNC = new RequestNCDetalle
                        {
                            AnnoProceso = DateTime.Now.Year,
                            Correlativo = 1,
                            IDAllGestEmpresa = 2,
                            IDEmpresa = 1,
                            IDOperacion = IDOperacionGlobalNC,
                            IDTipoOperacion = 61
                        };

                        ProductosADevolver.Clear();
                        List<ProductosNCDTO> listaDevolver = new List<ProductosNCDTO>();
                        int _subTotalVC = 0;
                        foreach (ProductoNCDTODevolver var in _listDetalleNC)
                        {

                            ProductoNCDTODevolver prod = new ProductoNCDTODevolver();
                            prod.Cantidad = var.Cantidad;
                            prod.CantidadADevolver = var.CantidadADevolver;
                            prod.IDArticulo = var.IDArticulo;
                            prod.Linea = var.Linea;
                            prod.Nombre = var.Nombre;
                            prod.PrecioVenta = var.PrecioVenta;
                            prod.Total = (var.CantidadADevolver * var.PrecioVenta);
                            _subTotalVC = _subTotalVC + (var.CantidadADevolver * var.PrecioVenta);
                            ProductosADevolver.Add(prod);

                            ProductosNCDTO auxDevolver = new ProductosNCDTO();
                            auxDevolver.Linea = var.Linea;
                            auxDevolver.Fecha = _productosDocRef.FirstOrDefault().Fecha;
                            auxDevolver.IDArticulo = var.IDArticulo;
                            auxDevolver.CorrelativoArticulo = 1;
                            auxDevolver.Nombre = var.Nombre;
                            auxDevolver.Cantidad = var.CantidadADevolver;
                            auxDevolver.PrecioVenta = var.PrecioVenta;
                            auxDevolver.StockAlComprar = 0;
                            auxDevolver.FechaActualizacion = _productosDocRef.FirstOrDefault().FechaActualizacion;
                            auxDevolver.IDUsuario = "crar";
                            auxDevolver.DescripcionUbicacion = "";
                            auxDevolver.IDUbicacion = "";
                            auxDevolver.IDOrigen = 0;
                            auxDevolver.IDArticuloNumber = 0;

                            listaDevolver.Add(auxDevolver);

                        }

                        dataPDF = new SetDatosPDF(
                            Convert.ToDateTime(fechaProceso2), UltimoNroDocumentoNC, IDOperacionGlobalNC, true, true, _tipoDevolucionSelect,
                            _descuentoText, _subTotalVC, _descuentoVC, _netoVC, _ivaVC, _totalVC, _comentario);

                        ResponseListaDetallePorDocumentoNC ObjEnviado = new ResponseListaDetallePorDocumentoNC(requestDatosNC, listaDevolver);
                        var response = await servicio.HttpClientInstance.PostAsJsonAsync<ResponseListaDetallePorDocumentoNC>(urlGuardarDetalleNC, ObjEnviado);
                        if (!response.IsSuccessStatusCode)
                        {
                            _snackBar.Add("Error al crear solicitud", Severity.Error);
                        }

                        else
                        {
                            MontosNCDTO montosNC = new MontosNCDTO();
                            montosNC.IDAllGestEmpresa = 2;
                            montosNC.IDEmpresa = 1;
                            montosNC.AnnoProceso = DatosNC.AnnoProceso;
                            montosNC.IDOperacion = IDOperacionGlobalNC;
                            montosNC.Subtotal = _subTotalVC;
                            montosNC.Neto = Convert.ToInt32(_netoVC);
                            montosNC.PorcentajeDesc = Convert.ToInt32(_dsctoPorcentajeDoc);
                            montosNC.Descto = Convert.ToInt32(_descuentoVC);
                            montosNC.IVA = Convert.ToInt32(_ivaVC);
                            montosNC.Total = Convert.ToInt32(_totalVC);
                            montosNC.Nota = _comentario == "" ? _tipoDocumentoSelect + " (" + _tipoDevolucionSelect + ")" : _comentario;
                            var postEstadoSolicitudActualizar = await servicio.HttpClientInstance.PostAsJsonAsync<MontosNCDTO>($"{urlMontosNC}", montosNC);
                            if (!postEstadoSolicitud.IsSuccessStatusCode)
                            {
                                _snackBar.Add("Error al crear NC. Consultar a TI", Severity.Error);
                            }
                            else
                            {
                                _snackBar.Add("Nota de credito solicitada", Severity.Success);
                                await GenerarPDF(SetDatos());
                                LimpiarValoresNC();
                            }
                        }


                    }


                    if (_tipoDocumentoSelect == "Valor concepto")
                    {
                        DatosNC.IDTipoSiDevolucion = 1;
                        int cantDevolver = 1;
                        RequestIngresaSolicitudNCDetalle request2 = new RequestIngresaSolicitudNCDetalle
                        {
                            AnnoProceso = Convert.ToInt32(DateTime.Now.Year),

                            CantDevolver = cantDevolver,
                            CantidadRef = 1,
                            Descripcion = _motivoDev,
                            IDAllGestEmpresa = 2,
                            IDEmpresa = 1,
                            Correlativo = 1,
                            FechaActualizacion = DateTime.Now,
                            IDArticulo = "Valor Concepto",
                            IDOperacion = IDOperacionGlobalNC,
                            IDUsuario = appSatate.IDUsuario!,
                            Linea = 0,
                            LineaRef = 0,
                            PrecioUnidad = Convert.ToInt32(_netoVC),
                            Total = Convert.ToInt32(_netoVC * cantDevolver)
                        };

                        ProductoNCDTODevolver prod = new ProductoNCDTODevolver();
                        prod.Cantidad = cantDevolver;
                        prod.CantidadADevolver = cantDevolver;
                        prod.IDArticulo = request2.IDArticulo;
                        prod.Linea = request2.Linea;
                        prod.Nombre = ConceptoReferencia_NC;
                        prod.PrecioVenta = request2.PrecioUnidad;
                        prod.Total = (request2.PrecioUnidad * cantDevolver);
                        ProductosADevolverValorConcepto.Add(prod);

                        dataPDF = new SetDatosPDF(
                                Convert.ToDateTime(fechaProceso2), UltimoNroDocumentoNC, IDOperacionGlobalNC, false, false, _tipoDevolucionSelect, "0", _netoVC, 0, _netoVC, _ivaVC, _totalVC, _motivoDev
                                );

                        var response = await servicio.HttpClientInstance.PostAsJsonAsync<RequestIngresaSolicitudNCDetalle>(urlEstadoNCDetalle, request2);
                        if (!response.IsSuccessStatusCode)
                        {
                            _snackBar.Add("Error al crear solicitud", Severity.Error);
                        }
                        else
                        {
                            MontosNCDTO montosNC = new MontosNCDTO();
                            montosNC.IDAllGestEmpresa = 2;
                            montosNC.IDEmpresa = 1;
                            montosNC.AnnoProceso = DatosNC.AnnoProceso;
                            montosNC.IDOperacion = IDOperacionGlobalNC;
                            montosNC.Subtotal = Convert.ToInt32(_netoVC); ;
                            montosNC.Neto = Convert.ToInt32(_netoVC);
                            montosNC.PorcentajeDesc = 0;
                            montosNC.Descto = Convert.ToInt32(_descuentoVC);
                            montosNC.IVA = Convert.ToInt32(_ivaVC);
                            montosNC.Total = Convert.ToInt32(_totalVC);
                            montosNC.Nota = "Valor concepto";
                            var postEstadoSolicitudActualizar = await servicio.HttpClientInstance.PostAsJsonAsync<MontosNCDTO>($"{urlMontosNC}", montosNC);
                            if (!postEstadoSolicitud.IsSuccessStatusCode)
                            {
                                _snackBar.Add("Error al crear NC. Consultar a TI", Severity.Error);
                            }
                            else
                            {
                                _snackBar.Add("Nota de credito solicitada VC", Severity.Success);
                                await GenerarPDFPorValorConcepto(SetDatos());
                                await GetGenerarNC(IDOperacionGlobalNC);
                                RespuestaProcesarNCsap RespuestaNCConsulta = new();
                                RespuestaNCConsulta = Respuesta.Respuesta.FirstOrDefault();
                                if (RespuestaNCConsulta.msgResult.Length > 0 && RespuestaNCConsulta.msgResult != null)
                                {
                                    Console.WriteLine(Datos);
                                    var parameters = new DialogParameters<DialogConfirmacionNC> {
                                        { x => x.TextDialog, RespuestaNCConsulta.msgMessage },
                                        { x => x.Titulo,  RespuestaNCConsulta.msgValor}  ,
                                        { x => x.nombreBoton, "OK" },
                                        { x => x.ocultarcancelar, false}
                                        };
                                    var dialog = await DialogServices.ShowAsync<DialogConfirmacionNC>("Question", parameters);
                                    LimpiarValoresNC();
                                }

                                LimpiarValoresNC();
                            }

                        }
                    }

                    StateHasChanged();
                }
            }
            else
            {
                _snackBar.Add("Por favor seleccionar tipo de documento y tipo devolucion", Severity.Error);
            }

        }

        public SetDatosPDF SetDatos()
        {
            return dataPDF;
        }


        public async Task GenerarPDF(SetDatosPDF datos)
        {
            using (MemoryStream memory = exportService.CreatePdfNC(clientes, ProductosADevolver, datos, string.IsNullOrEmpty(appSatate.IDUsuario) ? "S/N" : appSatate.IDUsuario))
            {
                await js.SaveAs($"Solicitud NC {datos.NumeroSolicitud}.pdf", memory.ToArray());
            }

        }

        // método adaptado para valor/concepto
        public async Task GenerarPDFPorValorConcepto(SetDatosPDF datos)
        {
            using (MemoryStream memory = exportService.CreatePdfPorValorConcepto(clientes, ProductosADevolverValorConcepto, datos))
            {
                await js.SaveAs($"Solicitud NC {datos.NumeroSolicitud}.pdf", memory.ToArray());

            }

        }

        #region Clase PDF

        public class SetDatosPDF
        {
            private DateTime fechaActualizacion;
            private int numeroSolicitud;
            private int idOperacion;
            private bool documentoCompleto;
            private bool porDevolucion;
            private string descripcionTipoDevolucion;
            private string porcentajeDescuento;
            private double subTotal;
            private double descuento;
            private double neto;
            private double iva;
            private double total;
            private string comentario;

            public SetDatosPDF() { }

            public SetDatosPDF(DateTime fechaActualizacion, int numeroSolicitud, int idOperacion, bool documentoCompleto, bool porDevolucion,
                string descripcionTipoDevolucion, string porcentajeDescuento, double subTotal, double descuento, double neto, double iva, double total, string comentario)
            {
                this.fechaActualizacion = fechaActualizacion;
                this.numeroSolicitud = numeroSolicitud;
                this.idOperacion = idOperacion;
                this.documentoCompleto = documentoCompleto;
                this.porDevolucion = porDevolucion;
                this.descripcionTipoDevolucion = descripcionTipoDevolucion;
                this.porcentajeDescuento = porcentajeDescuento;
                this.subTotal = subTotal;
                this.descuento = descuento;
                this.neto = neto;
                this.iva = iva;
                this.total = total;
                this.comentario = comentario;
            }

            public DateTime FechaActualizacion { get => fechaActualizacion; set => fechaActualizacion = value; }
            public int NumeroSolicitud { get => numeroSolicitud; set => numeroSolicitud = value; }
            public bool DocumentoCompleto { get => documentoCompleto; set => documentoCompleto = value; }
            public bool PorDevolucion { get => porDevolucion; set => porDevolucion = value; }
            public string DescripcionTipoDevolucion { get => descripcionTipoDevolucion; set => descripcionTipoDevolucion = value; }
            public double SubTotal { get => subTotal; set => subTotal = value; }
            public double Descuento { get => descuento; set => descuento = value; }
            public double Neto { get => neto; set => neto = value; }
            public double Iva { get => iva; set => iva = value; }
            public double Total { get => total; set => total = value; }
            public int IdOperacion { get => idOperacion; set => idOperacion = value; }
            public string PorcentajeDescuento { get => porcentajeDescuento; set => porcentajeDescuento = value; }
            public string Comentario { get => comentario; set => comentario = value; }
        }

        #endregion

        Func<string, string> converterTipoDocumento = p => p;
    }

}

public class ArregloDedatosNC
{
    private string[] datosNC;

    public ArregloDedatosNC() { }

    public ArregloDedatosNC(string[] datosNC)
    {
        this.DatosNC = datosNC;
    }

    public string[] DatosNC { get => datosNC; set => datosNC = value; }
}


