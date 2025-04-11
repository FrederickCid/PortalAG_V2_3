using ClosedXML.Excel;
using FullBikePos.Shared.Models.CargaMasiva.Articulos;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using PortalAG_V2.Auth;
using PortalAG_V2.Shared.Models.CargaMasiva.Articulos;
using PortalAG_V2.Shared.Services;
using PortalAG_V2.Shared.Helpers;
using System.Net.Http.Json;
using PortalAG_V2.Componentes.CargaMasivaArticulos;
using PortalAG_V2.Shared.Model.CargaMasiva.Articulos;
using Newtonsoft.Json;
using static PortalAG_V2.Shared.Model.AsientoContable.AsientosContablesModel;

namespace PortalAG_V2.Pages.CargaMasiva.CargaMasivaArticulos
{
    public partial class CargaMasivaPrecio
    {
        [Inject] IJSRuntime js { get; set; }
        MainServices service;

        private IBrowserFile fileComplete = null;
        IList<IBrowserFile> files = new List<IBrowserFile>();
        private string NameFile = "";
        private bool Loading = false;
        private bool EnviarBtn = true;
        string UrlBscarValidar = "CargarPlanilla/Validar";
        string UlrPostArticulo = "CargarPlanilla/ActualizarPrecioProducto";
        string UlrPostArchivo = "CargarPlanilla/CargaArchivo";
        private int Cotador = 0;

        public ArticuloExcel Cheque = new ArticuloExcel();
        public List<AticuloPrecioModel> dataTable = new List<AticuloPrecioModel>();
        public List<AticuloPrecioValidationModel> dataTableValidada = new List<AticuloPrecioValidationModel>();
        public List<ValidacionArticulo> validacionList = new();
        public List<ValidacionArticulo> validacionListValidada = new();
        public List<AticuloPrecioModel> ValidadaExcelDownload = new();
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
                worksheet.Row(1).Cell(2).Value = "Nombre";
                worksheet.Row(1).Cell(3).Value = "Precio Anterior";
                worksheet.Row(1).Cell(4).Value = "Precio Nuevo";

                // Aplicar formato al encabezado
                for (int col = 1; col <= 4; col++)
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

        private async Task DownloadFileProcesado(List<AticuloPrecioModel> data)
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("CargaArticulos");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Codigo";
                worksheet.Row(1).Cell(2).Value = "Nombre";
                worksheet.Row(1).Cell(3).Value = "Precio Anterior";
                worksheet.Row(1).Cell(4).Value = "Precio Nuevo";

                // Aplicar formato al encabezado
                for (int col = 1; col <= 4; col++)
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
                    worksheet.Row(i).Cell(2).Value = item.Nombre;
                    worksheet.Row(i).Cell(3).Value = item.PrecioAnterior;
                    worksheet.Row(i).Cell(4).Value = item.PrecioNuevo;
                    i++;
                }

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
                    dataTable = new() { };
                    fileComplete = null;
                    NameFile = "";
                    return;
                }
                dataTable = new() { };
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
        public static async Task<List<AticuloPrecioModel>> GetDataTableFromExcel(IBrowserFile file)
        {
            List<AticuloPrecioModel> articuloList = new List<AticuloPrecioModel>();

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
                        var columnasEsperadas = new[] { "Codigo", "Nombre", "Precio Anterior", "Precio Nuevo" };
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

                            var articulo = new AticuloPrecioModel() { };
                            articulo.Codigo = row.Cell(columnIndexes["Codigo"]).Value.ToString();
                            articulo.Nombre = row.Cell(columnIndexes["Nombre"]).Value.ToString();
                            articulo.PrecioAnterior = Int32.Parse(row.Cell(columnIndexes["Precio Anterior"]).Value.ToString());
                            articulo.PrecioNuevo = Int32.Parse(row.Cell(columnIndexes["Precio Nuevo"]).Value.ToString());

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
        public async Task ValidarDataTable(List<AticuloPrecioModel> datatable)
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
                    AticuloPrecioValidationModel articuloValidado = new AticuloPrecioValidationModel();
                    articuloValidado.Linea = linea++;
                    articuloValidado.Codigo = row.Codigo;
                    articuloValidado.Nombre = row.Nombre;
                    articuloValidado.PrecioNuevo = row.PrecioNuevo;

                    if (validacionListValidada.Find(x=> x.ID.ToUpper() == articuloValidado.Codigo).Estado == 0)
                    {
                        articuloValidado.Error = "Error - Articulo no existe";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (String.IsNullOrEmpty(articuloValidado.Nombre))
                    {
                        articuloValidado.Error = "Error - Articulo no tiene nombre";
                        dataTableValidada.Add(articuloValidado);
                        countError++;
                    }
                    else if (articuloValidado.PrecioNuevo <= 0)
                    {
                        articuloValidado.Error = "Error - El precio del articulo no puede ser 0";
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


        public async Task PostArticuloSAP(AticuloPrecioValidationModel item)
        {
            try
            {
                service = new MainServices();
                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();
                service.ConectionService.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                AticuloPrecioModel articuloPost = new AticuloPrecioModel();
                articuloPost.Linea = item.Linea;
                articuloPost.Codigo = item.Codigo;
                articuloPost.PrecioAnterior = item.PrecioAnterior;
                articuloPost.PrecioNuevo = item.PrecioNuevo;
                articuloPost.Nombre = item.Nombre;
                var Response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostArticulo}", articuloPost);
                ValidadaExcelDownload.Add(articuloPost);

            }
            catch (Exception e)
            {
                snakBarCreation($"Error - {e.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }

        }

        #endregion

        private async Task EnviarSolicitud()
        {
            ValidadaExcelDownload = new() { };
            var parameters = new DialogParameters<DialogCargaMasiva> { };
            var options = new MudBlazor.DialogOptions() { CloseButton = false, DisableBackdropClick = true, };
            var dialog = await DialogService.ShowAsync<DialogCargaMasiva>("Question", parameters, options);
            Cotador = 0;
            var result = await dialog.Result;
            if ((bool)result.Data)
            {
                Loading = true;
                StateHasChanged();
                foreach (var item in dataTableValidada)
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

        private async Task limpiar()
        {
            fileComplete = null;
            NameFile = "";
            dataTable = new();
            dataTableValidada = new();
            EnviarBtn = true;

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
