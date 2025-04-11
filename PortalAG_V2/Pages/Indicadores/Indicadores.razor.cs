using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.Indicadores;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Pages.Indicadores
{
    partial class Indicadores
    {
        MainServices service;
        string URL = "api/v2/Lineas/PendientesPicking";
        int Lineas = 0;
        int Pedidos = 0;
        int LineasTerm = 0;
        int PedidosTerm = 0;
        int pikingVerde = 0;
        int pikingRojo = 0;
        int pickingPedidosVerde = 0;
        int pickingPedidosRojo = 0;

        int lineasVentasVerde = 0;
        int lineasVentasRojo = 0;
        int lineasInterVerde = 0;
        int lineasInterRojo = 0;
        int lineasMayorVerde = 0;
        int lineasMayorRojo = 0;
        int lineasFBVerde = 0;
        int lineasFBRojo = 0;

        int lineasRevisionVerde = 0;
        int lineasRevisionRojo = 0;
        int pedidosRevisionVerde = 0;
        int pedidosRevisionRojo = 0;


        int revisionLineasVentasVerde = 0;
        int revisionLineasInterVerde = 0;
        int revisionLineasMayorVerde = 0;
        int revisionLineasFBVerde = 0;

        int revisionLineasVentasRojo = 0;
        int revisionLineasInterRojo = 0;
        int revisionLineasMayorRojo = 0;
        int revisionLineasFBRojo = 0;

        int LineasTodasBodegas = 0;
        string diaDeLaSemana = "";
        string nombreDelMes = "";
        string hora = "";
        string dia = "";
        string diaMes = "";

     
        Timer aTimer;
        Timer _timer;




        protected override void OnInitialized()
        {
            service = new MainServices();
            aTimer = new Timer(Tick, null, 0, 1000);
            _timer = new Timer(async _ => await ConsultarIndicadores(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            //await ConsultarIndicadores();
            base.OnInitialized();

        }

        private void Tick(object _)
        {
            DateTime fechaActual = DateTime.Now;

            diaDeLaSemana = fechaActual.ToString("dddd");
            if (!string.IsNullOrEmpty(diaDeLaSemana))
            {
                diaDeLaSemana = char.ToUpper(diaDeLaSemana[0]) + diaDeLaSemana.Substring(1).ToLower();
            }
            hora = fechaActual.ToString("h:mm tt");

            nombreDelMes = fechaActual.ToString("MMMM");
            if (!string.IsNullOrEmpty(nombreDelMes))
            {
                nombreDelMes = char.ToUpper(nombreDelMes[0]) + nombreDelMes.Substring(1).ToLower();
            }
            diaMes = $"{fechaActual.ToString("dd")} {nombreDelMes}";

            StateHasChanged();


        }

       

        public void Dispose()
        {
            aTimer?.Dispose();
            _timer?.Dispose();
        }

        private async Task ConsultarIndicadores()
        {
            LimpiarVariables();
            List<IndicadoresDTO> indicadores = new List<IndicadoresDTO>();
            var auxIndicadores = await service.ConectionService.HttpClientInstance.GetAsync(URL);
            if (auxIndicadores.IsSuccessStatusCode) {

                indicadores = JsonConvert.DeserializeObject<List<IndicadoresDTO>>(await auxIndicadores.Content.ReadAsStringAsync());
                if(indicadores != null)
                {
                    foreach (var item in indicadores)
                    {
                        if (item.bodega.Equals("Por Aprobar") ){
                            Pedidos = item.nroPedido;
                            Lineas = item.totalPicking;
                        }
                        if (item.bodega.Equals("Terminados"))
                        {
                            PedidosTerm = item.nroPedido;
                            LineasTerm = item.totalPicking;
                        }
                        if (item.bodega.Equals("TotalPicking"))
                        {
                            pikingVerde = item.totalVerdePicking;
                            pikingRojo = item.totalRojoPicking;
                            pickingPedidosVerde = item.nroPedido;
                            pickingPedidosRojo = item.nroPedidoRojo;
                        }

                        if (item.bodega.Equals("BV_BVN"))
                        {
                            lineasVentasVerde = item.totalVerdePicking;
                            lineasVentasRojo = item.totalRojoPicking;
                        }
                        if (item.bodega.Equals("BV_BIT"))
                        {
                            lineasInterVerde = item.totalVerdePicking;
                            lineasInterRojo = item.totalRojoPicking;
                        }
                        if (item.bodega.Equals("BV_BPM"))
                        {
                            lineasMayorVerde = item.totalVerdePicking;
                            lineasMayorRojo = item.totalRojoPicking;
                        }
                        if (item.bodega.Equals("FULLBIKE"))
                        {
                            lineasFBVerde = item.totalVerdePicking;
                            lineasFBRojo = item.totalRojoPicking;
                        }

                        if (item.bodega.Equals("Packing"))
                        {
                            lineasRevisionVerde += item.totalVerdePicking;
                            lineasRevisionRojo += item.totalRojoPicking;
                            pedidosRevisionVerde += item.nroPedido;
                            pedidosRevisionRojo += item.nroPedidoRojo;
                        }

                        if (item.bodega.Equals("PackingFullBike"))
                        {
                            lineasRevisionVerde += item.totalVerdePicking;
                            lineasRevisionRojo += item.totalRojoPicking;
                            pedidosRevisionVerde += item.nroPedido;
                            pedidosRevisionRojo += item.nroPedidoRojo;
                        }
                        if (item.bodega.Equals("TerminadosBodega"))
                        {
                            LineasTodasBodegas = item.totalPicking;
                        }


                    }

                }
            }
            else
            {
                //Error
            }
            StateHasChanged();
            
        }

        private void LimpiarVariables()
        {
            Lineas = 0;
            Pedidos = 0;
            LineasTerm = 0;
            PedidosTerm = 0;
            pikingVerde = 0;
            pikingRojo = 0;
            pickingPedidosVerde = 0;
            pickingPedidosRojo = 0;

            lineasVentasVerde = 0;
            lineasVentasRojo = 0;
            lineasInterVerde = 0;
            lineasInterRojo = 0;
            lineasMayorVerde = 0;
            lineasMayorRojo = 0;
            lineasFBVerde = 0;
            lineasFBRojo = 0;

            lineasRevisionVerde = 0;
            lineasRevisionRojo = 0;
            pedidosRevisionVerde = 0;
            pedidosRevisionRojo = 0;


            revisionLineasVentasVerde = 0;
            revisionLineasInterVerde = 0;
            revisionLineasMayorVerde = 0;
            revisionLineasFBVerde = 0;

            revisionLineasVentasRojo = 0;
            revisionLineasInterRojo = 0;
            revisionLineasMayorRojo = 0;
            revisionLineasFBRojo = 0;
        }
    }
}
