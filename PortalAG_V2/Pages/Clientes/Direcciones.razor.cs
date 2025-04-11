using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Shared.Model.Direcciones;
using PortalAG_V2.Shared.Model.Pagos;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Charts;
using System.Net.Http.Json;
using static MudBlazor.CategoryTypes;

namespace PortalAG_V2.Pages.Clientes
{
    partial class Direcciones
    {
        private ClientFactory service;
        private ClientFactory conexion;
        private string UrlComuna = "api/v1/Cliente/listaComunas";
        private string UrlRegion = "api/v1/Cliente/listaRegiones";
        private string UrlDatosCliente = "api/v2/DireccionDespacho/ConsultarCliente";
        private string UrlDireccionesCliente = "api/v2/DireccionDespacho/ConsultarDireccion";
        private string UrlGrabarDirecciones = "api/v2/DireccionDespacho/IngresaDireccion";
        private string UrlGrabarNumero = "api/v2/DireccionDespacho/IngresaNumero";

        public int Linea { get; set; } = -1;
        public string idCliente { get; set; }
        public string nombreCliente { get; set; }
        public string correElectronioCliente { get; set; }
        public string giroClienteEmpresa { get; set; }
        public int telefonoCliente { get; set; }
        public string idDireccion { get; set; }
        public string codigoPostal { get; set; }
        public string calle { get; set; }
        public string nroCalle { get; set; }
        public string ciudad { get; set; }
        public string localidad { get; set; }
        public string idUsuario { get; set; } = "";

        public List<ClientesDTO> DatosClientes { get; set; } = new List<ClientesDTO>();
        public List<DireccionesDTO> DireccionesCliente { get; set; } = new List<DireccionesDTO>();

        ComunaDTO comunas { get; set; }  = new ComunaDTO();
        List<ComunaDTO> listaComunas { get; set; } = new List<ComunaDTO>();
        RegionDTO regiones { get; set; } = new RegionDTO();
        List<RegionDTO> listaRegiones { get; set; }  = new List<RegionDTO>();

        private TipoDireccionDTO tipoDirecciones { get; set; } = new TipoDireccionDTO();

        protected override async Task OnInitializedAsync()
        {
            service = new MainServices().BackOffice; // Produccion
            conexion = new MainServices().ConectionService; // Produccion
            tipoDirecciones.idDirecion = "S";
            tipoDirecciones.tipoDireccion = "DESPACHO";

            var Usuario = await _authenticationManager.CurrentUser();
            idUsuario = Usuario.GetUserId();

            await CargarRegionesAsync();
        }

        private async Task CargarRegionesAsync()
        {
            try
            {
                var response = await service.HttpClientInstance.GetAsync(UrlRegion);
                if (response.IsSuccessStatusCode)
                {
                    //var result = await response.Content.ReadAsStringAsync();
                    var item1 = JsonConvert.DeserializeObject<ItemRegionDTO>(await response.Content.ReadAsStringAsync());
                    listaRegiones = item1.Item1;
                }
            }
            catch (Exception ex)
            {
                _snackBar.Add(ex.Message, Severity.Error);
            }
        }

        private async Task CambioRegion(RegionDTO data)
        {
            regiones = data;
            try
            {
                var response = await service.HttpClientInstance.GetAsync($"{UrlComuna}/{data.idRegion}");
                if (response.IsSuccessStatusCode)
                {
                    //var result = await response.Content.ReadAsStringAsync();
                    var item1 = JsonConvert.DeserializeObject<ItemComunaDTO>(await response.Content.ReadAsStringAsync());
                    listaComunas = item1.Item1;
                }
            }
            catch (Exception ex)
            {
                _snackBar.Add(ex.Message, Severity.Error);
            }
            StateHasChanged();
        }

        private async Task CambioComuna(ComunaDTO data)
        {
            comunas = data;
            
            StateHasChanged();
        }
        private async Task BuscarCliente()
        {
            var id = idCliente;
            var aux = await conexion.HttpClientInstance.GetAsync($"{UrlDatosCliente}/{id}");
            if (aux.IsSuccessStatusCode)
            {
                try
                {
                    DatosClientes = JsonConvert.DeserializeObject<List<ClientesDTO>>(await aux.Content.ReadAsStringAsync());
                    nombreCliente = DatosClientes.FirstOrDefault().razonSocial;
                    correElectronioCliente = DatosClientes.FirstOrDefault().eMail;
                    giroClienteEmpresa = DatosClientes.FirstOrDefault().giro;
                    telefonoCliente = Convert.ToInt32(DatosClientes.FirstOrDefault().celular.Replace("+56", ""));
                    LimpearCampos();
                    await ConsultarDirecciones(id);


                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error al deserealizar datos de cliente", Severity.Error);
                    LimpearCampos();
                    DireccionesCliente.Clear();
                    telefonoCliente = 0;
                    giroClienteEmpresa = "";
                    correElectronioCliente = "";
                    nombreCliente = "";
                    idCliente = "";
                }
            }
            else
            {
                _snackBar.Add("Error al consultar datos de cliente", Severity.Error);
                LimpearCampos();
                DireccionesCliente.Clear();
                telefonoCliente = 0;
                giroClienteEmpresa = "";
                correElectronioCliente = "";
                nombreCliente = "";
                idCliente = "";
            }
        }

        private async Task ConsultarDirecciones(string id)
        {
            var aux = await conexion.HttpClientInstance.GetAsync($"{UrlDireccionesCliente}/{id}");
            if (aux.IsSuccessStatusCode)
            {
                try
                {
                    DireccionesCliente = JsonConvert.DeserializeObject<List<DireccionesDTO>>(await aux.Content.ReadAsStringAsync());
                    DireccionesCliente.RemoveAll(x => x.tipoDireccion.Trim() == "B"); 


                }
                catch (Exception ex)
                {
                    _snackBar.Add("Error al deserealizar datos de cliente", Severity.Error);
                }
            }
            else
            {
                _snackBar.Add("Error al consultar datos de cliente", Severity.Error);
            }
            StateHasChanged();
        }

        private async Task RowClicked(DataGridRowClickEventArgs<DireccionesDTO> args)
        {
            idDireccion = args.Item.idDireccion;
            codigoPostal = args.Item.codigoPostal;
            Linea = args.Item.linea;
            calle = args.Item.direccion;
            nroCalle = args.Item.nroDireccion;
            ciudad = args.Item.ciudad;
            localidad = args.Item.localidad;
            regiones = listaRegiones.Exists(x => x.idRegion == args.Item.idRegion) ? listaRegiones.Find(x => x.idRegion == args.Item.idRegion) : new RegionDTO ();
            await CambioComunas(regiones.idRegion);
            comunas = listaComunas.Exists(x => x.idComuna == args.Item.idComuna) ? listaComunas.Find(x => x.idComuna == args.Item.idComuna) : new ComunaDTO ();
            StateHasChanged();
        }

        private async Task CambioComunas(int region)
        {
            try
            {
                var response = await service.HttpClientInstance.GetAsync($"{UrlComuna}/{region}");
                if (response.IsSuccessStatusCode)
                {
                    //var result = await response.Content.ReadAsStringAsync();
                    var item1 = JsonConvert.DeserializeObject<ItemComunaDTO>(await response.Content.ReadAsStringAsync());
                    listaComunas = item1.Item1;
                }
            }
            catch (Exception ex)
            {
                _snackBar.Add(ex.Message, Severity.Error);
            }
            StateHasChanged();
        }

        private void AgregarDireccion()
        {
            DireccionesDTO direccionesDTO = new DireccionesDTO();
            if (idDireccion != "")
            {
                if (calle != "")
                {
                    if (nroCalle != "")
                    {
                        if (ciudad != "")
                        {
                            if (regiones.idRegion != 0)
                            {
                                if (comunas.idComuna != 0)
                                {
                                  
                                    direccionesDTO.idCliente = idCliente;
                                    direccionesDTO.linea = Linea == -1 ? DireccionesCliente.Max(x => x.linea) + 1 : Linea;
                                    direccionesDTO.idDireccion = idDireccion;
                                    direccionesDTO.tipoDireccion = "S";
                                    direccionesDTO.direccion = calle;
                                    direccionesDTO.nroDireccion = nroCalle;
                                    direccionesDTO.codigoPostal = codigoPostal;
                                    direccionesDTO.idRegion = regiones.idRegion;
                                    direccionesDTO.idComuna = comunas.idComuna;
                                    direccionesDTO.comuna = comunas.comuna;
                                    direccionesDTO.ciudad = ciudad;
                                    direccionesDTO.localidad = localidad;
                                    direccionesDTO.siPorDefecto = 0;
                                    direccionesDTO.siEnvioSAP = 0;
                                    direccionesDTO.fechaActualizacion = DateTime.UtcNow;
                                    direccionesDTO.idUsuario = "mantenedor";

                                    if (DireccionesCliente.Exists(x => x.linea == Linea))
                                    {
                                        DireccionesCliente.RemoveAll(x => x.linea == Linea);
                                        
                                    }
                                    DireccionesCliente.Add(direccionesDTO);

                                }
                                else
                                {
                                    _snackBar.Add("Ingrese Comuna", Severity.Warning);
                                }
                            }
                            else
                            {
                                _snackBar.Add("Ingrese Region", Severity.Warning);
                            }
                        }
                        else
                        {
                            _snackBar.Add("Ingrese Ciudad", Severity.Warning);
                        }
                    }
                    else
                    {
                        _snackBar.Add("Ingrese Nro de calle", Severity.Warning);
                    }
                }
                else
                {
                    _snackBar.Add("Ingrese Nombre de la calle", Severity.Warning);
                }
            }
            else
            {
                _snackBar.Add("Ingrese ID Direccion", Severity.Warning);
            }
            
        }

        private void LimpearCampos()
        {
            idDireccion = "";
            calle = "";
            nroCalle = "";
            codigoPostal = "";
            comunas = new ComunaDTO();
            regiones = new RegionDTO();
            ciudad = "";
            localidad = "";
            Linea = -1;

        }

        private async Task GrabarDirecciones()
        {
            if(DireccionesCliente == null) // validacion cliente nulo
            {
                _snackBar.Add("Direccioes vacias", Severity.Warning);
            }
            else if (telefonoCliente == 0) // numero de telefono no  vacio
            {
                _snackBar.Add("Teléfono vacias", Severity.Warning);
            }
            else if (validarNumero(telefonoCliente.ToString())) // formato de numero correcto
            {
                _snackBar.Add("Formato Teléfono presenta error", Severity.Warning);
            }
            else if (DireccionesCliente[0].idCliente.Trim() != idCliente.Trim()) // direcciones  
            {
                _snackBar.Add("Direcciones no coinciden con el Cliente", Severity.Error);
                LimpearCampos();
                DireccionesCliente.Clear();
                telefonoCliente = 0;
                giroClienteEmpresa = "";
                correElectronioCliente = "";
                nombreCliente = "";
                idCliente = "";
            }
            else
            {
                ClientesDTO _clinteAux = new ClientesDTO();
                _clinteAux.idCliente = idCliente;
                _clinteAux.celular = telefonoCliente.ToString();
                _clinteAux.idUsuario = idUsuario;

                var response1 = await conexion.HttpClientInstance.PostAsJsonAsync<ClientesDTO>($"{UrlGrabarNumero}", _clinteAux);
                if (response1.IsSuccessStatusCode)
                {
                    _snackBar.Add("Numero guardadas con exito", Severity.Success);
                }
                else
                {
                    _snackBar.Add("Error al grabar Numero", Severity.Error);
                }

                List<IngresaDireccionDTO> _listDatosGrabar = new List<IngresaDireccionDTO>();
                foreach (var data in DireccionesCliente)
                {
                    IngresaDireccionDTO _datosGrabar = new IngresaDireccionDTO();
                    _datosGrabar.IDCliente = idCliente;
                    _datosGrabar.Linea = data.linea;
                    _datosGrabar.IDDireccion = data.idDireccion;
                    _datosGrabar.Direccion = data.direccion;
                    _datosGrabar.NroDireccion = data.nroDireccion;
                    _datosGrabar.CodigoPostal = data.codigoPostal;
                    _datosGrabar.IDRegion = data.idRegion.ToString();
                    _datosGrabar.Comuna = data.comuna;
                    _datosGrabar.Ciudad = data.ciudad;
                    _datosGrabar.Localidad = data.localidad;
                    _datosGrabar.IDUsuario = idUsuario;

                    _listDatosGrabar.Add(_datosGrabar);
                }

                var response = await conexion.HttpClientInstance.PostAsJsonAsync<List<IngresaDireccionDTO>>($"{UrlGrabarDirecciones}", _listDatosGrabar);
                if (response.IsSuccessStatusCode)
                {
                    _snackBar.Add("Direcciones guardadas con exito", Severity.Success);
                    LimpearCampos();
                    DireccionesCliente.Clear();
                    telefonoCliente = 0;
                    giroClienteEmpresa = "";
                    correElectronioCliente = "";
                    nombreCliente = "";
                    idCliente = "";
                }
                else
                {
                    _snackBar.Add("Error al grabar direcciones", Severity.Error);
                }
            }
                
            
            
        }

        private bool validarNumero(string telefono) {

            if (telefono == null) { return true; }
            if (telefono.Count() != 9) { return true; }
            return false;
        }

        Func<RegionDTO, string> converterRegiones = p => p.region;
        Func<ComunaDTO, string> converterComunas = p => p.comuna;
    }
}
