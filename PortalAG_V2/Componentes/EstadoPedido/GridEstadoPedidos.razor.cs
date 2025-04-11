using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using System.Reflection.Metadata;
using MudBlazor;

using PortalAG_V2.Shared.Services;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using Microsoft.Extensions.Options;
using static MudBlazor.CategoryTypes;
using Syncfusion.Blazor.Grids;
using agDataAccess.Models;
using Syncfusion.Blazor.Charts.Chart.Models;
using System;

namespace PortalAG_V2.Componentes.EstadoPedido
{
    public partial class GridEstadoPedidos
    {

        SfGrid<EstadoPedidosNoMOD> Grid;

        public MainServices? service;

        private int Count = 60;

        private Timer timer;

        #region Variables
        bool Loading = false;
        string[] SpecificCols = { "NroDocumento", "IDCliente", "RazonSocial" };

        [Parameter]
        public string? opcion { get; set; }
        public int idEstado { get; set; }
        [Parameter] public Func<Task> funcionCargarTabs { get; set; }

        //RadzenTabs Tabs;
        TabPosition tabPositionDetail = TabPosition.Left;

        #endregion

        #region Listas

        public List<EstadoPedidosNoMOD>? listaPedidos = new List<EstadoPedidosNoMOD>();
        public List<EstadoPedidosNoMOD>? ListaPedidosDetail = new List<EstadoPedidosNoMOD>();
        public List<EstadoPedidosNoMOD>? listaPedidosDetailEnv = new List<EstadoPedidosNoMOD>();
        IEnumerable<EstadoPedidosNoMOD> estado;
        private List<string> ButtonsGrid = new List<string>() { "Actualizar", "ExcelExport", "Search" };

        #endregion

        #region Url

        string urlEncabezadoG = "GetEstadoPedidos/ObtenerEstadoPedidos";

        #endregion

        protected override async Task OnInitializedAsync()
        {
            if(listaPedidos.Count < 1)  await CargarGridPedidos();

           await base.OnInitializedAsync();
            //OnButtonClicked();
            estado = ListaPedidosDetail;
            //StartCountdown();

        }

        public async Task CargarGridPedidos()
        {
            try
            {
                Loading = true;
                
                listaPedidos = new List<EstadoPedidosNoMOD>();
                Count = 60;
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{urlEncabezadoG}/{opcion}");
                if (lista.IsSuccessStatusCode)
                {
                    listaPedidos = JsonConvert.DeserializeObject<List<EstadoPedidosNoMOD>>(await lista.Content.ReadAsStringAsync());
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    Loading = false;
                }
                else
                {
                    listaPedidos = new List<EstadoPedidosNoMOD>();
                    Loading = false;
                }
                StateHasChanged();
                Loading = false;
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }


        #region Dialog Fuciones

        public void DobleClick(RowSelectEventArgs<EstadoPedidosNoMOD> args)
        {
            var parameters = new DialogParameters<MudBlazorDialogCustom>
            {
                { x => x.Nombre, $"{args.Data.Vendedor}" },
                { x => x.Usuario, $"{args.Data.IDVendedor}" },
                { x => x.RSocial, $"{args.Data.RazonSocial}" },
                { x => x.RutRS, $"{args.Data.IDCliente}" },
                { x => x.option, $"{opcion}" },
                { x => x.FechaSolicitud, $"{args.Data.FechaTerminoAutorizacion}" },
                { x => x.Lista, args.Data }
            };
            //parameters.Add(x => x.ListaBulos, args.Data.Bultos);

            var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = true, MaxWidth = MaxWidth.ExtraExtraLarge };

            DialogService.Show<MudBlazorDialogCustom>("", parameters, options);

        }

        #endregion

        #region Actualizar
        public async Task Actualizar()
        {
            try
            {
                await CargarGridPedidos();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }

        #endregion

        #region StartCountdown



        //public void StartCountdown()
        //{
        //	try
        //	{
        //		timer = new Timer(new TimerCallback(async (e) =>
        //		{
        //			if (Count <= 0)
        //			{
        //				await Actualizar();
        //				Count = 60;
        //			}
        //			else
        //			{
        //				Count--;
        //			}
        //			await InvokeAsync(StateHasChanged);
        //		}), null, 1000, 1000);
        //	}
        //	catch (Exception ex)
        //	{
        //		string mensaje = ex.Message;
        //	}
        //}



        #endregion

        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            Console.WriteLine(args.Item.Id);
            if (args.Item.Id == "Grid_excelexport")
            {
                if (listaPedidos.Count() > 0)
                {
                    snakBarCreation("Exportando...", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    await Grid.ExportToExcelAsync();                    
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }                
                else
                {
                    snakBarCreation("No hay datos para exportar", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                }

            }
            if (args.Item.Id == "Grid_Actualizar")
            {
                Loading = true;
                await CargarGridPedidos();
                await funcionCargarTabs();
                Loading = false;
            }
        }

        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion



    }

}




