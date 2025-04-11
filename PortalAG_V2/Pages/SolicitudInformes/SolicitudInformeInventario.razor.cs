using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Model.Impresion;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Services;
using Syncfusion.Blazor.Grids;

namespace PortalAG_V2.Pages.SolicitudInformes
{
    public partial class SolicitudInformeInventario
    {
        [Inject] IJSRuntime js { get; set; }
        MainServices service;
        MudDatePicker? _DatePickerInicio;
        MudDatePicker? _DatePickerfin;
        DateTime? dateToday = DateTime.Today;
        DateTime? dateNull = null;
        private string fInicio;
        private string fFin;
        private bool _processing = false;
        private bool _validateData = true;
        List<ConsultarBodegasModel> consultarBodegasModel = new();
        ConsultarBodegasModel SelectedconsultarBodegasModel = new();
        List<ConsultarSectorModel> consultarSector = new();
        ConsultarSectorModel SelectedconsultarSector = new();
        List<inventarioModel> data = new();
        private bool showCallAlert = true;
        bool Loading = false;
        string _searchString;


        string urlBodegas = "/api/v2/ImpresionEtiquetas/ConsultarBodega/";
        string urlConsultarSector = "/api/v2/ImpresionEtiquetas/ConsultarSector/";
        string url = "api/v2/InformeInventario";


        protected override async Task OnInitializedAsync()
        {
            await ConsultarBodegas();
        }

        private async Task ConsultarBodegas()
        {
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlBodegas}");
            if (result.IsSuccessStatusCode)
            {
                consultarBodegasModel = new();
                consultarBodegasModel = JsonConvert.DeserializeObject<List<ConsultarBodegasModel>>(await result.Content.ReadAsStringAsync());
            }
        }


        private async Task OnChangeBodega(ConsultarBodegasModel bodega)
        {
            SelectedconsultarBodegasModel = bodega;
            await ConsultarSector(bodega.IDBodega);

        }
        private async Task OnChangeSector(ConsultarSectorModel Sector)
        {
            SelectedconsultarSector = Sector;

        }
        private async Task ConsultarSector(int idBodega)
        {
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlConsultarSector}{idBodega}");
            if (result.IsSuccessStatusCode)
            {
                consultarSector = new();
                consultarSector = JsonConvert.DeserializeObject<List<ConsultarSectorModel>>(await result.Content.ReadAsStringAsync());
            }
        }
        async Task ProcessSomething()
        {
            try
            {
                Loading = true;
                _processing = true;
                service = new MainServices();
                var result = await service.ConectionService.HttpClientInstance.GetAsync($"{url}/{SelectedconsultarBodegasModel.SiglaBodega}/{SelectedconsultarSector.IDSector}/{fInicio}/{fFin}");
                if (result.IsSuccessStatusCode)
                {
                    data = new();
                    data = JsonConvert.DeserializeObject<List<inventarioModel>>(await result.Content.ReadAsStringAsync());
                    if (data.Count <= 0)
                    {
                        snakBarCreation("No hay datos para mostrar!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                        _processing = false;
                        Loading = false;
                        StateHasChanged();
                        return;
                    }
                    snakBarCreation("Encontrado!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    _processing = false;
                    Loading = false;
                    StateHasChanged();


                }
                else
                {
                    snakBarCreation("No hay datos para mostrar!!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    _processing = false;
                    Loading = false;
                    StateHasChanged();

                }

                _processing = false;
                Loading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _processing = false;
                Loading = false;
                StateHasChanged();
                snakBarCreation("ERROR AL PROCESAR!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }
        }
        #region Funcion Alerta
        private void CerrarAlerta()
        {
            showCallAlert = !showCallAlert;
        }
        #endregion

        private async Task GenerarExcel()
        {
            try
            {

                Loading = true;
                if (data.Count() > 0)
                {

                    XLWorkbook workBook = new XLWorkbook();
                    var worksheet = workBook.Worksheets.Add("Inventario");

                    // Agregar encabezado
                    worksheet.Row(1).Cell(1).Value = "IDUbicacion ";
                    worksheet.Row(1).Cell(2).Value = "Ubicacion";
                    worksheet.Row(1).Cell(3).Value = "IDArticulo";
                    worksheet.Row(1).Cell(4).Value = "Unidad x Bulto";
                    worksheet.Row(1).Cell(5).Value = "Stock AG";
                    worksheet.Row(1).Cell(6).Value = "Stock Sap";
                    worksheet.Row(1).Cell(7).Value = "Conteo Uno ";
                    worksheet.Row(1).Cell(8).Value = "Conteo Dos";
                    worksheet.Row(1).Cell(9).Value = "Conteo Tres ";
                    worksheet.Row(1).Cell(10).Value = "Total Inventario";
                    worksheet.Row(1).Cell(11).Value = "Observacion";


                    // Aplicar formato al encabezado
                    for (int col = 1; col <= 11; col++)
                    {
                        var cell = worksheet.Cell(1, col);
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                    }
                    int row = 2;
                    foreach (inventarioModel item in data)
                    {
                        worksheet.Row(row).Cell(1).Value = item.IDUbicacion;
                        worksheet.Row(row).Cell(2).Value = item.Ubicacion;
                        worksheet.Row(row).Cell(3).Value = item.IDArticulo;
                        worksheet.Row(row).Cell(4).Value = item.UnidadxBulto;
                        worksheet.Row(row).Cell(5).Value = item.StockAG;
                        worksheet.Row(row).Cell(6).Value = item.StockSap;
                        worksheet.Row(row).Cell(7).Value = item.ConteoUno;
                        worksheet.Row(row).Cell(8).Value = item.ConteoDos;
                        worksheet.Row(row).Cell(9).Value = item.ConteoTres;
                        worksheet.Row(row).Cell(10).Value = item.TotalInventario;
                        worksheet.Row(row).Cell(11).Value = item.Observacion;
                        row++;
                    }
                    Loading = false;
                    StateHasChanged();
                    worksheet.Columns().AdjustToContents();
                    MemoryStream memoryStream = new MemoryStream();
                    workBook.SaveAs(memoryStream);
                    await js.SaveAs($"iNFORME INVENTARIO {DateTime.Today.Day}_{DateTime.Now.Month}_ {DateTime.Now.Year}.xlsx", memoryStream.ToArray());
                }
                else
                {
                    Loading = false;
                    StateHasChanged();
                    snakBarCreation("ERROR AL EXPORTAR!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);

                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                Loading = false;
                StateHasChanged();
                Console.WriteLine(ex.Message);
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
        private Func<inventarioModel, bool> QuickFilter => x =>
        {
            // si no encuetra nada no manda nada
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            //Buscar por NRO de Documento 
            if (x.IDUbicacion.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Buscar Por CLiente
            if (x.IDArticulo.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;


            return false;
        };

        Func<ConsultarBodegasModel, string> convertBodegas = p => p.SiglaBodega.ToString();
        Func<ConsultarSectorModel, string> convertSector = p => p.Sector.ToString();
    }
}
