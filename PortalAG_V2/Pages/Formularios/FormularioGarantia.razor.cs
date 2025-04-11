
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.Formularios;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;

namespace PortalAG_V2.Pages.Formularios
{
    public partial class FormularioGarantia
    {
        #region Variables
        [CascadingParameter]
        public AppState appSatate { get; set; }
        public MainServices? service;
        bool success = true;
        string[] errors = { };
        MudTextField<string> MudTextNroFactura;
        MudForm form;
        MudSelect<string> Direcciones;
        MudDataGrid<DetalleFacturaModelDTO> GridPedido;
        MudDataGrid<DetalleFacturaModelDTO> GridPedidoSeleccionados;
        //Datos formulario
        string _NroFactura;
        string _FechaDeCompra = "";
        string _RutCliente = "";
        string _NombreCliente = "";
        string _Telefono = "";
        string _Correo = "";
        int _Total = 0;
        string _Observaciones = "";
        string _Direccion;
        string _Comentarios = "";
        int solicitud;
        int _CantidadCajas;
        private static string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full z-10";
        private string DragClass = DefaultDragClass;
        //Listas
        private List<IBrowserFile> fileComplete = new List<IBrowserFile>();
        private List<DetalleFacturaModel> ListaDePedidosData = new List<DetalleFacturaModel>();
        private List<DetalleFacturaModelDTO> ListaPedidos = new List<DetalleFacturaModelDTO>();
        private List<DetalleFacturaModelDTO> Detalle = new List<DetalleFacturaModelDTO>();
        // private DetalleFacturaModelDTO DetalleItem;
        private List<DireccionesModel> Direccion = new List<DireccionesModel>();
        private List<FormularioModel> Campos = new List<FormularioModel>();

        string _NroCliente = "0";
        int _Tipo = 2;
        //url
        string urlFormulario = "Formulario/GetFormulario";
        string urlFormularioPost = "Formulario/ActualizarFormularioGarantia/";
        #endregion

        protected async Task OnInitializedAsync()
        {
            //ListaPedidos = ListaDePedidosData;
            //StateHasChanged();

        }
        //Cargar los datos al momento de buscar con Enter
        #region funcion async para traer los datos de la factura
        public async Task CargarDatos(string Nrofactura)
        {
            try
            {
                service = new MainServices();
                var lista = await service.Formulario.HttpClientInstance.GetAsync($"api/v2/{urlFormulario}/{_Tipo}/{Convert.ToInt32(Nrofactura)}/{_NroCliente}/");
                if (lista.IsSuccessStatusCode)
                {
                    ListaDePedidosData.Clear();
                    ListaPedidos.Clear();
                    Detalle.Clear();
                    Direccion.Clear();
                    Campos.Clear();
                    Campos = JsonConvert.DeserializeObject<List<FormularioModel>>(await lista.Content.ReadAsStringAsync());
                }
                else
                {
                    Campos = new List<FormularioModel>() { };
                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }
        #endregion

        #region Funciones De Botones

        //Limpiar Formulario Completo
        private async Task Limpiar()
        {
            if (_NroFactura == "")
            {
                snakBarCreation("Nada que Borrar!", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
            }
            else
            {
                fileComplete = new List<IBrowserFile>();
                ClearDragClass();
                ListaPedidos = new List<DetalleFacturaModelDTO>();
                Detalle = new List<DetalleFacturaModelDTO>();
                Direccion = new List<DireccionesModel>();
                Direcciones.Clear();
                await Task.Delay(100);
                _NroFactura = "";
                _FechaDeCompra = "";
                _RutCliente = "";
                _NombreCliente = "";
                _Telefono = "";
                _Correo = "";
                _Total = 0;
                _Observaciones = "";
                _Comentarios = "";
                _CantidadCajas = 0;
                snakBarCreation("Se limpio el formulario", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
            }
        }


        //Limpiar todas las fotos
        private void Clear()
        {
            fileComplete = new List<IBrowserFile>();
            snakBarCreation("Se eliminaron fotos", Defaults.Classes.Position.BottomStart, Severity.Warning, 5000);
        }

        //Quitar Articulo de la grilla
        private void RemoveItem(DetalleFacturaModelDTO args)
        {
            Detalle.RemoveAt(Detalle.FindIndex(x => x.IDArticulo == args.IDArticulo));
            // ListaPedidos.Add(args);
            snakBarCreation("Se quito Correctamente", Defaults.Classes.Position.BottomStart, Severity.Warning, 5000);

        }

        //private void ResetGrid()
        //{
        //    Detalle = new List<DetalleFacturaModelDTO>();
        //    ListaPedidos = new List<DetalleFacturaModelDTO>();
        //    snakBarCreation("Recargando Grilla", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
        //    foreach (DetalleFacturaModel item in Campos.FirstOrDefault().DetalleFactura)
        //    {
        //        ListaDePedidosData.Add(item);

        //    }
        //    StateHasChanged();
        //}

        //Agregar todos los items al listado
        private void AllItems()
        {
            foreach (DetalleFacturaModelDTO item in ListaPedidos)
            {
                Detalle.Add(item);
            }
            //ListaPedidos.Clear();
            StateHasChanged();
            snakBarCreation("Se agregaron todos los productos", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);
        }
        //Procesar datos para ser enviados
        private async void Enviar()
        {
            bool prompt = await DialogService.ConfirmAsync("¿Esta seguro/a de crear la devolucion por garantia?");
            if (prompt)
            {
                DocumentoRequestActuaCheque DatosCabecera = new DocumentoRequestActuaCheque()
                {
                    IDtipo = 6,
                    NroFactura = Int32.Parse(_NroFactura),
                    IDCliente = _RutCliente,
                    IDDireccion = _Direccion,
                    Observacion = _Observaciones,
                    NroSolicitud = solicitud,
                    Comentarios = _Comentarios,
                    nroCajas = _CantidadCajas,
                    IDUsuario = appSatate?.IDUsuario
                };

                List<DetalleFacturaModelRequest> DatosDetalle = new List<DetalleFacturaModelRequest>();

                foreach (DetalleFacturaModelDTO item in Detalle)
                {
                    DatosDetalle.Add(new DetalleFacturaModelRequest()
                    {

                        Linea = (item.Linea),
                        IDArticulo = item.IDArticulo,
                        NombreArticulo = item.Nombre,
                        Precio = item.PrecioVenta,
                        Cantidad = item.Cantidad,
                        Total = item.Total
                    });

                }

                List<DetalleFacturaImagenModelDTO> DetalleImagen = new List<DetalleFacturaImagenModelDTO>();
                int linea = 1;

                foreach (IBrowserFile file in fileComplete)
                {
                    var aux = file.OpenReadStream();
                    var memoryStream = new MemoryStream();
                    await aux.CopyToAsync(memoryStream);

                    DetalleImagen.Add(new DetalleFacturaImagenModelDTO()
                    {
                        Linea = linea++,
                        NombreImagen = file.Name,
                        UrlImagen = Convert.ToBase64String(memoryStream.ToArray())
                    });
                }

                FormularioEnvio FormularioDatosEnvio = new FormularioEnvio()
                {
                    Cabecera = DatosCabecera,
                    Detalle = DatosDetalle,
                    Imagen = DetalleImagen
                };
                var formulario = await service.Formulario.HttpClientInstance.PostAsJsonAsync<FormularioEnvio>($"api/v2/{urlFormularioPost}", FormularioDatosEnvio);
                if (formulario.IsSuccessStatusCode)
                {
                    snakBarCreation("Enviado", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);
                    fileComplete.Clear();
                    ClearDragClass();
                    ListaPedidos = new List<DetalleFacturaModelDTO>();
                    Detalle = new List<DetalleFacturaModelDTO>();
                    Direccion = new List<DireccionesModel>();
                    Direcciones.Clear();
                    await Task.Delay(100);
                    _NroFactura = "";
                    _FechaDeCompra = "";
                    _RutCliente = "";
                    _NombreCliente = "";
                    _Telefono = "";
                    _Correo = "";
                    _Total = 0;
                    _Observaciones = "";
                    _Comentarios = "";
                    _CantidadCajas = 0;
                    MudTextNroFactura.Disabled = false;
                    StateHasChanged();
                }
                else
                {
                    snakBarCreation("Error al enviar revise los datos", Defaults.Classes.Position.BottomStart, Severity.Error, 6000);
                    StateHasChanged();
                }
            }
        }

        #endregion

        #region Funciones UploadIMG
        private void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearDragClass();
            var files = e.GetMultipleFiles();
            foreach (var file in files)
            {

                fileComplete.Add(file);
                StateHasChanged();

            }

            snakBarCreation("Se agrego la foto correctamente", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);
        }


        private void SetDragClass()
        {
            DragClass = $"{DefaultDragClass} mud-border-primary";

        }

        private void ClearDragClass()
        {
            DragClass = DefaultDragClass;
            StateHasChanged();

        }
        //AGregar prodictos de una lista a otra
        private void clickDesde(DataGridRowClickEventArgs<DetalleFacturaModelDTO> args)
        {
            // ListaPedidos.RemoveAt(ListaPedidos.FindIndex(x => x.IDArticulo == args.Item.IDArticulo));
            if (!Detalle.Exists(x => x.IDArticulo == args.Item.IDArticulo))
            {
                Detalle.Add(new DetalleFacturaModelDTO()
                {
                    Linea = args.Item.Linea,
                    IDArticulo = args.Item.IDArticulo,
                    Nombre = args.Item.Nombre,
                    Cantidad = args.Item.Cantidad,
                    PrecioVenta = args.Item.PrecioVenta,
                    Total = args.Item.Total,
                });
                snakBarCreation("Agregado correctamente", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);
            }
            else
            {
                snakBarCreation("Articulo ya a sido cargado", Defaults.Classes.Position.BottomStart, Severity.Warning, 5000);
            }
        }
        #endregion
        //SnackBAr para agregar de forma rapida
        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion

        #region Buscar Factura
        //Buscar al presionar enter
        private async Task onEnterPress(KeyboardEventArgs args)
        {
            if (args.Key == "Enter" && _NroFactura.Length > 2)
            {
                await CargarDatos(_NroFactura);
                MudTextNroFactura.Disabled = true;
                fileComplete.Clear();
                ListaPedidos = new List<DetalleFacturaModelDTO>();
                Detalle = new List<DetalleFacturaModelDTO>();
                Direccion = new List<DireccionesModel>();
                Direcciones.Clear();
                _FechaDeCompra = "";
                _RutCliente = "";
                _NombreCliente = "";
                _Telefono = "";
                _Correo = "";
                _Total = 0;
                _Observaciones = "";
                _Comentarios = "";
                _CantidadCajas = 0;
                snakBarCreation("Buscando...", Defaults.Classes.Position.BottomStart, Severity.Info, 2000);
                await Task.Delay(2000);

                if (Campos.FirstOrDefault().msgResult.ToUpper() == "OK")
                {
                    snakBarCreation("Cliente Encontrado", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);
                    solicitud = Campos.FirstOrDefault().NroSolicitud;
                    _NombreCliente = Campos.FirstOrDefault().RazonSocial;
                    _RutCliente = Campos.FirstOrDefault().IDCliente;
                    _Telefono = Campos.FirstOrDefault().Telefono;
                    _Correo = Campos.FirstOrDefault().Correo;
                    _FechaDeCompra = Campos.FirstOrDefault().FechaDocumento;
                    foreach (DireccionesModel item in Campos.FirstOrDefault().Direcciones)
                    {
                        if (item.TipoDireccion.Trim().Contains("B"))
                        {
                            Direccion.Add(item);
                        }
                    }
                    foreach (DetalleFacturaModel item in Campos.FirstOrDefault().DetalleFactura)
                    {
                        ListaDePedidosData.Add(item);

                    }
                    int i = 1;
                    foreach (DetalleFacturaModel item in ListaDePedidosData)
                    {
                        ListaPedidos.Add(new DetalleFacturaModelDTO()
                        {
                            Linea = i,
                            IDArticulo = item.IDArticulo,
                            Nombre = item.Nombre,
                            Cantidad = item.Cantidad,
                            PrecioVenta = item.PrecioVenta,
                            Total = (item.Cantidad * item.PrecioVenta),
                        });
                        _Total = _Total + (item.Cantidad * item.PrecioVenta);
                        i++;
                    }


                    MudTextNroFactura.Disabled = false;
                    StateHasChanged();
                }
                if (Campos.FirstOrDefault().msgResult.ToUpper() == "ERROR")
                {
                    await Task.Delay(3000);
                    snakBarCreation("Factura no encontrada", Defaults.Classes.Position.BottomStart, Severity.Error, 6000);
                    ClearDragClass();
                    fileComplete.Clear();
                    ListaPedidos = new List<DetalleFacturaModelDTO>();
                    Detalle = new List<DetalleFacturaModelDTO>();
                    Direccion = new List<DireccionesModel>();
                    Direcciones.Clear();
                    _NroFactura = "";
                    _FechaDeCompra = "";
                    _RutCliente = "";
                    _NombreCliente = "";
                    _Telefono = "";
                    _Correo = "";
                    _Total = 0;
                    _Observaciones = "";
                    _Comentarios = "";
                    _CantidadCajas = 0;
                    MudTextNroFactura.Disabled = false;
                    StateHasChanged();
                }
            }
        }
        #endregion
        //Cambia el valor total al modificar la cantidad 
        #region Cambiar total al modifical la cantidad
        private void onChangeDetalle(DetalleFacturaModelDTO args)
        {
            Detalle.Find(x => x.IDArticulo == args.IDArticulo && x.Linea == args.Linea).Total = (args.Cantidad * args.PrecioVenta);
            StateHasChanged();
        }
        #endregion

        #region Validaciones a modal de grilla

        void CommittedItemChanges(DetalleFacturaModelDTO args)
        {
            var articuloOriginal = ListaPedidos.Find(x => x.IDArticulo == args.IDArticulo);

            if (args.Cantidad <= 0)
            {
                args.Cantidad = 1;
                snakBarCreation("No puede ser menor a 1", Defaults.Classes.Position.BottomStart, Severity.Warning, 5000);
                onChangeDetalle(args);
            }
            else if (args.Cantidad > articuloOriginal.Cantidad)
            {
                args.Cantidad = articuloOriginal.Cantidad;
                snakBarCreation("Mayor a la cantidad original", Defaults.Classes.Position.BottomStart, Severity.Warning, 5000);
                onChangeDetalle(args);
            }
            else if (args.Cantidad > 0 && args.Cantidad <= articuloOriginal.Cantidad)
            {
                snakBarCreation("Cantidad cambiada", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);
                onChangeDetalle(args);
            }

        }
        #endregion
    }
}


