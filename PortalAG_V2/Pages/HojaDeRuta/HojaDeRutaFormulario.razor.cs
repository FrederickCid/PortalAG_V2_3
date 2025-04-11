using ClosedXML;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes.EvaluacionCliente;
using PortalAG_V2.Componentes.HojaDeRuta;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Model.AutorizarPedidos;
using PortalAG_V2.Shared.Model.FacturaPorServicio.Response;
using PortalAG_V2.Shared.Model.Formularios;
using PortalAG_V2.Shared.Model.HojaDeRuta;
using PortalAG_V2.Shared.Services;
using Radzen;
using System.Net.Http.Json;
using static PortalAG_V2.Pages.NotaDeCredito.NotaCredito;


namespace PortalAG_V2.Pages.HojaDeRuta
{
    public partial class HojaDeRutaFormulario
    {
        #region Variables
        [CascadingParameter]
        public AppState appSatate { get; set; }
        MainServices service;
        private bool Click = true;
        private bool ClickModificar = true;
        private bool Loading = false;
        private bool LoadingImpresion = false;
        private string _searchString;
        public int SelectedOption { get; set; }
        private List<ConsultaDisponiblesHojaDeRutaModel> ListaDisponibles = new List<ConsultaDisponiblesHojaDeRutaModel> { };
        private List<ConsultaDisponiblesHojaDeRutaModel> ListaHojaDetalle = new List<ConsultaDisponiblesHojaDeRutaModel> { };
        private List<ResponseHojaruta> responseHojaRuta = new List<ResponseHojaruta>() { };
        private List<ResponseHojaruta> responseHojaRutaTermino = new List<ResponseHojaruta>() { };
        private RequestHojaRuta HojaDerutaCabecera = new RequestHojaRuta { };
        private MudDataGrid<ConsultaDisponiblesHojaDeRutaModel> GridHO;
        private string responseBody;
        private int IDHoja = 0;
        bool transfiriendoDatos = false;


        //Para generar pdf
        [Inject] ExportService exportService { get; set; }
        [Inject] IJSRuntime js { get; set; }
        SetDatosPDF dataPDF = new SetDatosPDF();
        #endregion

        #region Url
        //POST
        private string urlCrearHoja = "Despacho/HojaRuta/";
        private string urlEnviaHoja = "Despacho/HojaRutaDesktop/";
        //GET
        private string urlConsultarDisponibles = "Despacho/DisponibleRuta/";
        private string urlImpresionAutomatica = "Despacho/GenerarImpresionDTE/";



        #endregion Url

        //protected async Task OnInitializedAsync()
        //{

        //    // Console.WriteLine(SelectedOption);
        //}

        #region Crear Hoja De Ruta
        private async Task onClickCrear()
        {
            try
            {
                RequestHojaRuta Hoja = new RequestHojaRuta()
                {
                    IDOperacion = 0,
                    CodigoBarra = "",
                    Correlativo = 0,
                    IDHojaRuta = 0,
                    Zona = 0,
                    TipoConsulta = 0,
                    IDUsuario = appSatate.IDUsuario,
                    Dispositivo = 99,
                };

                service = new MainServices();
                if (Hoja != null)
                {
                    Loading = true;
                    var HojaDeRutapost = await service.Formulario.HttpClientInstance.PostAsJsonAsync<RequestHojaRuta>($"api/v2/{urlCrearHoja}", Hoja);
                    if (HojaDeRutapost.IsSuccessStatusCode)
                    {
                        Loading = false;
                        Click = false;
                        responseBody = await HojaDeRutapost.Content.ReadAsStringAsync();
                        responseHojaRuta = JsonConvert.DeserializeObject<List<ResponseHojaruta>>(responseBody);
                        await CargarDatosRutasDisponibles();
                        StateHasChanged();
                        if (responseHojaRuta is not null)
                        {
                            IDHoja = responseHojaRuta.FirstOrDefault().IDGuiaHojaRuta;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("error");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            StateHasChanged();
        }
        #endregion

        #region Consulta Rutas Disponibles
        public async Task CargarDatosRutasDisponibles()
        {
            try
            {
                service = new MainServices();
                var lista = await service.Formulario.HttpClientInstance.GetAsync($"api/v2/{urlConsultarDisponibles}");
                if (lista.IsSuccessStatusCode)
                {
                    ListaDisponibles = JsonConvert.DeserializeObject<List<ConsultaDisponiblesHojaDeRutaModel>>(await lista.Content.ReadAsStringAsync());
                    if (ListaHojaDetalle.Count > 0)
                    {
                        ListaDisponibles.RemoveAll(disponible => ListaHojaDetalle.Any(detalle => detalle.IDOperacion == disponible.IDOperacion));
                    }
                }
                else
                {
                    ListaDisponibles = new List<ConsultaDisponiblesHojaDeRutaModel>();
                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }
        #endregion
        private void onClickCancelar()
        {
            Click = true;
            Clear();
        }

        #region Funciones Grilla

        private async Task clickDesde(DataGridRowClickEventArgs<ConsultaDisponiblesHojaDeRutaModel> args)
        {
            transfiriendoDatos = true;
            Loading = true;
            ConsultaDisponiblesHojaDeRutaModel dato = args.Item;
            if (!ListaHojaDetalle.Exists(x => x.IDOperacion == args.Item.IDOperacion))
            {
                ListaDisponibles.RemoveAt(ListaDisponibles.FindIndex(x => x.IDOperacion == args.Item.IDOperacion));
                Loading = true;
                ListaHojaDetalle.Add(new ConsultaDisponiblesHojaDeRutaModel()
                {
                    AnnoProceso = dato.AnnoProceso,
                    Ciudad = dato.Ciudad,
                    Comuna = dato.Comuna,
                    Correlativo = dato.Correlativo,
                    Descripcion = dato.Descripcion,
                    Direccion = dato.Direccion,
                    IDAllGestEmpresa = dato.IDAllGestEmpresa,
                    IDCliente = dato.IDCliente,
                    IDEmpresa = dato.IDEmpresa,
                    IDOperacion = dato.IDOperacion,
                    NroBultos = dato.NroBultos,
                    NroDocumento = dato.NroDocumento,
                    RazonSocial = dato.RazonSocial,
                    Region = dato.Region,
                    Tipo = dato.Tipo,
                    Transporte = dato.Transporte,
                });

                await Task.Delay(500);
                Loading = false;
                snakBarCreation("Agregado correctamente", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                transfiriendoDatos = false;
                StateHasChanged();
            }
            else
            {
                await Task.Delay(500);
                Loading = false;
                snakBarCreation("La opcion ya a sido cargada", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                transfiriendoDatos = false;
                StateHasChanged();
            }
        }

        //private void onChangeDetalle(HojaRutaDetalleCompleto args)
        //{
        //    ListaDetalle.Find(x => x.IDArticulo == args.IDArticulo && x.Linea == args.Linea).Total = (args.Cantidad * args.PrecioVenta);
        //    StateHasChanged();
        //}
        //

        private async Task RemoveItem(ConsultaDisponiblesHojaDeRutaModel args)
        {
            Loading = true;
            transfiriendoDatos = true;
            ConsultaDisponiblesHojaDeRutaModel dato = args;
            if (!ListaDisponibles.Exists(x => x.IDOperacion == args.IDOperacion))
            {
                ListaHojaDetalle.RemoveAt(ListaHojaDetalle.FindIndex(x => x.IDOperacion == args.IDOperacion));
                ListaDisponibles.Add(new ConsultaDisponiblesHojaDeRutaModel()
                {
                    AnnoProceso = dato.AnnoProceso,
                    Ciudad = dato.Ciudad,
                    Comuna = dato.Comuna,
                    Correlativo = dato.Correlativo,
                    Descripcion = dato.Descripcion,
                    Direccion = dato.Direccion,
                    IDAllGestEmpresa = dato.IDAllGestEmpresa,
                    IDCliente = dato.IDCliente,
                    IDEmpresa = dato.IDEmpresa,
                    IDOperacion = dato.IDOperacion,
                    NroBultos = dato.NroBultos,
                    NroDocumento = dato.NroDocumento,
                    RazonSocial = dato.RazonSocial,
                    Region = dato.Region,
                    Tipo = dato.Tipo,
                    Transporte = dato.Transporte,
                });
                await Task.Delay(500);
                Loading = false;
                snakBarCreation("Se quito Correctamente", Defaults.Classes.Position.BottomStart, Severity.Info, 1500);
                transfiriendoDatos = false;
                StateHasChanged();
            }
            else
            {
                ListaHojaDetalle.RemoveAt(ListaHojaDetalle.FindIndex(x => x.NroDocumento == args.NroDocumento));
                await Task.Delay(500);
                Loading = false;
                snakBarCreation("Se quito Correctamente", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                transfiriendoDatos = false;
                StateHasChanged();
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

        #endregion SnackBar

        #region Limpiar Campos
        private void Clear()
        {
            ListaDisponibles = new List<ConsultaDisponiblesHojaDeRutaModel> { };
            ListaHojaDetalle = new List<ConsultaDisponiblesHojaDeRutaModel> { };
            SelectedOption = 0;
        }
        #endregion

        #region Creacion hoja de ruta
        async Task CrearHojaDeRuta()
        {
            bool prompt = await DialogService.ConfirmAsync("¿Deseca creae la hoja de ruta?");
            if (prompt)
            {
                List<RequestHojaRutaTermino> terminoPost = new List<RequestHojaRutaTermino>() { };
                Loading = true;
                List<RequestHojaRuta> NuevaHojaDeRutaDetalle = new List<RequestHojaRuta>() { };

                foreach (ConsultaDisponiblesHojaDeRutaModel item in ListaHojaDetalle)
                {
                    if (item.Tipo == 1)
                    {
                        NuevaHojaDeRutaDetalle.Add(new RequestHojaRuta()
                        {
                            IDOperacion = item.Tipo,
                            CodigoBarra = Convert.ToString(item.NroDocumento),
                            Correlativo = item.Correlativo,
                            IDHojaRuta = IDHoja,
                            Zona = SelectedOption,
                            TipoConsulta = 2,
                            IDUsuario = appSatate.IDUsuario,
                            Dispositivo = 99

                        });
                    }
                    if (item.Tipo == 2 || item.Tipo == 3)
                    {
                        NuevaHojaDeRutaDetalle.Add(new RequestHojaRuta()
                        {
                            IDOperacion = item.Tipo,
                            CodigoBarra = Convert.ToString(item.IDOperacion),
                            Correlativo = item.Correlativo,
                            IDHojaRuta = IDHoja,
                            Zona = SelectedOption,
                            TipoConsulta = 2,
                            IDUsuario = appSatate.IDUsuario,
                            Dispositivo = 99
                        });
                    }

                }

                RequestHojaRuta Hoja = new RequestHojaRuta()
                {
                    IDOperacion = 0,
                    CodigoBarra = "",
                    Correlativo = 1,
                    IDHojaRuta = responseHojaRuta.FirstOrDefault().IDGuiaHojaRuta,
                    Zona = SelectedOption,
                    TipoConsulta = 4,
                    IDUsuario = appSatate.IDUsuario,
                    Dispositivo = 99,
                };

                var formulario = await service.Formulario.HttpClientInstance.PostAsJsonAsync<List<RequestHojaRuta>>($"api/v2/{urlEnviaHoja}", NuevaHojaDeRutaDetalle);

                if (formulario.IsSuccessStatusCode)
                {
                    Loading = false;
                    foreach (ConsultaDisponiblesHojaDeRutaModel datos in ListaHojaDetalle)
                    {
                        await MandarImpresionAutomatica(datos.AnnoProceso, datos.IDOperacion, datos.Correlativo, datos.TipoDoc);
                    }
                    var Termino = await service.Formulario.HttpClientInstance.PostAsJsonAsync<RequestHojaRuta>($"api/v2/{urlCrearHoja}", Hoja);
                    await GenerarPDFHojaRuta(SelectedOption);
                    Click = true;
                    SelectedOption = 0;
                    ListaDisponibles = new List<ConsultaDisponiblesHojaDeRutaModel> { };
                    ListaHojaDetalle = new List<ConsultaDisponiblesHojaDeRutaModel> { };
                    snakBarCreation("Eviado", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);
                    Clear();
                }
                else
                {
                    snakBarCreation(responseHojaRuta.FirstOrDefault().msgMensaje, Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                    Clear();
                    Click = true;
                    Loading = false;
                }
                Click = true;
                Loading = false;
            }
            else
            {
               await CargarDatosRutasDisponibles();
            }
        }

        public async Task MandarImpresionAutomatica(int AnnoProceso, int IDOperacion, int Correlativo, int Tipodoc)
        {
            try
            {
                LoadingImpresion = true;
                await Task.Delay(2000);
                service = new MainServices();
                var lista = await service.Formulario.HttpClientInstance.GetAsync($"api/v2/{urlImpresionAutomatica}{AnnoProceso}/{IDOperacion}/0/{Tipodoc}/0/0/VICTORZ/0");
                if (lista.IsSuccessStatusCode)
                {
                    LoadingImpresion = false;
                }
                else
                {
                    LoadingImpresion = false;
                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }


        #endregion


        #region Guardar PDF Hoja De Ruta
        public async Task GenerarPDFHojaRuta(int zona)
        {
            using (MemoryStream memory = exportService.CreatePdfHojaderuta(ListaHojaDetalle, responseHojaRuta, zona))
            {
                await js.SaveAs($"Hoja de Ruta {responseHojaRuta.FirstOrDefault().IDGuiaHojaRuta}.pdf", memory.ToArray());
            }
        }



        #endregion

        async Task PruebaPdf()
        {
            await GenerarPDFHojaRuta(SelectedOption);

        }
        async Task BtnCrear()
        {
         
           await CrearHojaDeRuta();
            Clear();
            //await GenerarPDFHojaRuta(SelectedOption);
        }

        async Task Actualizar()
        {
            Loading = true;
            await CargarDatosRutasDisponibles();
            Loading = false;

        }

        #region Filtro Input Text
        private Func<ConsultaDisponiblesHojaDeRutaModel, bool> QuickFilter => x =>
        {
            //si no encuetra nada no manda nada
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            //Buscar por NRO de Documento 
            if (x.IDCliente.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Buscar Por CLiente
            if (x.NroDocumento.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Busqueda mas amplia de los dos anterirore y agregando al vendedor, para mostrar multiples
            if ($"{x.RazonSocial} {x.Descripcion} {x.Region} {x.Comuna} {x.Ciudad} {x.Transporte}".Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
        #endregion

        private void OnClickModificar() 
        {
            var parameters = new DialogParameters<DialogModificarHR>
            {
                {x => x.IDusuario,  appSatate.IDUsuario},          
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = false, MaxWidth = MaxWidth.Medium, DisableBackdropClick = true, };
            DialogServices.Show<DialogModificarHR>($"", parameters, options);

        }

        private void OnClickModificarTransporte()
        {
            var parameters = new DialogParameters<DialogModificarTransporteHr>
            {
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = false, MaxWidth = MaxWidth.Medium, DisableBackdropClick = true, };
            DialogServices.Show<DialogModificarTransporteHr>($"", parameters, options);

        }

        private void ReimprimiHR()
        {
            var parameters = new DialogParameters<DialogReimpresionHR>
            {
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = false, MaxWidth = MaxWidth.Medium, DisableBackdropClick = true, };
            DialogServices.Show<DialogReimpresionHR>($"", parameters, options);

        }
    }
}