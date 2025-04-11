using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;


namespace PortalAG_V2.Pages.Movimientos
{
    partial class SolicitudDeMovimientos
    {

        #region Variables

        private ClientFactory conexion;
        private ClientFactory conexionGuia;
        private ClientFactory conexionSAP;

        private string _numero { get; set; }

        private string descripcionArticulo = "";
        private bool _processing = false;
        private bool _buscarProcessing = false;
        private bool _agregarProcessing = false;
        private bool _listoProcessing = false;
        private bool _processing1 = false;
        private bool _desactivar = true;
        private bool _desactivarGuia = false;
        private string _bodegaDesde;
        private int _numeroGuia;
        private int _numeroGuia2;
        private int _idGuia;
        private string _observaciones = "";
        private string _siglaBodegaDesde = "";
        private string _siglaBodegaHasta = "";
        private int _idBodegaDesde = 0;
        private int _idBodegaHasta = 0;

        private string _idArticulo310;
        private string _nombre310;
        private int _nroPallet310;
        private string _Bodega310;
        private int _idUbicacionDesde310;
        private int _idUbicacionHasta310;
        private int _bultos310;
        private int _unidadxBulto310;

        private string _idArticulo123;
        private string _nombre123;
        private int _nroPallet123;
        private string _Bodega123;
        private int _idUbicacionDesde123;
        private int _idUbicacionHasta123;
        private int _bultos123;
        private int _unidadxBulto123;
        private string _nombreArticulo123 = "";
        private string _nombreArticulo = "";
        private string _ubicacionDestino = "";

        private int _subTipo;

        private string _searchString;
        private int _ubicacionDesde = 0;
        //private int _ubicacionHasta = 0;
        private string idArticulo = "";

        //private bool _pallet { get; set; } = false;

        private List<UbicacionArticuloMayor> Elements = new List<UbicacionArticuloMayor>();
        private List<UbicacionDesde> Elements1 = new List<UbicacionDesde>();
        private List<UbicacionHasta> Elements2 = new List<UbicacionHasta>();
        private List<DetalleRecepcion> Detalle = new List<DetalleRecepcion>();
        private List<DetalleRecepcion310> Detalle310 = new List<DetalleRecepcion310>();
        private List<DetalleRecepcion123> Detalle123 = new List<DetalleRecepcion123>();
        public List<ItemsGrilla> listaDetalleSAP = new List<ItemsGrilla>();

        public List<Bodegas> _listBodega = new List<Bodegas>();
        public List<Bodegas> _listBodegaDesde = new List<Bodegas>();
        public List<Bodegas> _listBodegaHasta = new List<Bodegas>();
        Bodegas mostrarBodegasDesde = new Bodegas();
        Bodegas mostrarBodegasHasta = new Bodegas();

        //private List<UbicacionArticulo> Elements1 = new List<UbicacionArticulo>();

        private List<TipoOperacionModel> _listTipoSolicitud = new List<TipoOperacionModel>();
        TipoOperacionModel MostrarSolicitudDesde = new TipoOperacionModel();

        #endregion

        #region Endpoints

        private const string urlTipoSolicitud = "api/v2/MovimientoBodegas/TipoSolicitud/";
        private const string urlListaUbicacion = "api/v2/MovimientoBodegas/UbicacionArticulos";
        private const string urlListaUbicacion1 = "api/v2/MovimientoBodegas/UbicacionArticulos";
        private const string urlReposicion300 = "api/v2/MovimientoBodegas/Reposicion300";
        private const string urlReubicacion = "api/v2/MovimientoBodegas/Reubicacion";
        private const string urlTraspaso = "api/v2/MovimientoBodegas/Traspaso";
        private const string urlCrearGuia = "/api/v2/MovimientoBodegas/CrearGuia";
        const string UrlTraspasoSAP = "api/v2/MovimientoBodegas/EnvioSAP";
        const string UrlTraspasoEspecialSAP = "api/v2/MovimientoBodegas/TraspasoEspecialParaSAP";
        const string urlBodega = "api/v2/MovimientoBodegas/ListaBodegas";

        //private const string urlGenerarSolicitud = "/api/v2/Autorizacion/ProcesarSolicitud";

        #endregion

        protected override async Task OnInitializedAsync()
        {
            await ConsultarOperacions();
            await ConsultaInicialBodegas();
        }
        private async Task ConsultarOperacions()
        {
            conexion = new MainServices().SolMovimiento;
            var auxSolicitud = await conexion.HttpClientInstance.GetAsync($"{urlTipoSolicitud}");
            try
            {
                _listTipoSolicitud = JsonConvert.DeserializeObject<List<TipoOperacionModel>>(await auxSolicitud.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _snackBar.Add("Error al deserealizar", Severity.Error);
            }
        }

        private async Task ConsultaInicialBodegas(int localidadSelection = 2)
        {


            conexion = new MainServices().ConectionService;
            var auxBodega = await conexion.HttpClientInstance.GetAsync($"{urlBodega}/{localidadSelection}");
            if (auxBodega.IsSuccessStatusCode)
            {
                try
                {
                    _listBodega = JsonConvert.DeserializeObject<List<Bodegas>>(await auxBodega.Content.ReadAsStringAsync());
                    _listBodegaDesde = JsonConvert.DeserializeObject<List<Bodegas>>(await auxBodega.Content.ReadAsStringAsync());
                    _listBodegaHasta = JsonConvert.DeserializeObject<List<Bodegas>>(await auxBodega.Content.ReadAsStringAsync());

                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error al deserealizar Bodegas", Severity.Error);
                }

            }
            else
            {
                _snackBar.Add("Error al consultar Bodegas", Severity.Error);
            }

        }

        public class DetalleRecepcion
        {
            public string IDArticulo { get; set; }
            public string Nombre { get; set; }
            public int NroPallet { get; set; }
            public int IDUbicacion { get; set; }
            public string BodegaDesde { get; set; }
            public int Bultos { get; set; }
            public int UnidadxBultos { get; set; }
        }

        #region Cambio de Solicitud 
        private async Task LimpiarCrear()
        {


            _processing = false;
            _desactivar = true;
            _ubicacionDestino = "";
            bodegaDesde = new();
            bodegaDesde1 = new();
            bodegaHasta1 = new();
            bodegaHasta = new();
        }

        private async Task Solicitud(TipoOperacionModel args)
        {
            MostrarSolicitudDesde = args;
            if (args.IDTipoOperacion == 310)
            {
                await ConsultaInicialBodegas(args.IDTipoOperacion);
            }
            else
            {
                await ConsultaInicialBodegas(2);

            }
            Elements.Clear();
            Elements1.Clear();
            Elements2.Clear();
            Detalle.Clear();
            Detalle123.Clear();
            Detalle310.Clear();
            _numero = "";
            _nombreArticulo = "";
            _nombreArticulo123 = "";
            _processing = false;
            _desactivar = true;
            _ubicacionDestino = "";
            mostrarBodegasDesde = new() { };
            mostrarBodegasDesde = new() { };
            bodegaDesde = new();
            bodegaDesde1 = new();
            bodegaHasta1 = new();
            bodegaHasta = new();
        }
        #endregion
        private async Task ListDesde(Bodegas args)
        {
            mostrarBodegasDesde = args;
            mostrarBodegasHasta = new Bodegas();
            if (MostrarSolicitudDesde.IDTipoOperacion == 310)
            {
                await ConsultaInicialBodegas(MostrarSolicitudDesde.IDTipoOperacion);
            }
            else
            {
                await ConsultaInicialBodegas(2);

            }
            _listBodegaHasta.RemoveAll(x => x.SiglaBodega == args.SiglaBodega);
            StateHasChanged();
        }
        private async Task ListHasta(Bodegas args)
        {
            mostrarBodegasHasta = args;
        }

        #region Consultar Ubicacion Articulo/Pallet

        private async Task ListaUbicacion()
        {
            _buscarProcessing = true;
            var siPallet = 0;

            var bodegaBPM = "BV_BPM";
            var bodegaBIT = "BV_BIT";
            var bodegaBVN = "BV_BVN";

            conexion = new MainServices().SolMovimiento;

            if (MostrarSolicitudDesde.IDTipoOperacion == 123)
            {
                if (!System.String.IsNullOrEmpty(mostrarBodegasDesde.SiglaBodega))
                {
                    if (!System.String.IsNullOrEmpty(mostrarBodegasHasta.SiglaBodega))
                    {
                        if (!System.String.IsNullOrEmpty(_numero))
                        {
                            var auxListaUbicacion = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion}/{_numero}/{mostrarBodegasDesde.SiglaBodega}/{3}");
                            try
                            {
                                if (auxListaUbicacion.IsSuccessStatusCode)
                                {
                                    Elements1 = JsonConvert.DeserializeObject<List<UbicacionDesde>>(await auxListaUbicacion.Content.ReadAsStringAsync());
                                    try
                                    {
                                        _nombreArticulo123 = Elements1[0].Nombre;
                                    }
                                    catch (Exception ex)
                                    {
                                        _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                                    }
                                }
                                else
                                {
                                    _snackBar.Add("Articulo no encontrado!", Severity.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                            }

                            var auxListaUbicacion1 = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion1}/{_numero}/{mostrarBodegasHasta.SiglaBodega}/{4}");
                            try
                            {
                                if (auxListaUbicacion1.IsSuccessStatusCode)
                                {
                                    Elements2 = JsonConvert.DeserializeObject<List<UbicacionHasta>>(await auxListaUbicacion1.Content.ReadAsStringAsync());
                                    try
                                    {
                                        _nombreArticulo = Elements1[0].Nombre;
                                    }
                                    catch (Exception ex)
                                    {
                                        _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                                    }
                                }
                                else
                                {
                                    _snackBar.Add("Articulo no encontrado!", Severity.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                            }
                        }
                        else
                        {
                            _snackBar.Add("Por favor ingrese ID Articulo!", Severity.Warning);
                        }

                    }
                    else
                    {
                        _snackBar.Add("Por favor seleccione bodega hasta!", Severity.Warning);
                    }
                }
                else
                {
                    _snackBar.Add("Por favor seleccione bodega desde!", Severity.Warning);
                }
            }

            if (MostrarSolicitudDesde.IDTipoOperacion == 270)
            {
                var auxListaUbicacion = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion}/{_numero}/{"2"}/{siPallet}");
                try
                {
                    Elements = JsonConvert.DeserializeObject<List<UbicacionArticuloMayor>>(await auxListaUbicacion.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error al deserealizar Lista ubicacion", Severity.Error);
                }
            }

            if (MostrarSolicitudDesde.IDTipoOperacion == 280)
            {
                if (!System.String.IsNullOrEmpty(_numero))
                {
                    var auxListaUbicacionMayor = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion}/{_numero}/{"BV_BPM"}/{3}");
                    try
                    {
                        if (auxListaUbicacionMayor.IsSuccessStatusCode)
                        {
                            Elements = JsonConvert.DeserializeObject<List<UbicacionArticuloMayor>>(await auxListaUbicacionMayor.Content.ReadAsStringAsync());
                            try
                            {
                                _nombreArticulo = Elements[0].Nombre;
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                            }
                        }
                        else
                        {
                            _snackBar.Add("Articulo no encontrado!", Severity.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                    }

                    var auxListaUbicacion = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion}/{_numero}/{"BV_BIT"}/{3}");
                    try
                    {
                        if (auxListaUbicacion.IsSuccessStatusCode)
                        {
                            Elements1 = JsonConvert.DeserializeObject<List<UbicacionDesde>>(await auxListaUbicacion.Content.ReadAsStringAsync());
                            try
                            {
                                _ubicacionDestino = Elements1[0].Ubicacion;
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                            }
                        }
                        else
                        {
                            _snackBar.Add("Articulo no encontrado!", Severity.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                    }

                }
                else
                {
                    _snackBar.Add("Por favor ingrese ID Articulo!", Severity.Warning);
                }
            }

            if (MostrarSolicitudDesde.IDTipoOperacion == 290)
            {
                if (!System.String.IsNullOrEmpty(_numero))
                {
                    var auxListaUbicacionMayor = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion}/{_numero}/{"BV_BPM"}/{3}");
                    try
                    {
                        if (auxListaUbicacionMayor.IsSuccessStatusCode)
                        {
                            Elements = JsonConvert.DeserializeObject<List<UbicacionArticuloMayor>>(await auxListaUbicacionMayor.Content.ReadAsStringAsync());
                            try
                            {
                                _nombreArticulo = Elements[0].Nombre;
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                            }
                        }
                        else
                        {
                            _snackBar.Add("Articulo no encontrado!", Severity.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                    }

                    var auxListaUbicacion = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion}/{_numero}/{"BV_BVN"}/{3}");
                    try
                    {
                        if (auxListaUbicacion.IsSuccessStatusCode)
                        {
                            Elements1 = JsonConvert.DeserializeObject<List<UbicacionDesde>>(await auxListaUbicacion.Content.ReadAsStringAsync());
                            try
                            {
                                _ubicacionDestino = Elements1[0].Ubicacion;
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                            }
                        }
                        else
                        {
                            _snackBar.Add("Articulo no encontrado!", Severity.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                    }

                }
                else
                {
                    _snackBar.Add("Por favor ingrese ID Articulo!", Severity.Warning);
                }

            }

            if (MostrarSolicitudDesde.IDTipoOperacion == 285)
            {
                if (!System.String.IsNullOrEmpty(_numero))
                {
                    var auxListaUbicacionMayor = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion}/{_numero}/{"BV_BIT"}/{3}");
                    try
                    {
                        if (auxListaUbicacionMayor.IsSuccessStatusCode)
                        {
                            Elements = JsonConvert.DeserializeObject<List<UbicacionArticuloMayor>>(await auxListaUbicacionMayor.Content.ReadAsStringAsync());
                            try
                            {
                                _nombreArticulo = Elements[0].Nombre;
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                            }
                        }
                        else
                        {
                            _snackBar.Add("Articulo no encontrado!", Severity.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                    }

                    var auxListaUbicacion = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion}/{_numero}/{"BV_BVN"}/{3}");
                    try
                    {
                        if (auxListaUbicacion.IsSuccessStatusCode)
                        {
                            Elements1 = JsonConvert.DeserializeObject<List<UbicacionDesde>>(await auxListaUbicacion.Content.ReadAsStringAsync());
                            try
                            {
                                _ubicacionDestino = Elements1[0].Ubicacion;
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                            }
                        }
                        else
                        {
                            _snackBar.Add("Articulo no encontrado!", Severity.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                    }

                }
                else
                {
                    _snackBar.Add("Por favor ingrese ID Articulo!", Severity.Warning);
                }

            }

            if (MostrarSolicitudDesde.IDTipoOperacion == 310)
            {
                if (!System.String.IsNullOrEmpty(mostrarBodegasDesde.SiglaBodega))
                {
                    if (!System.String.IsNullOrEmpty(_numero))
                    {
                        var auxListaUbicacion = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion}/{_numero}/{mostrarBodegasDesde.SiglaBodega}/{3}");
                        try
                        {
                            if (auxListaUbicacion.IsSuccessStatusCode)
                            {
                                Elements1 = JsonConvert.DeserializeObject<List<UbicacionDesde>>(await auxListaUbicacion.Content.ReadAsStringAsync());
                                try
                                {
                                    _nombreArticulo = Elements1[0].Nombre;
                                }
                                catch (Exception ex)
                                {
                                    _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                                }
                            }
                            else
                            {
                                _snackBar.Add("Articulo no encontrado!", Severity.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                        }

                        var auxListaUbicacion1 = await conexion.HttpClientInstance.GetAsync($"{urlListaUbicacion1}/{_numero}/{mostrarBodegasDesde.SiglaBodega}/{4}");
                        try
                        {
                            if (auxListaUbicacion1.IsSuccessStatusCode)
                            {
                                Elements2 = JsonConvert.DeserializeObject<List<UbicacionHasta>>(await auxListaUbicacion1.Content.ReadAsStringAsync());
                                try
                                {
                                    _nombreArticulo = Elements1[0].Nombre;
                                }
                                catch (Exception ex)
                                {
                                    _snackBar.Add("Articulo no encontrado o no disponible en bodega", Severity.Error);
                                }
                            }
                            else
                            {
                                _snackBar.Add("Articulo no encontrado!", Severity.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            _snackBar.Add("Error en consultar API" + ex.Message, Severity.Error);
                        }
                    }
                    else
                    {
                        _snackBar.Add("Por favor ingrese ID Articulo!", Severity.Warning);
                    }
                }
                else
                {
                    _snackBar.Add("Por favor seleccione bodega desde!", Severity.Warning);
                }
            }

            await Task.Delay(1000);
            _buscarProcessing = false;
        }

        #endregion



        #region Agregar Detalle
        private async void clickDesde(UbicacionArticuloMayor args)
        {
            if (args != null)
            {

                var parameters = new DialogParameters
            {
                { nameof(Dialog.args), args}
            };

                var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall, FullWidth = false };

                var dialogo = _dialogService.Show<Dialog>("�Cuantos bultos quiere mover?", parameters, options);
                var result = await dialogo.Result;

                if (!result.Cancelled)
                {

                    var _bulto = result.Data.ToString();

                    if (MostrarSolicitudDesde.IDTipoOperacion == 280)
                    {
                        Detalle.Add(new DetalleRecepcion
                        {
                            IDArticulo = _numero,
                            Nombre = args.Nombre,
                            NroPallet = args.NroPallet,
                            IDUbicacion = args.IDUbicacion,
                            BodegaDesde = "BV_BPM",
                            Bultos = int.Parse(_bulto),
                            UnidadxBultos = args.UnidadPorBulto
                        });
                    }
                    else if (MostrarSolicitudDesde.IDTipoOperacion == 290)
                    {
                        Detalle.Add(new DetalleRecepcion
                        {
                            IDArticulo = _numero,
                            Nombre = args.Nombre,
                            NroPallet = args.NroPallet,
                            IDUbicacion = args.IDUbicacion,
                            BodegaDesde = "BV_BPM",
                            Bultos = int.Parse(_bulto),
                            UnidadxBultos = args.UnidadPorBulto
                        });
                    }
                    else if (MostrarSolicitudDesde.IDTipoOperacion == 285)
                    {
                        Detalle.Add(new DetalleRecepcion
                        {
                            IDArticulo = _numero,
                            Nombre = args.Nombre,
                            NroPallet = args.NroPallet,
                            IDUbicacion = args.IDUbicacion,
                            BodegaDesde = "BV_BIT",
                            Bultos = int.Parse(_bulto),
                            UnidadxBultos = args.UnidadPorBulto
                        });
                    }

                    _snackBar.Add("Agregado!", Severity.Info);
                    StateHasChanged();
                }
            }
            else
            {
                bodegaDesde = new();
            }
        }

        private async void ClickDesde310(UbicacionDesde argsDesde310)
        {
            if (argsDesde310 != null && !bodegaDesde1.Any(x => x.IDArticulo == argsDesde310.IDArticulo))
            {

                bodegaDesde1 = new();
                bodegaDesde1.Add(argsDesde310);
                if (mostrarBodegasDesde.SiglaBodega == "BV_BPM")
                {
                    _nombre310 = argsDesde310.Nombre;
                    _nroPallet310 = argsDesde310.NroPallet;
                    _Bodega310 = mostrarBodegasDesde.SiglaBodega;
                    _idUbicacionDesde310 = argsDesde310.IDUbicacion;
                    _bultos310 = argsDesde310.Cantidad;
                    _unidadxBulto310 = argsDesde310.UnidadPorBulto;
                    StateHasChanged();
                }
                else
                {
                    _nombre310 = argsDesde310.Nombre;
                    _nroPallet310 = 0;
                    _Bodega310 = mostrarBodegasDesde.SiglaBodega;
                    _idUbicacionDesde310 = argsDesde310.IDUbicacion;
                    _bultos310 = argsDesde310.Cantidad;
                    _unidadxBulto310 = argsDesde310.UnidadPorBulto;
                    StateHasChanged();
                }
            }
            else
            {
                bodegaDesde1 = new();
            }
        }

        private async void ClickHasta310(UbicacionHasta argsHasta310)
        {
            if (argsHasta310 != null && !bodegaHasta1.Any(x => x.IDArticulo == argsHasta310.IDArticulo))
            {
                bodegaHasta1 = new();
                bodegaHasta1.Add(argsHasta310);
                _idUbicacionHasta310 = argsHasta310.IDUbicacion;
            }
            else
            {
                bodegaHasta1 = new();
            }

        }

        private void ClickDesde123(UbicacionDesde argsDesde123)
        {
            if (argsDesde123 != null && !bodegaDesde1.Any(x => x.IDArticulo == argsDesde123.IDArticulo && x.IDUbicacion == argsDesde123.IDUbicacion))
            {
                bodegaDesde1 = new();
                bodegaDesde1.Add(argsDesde123);
            }
            else
            {
                bodegaDesde1 = new();
            }
        }


        private void ClickHasta123(UbicacionHasta argsHasta123)
        {
            if (argsHasta123 != null && !bodegaHasta1.Any(x => x.IDArticulo == argsHasta123.IDArticulo && x.IDUbicacion == argsHasta123.IDUbicacion))
            {
                bodegaHasta1 = new();
                bodegaHasta1.Add(argsHasta123);
                if (mostrarBodegasDesde.SiglaBodega == "BV_BVN")
                {
                    _idUbicacionHasta123 = argsHasta123.IDUbicacion;
                    _nroPallet123 = argsHasta123.NroPallet;
                }
                else
                {
                    _idUbicacionHasta123 = argsHasta123.IDUbicacion;
                }
            }
            else
            {
                bodegaHasta1 = new();

            }
            StateHasChanged();
        }

        #endregion

        #region Envio AG/SAP

        private async Task Listo()
        {
            conexionGuia = new MainServices().CrearGuia;
            _processing1 = true;
            await Task.Delay(3000);

            if (MostrarSolicitudDesde.IDTipoOperacion == 280) //Reposicion a intermedia
            {
                _siglaBodegaDesde = "BV_BPM";
                _siglaBodegaHasta = "BV_BIT";
                _idBodegaDesde = 21;
                _idBodegaHasta = 22;
                _observaciones = "Solicitud Reposicion 300 - 280";
            }
            else if (MostrarSolicitudDesde.IDTipoOperacion == 285) //Reposicion de intermedia a venta
            {
                _siglaBodegaDesde = "BV_BIT";
                _siglaBodegaHasta = "BV_BVN";
                _idBodegaDesde = 22;
                _idBodegaHasta = 299;
                _observaciones = "Solicitud Reposicion 300 - 285";
            }
            else if (MostrarSolicitudDesde.IDTipoOperacion == 290) //Reposicion a venta
            {
                _siglaBodegaDesde = "BV_BPM";
                _siglaBodegaHasta = "BV_BVN";
                _idBodegaDesde = 21;
                _idBodegaHasta = 299;
                _observaciones = "Solicitud Reposicion 300 - 290";
            }

            CabeceraTraspasoDTO cabeceraGuia = new CabeceraTraspasoDTO
            {
                IDBodegaDesde = _idBodegaDesde,
                BodegaDesde = _siglaBodegaDesde,
                IDBodegaHasta = _idBodegaHasta,
                BodegaHasta = _siglaBodegaHasta,
                Comentario = _observaciones,
                IDEstado = 9,
                Fecha = DateTime.Now.ToString(),
                IDTipoGuia = 101,
                IDGuia = 0,
                NumeroGuia = 0
            };

            List<DetalleTraspasoDTO> detalleGuia;
            detalleGuia = new List<DetalleTraspasoDTO>();

            foreach (DetalleRecepcion x in Detalle)
            {
                detalleGuia.Add(
                new DetalleTraspasoDTO()
                {
                    Linea = 0,
                    Valido = false,
                    Estado = 0,
                    Codigo = x.IDArticulo,
                    Nombre = x.Nombre,
                    BodegaDes = _siglaBodegaDesde,
                    CantDes = x.Bultos.ToString(),
                    UnidadBultoDes = x.UnidadxBultos.ToString(),
                    BodegaHast = _siglaBodegaHasta,
                    CantHast = x.Bultos.ToString(),
                    UnidadBultoHast = x.UnidadxBultos.ToString(),
                    Total = (x.UnidadxBultos * x.Bultos).ToString(),
                    Comentario = _observaciones
                });
            }

            GuiaTraspasoDTO data = new GuiaTraspasoDTO()
            {
                Cabecera = cabeceraGuia,
                Detalle = detalleGuia
            };

            var agGuia = await conexionGuia.HttpClientInstance.PostAsJsonAsync<GuiaTraspasoDTO>(urlCrearGuia, data);
            if (agGuia.IsSuccessStatusCode)
            {

                _snackBar.Add("Cabecera creada!", Severity.Success);

                ResultGuiaMovimiento idGuia = JsonConvert.DeserializeObject<ResultGuiaMovimiento>(await agGuia.Content.ReadAsStringAsync());

                data.Cabecera.IDEstado = 0;
                data.Cabecera.IDTipoGuia = 0;
                data.Cabecera.IDGuia = idGuia.IDGuias;
                data.Cabecera.NumeroGuia = idGuia.NumeroGuia;

                var agGuiaa = await conexionGuia.HttpClientInstance.PostAsJsonAsync<GuiaTraspasoDTO>(urlCrearGuia, data);
                if (agGuiaa.IsSuccessStatusCode)
                {
                    _snackBar.Add("Guia creada y actualizada con exito!", Severity.Success);
                    _numeroGuia2 = data.Cabecera.NumeroGuia;
                    _numeroGuia = _numeroGuia2;
                    _idGuia = idGuia.IDGuias;
                    _snackBar.Add("Listo para enviar!", Severity.Info);
                    _desactivar = false;
                    _processing1 = false;
                }
                else
                {
                    _snackBar.Add("Error al actualizar guia!", Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al crear cabecera!", Severity.Error);
            }
        }

        private async Task Listo310()
        {
            _processing1 = true;
            await Task.Delay(2000);
            var user = await _authenticationManager.CurrentUser();
            if (!Detalle310.Any(x => x.IDArticulo == _numero))
            {
                Detalle310.Add(new DetalleRecepcion310
                {
                    TipoConsulta = 1,
                    Solicitud = 0,
                    IDArticulo = _numero,
                    Nombre = _nombre310,
                    NroPallet = _nroPallet310,
                    IDUbicacionDesde = bodegaDesde1.FirstOrDefault().IDUbicacion,
                    IDUbicacionHasta = bodegaHasta1.FirstOrDefault().IDUbicacion,
                    BodegaDesde = _Bodega310,
                    Bultos = _bultos310,
                    UnidadxBultos = _unidadxBulto310,
                    IDUsuario = user.GetUserId()
                });
                _desactivar = false;
                _processing1 = false;
                bodegaHasta1 = new();
                bodegaDesde1 = new();
                _snackBar.Add("Agregado!", Severity.Info);
                StateHasChanged();
            }
            else
            {
                _desactivar = false;
                _processing1 = false;
                _snackBar.Add("Articulo ya esta agregado!", Severity.Warning);
                StateHasChanged();
            }
        }

        private async Task Agregar123()
        {
            var parameters = new DialogParameters
            {
                { nameof(Dialog123.args), bodegaDesde1.FirstOrDefault() },
                { nameof(Dialog123.ubiHasta), mostrarBodegasHasta },
                { nameof(Dialog123.ubiDesde), mostrarBodegasDesde }
            };
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall, FullWidth = false };
            var dialogo = _dialogService.Show<Dialog123>("�Cuantos bultos quiere mover?", parameters, options);
            var result = await dialogo.Result;

            if (!result.Canceled)
            {
                var _data = (Dialog123.Datos)result.Data;

                _nombre123 = bodegaDesde1.FirstOrDefault().Nombre;
                _nroPallet123 = bodegaDesde1.FirstOrDefault().NroPallet;
                _Bodega123 = mostrarBodegasDesde.SiglaBodega;
                _idUbicacionDesde123 = bodegaDesde1.FirstOrDefault().IDUbicacion;
                _bultos123 = (int)_data.Cantidad;
                _unidadxBulto123 = (int)_data.Unidades;
                _agregarProcessing = true;

                var user = await _authenticationManager.CurrentUser();

                if ((mostrarBodegasDesde.SiglaBodega == "BV_BVN" && mostrarBodegasHasta.SiglaBodega == "BV_BIT") || // Ventas  -> Intermedia
                    (mostrarBodegasDesde.SiglaBodega == "BV_BVN" && mostrarBodegasHasta.SiglaBodega == "BV_BPM") || // Ventas  -> Mayor
                    (mostrarBodegasDesde.SiglaBodega == "BV_BIT" && mostrarBodegasHasta.SiglaBodega == "BV_BPM") || // Intermedia -> Mayor
                    (!mostrarBodegasDesde.SiglaBodega.Contains("BV_") && !mostrarBodegasHasta.SiglaBodega.Contains("BV_")) || // Cuando no son de CDA
                   (mostrarBodegasDesde.SiglaBodega.Contains("BV_") && !mostrarBodegasHasta.SiglaBodega.Contains("BV_")))// Cuando es CDA -> Cuando no es CDA
                {
                    Detalle123.Add(new DetalleRecepcion123
                    {
                        TipoConsulta = 6,
                        Solicitud = 0,
                        IDArticulo = _numero,
                        Nombre = _nombre123,
                        NroPallet = _nroPallet123,
                        IDUbicacionDesde = _idUbicacionDesde123,
                        IDUbicacionHasta = _idUbicacionHasta123,
                        BodegaDesde = _Bodega123,
                        Bultos = _bultos123,
                        UnidadxBultos = _unidadxBulto123,
                        IDUsuario = user.GetUserId()
                    });
                }
                else
                {
                    Detalle123.Add(new DetalleRecepcion123
                    {
                        TipoConsulta = 1,
                        Solicitud = 0,
                        IDArticulo = _numero,
                        Nombre = _nombre123,
                        NroPallet = _nroPallet123,
                        IDUbicacionDesde = _idUbicacionDesde123,
                        IDUbicacionHasta = _idUbicacionHasta123,
                        BodegaDesde = _Bodega123,
                        Bultos = _bultos123,
                        UnidadxBultos = _unidadxBulto123,
                        IDUsuario = user.GetUserId()
                    });
                }


                await Task.Delay(1000);
                await LimpiarCrear();
                _agregarProcessing = false;
                _snackBar.Add("Agregado!", Severity.Info);
                StateHasChanged();
            }

        }

        private async Task Listo123()
        {
            conexionGuia = new MainServices().CrearGuia;
            _listoProcessing = true;
            //Console.WriteLine(mostrarBodegasHasta.SiglaBodega);

            switch (mostrarBodegasDesde.SiglaBodega)
            {
                case "BPM":
                    _idBodegaDesde = 1;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BDV":
                    _idBodegaDesde = 2;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BMT":
                    _idBodegaDesde = 3;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BVE":
                    _idBodegaDesde = 4;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BSR":
                    _idBodegaDesde = 5;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BVM":
                    _idBodegaDesde = 7;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BPR":
                    _idBodegaDesde = 8;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BVT":
                    _idBodegaDesde = 9;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BRV":
                    _idBodegaDesde = 10;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BFI":
                    _idBodegaDesde = 11;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BV_BPM":
                    _idBodegaDesde = 21;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BV_BIT":
                    _idBodegaDesde = 22;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BV_BMT":
                    _idBodegaDesde = 23;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BV_BRE":
                    _idBodegaDesde = 24;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BV_BME":
                    _idBodegaDesde = 27;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BVN":
                    _idBodegaDesde = 99;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                case "BV_BVN":
                    _idBodegaDesde = 299;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
                default:
                    _idBodegaDesde = 0;
                    _siglaBodegaDesde = mostrarBodegasDesde.SiglaBodega;
                    break;
            }

            switch (mostrarBodegasHasta.SiglaBodega)
            {
                case "BPM":
                    _idBodegaHasta = 1;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BDV":
                    _idBodegaHasta = 2;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BMT":
                    _idBodegaHasta = 3;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BVE":
                    _idBodegaHasta = 4;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BSR":
                    _idBodegaHasta = 5;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BVM":
                    _idBodegaHasta = 7;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BPR":
                    _idBodegaHasta = 8;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BVT":
                    _idBodegaHasta = 9;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BRV":
                    _idBodegaHasta = 10;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BFI":
                    _idBodegaHasta = 11;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BV_BPM":
                    _idBodegaHasta = 21;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BV_BIT":
                    _idBodegaHasta = 22;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BV_BMT":
                    _idBodegaHasta = 23;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BV_BRE":
                    _idBodegaHasta = 24;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BV_BME":
                    _idBodegaHasta = 27;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BVN":
                    _idBodegaHasta = 99;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                case "BV_BVN":
                    _idBodegaHasta = 299;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
                default:
                    _idBodegaHasta = 0;
                    _siglaBodegaHasta = mostrarBodegasHasta.SiglaBodega;
                    break;
            }
            _observaciones = "Traspaso";

            Console.WriteLine(_siglaBodegaHasta);

            CabeceraTraspasoDTO cabeceraGuia = new CabeceraTraspasoDTO
            {
                IDBodegaDesde = _idBodegaDesde,
                BodegaDesde = _siglaBodegaDesde,
                IDBodegaHasta = _idBodegaHasta,
                BodegaHasta = _siglaBodegaHasta,
                Comentario = _observaciones,
                IDEstado = 9,
                Fecha = DateTime.Now.ToString(),
                IDTipoGuia = 101,
                IDGuia = 0,
                NumeroGuia = 0
            };

            List<DetalleTraspasoDTO> detalleGuia;
            detalleGuia = new List<DetalleTraspasoDTO>();

            foreach (DetalleRecepcion123 x in Detalle123)
            {
                detalleGuia.Add(
                new DetalleTraspasoDTO()
                {
                    Linea = 0,
                    Valido = false,
                    Estado = 0,
                    Codigo = x.IDArticulo,
                    Nombre = x.Nombre,
                    BodegaDes = _siglaBodegaDesde,
                    CantDes = x.Bultos.ToString(),
                    UnidadBultoDes = x.UnidadxBultos.ToString(),
                    BodegaHast = _siglaBodegaHasta,
                    CantHast = x.Bultos.ToString(),
                    UnidadBultoHast = x.UnidadxBultos.ToString(),
                    Total = (x.UnidadxBultos * x.Bultos).ToString(),
                    Comentario = _observaciones
                });
            }

            GuiaTraspasoDTO data = new GuiaTraspasoDTO()
            {
                Cabecera = cabeceraGuia,
                Detalle = detalleGuia
            };

            var agGuia = await conexionGuia.HttpClientInstance.PostAsJsonAsync<GuiaTraspasoDTO>(urlCrearGuia, data);
            if (agGuia.IsSuccessStatusCode)
            {
                ResultGuiaMovimiento idGuia = JsonConvert.DeserializeObject<ResultGuiaMovimiento>(await agGuia.Content.ReadAsStringAsync());

                data.Cabecera.IDEstado = 0;
                data.Cabecera.IDTipoGuia = 0;
                data.Cabecera.IDGuia = idGuia.IDGuias;
                data.Cabecera.NumeroGuia = idGuia.NumeroGuia;

                var agGuiaa = await conexionGuia.HttpClientInstance.PostAsJsonAsync<GuiaTraspasoDTO>(urlCrearGuia, data);
                if (agGuiaa.IsSuccessStatusCode)
                {
                    await Task.Delay(2000);
                    _numeroGuia2 = data.Cabecera.NumeroGuia;
                    _numeroGuia = _numeroGuia2;
                    _idGuia = idGuia.IDGuias;
                    _snackBar.Add("Listo para enviar!", Severity.Info);
                    _desactivar = false;
                    _listoProcessing = false;
                }
                else
                {
                    _snackBar.Add("Error al actualizar guia!", Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al crear cabecera!", Severity.Error);
            }

            StateHasChanged();
        }

        private async Task Enviar()
        {
            _processing = true;

            if (MostrarSolicitudDesde.IDTipoOperacion == 310)
            {
                var respuesta310 = await conexion.HttpClientInstance.PostAsJsonAsync<List<DetalleRecepcion310>>(urlReubicacion, Detalle310);
                try
                {
                    if (respuesta310.IsSuccessStatusCode)
                    {
                        await Task.Delay(3000);
                        _snackBar.Add("Enviado!", Severity.Success);
                        Elements.Clear();
                        Elements1.Clear();
                        Elements2.Clear();
                        Detalle.Clear();
                        Detalle310.Clear();
                        Detalle123.Clear();
                        _numero = "";
                        _numeroGuia = 0;
                        _processing = false;
                        _desactivar = true;
                        _nombreArticulo123 = "";
                    }
                    else
                    {
                        _snackBar.Add("Error al crear solicitud 310", Severity.Error);
                    }
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error al consultar API Lineas310: " + ex.Message, Severity.Error);
                }


            }
            else if (MostrarSolicitudDesde.IDTipoOperacion == 123)
            {
                conexionSAP = new MainServices().ConectionService;

                listaDetalleSAP = new List<ItemsGrilla>();
                foreach (DetalleRecepcion123 x in Detalle123)
                {
                    ItemsGrilla itemsGrilla = new ItemsGrilla
                    {
                        ItemCode = x.IDArticulo,
                        Quantity = (x.UnidadxBultos * x.Bultos).ToString(),
                        WarehouseCode = mostrarBodegasHasta.SiglaBodega
                    };
                    listaDetalleSAP.Add(itemsGrilla);
                }

                EnvioTraspaso envioTraspaso = new EnvioTraspaso
                {
                    Comments = _observaciones,
                    JournalMemo = "Guia N� " + _numeroGuia + " - " + MostrarSolicitudDesde.IDTipoOperacion,
                    FromWarehouse = mostrarBodegasDesde.SiglaBodega,
                    ToWarehouse = mostrarBodegasHasta.SiglaBodega,
                    StockTransferLines = listaDetalleSAP
                };
                HttpResponseMessage respuesta;
                HttpResponseMessage auxDetalle123;

                if ((envioTraspaso.FromWarehouse == "BPM") && envioTraspaso.ToWarehouse == "BV_BMT")
                {
                    var user = await _authenticationManager.CurrentUser();
                    var IDuse = user.GetFirstName();
                    string _IDUser = user.GetUserId();
                    conexionSAP.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                    respuesta = await conexionSAP.HttpClientInstance.GetAsync($"{UrlTraspasoEspecialSAP}/{_numeroGuia}");
                }
                else
                {
                    respuesta = await conexionSAP.HttpClientInstance.PostAsJsonAsync<EnvioTraspaso>(UrlTraspasoSAP, envioTraspaso);
                }
                try
                {
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var resAPI = JsonConvert.DeserializeObject<Response>(await respuesta.Content.ReadAsStringAsync());
                        if (resAPI.IsSuccess)
                        {
                            int DocEntryA = (System.String.IsNullOrEmpty(resAPI.Result.ToString()) ? 0 : int.Parse(resAPI.Result.ToString()));

                            var user = await _authenticationManager.CurrentUser();
                            conexion = new MainServices().SolMovimiento;
                            if ((envioTraspaso.FromWarehouse != "BPM") && envioTraspaso.ToWarehouse != "BV_BMT")
                            {
                                auxDetalle123 = await conexion.HttpClientInstance.PostAsJsonAsync<List<DetalleRecepcion123>>(urlTraspaso, Detalle123);
                                try
                                {
                                    if (auxDetalle123.IsSuccessStatusCode)
                                    {
                                        await Task.Delay(3000);
                                        _snackBar.Add("Enviado!", Severity.Success);
                                        Elements.Clear();
                                        Elements1.Clear();
                                        Elements2.Clear();
                                        Detalle.Clear();
                                        Detalle310.Clear();
                                        Detalle123.Clear();
                                        _numero = "";
                                        _numeroGuia = 0;
                                        _processing = false;
                                        _desactivar = true;
                                        _nombreArticulo123 = "";
                                    }
                                    else
                                    {
                                        _snackBar.Add("Error al crear solicitud 123", Severity.Error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _snackBar.Add("Error al consultar API Lineas123: " + ex.Message, Severity.Error);
                                }
                            }
                            else
                            {
                                await Task.Delay(3000);
                                _snackBar.Add("Enviado!", Severity.Success);
                                Elements.Clear();
                                Elements1.Clear();
                                Elements2.Clear();
                                Detalle.Clear();
                                Detalle310.Clear();
                                Detalle123.Clear();
                                _numero = "";
                                _numeroGuia = 0;
                                _processing = false;
                                _desactivar = true;
                                _nombreArticulo123 = "";
                            }
                        }
                        else
                        {
                            resAPI = JsonConvert.DeserializeObject<Response>(await respuesta.Content.ReadAsStringAsync());

                            _snackBar.Add(resAPI.Message, Severity.Error);
                        }
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<Response>(await respuesta.Content.ReadAsStringAsync());
                        _snackBar.Add("Error en traspaso a SAP", Severity.Error);
                    }

                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error consulta API SAP: " + ex.Message, Severity.Error);
                }
                _processing = false;
                { }
            }
            else
            {
                conexionSAP = new MainServices().SAP;

                listaDetalleSAP = new List<ItemsGrilla>();
                foreach (DetalleRecepcion x in Detalle)
                {
                    ItemsGrilla itemsGrilla = new ItemsGrilla
                    {
                        ItemCode = x.IDArticulo,
                        Quantity = (x.UnidadxBultos * x.Bultos).ToString(),
                        WarehouseCode = _siglaBodegaHasta
                    };
                    listaDetalleSAP.Add(itemsGrilla);
                }

                EnvioTraspaso envioTraspaso = new EnvioTraspaso
                {
                    Comments = _observaciones,
                    JournalMemo = "Guia N� " + _numeroGuia + " - " + MostrarSolicitudDesde.IDTipoOperacion,
                    FromWarehouse = _siglaBodegaDesde,
                    ToWarehouse = _siglaBodegaHasta,
                    StockTransferLines = listaDetalleSAP
                };

                if (MostrarSolicitudDesde.IDTipoOperacion == 280)
                {
                    _subTipo = 280;
                }
                else if (MostrarSolicitudDesde.IDTipoOperacion == 285)
                {
                    _subTipo = 285;
                }
                else if (MostrarSolicitudDesde.IDTipoOperacion == 290)
                {
                    _subTipo = 290;
                }

                var respuesta = await conexionSAP.HttpClientInstance.PostAsJsonAsync<EnvioTraspaso>(UrlTraspasoSAP, envioTraspaso);
                try
                {
                    if (respuesta.IsSuccessStatusCode)
                    {
                        var resAPI = JsonConvert.DeserializeObject<Response>(await respuesta.Content.ReadAsStringAsync());
                        if (resAPI.IsSuccess)
                        {
                            int DocEntryA = (System.String.IsNullOrEmpty(resAPI.Result.ToString()) ? 0 : int.Parse(resAPI.Result.ToString()));

                            var user = await _authenticationManager.CurrentUser();

                            List<DetalleRecepcion300> _detalleRecepcion;
                            _detalleRecepcion = new List<DetalleRecepcion300>();

                            foreach (DetalleRecepcion x in Detalle)
                            {
                                _detalleRecepcion.Add(
                                    new DetalleRecepcion300
                                    {
                                        SubTipoSolicitud = _subTipo,
                                        IDArticulo = x.IDArticulo,
                                        NroPallet = x.NroPallet,
                                        Bultos = x.Bultos,
                                        UnidadxBulto = x.UnidadxBultos,
                                        IDUbicacion = x.IDUbicacion,
                                        DocEntry = DocEntryA,
                                        IDGuia = _idGuia,
                                        IDUsuario = user.GetUserId()
                                    });
                            }

                            conexion = new MainServices().SolMovimiento;
                            var auxDetalle300 = await conexion.HttpClientInstance.PostAsJsonAsync<List<DetalleRecepcion300>>(urlReposicion300, _detalleRecepcion);
                            try
                            {
                                if (auxDetalle300.IsSuccessStatusCode)
                                {
                                    await Task.Delay(3000);
                                    _snackBar.Add("Enviado!", Severity.Success);
                                    Elements.Clear();
                                    Elements1.Clear();
                                    Elements2.Clear();
                                    Detalle.Clear();
                                    _numero = "";
                                    _numeroGuia = 0;
                                    _processing = false;
                                    _desactivar = true;
                                    _nombreArticulo = "";
                                    _nombreArticulo123 = "";
                                    _ubicacionDestino = "";
                                }
                                else
                                {
                                    _snackBar.Add("Error al crear solicitud 300", Severity.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                _snackBar.Add("Error al consultar API Lineas300: " + ex.Message, Severity.Error);
                            }
                        }
                        else
                        {
                            resAPI = JsonConvert.DeserializeObject<Response>(await respuesta.Content.ReadAsStringAsync());

                            _snackBar.Add(resAPI.Message, Severity.Error);
                        }
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<Response>(await respuesta.Content.ReadAsStringAsync());
                        _snackBar.Add("Error en traspaso a SAP", Severity.Error);

                    }
                    _processing = false;
                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error consulta API SAP: " + ex.Message, Severity.Error);
                    _processing = false;
                }
            }

            _processing = false;
        }

        #endregion

        #region Borrar Lineas
        private async Task BorrarLinea(DetalleRecepcion args)
        {
            Detalle.RemoveAt(Detalle.FindIndex(x => x.IDArticulo == args.IDArticulo && x.IDUbicacion == args.IDUbicacion && x.UnidadxBultos == args.UnidadxBultos));
            StateHasChanged();
        }

        private async Task BorrarLinea310(DetalleRecepcion310 args310)
        {
            Detalle310.RemoveAt(Detalle310.FindIndex(x => x.IDArticulo == args310.IDArticulo && x.IDUbicacionDesde == args310.IDUbicacionDesde && x.UnidadxBultos == args310.UnidadxBultos));
            StateHasChanged();
        }

        private async Task BorrarLinea123(DetalleRecepcion123 args123)
        {
            Detalle123.RemoveAt(Detalle123.FindIndex(x => x.IDArticulo == args123.IDArticulo && x.IDUbicacionDesde == args123.IDUbicacionDesde && x.UnidadxBultos == args123.UnidadxBultos));
            StateHasChanged();
        }
        #endregion

        #region Filtro
        private Func<UbicacionHasta, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.Ubicacion.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        };
        #endregion

        Func<TipoOperacionModel, string> convertTipoSolicitud = p => p.Descripcion;
        Func<Bodegas, string> convertBodega = p => p.SiglaBodega;

    }
}