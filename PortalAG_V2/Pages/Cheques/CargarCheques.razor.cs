using agDataAccess.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaLineasPickingPacking;
using PortalAG_V2.Shared.Models.Cheques;
using PortalAG_V2.Shared.Services;
using static PortalAG_V2.Pages.Movimientos.Dialog123;

namespace PortalAG_V2.Pages.Cheques
{
    public partial class CargarCheques
    {
        string _searchString;
        string UlrGetCheques = "Cheques/ConsultaListadoCheque/";
        public List<ChequesModel> ChequesList = new();
        public ChequesModel Cheque = new();
        bool Loading = false;
        bool _processing = false;
        DateTime? dateToday = DateTime.Today;
        DateTime? dateNull = null;
        private string fInicio;
        private string fFin;
        MainServices service;
        [Inject] IJSRuntime js { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //DateTime fechaActual = DateTime.Now;
            //string fFin = fechaActual.ToString("dd-MM-yyyy");
            //DateTime fechaSemanaAtras = fechaActual.AddDays(-7);
            //string fechaSemanaAtrasFormateada = fechaSemanaAtras.ToString("dd-MM-yyyy");
            //await GetCheques(fInicio, fFin);
            StateHasChanged();
        }

        private async Task GetCheques(string inicio, string fin)
        {
            try
            {
                ChequesList = new();
                service = new MainServices();
               
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UlrGetCheques}{inicio}/{fin}");
                if (lista.IsSuccessStatusCode)
                {
                    ChequesList = JsonConvert.DeserializeObject<List<ChequesModel>>(await lista.Content.ReadAsStringAsync());
                }
                else
                {
                    ChequesList = new List<ChequesModel>();
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
        private async Task PostCheques()
        {
            //TODO:  Eviara los cheques 
            await Task.Delay(1555);
        }

        private async Task cargarCheques()
        {
            _processing = true;
            //TODO: Para boton que cargara los cheques seleccionados
            await Task.Delay(3000);
            _processing = false;    
        }

        private Func<ChequesModel, bool> QuickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (x.IDCliente.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (x.RazonSocial.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{x.IDCliente}".Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        public async Task BuscarCheques()
        {
            _processing = true;
            await GetCheques(fInicio, fFin);
            _processing = false;
        }

        private async Task GenerarPDFCheques() 
        {
            await javaScript.GenerarPDFCheques(ChequesList);   
        }

        private async Task DownloadFileExcel()
        {
            try
            {
                Loading = true;
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("ChequesCarga");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Rut";
                worksheet.Row(1).Cell(2).Value = "Razon social";
                worksheet.Row(1).Cell(3).Value = "Banco";
                worksheet.Row(1).Cell(4).Value = "Nro Cta Banco";
                worksheet.Row(1).Cell(5).Value = "Nro Serie";
                worksheet.Row(1).Cell(6).Value = "Nro Comprobante";
                worksheet.Row(1).Cell(7).Value = "Fecha Ingreso";
                worksheet.Row(1).Cell(8).Value = "Fecha Vencimiento";
                worksheet.Row(1).Cell(9).Value = "Fecha Monto";
                worksheet.Row(1).Cell(10).Value = "Usuario";

                // Aplicar formato al encabezado
                for (int col = 1; col <= 10; col++)
                {
                    var cell = worksheet.Cell(1, col);
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                }               

                int row = 2;
                foreach (ChequesModel item in ChequesList)
                {
                    worksheet.Row(row).Cell(1).Value = item.IDCliente;
                    worksheet.Row(row).Cell(2).Value = item.RazonSocial;
                    worksheet.Row(row).Cell(3).Value = item.Banco;
                    worksheet.Row(row).Cell(4).Value = item.NroCtaCteBanco;
                    worksheet.Row(row).Cell(5).Value = item.NumeroSerie;
                    worksheet.Row(row).Cell(6).Value = item.NroComprobante;
                    worksheet.Row(row).Cell(7).Value = item.FechaCancelacion;
                    worksheet.Row(row).Cell(8).Value = item.FechaVencimiento;
                    worksheet.Row(row).Cell(9).Value = item.Monto;
                    worksheet.Row(row).Cell(10).Value = item.IDUsuario;
                    row++;
                }
                worksheet.Columns().AdjustToContents();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workBook.SaveAs(memoryStream);
                    await js.SaveAs("exelCheques.xlsx", memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Loading = false;
            StateHasChanged();
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
