using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes.Cheques;
using PortalAG_V2.Pages.AsientoContable;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Model.AsientoContable;
using PortalAG_V2.Shared.Model.Cheques;
using PortalAG_V2.Shared.Models.Cheques;
using PortalAG_V2.Shared.Services;
using System.Data;
using System.Globalization;
using System.Net.Http.Json;

namespace PortalAG_V2.Pages.Cheques
{
    public partial class CargaMasivaCheques
    {
        [Inject] IJSRuntime js { get; set; }
        MainServices service;

        private IBrowserFile fileComplete = null;
        IList<IBrowserFile> files = new List<IBrowserFile>();
        private string NameFile = "";
        private bool Loading = false;
        private bool EnviarBtn = true;
        string UrlBscar = "Cheques/ConsultaCheque/";
        string UlrPostChequesDeposito = "Cheques/ActualizarCheque/DepositoCarga";

        public ChequesModel Cheque = new ChequesModel();
        public List<ChequeExcelModel> dataTable = new List<ChequeExcelModel>();
        public List<ChequeExcelModel> dataTableValidada = new List<ChequeExcelModel>();
        public List<ChequePostModel> ChequesPost = new List<ChequePostModel>();

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            fileComplete = e.File;
            NameFile = e.File.Name;
        }
        private async Task Delete()
        {
            fileComplete = null;
            NameFile = "";
            EnviarBtn = true;

        }

        private async Task DownloadFile()
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("ChequesCarga");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "NroCtaCteBanco";
                worksheet.Row(1).Cell(2).Value = "NroComprobante";
                worksheet.Row(1).Cell(3).Value = "NumeroSerie";
                worksheet.Row(1).Cell(4).Value = "Monto";
                worksheet.Row(1).Cell(5).Value = "Rut Cliente";
                // Aplicar formato al encabezado
                for (int col = 1; col <= 5; col++)
                {
                    var cell = worksheet.Cell(1, col);
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                }
                worksheet.Columns().AdjustToContents();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workBook.SaveAs(memoryStream);
                    await js.SaveAs("Ejemplo.xlsx", memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region Procesar Archivo
        private async Task ProcesarFiles()
        {
            try
            {
                if (fileComplete == null)
                {
                    snakBarCreation("Debe seleccionar archivo", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                    Loading = false;
                    dataTable = new List<ChequeExcelModel>();
                    fileComplete = null;
                    NameFile = "";
                    return;
                }

                Loading = true;
                dataTable = await GetDataTableFromExcel(fileComplete);
                await ValidarDataTable(dataTable);
                Loading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                snakBarCreation($"Error - {ex.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                Loading = false;
            }
        }
        public static async Task<List<ChequeExcelModel>> GetDataTableFromExcel(IBrowserFile file)
        {
            List<ChequeExcelModel> chequesList = new List<ChequeExcelModel>();

            // Validar la extensión del archivo
            var allowedExtensions = new[] { ".xlsx" };
            var fileExtension = Path.GetExtension(file.Name);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("Formato de archivo no válido. Solo se permiten archivos .xlsx.");
            }
            using (MemoryStream memStream = new MemoryStream())
            {
                await file.OpenReadStream(file.Size).CopyToAsync(memStream);

                using (XLWorkbook workbook = new XLWorkbook(memStream))
                {
                    try
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed().Skip(1);
                        foreach (IXLRow row in rows)
                        {
                            if (row.Cell(1).IsEmpty() || row.Cell(2).IsEmpty())
                            {
                                // Salta la fila si falta información en las primeras dos
                                throw new Exception("Error Al Leer Los Datos");
                            }

                            ChequeExcelModel cheque = new ChequeExcelModel
                            {
                                NroCtaCteBanco = row.Cell(1).Value.ToString(),
                                NroComprobante = row.Cell(2).Value.ToString(),
                                NumeroSerie = row.Cell(3).Value.ToString(),
                                Monto = int.Parse(row.Cell(4).Value.ToString()),
                                IDCliente = row.Cell(5).Value.ToString(),
                                Alerta = ""
                            };
                            chequesList.Add(cheque);
                        }
                        return chequesList;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                        return null;
                    }
                }
            }
        }

        public async Task ValidarDataTable(List<ChequeExcelModel> datatable)
        {
            dataTableValidada = new List<ChequeExcelModel>();
            ChequesPost = new List<ChequePostModel>();
            int errorCount = 0;

            foreach (ChequeExcelModel item in datatable)
            {
                Cheque = await BuscarChecque(item.NumeroSerie);

                if (Cheque == null)
                {
                    item.Alerta = "Error - Cheque no existe";
                    dataTableValidada.Add(item);
                    errorCount++;
                }
                else
                {
                    if (item.NroComprobante != Cheque.NroComprobante)
                    {
                        item.Alerta = "Error - en el Nro de Comprobante";
                        dataTableValidada.Add(item);
                        errorCount++;
                    }
                    else if (item.IDCliente != Cheque.IDCliente)
                    {
                        item.Alerta = "Error - en el Rut Del cliente";
                        dataTableValidada.Add(item);
                        errorCount++;
                    }
                    else if (item.Monto != Cheque.Monto)
                    {
                        item.Alerta = "Error - en el monto";
                        dataTableValidada.Add(item);
                        errorCount++;
                    }
                    else if (item.NroCtaCteBanco != Cheque.NroCtaCteBanco)
                    {
                        item.Alerta = "Error - en el nro de cta banco";
                        dataTableValidada.Add(item);
                        errorCount++;
                    }
                    else
                    {
                        item.Alerta = "Sin Error";
                        dataTableValidada.Add(item);

                        var cheque = new ChequePostModel()
                        {
                            IDOperacion = Cheque.IDOperacion,
                            AnnoProceso = Cheque.AnnoProceso,
                            FechaCancelacion = Cheque.FechaCancelacion,
                            NroCtaCteBanco = Cheque.NroCtaCteBanco,
                            NroComprobante = Cheque.NroComprobante,
                            NumeroSerie = Cheque.NumeroSerie,
                            IDCliente = Cheque.IDCliente,
                            RazonSocial = Cheque.RazonSocial,
                            Monto = Cheque.Monto,
                            FechaVencimiento = Cheque.FechaVencimiento,
                            IDBanco = Cheque.IDBanco,
                            Banco = Cheque.Banco,
                            DocEntry = Cheque.DocEntry,
                            Comentario = "",
                        };
                        ChequesPost.Add(cheque);
                    }
                }
            }

            if (errorCount == 0)
            {
                EnviarBtn = false;
            }
        }
        #endregion

        #region Consulta a Endpoint 
        private async Task<ChequesModel> BuscarChecque(string serial)
        {
            try
            {
                service = new MainServices();

                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlBscar}{serial}");
                if (lista.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ChequesModel>(await lista.Content.ReadAsStringAsync());
                }
                else
                {
                    StateHasChanged();
                    return null;
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                return null;
            }
        }
        private async Task PostChequeDeposito(List<ChequePostModel> Post)
        {
            try
            {
                service = new MainServices();

                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostChequesDeposito}", Post);
                if (Response.IsSuccessStatusCode)
                {
                    var Data = Response.Content.ReadFromJsonAsync<ChequesModel>();
                    snakBarCreation($"ingresado son exito", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);

                }
                else
                {
                    snakBarCreation($"Error Al envíar", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

                }
            }
            catch (Exception e)
            {
                snakBarCreation($"Error - {e.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);

            }
        }
        #endregion
        private async Task EnviarSolicitud()
        {
            var parameters = new DialogParameters<DialogCargaMasiva> { };
            var options = new MudBlazor.DialogOptions() { CloseButton = false, DisableBackdropClick = true, };
            var dialog = await DialogService.ShowAsync<DialogCargaMasiva>("Question", parameters, options);

            var result = await dialog.Result;
            if (!string.IsNullOrEmpty((string)result.Data))
            {
                foreach (ChequePostModel item in ChequesPost) 
                {
                    item.Comentario = (string)result.Data;
                }
                await PostChequeDeposito(ChequesPost);
                limpiar();
            }
        }
        private void limpiar()
        {
            fileComplete = null;
            NameFile = "";
            dataTable = new List<ChequeExcelModel>();
            dataTableValidada = new List<ChequeExcelModel>();
            ChequesPost = new List<ChequePostModel>();
            EnviarBtn = true;

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
