using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Shared.Model.VisualzadorFacturacion.VisualzadorFacturacionDTO;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace PortalAG_V2.Pages.VisualzadorFacturacion
{
    public partial class VisualizadorFacturacion
    {
        #region Variables


        Loading? loading;
        PopUp? popup;
        private int Count = 240;
        private string? Error;
        private Timer? timer;
        private MainServices? service;
        public bool IsVisible { get; set; } = false;
        private string? Xvalue = "center";
        private string? Yvalue = "center";
        private SfGrid<PorFacturarDTO>? Grid;
        

        public int errorFac { get; set; } = 20; // No facturado

        #endregion

        #region Listas

        public PorFacturarDTO RowDetails { get; set; }
        private SfGrid<DetalleVentaDTO> GridDetalle;       
        private List<PorFacturarDTO>? lista = new List<PorFacturarDTO>();
        private List<PorFacturarDTO>? FacturasList = new List<PorFacturarDTO>();
        private List<DetalleVentaDTO>? listaDetalle = new List<DetalleVentaDTO>();
        private List<DetalleVentaDTO> DetalleVentaList = new List<DetalleVentaDTO>();

        #endregion

        #region Urls

        private const string UrlPorFacturar = "agAPIFEAutomatica/api/v1/Facturacion/GetPedidoFactura";
        private const string UrlDetalle = "agAPIFEAutomatica/api/v1/Facturacion/GetDetalleVentas";


        #endregion
        protected override async Task OnInitializedAsync()
        {
            StartCountdown();
            await ObtenerFacturas();
        }

        #region StartCountdown

        public void StartCountdown()
        {
            try
            {
                timer = new Timer(new TimerCallback(async (e) =>
                {
                    if (Count <= 0)
                    {
                        await Actualizar();
                        Count = 240;
                    }
                    else
                    {
                        Count--;
                    }
                    await InvokeAsync(StateHasChanged);
                }), null, 1000, 1000);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }

        #endregion

        #region Actualizar

        public async Task Actualizar()
        {
            try
            {
                await ObtenerFacturas();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                loading?.Cerrar();
            }
        }



        #endregion

        #region ObtenerFacturas

        public async Task ObtenerFacturas()
        {
            try
            {
                using (TransactionScope scope = new())
                {
                    loading?.Abrir();
                    bool error = false;
                    await Task.Delay(900);
                    service = new MainServices();
                    var respon = await service.ConectionServicePublic.HttpClientInstance.GetAsync(UrlPorFacturar);
                    Count = 240;
                    if (respon != null)
                    {
                        lista = new List<PorFacturarDTO>();

                        List<PorFacturarDTO> listaQuantity = new List<PorFacturarDTO>();
                        List<PorFacturarDTO> listaInvalidy = new List<PorFacturarDTO>();
                        List<PorFacturarDTO> listaFaltan = new List<PorFacturarDTO>();

                        lista = JsonConvert.DeserializeObject<List<PorFacturarDTO>>(await respon.Content.ReadAsStringAsync());
                        FacturasList = lista;

                        listaFaltan = lista;
                        listaQuantity = lista;
                        listaInvalidy = lista;
                        
                        List<PorFacturarDTO> ListaErrorQuantity = new List<PorFacturarDTO>();
                        List<PorFacturarDTO> ListaErrorInvalidy = new List<PorFacturarDTO>();
                        List<PorFacturarDTO> ListaErrorFaltan = new List<PorFacturarDTO>();

                        ListaErrorFaltan = listaFaltan.FindAll(x => x.ErrorUltimoProceso.Contains("Faltan"));
                        ListaErrorQuantity = listaQuantity.FindAll(x => x.ErrorUltimoProceso.Contains("Quantity falls into negative inventory"));
                        ListaErrorInvalidy = listaInvalidy.FindAll(x => x.ErrorUltimoProceso.Contains("Invalid"));
                       
                        foreach (PorFacturarDTO item in ListaErrorQuantity)
                        {
                            item.ErrorUltimoProceso = "Stock en Rojo";                           
                        }
                        
                        foreach (PorFacturarDTO item in ListaErrorInvalidy)
                        {
                            item.ErrorUltimoProceso = "Cliente no ha sido creado o esta Bloqueado";
                        }

                        foreach (PorFacturarDTO item in ListaErrorFaltan)
                        {
                            item.ErrorUltimoProceso = "Falta realizar Traspaso";
                        }
                    }
                    popup?.CerrarPopUp();
                    StateHasChanged();
                    loading?.Cerrar();
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                loading?.Cerrar();
            }
        }

        #endregion

        #region OnCommandClicked

        public async Task OnCommandClicked(CommandClickEventArgs<PorFacturarDTO> args)
        {
            IsVisible = true;
            RowDetails = args.RowData;
            service = new MainServices();
            var respon = await service.ConectionServicePublic.HttpClientInstance.GetAsync($"{UrlDetalle}/{args.RowData.AnnoProceso}/{args.RowData.IDOperacion}/1");
            if (respon != null)
            {
                listaDetalle = new List<DetalleVentaDTO>();
                DetalleVentaList = new List<DetalleVentaDTO>();
                listaDetalle = JsonConvert.DeserializeObject<List<DetalleVentaDTO>>(await respon.Content.ReadAsStringAsync());
                DetalleVentaList = listaDetalle;
                popup?.AbrirPopUp(IsVisible, DetalleVentaList, RowDetails);
            }

            StateHasChanged();
        }

        #endregion
    }
}
