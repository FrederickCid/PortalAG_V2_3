using DocumentFormat.OpenXml.Drawing.Charts;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes;
using PortalAG_V2.Componentes.EvaluacionCliente;
using PortalAG_V2.Componentes.PrePacking;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Models.ClienteEvaluacion;
using PortalAG_V2.Shared.Models.HubSpotModels;
using PortalAG_V2.Shared.Services;
using SheriffDataAccess.Models.SheriffModel;
using System.Globalization;
using static PortalAG_V2.Shared.Models.ClienteEvaluacion.ClienteAdicionalModel;
using static PortalAG_V2.Shared.Storage.StorageConstants;

namespace PortalAG_V2.Pages.EvaluacionClientes
{
    public partial class EvaluacionClientes
    {

        private string UrlGetCliente = "api/v2/Sheriff/Cliente/";
        private string UrlGetClienteAdicional = "api/v2/Sheriff/ClienteAdicional/";
        private string UrlGetClienteHubSpot = "api/v2/HubSpot/search/tickets/";
        private string UrlGetClienteHubSpotOwners = "api/v2/HubSpot/Owners/All/";
        private string UrlGetClienteHubSpotStages = "api/v2/HubSpot/Stages/All/";
        public bool _processing = false;
        public MainServices? service;
        public List<ClienteEvaluacionModel> ClienteResponse = new List<ClienteEvaluacionModel> { };
        public ResponseSearchModel ResponseSearch = new ResponseSearchModel { };
        public OwnerModel Owner = new OwnerModel { };
        public StagesModels stasge = new StagesModels { };
        public ClienteEvaluacionModel Cliente = new ClienteEvaluacionModel();
        public List<ClienteEvaluacionModel> ClientesList = new List<ClienteEvaluacionModel>();
        public List<ClienteEvaluacionModel> ClientesListResp = new List<ClienteEvaluacionModel>();
        public ClienteAdicionalModel ClienteAdicional = new ClienteAdicionalModel();
        public List<Ventasporsubcastegoria> ClienteAdicionalCategorias = new List<Ventasporsubcastegoria>() { };
        public List<Ventasporgama> ClienteAdicionalGama = new List<Ventasporgama>() { };
        public List<dynamic> HubSpotLista = new List<dynamic>();
        public List<dynamic> HubSpotListaOwners = new List<dynamic>();
        public bool _Loading = false;
        public string IDcliente;
        public string Enter = "Enter";
        public bool Evaluacion = false;
        public bool ListaClientes = false;
        public bool _expanded = false;
        public bool _expanded2 = false;
        public bool _expanded3 = false;
        public bool _expanded4 = false;
        public int minAnual = 0;
        public int maxAnual = 0;
        public int minMensual = 0;
        public int maxMensual = 0;
        public string vendedorFiltro = "";
        CultureInfo CL = new CultureInfo("es-CL");


        public List<string> nombresVendedores = new List<string>();

        PortalAG_V2.LazyLoad.Componentes.ChartGama chartGamaComponent = new PortalAG_V2.LazyLoad.Componentes.ChartGama();
        PortalAG_V2.LazyLoad.Componentes.ChartSubCategoria chartSubCategoriaComponent = new PortalAG_V2.LazyLoad.Componentes.ChartSubCategoria();

        public void FiltroVendedor(string vs)
        {
            vendedorFiltro = vs;
            Filtros();
        }
        public void Filtros()
        {
            List<ClienteEvaluacionModel> clientesFiltrados = new List<ClienteEvaluacionModel>();
            if (maxAnual > 0)
            {
                clientesFiltrados = ClientesListResp.Where(c => c.MontoCompraAnual >= minAnual && c.MontoCompraAnual <= maxAnual).ToList();
                if (maxMensual > 0)
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.MontoCompraMensual >= minMensual && c.MontoCompraMensual <= maxMensual).ToList();
                }
                else
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.MontoCompraMensual >= minMensual).ToList();
                }
            }
            else
            {
                clientesFiltrados = ClientesListResp.Where(c => c.MontoCompraAnual >= minAnual).ToList();
                if (maxMensual > 0)
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.MontoCompraMensual >= minMensual && c.MontoCompraMensual <= maxMensual).ToList();
                }
                else
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.MontoCompraMensual >= minMensual).ToList();
                }
            }

            //if (maxMensual > 0)
            //{
            //    clientesFiltrados = ClientesListResp.Where(c => c.MontoCompraMensual >= minMensual && c.MontoCompraMensual <= maxMensual).ToList();
            //}
            //else
            //{
            //    clientesFiltrados = ClientesListResp.Where(c => c.MontoCompraMensual >= minMensual).ToList();
            //}

            if (!String.IsNullOrEmpty(vendedorFiltro) && vendedorFiltro != "")
            {
                clientesFiltrados = clientesFiltrados.Where(c => c.Vendedor == vendedorFiltro).ToList();
            }
            ClientesList = clientesFiltrados;
        }
        public async void BuscarListaClientes()
        {
            try
            {
                _Loading = true;
                service = new MainServices();
                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();
                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"{UrlGetCliente}All");
                if (lista.IsSuccessStatusCode)
                {
                    ClientesListResp = JsonConvert.DeserializeObject<List<ClienteEvaluacionModel>>(await lista.Content.ReadAsStringAsync());
                    ClientesList = JsonConvert.DeserializeObject<List<ClienteEvaluacionModel>>(await lista.Content.ReadAsStringAsync());
                }

                nombresVendedores = ClientesList.Select(c => c.Vendedor).Distinct().ToList();
                Evaluacion = false;
                ListaClientes = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            _Loading = false;
            StateHasChanged();

        }
        public void BuscarTest(string _idcliente)
        {
            IDcliente = _idcliente;
            Buscar();
        }
        public async Task GetClienteEvaluado()
        {
            try
            {
                _Loading = true;
                service = new MainServices();
                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();
                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"{UrlGetCliente}{IDcliente}");

                if (lista.IsSuccessStatusCode)
                {
                    _processing = true;
                    ClienteResponse = JsonConvert.DeserializeObject<List<ClienteEvaluacionModel>>(await lista.Content.ReadAsStringAsync());
                    if (ClienteResponse.Count == 1 && ClienteResponse.FirstOrDefault().IDCliente != null)
                    {
                      
                        Cliente = ClienteResponse.FirstOrDefault();
                        await GetStagesHubSpot();
                        await GetOwnersHubSpot();
                        await GetClienteHubSpot();
                        await ClienteAdicionalConsulta();
                        await UpdateChartSeries();
                        Task.Delay(3000);
                        _processing = false;
                        ListaClientes = false;
                        Evaluacion = true;
                        _Loading = false;
                        snakBarCreation("Cliente Encontrado!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                        return;
                    }
                    if (ClienteResponse.Count > 1)
                    {
                        _processing = false;
                        _Loading = false;
                        StateHasChanged();
                        var parameters = new DialogParameters<ModalMultipleClientesComponent> { 
                            { x => x.List, ClienteResponse } 
                        };
                        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraLarge };
                        var dialog = await DialogService.ShowAsync<ModalMultipleClientesComponent>("Seleccionar Cliente", parameters, options);
                        var result = await dialog.Result;
                        var Data = (ClienteEvaluacionModel)result.Data;
                        if (Data != null)
                        {                            
                            _Loading =true;
                            Cliente = ClienteResponse.Find(x => x.IDCliente == Data.IDCliente);
                            await GetStagesHubSpot();
                            await GetOwnersHubSpot();
                            await GetClienteHubSpot();
                            await ClienteAdicionalConsulta();
                            await UpdateChartSeries();
                            Task.Delay(3000);
                            ListaClientes = false;
                            Evaluacion = true;
                            _Loading = false;
                            _processing = false;
                            snakBarCreation("Cliente Seleccionado!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                        }
                        return;

                    }
                    else
                    {
                        _Loading = false;
                        _processing = false;
                        Evaluacion = false;
                        snakBarCreation("Cliente No Encontrado!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    }

                }
                _Loading = false;
                StateHasChanged();

            }
            catch (Exception ex)
            {
                _Loading = false;
                _processing = false;
                Evaluacion = false;
                string mensaje = ex.Message;
            }
        }
        public async Task GetClienteHubSpot()
        {
            try
            {
                ResponseSearch = new ResponseSearchModel();
                service = new MainServices();
                var lista = await service.ConectionServiceNotaCredito.HttpClientInstance.GetAsync($"{UrlGetClienteHubSpot}{IDcliente}");

                if (lista.IsSuccessStatusCode)
                {
                    _processing = true;
                    ResponseSearch = JsonConvert.DeserializeObject<ResponseSearchModel>(await lista.Content.ReadAsStringAsync());

                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }
        public async Task GetOwnersHubSpot()
        {
            try
            {
                Owner = new OwnerModel();
                service = new MainServices();
                var lista = await service.ConectionServiceNotaCredito.HttpClientInstance.GetAsync($"{UrlGetClienteHubSpotOwners}");

                if (lista.IsSuccessStatusCode)
                {
                    _processing = true;
                    Owner = JsonConvert.DeserializeObject<OwnerModel>(await lista.Content.ReadAsStringAsync());

                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }
        public async Task GetStagesHubSpot()
        {
            stasge = new StagesModels();
            try
            {
                service = new MainServices();
                var lista = await service.ConectionServiceNotaCredito.HttpClientInstance.GetAsync($"{UrlGetClienteHubSpotStages}");

                if (lista.IsSuccessStatusCode)
                {
                    _processing = true;
                    stasge = JsonConvert.DeserializeObject<StagesModels>(await lista.Content.ReadAsStringAsync());

                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }
        public async Task ClienteAdicionalConsulta()
        {
            try
            {
                ClienteAdicionalGama = new List<Ventasporgama> { };
                ClienteAdicionalCategorias = new List<Ventasporsubcastegoria> { };
                ClienteAdicional = new ClienteAdicionalModel();
                service = new MainServices();
                var lista = await service.ConectionServiceNotaCredito.HttpClientInstance.GetAsync($"{UrlGetClienteAdicional}{Cliente.IDCliente}");

                if (lista.IsSuccessStatusCode)
                {
                    _processing = true;
                    ClienteAdicional = JsonConvert.DeserializeObject<ClienteAdicionalModel>(await lista.Content.ReadAsStringAsync());
                    ClienteAdicionalCategorias = ClienteAdicional.VentasPorSubCastegoria;
                    ClienteAdicionalGama = ClienteAdicional.VentasPorGama;

                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }

        public async Task Buscar()
        {
            try
            {
                await GetClienteEvaluado();
                if (Cliente != null)
                {
                    await UpdateChartSeries();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                snakBarCreation(ex.Message, Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }
        }

        private void OnExpandCollapseClickMarcas()
        {
            _expanded = !_expanded;
        }
        private void OnExpandCollapseClickGC()
        {
            _expanded2 = !_expanded2;
        }
        private void OnExpandCollapseClickDatosAdicionales()
        {
            _expanded3 = !_expanded3;
        }
        private void OnExpandCollapseClickEstadisticas()
        {
            _expanded4 = !_expanded4;
        }

        #region SnackBar


        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }

        #endregion SnackBar

        private void OnClickOpenModal()
        {
            var parameters = new DialogParameters<ModalHubSpotComponent>
            {
                {x => x.tickets, ResponseSearch },
                {x => x.OwnerLista, Owner },
                {x => x.StagesLista, stasge }
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = true, FullWidth = true, MaxWidth = MaxWidth.ExtraExtraLarge };
            DialogService.Show<ModalHubSpotComponent>($"", parameters, options);

        }
        private void OnClickOpenModalCredito()
        {
            var parameters = new DialogParameters<ModalCredito>
            {
                { x => x.creditoTotal, ClienteAdicional.CréditoTotal == null ? 0 : ClienteAdicional.CréditoTotal },
                { x => x.creditoUtilizado, ClienteAdicional.CréditoUtilizado == null ? 0 : ClienteAdicional.CréditoTotal },
                { x => x.creditoDisponible, ClienteAdicional.CréditoDisponible == null ? 0 : ClienteAdicional.CréditoTotal },
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium };
            DialogService.Show<ModalCredito>($"", parameters, options);
        }
        private void OnClickOpenModalCompPag()
        {
            var parameters = new DialogParameters<ModalComportamientoPago>
            {
                { X => X.compPag, ClienteAdicional.ComportamientosPago}
            };
            var options = new MudBlazor.DialogOptions() { CloseButton = true, FullWidth = true, MaxWidth = MaxWidth.Medium };
            DialogService.Show<ModalComportamientoPago>($"", parameters, options);

        }

        #region Actualiza Charts al momento de la carga de datos
        private async Task UpdateChartSeries()
        {
            try
            {
                await chartGamaComponent.chart.UpdateOptionsAsync(true, true, false);
                await chartSubCategoriaComponent.chart.UpdateOptionsAsync(true, true, false);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        private int CacularAnno(int Anno)
        {
            int AnnoCaculado = (Anno / 12);
            return AnnoCaculado;
        }
        private int CacularMesResta(int Anno)
        {
            int MessesCaculado = (Anno - ((Anno / 12) * 12));
            return MessesCaculado;
        }


        private string AntiguedadTexto(int anno)
        {
            if (CacularAnno(anno) == 0)
            {
                return $"{anno} Meses";
            }
            if (CacularAnno(anno) == 1 && CacularMesResta(anno) == 0)
            {
                return $"{CacularAnno(anno)} Año";
            }
            if (CacularAnno(anno) > 1 && CacularMesResta(anno) == 0)
            {
                return $"{CacularAnno(anno)} Años";
            }
            if (CacularAnno(anno) == 1 && CacularMesResta(anno) == 1)
            {
                return $"{CacularAnno(anno)} Año, {CacularMesResta(anno)} Mes";
            }
            if (CacularAnno(anno) > 1 && CacularMesResta(anno) == 1)
            {
                return $"{CacularAnno(anno)} Años, {CacularMesResta(anno)} Mes";
            }
            if (CacularAnno(anno) > 1 && CacularMesResta(anno) > 1)
            {
                return $"{CacularAnno(anno)} Años, {CacularMesResta(anno)} Meses";
            }
            else
            {
                return "< Mes";
            }
        }
    }
}
