using MudBlazor.Charts;
using MudBlazor;
using PortalAG_V2.Shared.Services;
using System.Net.Http;
using static PortalAG_V2.Pages.Movimientos.SolicitudDeMovimientos;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.AutorizarPedidos;
using System.Net.Http.Json;
using PortalAG_V2.Auth;
using static PortalAG_V2.Shared.Permission.Permissions;
using static MudBlazor.CategoryTypes;
using System.Xml.Linq;

namespace PortalAG_V2.Pages.AutorizarPedidos
{
    partial class AutorizarPedidos
    {

        private ClientFactory conexion;
        private Timer _timer;
        private int Count = 60;
        private string _searchString;
        private string porcentaje = "% ";
        private bool _processing = false;

        private const string urlGetPedidos = "/api/v2/Autorizacion/GetPedidos";
        private const string urlGetPedidosAuto = "/api/v2/Autorizacion/GetPedidosAutorizados";
        private const string urlGetPedidosUrg = "/api/v2/Autorizacion/GetPedidosUrgencia";

        private const string urlAutorizacion = "/api/v2/Autorizacion/AutorizarPedido";

        private List<PedidoModel> Elements = new List<PedidoModel>();
        private List<PedidoModel> ElementsAutorizado = new List<PedidoModel>();
        private List<Urgencia> ElementsUrgencia = new List<Urgencia>();


        public List<PedidoModel> _listGetPedidos = new List<PedidoModel>();
        public List<PedidoModel> _listGetPedidosAuto = new List<PedidoModel>();
        public List<PedidoModel> _listGetPedidosUrg = new List<PedidoModel>();

        public class Urgencia
        {
            public string Vendedor;
            public string Cliente;
            public int Pedido;
            public int Lineas;
        }

        protected override async Task OnInitializedAsync()
        {
            
            StartCountdown();
            ListaPorAutorizar();
            ListaUrgencia();
            ListadoAutorizado();
        }

        #region Timer
        public void StartCountdown()
        {
            _timer = new Timer(new TimerCallback(async (e) =>
            {
                if (Count <= 0)
                {
                    await ListaPorAutorizar();
                    await ListaUrgencia();
                    await ListadoAutorizado();
                    Count = 60;
                }
                else
                {
                    Count--;
                }
                await InvokeAsync(StateHasChanged);
            }), null, 1000, 1000);
        }

        private async Task Refrescar()
        {
            await ListaPorAutorizar();
            await ListaUrgencia();
            await ListadoAutorizado();
            Count = 60;
            _snackBar.Add("Actualizado!", Severity.Success);
        }
        #endregion

        #region Lista de Pedidos
        private async Task ListaPorAutorizar()
        {
            conexion = new MainServices().ConectionService;
            var auxGetPedidos = await conexion.HttpClientInstance.GetAsync($"{urlGetPedidos}");
            try
            {
                Elements = JsonConvert.DeserializeObject<List<PedidoModel>>(await auxGetPedidos.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _snackBar.Add("Error al deserealizar Pedidos", Severity.Error);
            }
        }

        private async Task ListaUrgencia()
        {
            conexion = new MainServices().Prueba;
            var auxGetUrg = await conexion.HttpClientInstance.GetAsync($"{urlGetPedidosUrg}");
            try
            {
                _listGetPedidosUrg = JsonConvert.DeserializeObject<List<PedidoModel>>(await auxGetUrg.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _snackBar.Add("Error al deserealizar Pedidos con Urgencia", Severity.Error);
            }

            ElementsUrgencia.Clear();
            foreach (PedidoModel item in _listGetPedidosUrg)
            {
                ElementsUrgencia.Add(new Urgencia
                {
                    Vendedor = item.Vendedor,
                    Cliente = item.RazonSocial,
                    Pedido = item.IDOperacion,
                    Lineas = item.LineasPedido
                });
            }
        }

        private async Task ListadoAutorizado()
        {
            conexion = new MainServices().Prueba;
            var auxPedidosAuto = await conexion.HttpClientInstance.GetAsync($"{urlGetPedidosAuto}");
            try
            {
                ElementsAutorizado = JsonConvert.DeserializeObject<List<PedidoModel>>(await auxPedidosAuto.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                _snackBar.Add("Error al deserealizar Pedidos Autorizados", Severity.Error);
            }

        }
        #endregion

        #region Autorizar o Anular
        private async Task Normal(PedidoModel args)
        {
            _processing = true;

            var user = await _authenticationManager.CurrentUser();
            RequestPedidoModel data = new RequestPedidoModel()
            {
                AnnoProceso = args.AnnoProceso,
                IDOperacion = args.IDOperacion,
                Correlativo = args.Correlativo,
                IDEtapa = 0,
                IDEstado = 5,
                IDUsuario = user.GetUserId()
            };

            var aNormal = await conexion.HttpClientInstance.PostAsJsonAsync<RequestPedidoModel>(urlAutorizacion, data);
            if (aNormal.IsSuccessStatusCode)
            {
                _snackBar.Add("Pedido Autorizado", Severity.Success);
                await ListaPorAutorizar();
            }
            else
            {

                _snackBar.Add("Error al autorizar pedido", Severity.Error);
            }

            _processing = false;
        }

        private async Task Urgencias(PedidoModel args)
        {
            _processing = true;

            var user = await _authenticationManager.CurrentUser();
            RequestPedidoModel dataUrgencia = new RequestPedidoModel()
            {
                AnnoProceso = args.AnnoProceso,
                IDOperacion = args.IDOperacion,
                Correlativo = args.Correlativo,
                IDEtapa = 11,
                IDEstado = 5,
                IDUsuario = user.GetUserId()
            };

            var aUrgencia = await conexion.HttpClientInstance.PostAsJsonAsync<RequestPedidoModel>(urlAutorizacion, dataUrgencia);
            if (aUrgencia.IsSuccessStatusCode)
            {
                _snackBar.Add("Pedido Autorizado", Severity.Success);
                await ListaPorAutorizar();
            }
            else
            {
                _snackBar.Add("Error al autorizar pedido con Urgencia", Severity.Error);
            }

            _processing = false;
        }

        private async Task Anular(PedidoModel args)
        {
            _processing = true;

            var user = await _authenticationManager.CurrentUser();
            RequestPedidoModel dataAnular = new RequestPedidoModel()
            {
                AnnoProceso = args.AnnoProceso,
                IDOperacion = args.IDOperacion,
                Correlativo = args.Correlativo,
                IDEtapa = 0,
                IDEstado = 9,
                IDUsuario = user.GetUserId()
            };

            var aAnular = await conexion.HttpClientInstance.PostAsJsonAsync<RequestPedidoModel>(urlAutorizacion, dataAnular);
            if (aAnular.IsSuccessStatusCode)
            {
                _snackBar.Add("Pedido Anulado", Severity.Success);
                await ListaPorAutorizar();
            }
            else
            {
                _snackBar.Add("Error al anular pedido", Severity.Error);
            }

            _processing = false;
        }
        #endregion

        #region Filtro Input Text
        private Func<PedidoModel, bool> QuickFilter => x =>
        {
            //si no encuetra nada no manda nada
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            //Buscar por NRO de Documento 
            if (x.NroDocumento.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Buscar Por CLiente
            if (x.RazonSocial.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Busqueda mas amplia de los dos anterirore y agregando al vendedor, para mostrar multiples
            if ($"{x.NroDocumento} {x.RazonSocial} {x.Vendedor}".Contains(_searchString))
                return true;

            return false;
        };
        #endregion

    }
}
