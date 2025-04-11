﻿using agDataAccess.Models.Stock;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;
namespace PortalAG_V2.Pages.SolicitudInformes
{
    public partial class SolcitudInformeStock
    {
        #region Variables
        MainServices service;
        SfGrid<StockBodegaBVModel> Grid;
        private bool _processing = false;
        private List<string> ButtonsGrid = new List<string>() { "Generar", "ExcelExport", "Search" };
        private List<StockBodegaBVModel> ResumenPedidos = new List<StockBodegaBVModel>() { };
        bool Loading = false;
        string[] SpecificCols = { "ItemCode" };
        #endregion

        #region URL's
        string URL = "/Stock/GetListadoStockBV";
        #endregion

        #region Cargar Datos
        public async Task CargarDatos()
        {
            try
            {
                Loading = true;                
                service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2{URL}");
                if (lista.IsSuccessStatusCode)
                {
                    ResumenPedidos = JsonConvert.DeserializeObject<List<StockBodegaBVModel>>(await lista.Content.ReadAsStringAsync());                  
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    Loading = false;
                }
                else
                {
                    ResumenPedidos = new List<StockBodegaBVModel>();
                    snakBarCreation("ERROR!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }
        #endregion

        #region Cargar al inicio
        protected override async Task OnInitializedAsync()
        {
            await CargarDatos();
        }
        #endregion

        #region Actualizar Btn
        async Task ProcessSomething()
        {
            _processing = true;            
            await CargarDatos();
            _processing = false;
        }
        #endregion

        #region GridBar Tools
        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (ResumenPedidos.Count() > 0)
                {
                    snakBarCreation("Exportando...", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    _processing = true;
                    await Grid.ExportToExcelAsync();
                    _processing = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    snakBarCreation("No hay datos para exportar", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                }
            }
            if (args.Item.Id == "Grid_Generar")
            {

                await CargarDatos();

            }
        }
        #endregion

        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
            _snackBar.Configuration.PreventDuplicates = false;           
        }

        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, bool Shows)
        {           
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
            _snackBar.Configuration.PreventDuplicates = Shows;
        }
        #endregion
    }
}
