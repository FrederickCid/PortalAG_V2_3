using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2013.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes;
using PortalAG_V2.Pages.AvisoDePago;
using PortalAG_V2.Shared.Model.AsientoContable;
using PortalAG_V2.Shared.Model.AvisoDePago;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using PortalAG_V2.Shared.Services;
using PortalAG_V2.Shared.Helpers;
using SAPB1_Class.ResponseSB1;
using Syncfusion.Blazor.Grids;
using System;
using System.Data;
using System.Formats.Asn1;
using System.IO;
using System.Net.Http.Json;
using static PortalAG_V2.Shared.Models.HubSpotModels.OwnerModel;
using ApexCharts;


namespace PortalAG_V2.Pages.AsientoContable
{
    
    partial class AsientoContable
    {
        [Inject] IJSRuntime js { get; set; }

        MainServices service;

        private static string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full z-10";
        private string DragClass = DefaultDragClass;

        private IBrowserFile fileComplete = null;
        private string NameFile = "";

        //private IList<IBrowserFile> files = new List<IBrowserFile>();
        private AsientosContablesModel dataTable;

        string UrlCuentaContable = "api/v2/AsientoContable/ConsultarCuenta";
        string UrlEnvioSAP = "api/v2/AsientoContable/EnvioSAP";

        bool visibility = false;


        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion



        private void Clear()
        {
            fileComplete = null;
            NameFile = "";
            dataTable = null;
        }


        
        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            fileComplete = e.File;
            NameFile = e.File.Name;
        }
        private async Task ProcesarFiles()
        {
            visibility = true;
            service = new MainServices();
        
            if (fileComplete == null)
            {
                snakBarCreation("Debe seleccionar archivo", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                visibility = false;
                dataTable = null;
                fileComplete = null;
                NameFile = "";
            }
            else
            {

                dataTable = await GetDataTableFromExcel(fileComplete);

                if (dataTable == null)
                {
                    snakBarCreation("Error datos de archivo", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    visibility = false;
                }
                else if (dataTable.Detalle == null)
                {
                    snakBarCreation("Error en detalle de archivo", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    visibility = false;
                }
                else if (dataTable.Detalle.Exists(x => x.Cuenta == null || x.Cuenta == ""))
                {
                    dataTable = null;
                    snakBarCreation("Error en cuentas", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    visibility = false;

                }
                else if (dataTable.Detalle.Sum(x => x.Debito) != dataTable.Detalle.Sum(x => x.Credito))
                {
                    dataTable = null;
                    snakBarCreation("Error montos no concuerdan", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    visibility = false;
                }
                else if (dataTable.Detalle.Count() < 2)
                {
                    dataTable = null;
                    snakBarCreation("Error en cantidad de lineas", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    visibility = false;
                }
                else
                {
                    try
                    {
                        foreach (var detalle in dataTable.Detalle)
                        {

                            var auxdata = new CuentaContableModel()
                            {
                                Cuenta = detalle.Cuenta,
                                IDCliente = detalle.Cliente
                            };
                            var response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync<CuentaContableModel>(UrlCuentaContable, auxdata);
                            //var response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync(UrlEnvioAviso, avisoPago);
                            if (response.IsSuccessStatusCode)
                            {
                                var aux = await response.Content.ReadFromJsonAsync<List<CuentaContableModel>>();

                                dataTable.Detalle.Find(x => x.Id == detalle.Id ).nombreCuenta = aux.Find(x => x.Cuenta == detalle.Cuenta || x.Cuenta == detalle.Cliente).Nombre ?? "";
                                dataTable.Detalle.Find(x => x.Id == detalle.Id).SiCuentaCliente = aux.Find(x => x.Cuenta == detalle.Cuenta || x.Cuenta == detalle.Cliente).Cliente;
                            }
                            else
                            {
                                snakBarCreation("Error Consulta datos", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                                visibility = false;
                            }
                        }

                        if (dataTable.Detalle.Exists(x => x.nombreCuenta == "" || x.nombreCuenta == null))
                        {

                            dataTable = null;
                            visibility = false;
                            snakBarCreation("Error en cuentas contables", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        dataTable = null;
                        visibility = false;
                        snakBarCreation($"Error : {ex.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                    }

                    visibility = false;

                }
            }
}
        private async Task DownloadFile() {
            try
            {


                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("Asiento Contable");

                worksheet.ColumnWidth = 26;

                worksheet.Row(1).Cell(1).Value = "Fecha Contable";
                worksheet.Row(1).Cell(1).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(2).Cell(1).Value = "Fecha Vencimiento";
                worksheet.Row(2).Cell(1).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(3).Cell(1).Value = "Fecha Documento";
                worksheet.Row(3).Cell(1).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(4).Cell(1).Value = "Comentarios";
                worksheet.Row(4).Cell(1).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(5).Cell(1).Value = "Codigo Transaccion";
                worksheet.Row(5).Cell(1).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(6).Cell(1).Value = "Referencia 1";
                worksheet.Row(6).Cell(1).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(7).Cell(1).Value = "Referencia 2";
                worksheet.Row(7).Cell(1).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(8).Cell(1).Value = "Referencia 3";
                worksheet.Row(8).Cell(1).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(9).Cell(1).Value = "Cuenta movimiento";
                worksheet.Row(9).Cell(1).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(9).Cell(2).Value = "Debito";
                worksheet.Row(9).Cell(2).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(9).Cell(3).Value = "Credito";
                worksheet.Row(9).Cell(3).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(9).Cell(4).Value = "Referencia 1";
                worksheet.Row(9).Cell(4).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(9).Cell(5).Value = "Referencia 2";
                worksheet.Row(9).Cell(5).Style.Fill.BackgroundColor = XLColor.Yellow;
                worksheet.Row(9).Cell(6).Value = "Comentario";
                worksheet.Row(9).Cell(6).Style.Fill.BackgroundColor = XLColor.Yellow;

                

                //workBook.SaveAs("ejemplo.xlsx");
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workBook.SaveAs(memoryStream);
                    //memoryStream.Write();

                    await js.SaveAs("Ejemplo.xlsx", memoryStream.ToArray());

                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task EnvioSAP()
        {
           
            service = new MainServices();
            var user = await _authenticationManager.CurrentUser();
            var IDuse = user.GetFirstName();
            string _IDUser = user.GetUserId();

            var parameters = new DialogParameters<DialogConfirmacion> {
            { x => x.TextDialog, "El proceso es irreversible, solo puede ser anulado en SAP." },
            { x => x.Titulo, "Ingresar datos" },
            { x => x.nombreBoton, "ingresar" },
            };

            var dialog = await DialogService.ShowAsync<DialogConfirmacion>("Question", parameters);
            var result = await dialog.Result;

            if ((bool)result.Data)
            {
                visibility = true;
                service.ConectionService.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync<AsientosContablesModel>(UrlEnvioSAP, dataTable);
               
                if (response.IsSuccessStatusCode) 
                {
                   var aux = await response.Content.ReadFromJsonAsync<JournalEntriesResponse>();
                    snakBarCreation($"ingresado son exito : N-{aux.Number}", Defaults.Classes.Position.BottomStart, Severity.Success, 5000);
                    dataTable = null;
                    fileComplete = null;
                    NameFile = "";
                    visibility = false;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        var aux = await response.Content.ReadFromJsonAsync<string>();
                        snakBarCreation($"Error 500: {aux}", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                    }
                    else
                    {
                        var aux = await response.Content.ReadFromJsonAsync<string>();
                        snakBarCreation($"Error Consulta datos: {aux}", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                    }
                    visibility = false;
                }

            }
        }

        public static async Task<AsientosContablesModel> GetDataTableFromExcel(IBrowserFile file)
        {
            AsientosContablesModel asientosContables = new AsientosContablesModel();
            asientosContables.Detalle = new List<AsientosContablesModel.DetalleAsientos>();

            using (MemoryStream memStream = new MemoryStream())
            {
                await file.OpenReadStream(file.Size).CopyToAsync(memStream);

                asientosContables.file = new AsientosContablesModel.ArchivoAsiento()
                {
                    FileInfo = file.Name,
                    Stream = Convert.ToBase64String(memStream.ToArray())
                };

                using (XLWorkbook workBook = new XLWorkbook(memStream))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    int count = 1;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        //Use the first row to add columns to DataTable.
                        if (count < 9)
                        {
                            switch (count)
                            {
                                case 1:
                                    asientosContables.fechaContable = row.Cell(2).Value.ToString();
                                    break;
                                case 2:
                                    asientosContables.fechaVencimiento = row.Cell(2).Value.ToString();
                                    break;
                                case 3:
                                    asientosContables.fecha = row.Cell(2).Value.ToString();
                                    break;
                                case 4:
                                    asientosContables.Comentario = row.Cell(2).Value.ToString();
                                    break;
                                case 5:
                                    asientosContables.Codigo = row.Cell(2).Value.ToString();
                                    break;
                                case 6:
                                    asientosContables.Referencia = row.Cell(2).Value.ToString();
                                    break;
                                case 7:
                                    asientosContables.Referencia2 = row.Cell(2).Value.ToString();
                                    break;
                                case 8:
                                    asientosContables.Referencia3 = row.Cell(2).Value.ToString();
                                    break;
                            }
                        }
                        else
                        {
                            if (firstRow)
                            {
                                firstRow = false;
                            }
                            else if (!String.IsNullOrEmpty(row.Cell(1).Value.ToString()))
                            {
                                //Add rows to DataTable.
                                int i = 0;
                                var detalle = new AsientosContablesModel.DetalleAsientos();
                                try
                                {
                                    foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                    {
                                        switch (i)
                                        {
                                            case 0:
                                                detalle.Cuenta = cell.Value.ToString();
                                                if (detalle.Cuenta.Contains("-"))
                                                {
                                                    detalle.Cliente = detalle.Cuenta.Split("-").Count() > 2 ? detalle.Cuenta.Split("-")[1] + "-" + detalle.Cuenta.Split("-")[2] : detalle.Cuenta.Split("-")[1];
                                                }
                                                else
                                                {
                                                    detalle.Cliente = "";
                                                }
                                                break;
                                            case 1:
                                                detalle.Debito = Convert.ToInt32(cell.Value.ToString().Trim() == "" ? 0 : cell.Value.ToString().Trim());
                                                break;
                                            case 2:
                                                detalle.Credito = Convert.ToInt32(cell.Value.ToString().Trim() == "" ? 0 : cell.Value.ToString().Trim());
                                                break;
                                            case 3:
                                                detalle.Referencia1 = cell.Value.ToString();
                                                break;
                                            case 4:
                                                detalle.Referencia2 = cell.Value.ToString();
                                                break;
                                            case 5:
                                                detalle.Comentario = cell.Value.ToString();
                                                break;
                                        }
                                        i++;

                                    }
                                    detalle.Id = count;
                                    asientosContables.Detalle.Add(detalle);

                                }
                                catch (Exception ex) { 
                                    Console.WriteLine(ex.ToString());
                                }
                            } 
                        }
                        count += 1;
                    }
                }
                return asientosContables;
            }
        }

    }
}
