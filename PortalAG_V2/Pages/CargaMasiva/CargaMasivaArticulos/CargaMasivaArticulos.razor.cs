using ClosedXML.Excel;
using FullBikePos.Shared.Models.CargaMasiva.Articulos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes.CargaMasivaArticulos;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Models.CargaMasiva.Articulos;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;
using static PortalAG_V2.Shared.Model.AsientoContable.AsientosContablesModel;

namespace PortalAG_V2.Pages.CargaMasiva.CargaMasivaArticulos
{
    public partial class CargaMasivaArticulos
    {
        [Inject] IJSRuntime js { get; set; }
        MainServices service;

        private IBrowserFile fileComplete = null;
        IList<IBrowserFile> files = new List<IBrowserFile>();
        private string NameFile = "";
        private bool Loading = false;
        private bool EnviarBtn = true;
        string UrlBscarValidar = "CargarPlanilla/Validar";
        string UlrPostArticulo = "CargarPlanilla/IngresarProducto";
        string UlrPostArchivo = "CargarPlanilla/CargaArchivo";
        private int Cotador = 0;

        public List<ArticuloExcel> dataTable = new List<ArticuloExcel>();
        public List<ArticuloExcelValidada> dataTableValidada = new List<ArticuloExcelValidada>();
        public List<ValidacionArticulo> validacionList = new();
        public List<ValidacionArticulo> validacionListValidada = new();
        public List<ArticuloExcel> ValidadaExcelDownload = new();
        public ArchivoAsiento ArchivoPost = new();
        public string GlobalFileName = "";

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
                var worksheet = workBook.Worksheets.Add("CargaArticulos");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Codigo";
                worksheet.Row(1).Cell(2).Value = "Rango";
                worksheet.Row(1).Cell(3).Value = "Nombre Factura (MAX 100 CARACTERES)";
                worksheet.Row(1).Cell(4).Value = "Nombre Interno";
                worksheet.Row(1).Cell(5).Value = "Unidad Venta";
                worksheet.Row(1).Cell(6).Value = "Unidad Compra";
                worksheet.Row(1).Cell(7).Value = "Provedor";
                worksheet.Row(1).Cell(8).Value = "Grupo Contable";
                worksheet.Row(1).Cell(9).Value = "Marca";
                worksheet.Row(1).Cell(10).Value = "NroParte";
                worksheet.Row(1).Cell(11).Value = "Si Desactivado (1 SI / 0 NO)";
                worksheet.Row(1).Cell(12).Value = "Si Bloqueado (1 SI / 0 NO)";
                worksheet.Row(1).Cell(13).Value = "Comentario SAP";


                // Aplicar formato al encabezado
                for (int col = 1; col <= 13; col++)
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

        private async Task DownloadFileProcesado(List<ArticuloExcel> data)
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("CargaArticulos");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Codigo";
                worksheet.Row(1).Cell(2).Value = "Rango";
                worksheet.Row(1).Cell(3).Value = "Nombre Factura (MAX 100 CARACTERES)";
                worksheet.Row(1).Cell(4).Value = "Nombre Interno";
                worksheet.Row(1).Cell(5).Value = "Unidad Venta";
                worksheet.Row(1).Cell(6).Value = "Unidad Compra";
                worksheet.Row(1).Cell(7).Value = "Provedor";
                worksheet.Row(1).Cell(8).Value = "Grupo Contable";
                worksheet.Row(1).Cell(9).Value = "Marca";
                worksheet.Row(1).Cell(10).Value = "NroParte";
                worksheet.Row(1).Cell(11).Value = "Si Desactivado (1 SI / 0 NO)";
                worksheet.Row(1).Cell(12).Value = "Si Bloqueado (1 SI / 0 NO)";
                worksheet.Row(1).Cell(13).Value = "Comentario SAP";


                // Aplicar formato al encabezado
                for (int col = 1; col <= 13; col++)
                {
                    var cell = worksheet.Cell(1, col);
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                }
                worksheet.Columns().AdjustToContents();
                int i = 2;
                foreach (var item in data)
                {
                    worksheet.Row(i).Cell(1).Value = item.Codigo;
                    worksheet.Row(i).Cell(2).Value = item.Rango;
                    worksheet.Row(i).Cell(3).Value = item.NombreFactura;
                    worksheet.Row(i).Cell(4).Value = item.NombreInterno;
                    worksheet.Row(i).Cell(5).Value = item.UnidadVenta;
                    worksheet.Row(i).Cell(6).Value = item.UnidadCompra;
                    worksheet.Row(i).Cell(7).Value = item.Provedor;
                    worksheet.Row(i).Cell(8).Value = item.GrupoContable;
                    worksheet.Row(i).Cell(9).Value = item.Marca;
                    worksheet.Row(i).Cell(10).Value = item.Marca;
                    worksheet.Row(i).Cell(11).Value = item.SiDesactivado == 0 ? "NO" : "SI";
                    worksheet.Row(i).Cell(12).Value = item.SiBloqueado == 0 ? "NO" : "SI";
                    worksheet.Row(i).Cell(13).Value = item.ComentarioSAP;
                    i++;
                }

                worksheet.Columns().AdjustToContents();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workBook.SaveAs(memoryStream);
                    ArchivoPost = new() { };
                    ArchivoPost.FileInfo = $"Procesado {GlobalFileName}";
                    ArchivoPost.Stream = Convert.ToBase64String(memoryStream.ToArray());
                    await PostArchivo(ArchivoPost);
                    //await js.SaveAs("ExcelProcesado.xlsx", memoryStream.ToArray());
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
                    dataTable = new List<ArticuloExcel>();
                    fileComplete = null;
                    NameFile = "";
                    return;
                }
                dataTable = new List<ArticuloExcel>();
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
        public static async Task<List<ArticuloExcel>> GetDataTableFromExcel(IBrowserFile file)
        {
            List<ArticuloExcel> articuloList = new List<ArticuloExcel>();

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
                        var headerRow = worksheet.Row(1);

                        // Validar que el archivo tenga las columnas esperadas
                        var columnasEsperadas = new[] { "Codigo", "Rango", "Nombre Factura (MAX 100 CARACTERES)", "Nombre Interno", "Unidad Venta", "Unidad Compra", "Provedor", "Grupo Contable", "Marca", "NroParte", "Si Desactivado (1 SI / 0 NO)", "Si Bloqueado (1 SI / 0 NO)", "Comentario SAP" };
                        var columnasEncontradas = headerRow.CellsUsed().Select(c => c.Value.ToString()).ToList();

                        if (!columnasEsperadas.All(col => columnasEncontradas.Contains(col)))
                        {
                            throw new Exception("El archivo no contiene todas las columnas esperadas.");
                        }

                        // Obtener índices de columnas
                        var columnIndexes = new Dictionary<string, int>();
                        foreach (var columna in columnasEsperadas)
                        {
                            columnIndexes[columna] = headerRow.CellsUsed()
                                .First(c => c.Value.ToString() == columna)
                                .Address.ColumnNumber;
                        }

                        // Recorrer filas
                        foreach (var row in worksheet.RowsUsed().Skip(1))
                        {
                            if (row.Cell(columnIndexes["Codigo"]).IsEmpty())
                            {
                                // Salta la fila si falta información en la celda de "Codigo"
                                continue;
                            }

                            var articulo = new ArticuloExcel() { };
                            articulo.Valido = true;
                            articulo.Codigo = row.Cell(columnIndexes["Codigo"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.Rango = row.Cell(columnIndexes["Rango"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.NombreFactura = row.Cell(columnIndexes["Nombre Factura (MAX 100 CARACTERES)"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.NombreInterno = row.Cell(columnIndexes["Nombre Interno"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.UnidadVenta = row.Cell(columnIndexes["Unidad Venta"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.UnidadCompra = row.Cell(columnIndexes["Unidad Compra"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.Provedor = row.Cell(columnIndexes["Provedor"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.GrupoContable = row.Cell(columnIndexes["Grupo Contable"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.Marca = row.Cell(columnIndexes["Marca"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.NroParte = row.Cell(columnIndexes["NroParte"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.ComentarioSAP = row.Cell(columnIndexes["Comentario SAP"]).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            articulo.SiDesactivado = !string.IsNullOrEmpty(row.Cell(columnIndexes["Si Desactivado (1 SI / 0 NO)"]).Value.ToString()) ? Int32.Parse(row.Cell(columnIndexes["Si Desactivado (1 SI / 0 NO)"]).Value.ToString()) : -1;
                            articulo.SiBloqueado = !string.IsNullOrEmpty(row.Cell(columnIndexes["Si Bloqueado (1 SI / 0 NO)"]).Value.ToString()) ? Int32.Parse(row.Cell(columnIndexes["Si Bloqueado (1 SI / 0 NO)"]).Value.ToString()) : -1;

                            articuloList.Add(articulo);
                        }

                        return articuloList;
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Error al procesar el archivo: {e.Message}");
                    }
                }
            }
        }

        #endregion

        #region Consulta a Endpoint 
        public async Task ValidarDataTable(List<ArticuloExcel> datatable)
        {
            try
            {
                dataTableValidada = new();
                validacionList = new();
                service = new MainServices();
                int linea = 1;
                int countError = 0;
                foreach (var row in datatable)
                {
                    ValidacionArticulo articulo = new ValidacionArticulo();
                    if (string.IsNullOrEmpty(row.Codigo))
                    {
                        snakBarCreation("Hay un Codigo de articulo vacio", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                        return;
                    }
                    articulo.ID = row.Codigo;
                    articulo.Estado = 0;
                    validacionList.Add(articulo);
                }
                var Response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync($"api/v2/{UrlBscarValidar}", validacionList);
                if (Response.IsSuccessStatusCode)
                {
                    var respuestaXarticulo = JsonConvert.DeserializeObject<List<ValidacionArticulo>>(await Response.Content.ReadAsStringAsync());
                    validacionListValidada = respuestaXarticulo;
                }
                foreach (var row in datatable)
                {
                    ArticuloExcelValidada articuloValidado = new ArticuloExcelValidada();
                    articuloValidado.Linea = linea++;
                    articuloValidado.Valido = true;
                    articuloValidado.Codigo = row.Codigo;
                    articuloValidado.Rango = row.Rango;
                    articuloValidado.NombreFactura = row.NombreFactura;
                    articuloValidado.NombreInterno = row.NombreInterno;
                    articuloValidado.UnidadVenta = row.UnidadVenta;
                    articuloValidado.UnidadCompra = row.UnidadCompra;
                    articuloValidado.Provedor = row.Provedor;
                    articuloValidado.GrupoContable = row.GrupoContable;
                    articuloValidado.Marca = row.Marca;
                    articuloValidado.NroParte = row.NroParte;
                    articuloValidado.ComentarioSAP = row.ComentarioSAP;
                    articuloValidado.SiBloqueado = row.SiBloqueado;
                    articuloValidado.SiDesactivado = row.SiDesactivado;
                    articuloValidado.Estado = validacionListValidada?.Find(x => x.ID == row.Codigo)?.Estado ?? 0;
                    if (String.IsNullOrEmpty(articuloValidado.NombreFactura) && articuloValidado.Estado == 0)
                    {
                        articuloValidado.Error = "Error - Nombre Factura no ingresado";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (String.IsNullOrEmpty(articuloValidado.NombreInterno) && articuloValidado.Estado == 0)
                    {
                        articuloValidado.Error = "Error - Nombre Interno";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (articuloValidado.NombreFactura.Length > 100)
                    {
                        articuloValidado.Error = "Error - Nombre Interno";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (articuloValidado.Codigo.Length > 15)
                    {
                        articuloValidado.Error = "Error - ID Articulo no puede tener mas de 15 caracteres";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (String.IsNullOrEmpty(articuloValidado.UnidadCompra) && articuloValidado.Estado == 0)
                    {
                        articuloValidado.Error = "Error - Unidad Compra";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (String.IsNullOrEmpty(articuloValidado.Provedor) && articuloValidado.Estado == 0)
                    {
                        articuloValidado.Error = "Error - Provedor";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (String.IsNullOrEmpty(articuloValidado.GrupoContable) && articuloValidado.Estado == 0)
                    {
                        articuloValidado.Error = "Error - Grupo Contable";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (String.IsNullOrEmpty(articuloValidado.Marca) && articuloValidado.Estado == 0)
                    {
                        articuloValidado.Error = "Error - Marca";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (String.IsNullOrEmpty(articuloValidado.ComentarioSAP) && articuloValidado.Estado == 0)
                    {
                        articuloValidado.Error = "Error - Comentario SAP";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (articuloValidado.SiDesactivado == null || articuloValidado.SiDesactivado == -1)
                    {
                        articuloValidado.Error = "Error - Si descativado vacio";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (string.IsNullOrEmpty(articuloValidado.Rango) || articuloValidado.Rango.Any(char.IsLetter))
                    {
                        articuloValidado.Error = "Error - Rango contiene caracteres no válidos";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (articuloValidado.SiBloqueado == null || articuloValidado.SiBloqueado == -1)
                    {
                        articuloValidado.Error = "Error - Si bloqueado vacio";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else
                    {
                        articuloValidado.Error = "Sin Error";
                        dataTableValidada.Add(articuloValidado);
                    }
                }
                if (countError == 0)
                {
                    EnviarBtn = false;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task PostArticuloSAP(ArticuloExcelValidada item)
        {
            try
            {
                service = new MainServices();
                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();
                service.ConectionService.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                ArticuloExcel articuloPost = new ArticuloExcel();
                articuloPost.Linea = item.Linea;
                articuloPost.Valido = item.Valido;
                articuloPost.Codigo = item.Codigo;
                articuloPost.Rango = item.Rango;
                articuloPost.NombreFactura = item.NombreFactura;
                articuloPost.NombreInterno = item.NombreInterno;
                articuloPost.UnidadVenta = item.UnidadVenta;
                articuloPost.UnidadCompra = item.UnidadCompra;
                articuloPost.Provedor = item.Provedor;
                articuloPost.GrupoContable = item.GrupoContable;
                articuloPost.Marca = item.Marca;
                articuloPost.NroParte = item.NroParte;
                articuloPost.ComentarioSAP = item.ComentarioSAP;
                articuloPost.Estado = item.Estado;
                articuloPost.SiBloqueado = item.SiBloqueado;
                articuloPost.SiDesactivado = item.SiDesactivado;
                service.ConectionService.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostArticulo}", articuloPost);
                if (Response.IsSuccessStatusCode)
                {
                    var data = await Response.Content.ReadAsStreamAsync();
                    ValidadaExcelDownload.Add(articuloPost);

                }
                else
                {
                    var data = await Response.Content.ReadAsStringAsync();
                    snakBarCreation($"Articulo {articuloPost.Codigo} no creado", Defaults.Classes.Position.BottomStart, Severity.Error, 3000);
                }
            }
            catch (Exception e)
            {
                snakBarCreation($"Error - {e.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }

        }

        #endregion

        private async Task EnviarSolicitud()
        {
            ValidadaExcelDownload = new();
            var parameters = new DialogParameters<DialogCargaMasiva> { };
            var options = new MudBlazor.DialogOptions() { CloseButton = false, DisableBackdropClick = true, };
            var dialog = await DialogService.ShowAsync<DialogCargaMasiva>("Question", parameters, options);
            Cotador = 0;
            var result = await dialog.Result;
            if ((bool)result.Data)
            {
                Loading = true;
                StateHasChanged();
                foreach (ArticuloExcelValidada item in dataTableValidada)
                {
                    await PostArticuloSAP(item);
                    Cotador++;
                    StateHasChanged();
                }
                await DownloadFileProcesado(ValidadaExcelDownload);
                await limpiar();

                snakBarCreation("Enviado", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                Loading = false;
                StateHasChanged();
            }
        }

        #region GuardarArchivo
        public async Task PostArchivo(ArchivoAsiento item)
        {
            try
            {
                service = new MainServices();
                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();
                service.ConectionService.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostArchivo}", item);


            }
            catch (Exception e)
            {
                snakBarCreation($"Error - {e.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }

        }
        #endregion

        private async Task limpiar()
        {
            fileComplete = null;
            NameFile = "";
            dataTable = new();
            dataTableValidada = new();
            EnviarBtn = true;
            validacionList = new();
            validacionListValidada = new();
            ValidadaExcelDownload = new();
            ArchivoPost = new();
            GlobalFileName = "";
        }


        #region SnackBar
        private void snakBarCreation(string msj, string position, Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion
    }
}
