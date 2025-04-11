using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.Direcciones;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Charts;

namespace PortalAG_V2.Pages.Clientes
{
    partial class CambioDireccionPedido
    {
        private ClientFactory conexion;

        private string UrlActualizaDireccion = "api/v2/DireccionDespacho/ActualizaDireccion";
        private string UrlDireccionesCliente = "api/v2/DireccionDespacho/ConsultarDireccion";
        public string nroPedido { get; set; }
        public string nombreCliente { get; set; }
        public string idCliente { get; set; }
        public string estadoPedido { get; set; }
        public string direccionDes { get; set; }
        public string region { get; set; }
        public string ciudad { get; set; }
        public string comuna { get; set; }
        public bool hablitarBoton { get; set; } = true;

        public List<DireccionesDTO> listaDirecciones = new List<DireccionesDTO>();
        public DireccionesDTO direccion = new DireccionesDTO();

        protected override Task OnInitializedAsync()
        {
            conexion = new MainServices().ConectionService; // Produccion
            return base.OnInitializedAsync();
        }

        public async Task BuscarNroPedido()
        {
            try
            {
                if(nroPedido != "" && nroPedido != null)
                {
                    var response = await conexion.HttpClientInstance.GetAsync($"{UrlActualizaDireccion}/1/0/0/{nroPedido}");
                    if (response.IsSuccessStatusCode)
                    {
                        var item = JsonConvert.DeserializeObject<ResponseActualizaDireccionDTO>(await response.Content.ReadAsStringAsync());
                        nombreCliente = item.RazonSocial;
                        idCliente = item.IDCliente;
                        estadoPedido = item.EstadoPedido;
                        direccionDes = item.Direccion;
                        region = item.Region;
                        ciudad = item.Ciudad;
                        comuna = item.Comuna;
                        hablitarBoton = true;
                        await CargarDirecciones(idCliente);
                    }
                }
                else
                {
                    _snackBar.Add("Ingresa nro de pedido valido", Severity.Error);
                }
               
            }
            catch (Exception ex)
            {
                _snackBar.Add(ex.Message, Severity.Error);
            }
        }

        private async Task CargarDirecciones(string id)
        {
            try
            {
                if (id != "" && id != null)
                {
                    var aux = await conexion.HttpClientInstance.GetAsync($"{UrlDireccionesCliente}/{id}");
                    if (aux.IsSuccessStatusCode)
                    {
                        try
                        {
                            listaDirecciones = JsonConvert.DeserializeObject<List<DireccionesDTO>>(await aux.Content.ReadAsStringAsync());
                            listaDirecciones.RemoveAll(x => x.tipoDireccion.Trim() == "B");


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
                else
                {
                    _snackBar.Add("Ingresa id cliente valido", Severity.Error);
                }

            }
            catch (Exception ex)
            {
                _snackBar.Add(ex.Message, Severity.Error);
            }
        }
        public async Task ActualizarDireccion()
        {
            try
            {
                if (nroPedido != "" && nroPedido != null)
                {
                    if (estadoPedido != "Facturado" && estadoPedido != "Facturando" && estadoPedido != "En Jaula" && estadoPedido != "Despachado" && estadoPedido != "Sin Estado" && estadoPedido != "Anulado")
                    {
                        if (listaDirecciones[0].idCliente.Trim() != idCliente.Trim())
                        {
                            _snackBar.Add("El id del cliente no coincide con el pedido", Severity.Error);
                        }
                        else
                        {
                            var response = await conexion.HttpClientInstance.GetAsync($"{UrlActualizaDireccion}/2/{idCliente}/{direccion.idDireccion}/{nroPedido}");
                            if (response.IsSuccessStatusCode)
                            {
                                var item = JsonConvert.DeserializeObject<ResponseActualizaDireccionDTO>(await response.Content.ReadAsStringAsync());
                                _snackBar.Add("Direccion de despacho actualizada", Severity.Success);
                                Limpiar();
                            }
                        }
                        
                    }
                    else
                    {
                        _snackBar.Add("El pedido no se puede actualizar en esta etapa", Severity.Error);
                    }
                    
                }
                else
                {
                    _snackBar.Add("No se pudo actualizar la direccion", Severity.Error);
                }

            }
            catch (Exception ex)
            {
                _snackBar.Add(ex.Message, Severity.Error);
            }
        }
        private async Task CambioDireccion(DireccionesDTO data)
        {
            direccion = data;
            direccionDes = data.direccion +" "+data.nroDireccion;
            region = data.idRegion.ToString();
            ciudad = data.ciudad;
            comuna = data.comuna;
            hablitarBoton = false;


        }
          
        public void Limpiar()
        {
            nroPedido = "";
            nombreCliente = "";
            idCliente = "";
            estadoPedido = "";
            direccionDes = "";
            region = "";
            ciudad = "";
            comuna = "";

            direccion = new DireccionesDTO();
            listaDirecciones = new List<DireccionesDTO>();
        }

        Func<DireccionesDTO, string> converterDireccion = p => p.idDireccion;
    }
}
