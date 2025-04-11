using agDataAccess.Models.ConsultaLineasPickingPacking;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Componentes.SolicitudesLineas;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaLineasPickingPacking;
using PortalAG_V2.Shared.Services;
using System;
using static MudBlazor.CategoryTypes;

namespace PortalAG_V2.Pages.SolicitudInformes
{
    public partial class SolicitudesLineas
    {
        #region Variables
        LineasPendientes lineas;
        MainServices service;
        MudDatePicker? _DatePickerInicio;
        MudDatePicker? _DatePickerfin;
        private List<LineaspackingModel> Lineaspacking = new List<LineaspackingModel>();
        private List<LineaspickingModel> Lineaspicking = new List<LineaspickingModel>();
        private List<LineasModel> LineasPickeadas = new List<LineasModel>();        
        private List<ConsultarLinesaPickingPackingModel> Pedidos = new List<ConsultarLinesaPickingPackingModel>();
        private string _searchString;
        private string fInicio;
        private string fFin;
        DateTime? dateToday = DateTime.Today;
        DateTime? dateNull = null;
        private Timer _timer;
        private bool Loading = false;
        private bool _processing = false;
        #endregion

        #region URL's        
        string UrlPedidos = "Lineas/PedidosLineas";
        string UrlLineasPickeadas = "Lineas/LineasPorPickeadasPorUsuario";

        #endregion

        #region Al Cargar
        protected override async Task OnInitializedAsync()
        {
            //lineas.CargarDatosLineas();
            await CargarDatosPedidos();
        }
        #endregion

  

        #region Carga de datos
       
        public async Task CargarDatosPedidos()
        {
            try
            {               
                service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{UrlPedidos}");
                if (lista.IsSuccessStatusCode)
                {
                    Pedidos = JsonConvert.DeserializeObject<List<ConsultarLinesaPickingPackingModel>>(await lista.Content.ReadAsStringAsync());                                    
                }
                else
                {
                    Pedidos = new List<ConsultarLinesaPickingPackingModel>();
                    Loading = false;
                    snakBarCreation("Error!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }
        public async Task CargarDatosLineasPickeadas(string finicio, string ffin)
        {
            try
            {
                Loading = true;
                service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{UrlLineasPickeadas}/{finicio}/{ffin}");
                if (lista.IsSuccessStatusCode)
                {
                    LineasPickeadas = JsonConvert.DeserializeObject<List<LineasModel>>(await lista.Content.ReadAsStringAsync());
                    foreach (LineasModel item in LineasPickeadas)
                    {
                        foreach (var item1 in item.LineasPicking)
                        {
                            Lineaspicking.Add(item1);
                        }
                        foreach (var item2 in item.LineasPacking)
                        {
                            Lineaspacking.Add(item2);
                        }
                    }
                    Loading = false;
                }
                else
                {
                    LineasPickeadas = new List<LineasModel>();
                    Lineaspicking = new List<LineaspickingModel>();
                    Lineaspacking = new List<LineaspackingModel>();
                    Loading = false;
                    snakBarCreation("Error!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }

       
        #endregion

        #region FiltroSearchBar
        private Func<ConsultarLinesaPickingPackingModel, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.NroDocumento.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.RazonSocial.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if ($"{x.IDCliente} {x.NroOrden}".Contains(_searchString))
                return true;

            return false;
        };
        #endregion

        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion


        public async Task Buscar() 
        {
            //accion con boton buscar
            Lineaspicking = new List<LineaspickingModel>();
            Lineaspacking = new List<LineaspackingModel>();
            await CargarDatosLineasPickeadas(fInicio, fFin); 
        }
       

        // style the rows where the Element.Position == 0 to have italic text.
        private Func<CantidadLineadModel, string> _rowStyleFunc => (x) =>
        {
           
                return "font-style:italic; background-color:red;";

            
        };

    }
}
