using ClosedXML.Excel;
using FullBikePos.Shared.Models.CargaMasiva.Articulos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes.CargaMasivaPedidos;
using PortalAG_V2.Pages.LiberarPedidos;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Models.CargaMasiva.Pedidos;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using static PortalAG_V2.Shared.Model.AsientoContable.AsientosContablesModel;

namespace PortalAG_V2.Pages.CargaMasiva.CargaMasivaVentaEnVerde
{
    public partial class CargaMasivaPedidosVerde
    {
        [Inject] IJSRuntime js { get; set; }
        MainServices service;

        private IBrowserFile fileComplete = null;
        IList<IBrowserFile> files = new List<IBrowserFile>();
        private string NameFile = "";
        private bool Loading = false;
        private bool EnviarBtn = true;
        private string UrlBscarValidar = "CargarPlanilla/Validar";
        string UlrPostPedido = "CargaExcelPedido/Pedido";
        string UlrPostArchivo = "CargaExcelPedido/CargaArchivo";
        private string UrlFormaPagoCliente = "api/v1/Cotizacion/FormaPagoCliente";
        private int Contador = 0;
        public string GlobalFileName = "";

        public CargaMasivaModel Pedido = new();
        public List<CargaMasivaModel> dataTable = new();
        public List<CargaMasivaModelValidada> dataTableValidada = new();
        public List<CargaMasivaModel> dataPost = new();
        public CargaMasivaDetalleModel dataTableDetalle = new();
        public List<CargaMasivaDetalleModel> dataTableDetalles = new();
        public List<CargaMasivaDetalleModelValidada> dataTableDetalleValidada = new();
        public List<ValidacionArticulo> validacionList = new();
        public List<ValidacionArticulo> validacionListValidada = new();
        public List<CargaMasivaModelResponse> dataTableResponse = new();
        public ArchivoAsiento ArchivoPost = new();

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
            GlobalFileName = "";

        }

        private async Task DownloadFile()
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("CargaPedidos");
                // Agregar encabezado                
                worksheet.Row(1).Cell(1).Value = "Nro. Orden (Codigo pedido)";
                worksheet.Row(1).Cell(2).Value = "Nro. F12";
                worksheet.Row(1).Cell(3).Value = "Fecha Emision";
                worksheet.Row(1).Cell(4).Value = "Rut (DNI)";
                worksheet.Row(1).Cell(5).Value = "SKU";
                worksheet.Row(1).Cell(6).Value = "Descripcion";
                worksheet.Row(1).Cell(7).Value = "Cantidad";
                worksheet.Row(1).Cell(8).Value = "Precio";
                worksheet.Row(1).Cell(9).Value = "Total Producto";
                worksheet.Row(1).Cell(10).Value = "Comentarios";
                //Por definir : pueden haber mas comentarios

                // Aplicar formato al encabezado
                for (int col = 1; col <= 10; col++)
                {
                    var cell = worksheet.Cell(1, col);
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#C0E6F5");
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
        private async Task DownloadFileProcesado(List<CargaMasivaModelResponse> data)
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("CargaPedidos");


                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Nro. Orden (Codigo pedido)";
                worksheet.Row(1).Cell(2).Value = "Nro. F12";
                worksheet.Row(1).Cell(3).Value = "Fecha Emision";
                worksheet.Row(1).Cell(4).Value = "Rut (DNI)";
                worksheet.Row(1).Cell(5).Value = "SKU";
                worksheet.Row(1).Cell(6).Value = "Descripcion";
                worksheet.Row(1).Cell(7).Value = "Cantidad";
                worksheet.Row(1).Cell(8).Value = "Precio";
                worksheet.Row(1).Cell(9).Value = "Total Producto";
                worksheet.Row(1).Cell(10).Value = "Comentarios";
                worksheet.Row(1).Cell(11).Value = "Nro. Pedido";
                worksheet.Row(1).Cell(12).Value = "Notas Pedido";
                worksheet.Row(1).Cell(13).Value = "Error";

                // Aplicar formato al encabezado
                for (int col = 1; col <= 14; col++)
                {
                    var cell = worksheet.Cell(1, col);
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#C0E6F5");
                }

                int i = 2;
                foreach (var item in data)
                {
                    foreach (var dett in item.DetallePedido)
                    {
                        // Agregar encabezado

                        worksheet.Row(i).Cell(1).Value = item.NroOrden;
                        worksheet.Row(i).Cell(2).Value = item.NroF12;
                        worksheet.Row(i).Cell(3).Value = item.FechaEmision;
                        worksheet.Row(i).Cell(4).Value = item.RutCliente;
                        worksheet.Row(i).Cell(5).Value = dett.SKU;
                        worksheet.Row(i).Cell(6).Value = dett.Descripcion;
                        worksheet.Row(i).Cell(7).Value = dett.Cantidad;
                        worksheet.Row(i).Cell(8).Value = dett.precio;
                        worksheet.Row(i).Cell(9).Value = dett.TotalProducto;
                        worksheet.Row(i).Cell(10).Value = item.Comentarios;
                        worksheet.Row(i).Cell(11).Value = item.NroPedido;
                        worksheet.Row(i).Cell(12).Value = item.NotasPedido;
                        worksheet.Row(i).Cell(13).Value = dett.ErrDescripcion;
                        i++;
                    }
                }
                worksheet.Columns().AdjustToContents();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workBook.SaveAs(memoryStream);
                    ArchivoPost = new() { };
                    ArchivoPost.FileInfo = $"Procesado {GlobalFileName}";
                    ArchivoPost.Stream = Convert.ToBase64String(memoryStream.ToArray());
                    await PostArchivo(ArchivoPost);
                    await js.SaveAs("ExcelProcesado.xlsx", memoryStream.ToArray());
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
                    dataTable = new();
                    fileComplete = null;
                    NameFile = "";
                    return;
                }
                dataTable = new();
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
        public async Task<List<CargaMasivaModel>> GetDataTableFromExcel(IBrowserFile file)
        {
            List<CargaMasivaModel> PedidoList = new List<CargaMasivaModel>();
            ArchivoAsiento archivo = new() { };
            // Validar la extensión del archivo
            var allowedExtensions = new[] { ".xlsx" };
            var fileExtension = Path.GetExtension(file.Name);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("Formato de archivo no válido. Solo se permiten archivos .xlsx.");
            }
            MemoryStream memStream = new MemoryStream();

            await file.OpenReadStream(file.Size).CopyToAsync(memStream);
            XLWorkbook workbook = new XLWorkbook(memStream);
            //enviar archivo
            archivo = new() { };
            GlobalFileName = file.Name;
            archivo.FileInfo = file.Name;
            archivo.Stream = Convert.ToBase64String(memStream.ToArray());
            await PostArchivo(archivo);

            try
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);
                foreach (IXLRow row in rows)
                {
                    if (row.Cell(1).IsEmpty())
                    {
                        // Salta la fila si falta información en las primeras dos
                        throw new Exception("Error una casilla de numero de orden vacio");
                    }
                    if (row.Cell(2).IsEmpty())
                    {
                        throw new Exception("Error una casilla de numero f12 vacio");
                    }
                    if (row.Cell(4).IsEmpty())
                    {
                        throw new Exception("Error una casilla de rut vacio");
                    }

                    CargaMasivaModel pedido = new CargaMasivaModel();
                    CargaMasivaDetalleModel pedidoDetalle = new CargaMasivaDetalleModel();
                    pedido.DetallePedido = new List<CargaMasivaDetalleModel>();

                    if (PedidoList.Count == 0)
                    {
                        pedido.NroOrden = row.Cell(1).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        pedido.NroF12 = row.Cell(2).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        pedido.FechaEmision = row.Cell(3).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        pedido.RutCliente = row.Cell(4).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        pedido.Comentarios = row.Cell(10).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        pedidoDetalle.SKU = row.Cell(5).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                        pedidoDetalle.Descripcion = row.Cell(6).Value.ToString();
                        pedidoDetalle.Cantidad = row.Cell(7).Value.ToString() == "" ? 0 : Int32.Parse(row.Cell(7).Value.ToString());
                        pedidoDetalle.precio = row.Cell(8).Value.ToString() == "" ? 0 : Int32.Parse(row.Cell(8).Value.ToString());
                        pedidoDetalle.TotalProducto = row.Cell(9).Value.ToString() == "" ? 0 : Int32.Parse(row.Cell(9).Value.ToString());
                        pedido.DetallePedido.Add(pedidoDetalle);
                        PedidoList.Add(pedido);

                    }
                    else
                    {
                        if (PedidoList.Any(x => x.NroOrden == row.Cell(1).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "")
                        && x.NroF12 == row.Cell(2).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "")))
                        {
                            pedidoDetalle.SKU = row.Cell(5).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            pedidoDetalle.Descripcion = row.Cell(6).Value.ToString();
                            pedidoDetalle.Cantidad = row.Cell(7).Value.ToString() == "" ? 0 : Int32.Parse(row.Cell(7).Value.ToString());
                            pedidoDetalle.precio = row.Cell(8).Value.ToString() == "" ? 0 : Int32.Parse(row.Cell(8).Value.ToString());
                            pedidoDetalle.TotalProducto = row.Cell(9).Value.ToString() == "" ? 0 : Int32.Parse(row.Cell(9).Value.ToString());
                            PedidoList.Find(x => x.NroOrden == row.Cell(1).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "") &&
                            x.NroF12 == row.Cell(2).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "")).DetallePedido.Add(pedidoDetalle);
                        }
                        else
                        {
                            pedido.NroOrden = row.Cell(1).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            pedido.NroF12 = row.Cell(2).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            pedido.FechaEmision = row.Cell(3).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            pedido.RutCliente = row.Cell(4).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            pedido.Comentarios = row.Cell(10).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            pedidoDetalle.SKU = row.Cell(5).Value.ToString().Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                            pedidoDetalle.Descripcion = row.Cell(6).Value.ToString();
                            pedidoDetalle.Cantidad = row.Cell(7).Value.ToString() == "" ? 0 : Int32.Parse(row.Cell(7).Value.ToString());
                            pedidoDetalle.precio = row.Cell(8).Value.ToString() == "" ? 0 : Int32.Parse(row.Cell(8).Value.ToString());
                            pedidoDetalle.TotalProducto = row.Cell(9).Value.ToString() == "" ? 0 : Int32.Parse(row.Cell(9).Value.ToString());
                            pedido.DetallePedido.Add(pedidoDetalle);
                            PedidoList.Add(pedido);
                        }
                    }

                }
                return PedidoList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }


        #endregion

        #region Consulta a Endpoint 
        public async Task ValidarDataTable(List<CargaMasivaModel> datatable)
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
                    foreach (var item in row.DetallePedido)
                    {

                        ValidacionArticulo articulo = new ValidacionArticulo();
                        if (string.IsNullOrEmpty(item.SKU))
                        {
                            snakBarCreation("Hay un Codigo de articulo vacio", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                            return;
                        }
                        articulo.ID = item.SKU;
                        articulo.Estado = 0;
                        validacionList.Add(articulo);
                    }
                }
                var Response = await service.ConectionService.HttpClientInstance.PostAsJsonAsync($"api/v2/{UrlBscarValidar}", validacionList);
                if (Response.IsSuccessStatusCode)
                {
                    var respuestaXarticulo = JsonConvert.DeserializeObject<List<ValidacionArticulo>>(await Response.Content.ReadAsStringAsync());
                    validacionListValidada = respuestaXarticulo;
                }
                else
                {
                    snakBarCreation("Hay error en un SKU", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    return;
                }
                foreach (var row in datatable)
                {
                    CargaMasivaModelValidada pedidoValidado = new();
                    pedidoValidado.DetallePedido = new();
                    CargaMasivaDetalleModelValidada dataTableDetalleValidada = new();
                    int Suma = 0;

                    pedidoValidado.Linea = linea++;
                    pedidoValidado.NroOrden = row.NroOrden;
                    pedidoValidado.NroF12 = row.NroF12;
                    pedidoValidado.FechaEmision = row.FechaEmision;
                    pedidoValidado.RutCliente = row.RutCliente;
                    pedidoValidado.IDFormaPago = 2;
                    pedidoValidado.IDCondicionPago = 1;
                    pedidoValidado.TipoEntrega = 3;
                    pedidoValidado.SiUrgencia = 0;
                    pedidoValidado.IDTransporte = 119;
                    pedidoValidado.Referencia = "";
                    pedidoValidado.Comentarios = row.Comentarios;
                    pedidoValidado.TipoDocumento = 1;
                    foreach (var item in row.DetallePedido)
                    {
                        dataTableDetalleValidada = new();
                        if (validacionListValidada.Any(x => x.ID == item.SKU) && validacionListValidada.Find(x => x.ID == item.SKU).Estado == 1)
                        {
                            dataTableDetalleValidada.SKU = item.SKU;
                            dataTableDetalleValidada.Descripcion = item.Descripcion;
                            dataTableDetalleValidada.Cantidad = item.Cantidad;
                            dataTableDetalleValidada.precio = item.precio;
                            dataTableDetalleValidada.TotalProducto = item.TotalProducto;
                            dataTableDetalleValidada.Estado = validacionListValidada.Find(x => x.ID == item.SKU).Estado;
                            dataTableDetalleValidada.Error = "";
                            pedidoValidado.DetallePedido.Add(dataTableDetalleValidada);

                        }
                        else
                        {
                            dataTableDetalleValidada.SKU = item.SKU;
                            dataTableDetalleValidada.Descripcion = item.Descripcion;
                            dataTableDetalleValidada.Cantidad = item.Cantidad;
                            dataTableDetalleValidada.precio = item.precio;
                            dataTableDetalleValidada.TotalProducto = item.TotalProducto;
                            dataTableDetalleValidada.Error = "Erro - Articulo no existe";
                            dataTableDetalleValidada.Estado = validacionListValidada.Find(x => x.ID == item.SKU).Estado;
                            pedidoValidado.DetallePedido.Add(dataTableDetalleValidada);
                            countError++;
                        }
                    }

                    if (pedidoValidado.DetallePedido.Any(x => x.Estado == 0))
                    {
                        pedidoValidado.Error = "Error - Articulo";
                        dataTableValidada.Add(pedidoValidado);
                        countError++;
                    }
                    else if (pedidoValidado.DetallePedido.Any(x => x.Cantidad <= 0))
                    {
                        pedidoValidado.Error = "Error - Cantidad";
                        dataTableValidada.Add(pedidoValidado);
                        countError++;
                    }
                    else if (pedidoValidado.DetallePedido.Any(x => x.precio <= 0))
                    {
                        pedidoValidado.Error = "Error - Precio";
                        dataTableValidada.Add(pedidoValidado);
                        countError++;
                    }
                    else if (pedidoValidado.DetallePedido.Any(x => String.IsNullOrEmpty(x.Descripcion)))
                    {
                        pedidoValidado.Error = "Error - En descripcion del articulo";
                        dataTableValidada.Add(pedidoValidado);
                        countError++;
                    }
                    else if (String.IsNullOrEmpty(pedidoValidado.FechaEmision))
                    {
                        pedidoValidado.Error = "Error - En Fecha de emision";
                        dataTableValidada.Add(pedidoValidado);
                        countError++;
                    }
                    else if (pedidoValidado.DetallePedido.Any(x => x.TotalProducto <= 0))
                    {
                        pedidoValidado.Error = "Error - En Total de producto";
                        dataTableValidada.Add(pedidoValidado);
                        countError++;
                    }

                    //else if (!(pedidoValidado.RutCliente.Contains("-") ? ValidaRut(pedidoValidado.RutCliente) :
                    //    ValidaRut(pedidoValidado.RutCliente.Substring(0, pedidoValidado.RutCliente.Length - 1),
                    //    pedidoValidado.RutCliente.Substring(pedidoValidado.RutCliente.Length - 1, 1))))
                    //{
                    //    pedidoValidado.Error = "Error - Rut";
                    //    dataTableValidada.Add(pedidoValidado);
                    //    countError++;
                    //}
                    else if (pedidoValidado.RutCliente != "77261280-Kc")
                    {
                        pedidoValidado.Error = "Error - Rut";
                        dataTableValidada.Add(pedidoValidado);
                        countError++;
                    }
                    else if (pedidoValidado.NroF12.Length >= 30)
                    {
                        pedidoValidado.Error = "Error - El numero F12 no puede ser mayor a 30 caracteres";
                        dataTableValidada.Add(pedidoValidado);
                        countError++;
                    }  else if (pedidoValidado.Comentarios.Length >= 50)
                    {
                        pedidoValidado.Error = "Error - El numero F12 no puede ser mayor a 50 caracteres";
                        dataTableValidada.Add(pedidoValidado);
                        countError++;
                    }
                    else
                    {

                        pedidoValidado.Error = "";
                        dataTableValidada.Add(pedidoValidado);
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

        public async Task PostPedidos(CargaMasivaModelValidada item)
        {
            try
            {
                service = new MainServices();
                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();
                service.EstadoPedidoTest.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedidoTest.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostPedido}", item );
                if (Response.IsSuccessStatusCode)
                {

                    var dataTableResponsevar = JsonConvert.DeserializeObject<CargaMasivaModelResponse>(await Response.Content.ReadAsStringAsync());
                    dataTableResponse.Add(dataTableResponsevar);
                }
                else 
                {
                    var data = await Response.Content.ReadAsStringAsync();
                }

            }
            catch (Exception e)
            {
                snakBarCreation($"Error - {e.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }

        }

        #endregion

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

        private async Task EnviarSolicitud()
        {
            var parameters = new DialogParameters<DialogCargaMasivaPedidos> { };
            var options = new DialogOptions() { CloseButton = false, DisableBackdropClick = true, };
            var dialog = await DialogService.ShowAsync<DialogCargaMasivaPedidos>("Question", parameters, options);
            Contador = 0;
            var result = await dialog.Result;
            if ((bool)result.Data)
            {
                Loading = true;
                StateHasChanged();
                foreach (CargaMasivaModelValidada item in dataTableValidada)
                {
                    await PostPedidos(item);
                    Contador++;
                    StateHasChanged();

                }
                await DownloadFileProcesado(dataTableResponse);
                await limpiar();
                snakBarCreation("Procesado", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                Loading = false;
                StateHasChanged();
            }
        }

        private async Task limpiar()
        {
            fileComplete = null;
            NameFile = "";
            EnviarBtn = true;
            Pedido = new();
            dataTable = new();
            dataTableValidada = new();
            dataPost = new();
            dataTableDetalle = new();
            dataTableDetalles = new();
            dataTableDetalleValidada = new();
            validacionList = new();
            validacionListValidada = new();
            dataTableResponse = new();
            GlobalFileName = "";

        }
        #region Utilidades
        public static bool ValidaRut(string rut)
        {
            rut = rut.Replace(".", "").ToUpper();
            Regex expresion = new Regex("^([0-9]+-[0-9K])$");
            string dv = rut.Substring(rut.Length - 1, 1);
            if (!expresion.IsMatch(rut))
            {
                return false;
            }
            char[] charCorte = { '-' };
            string[] rutTemp = rut.Split(charCorte);
            if (dv != Digito(int.Parse(rutTemp[0])))
            {
                return false;
            }
            return true;
        }

        public static bool ValidaRut(string rut, string dv)
        {
            return ValidaRut(rut + "-" + dv);
        }

        public static string Digito(int rut)
        {
            int suma = 0;
            int multiplicador = 1;
            while (rut != 0)
            {
                multiplicador++;
                if (multiplicador == 8)
                    multiplicador = 2;
                suma += (rut % 10) * multiplicador;
                rut = rut / 10;
            }
            suma = 11 - (suma % 11);
            if (suma == 11)
            {
                return "0";
            }
            else if (suma == 10)
            {
                return "K";
            }
            else
            {
                return suma.ToString();
            }
        }
        private Func<CargaMasivaModelValidada, int, string> _rowStyleFunc => (x, i) =>
        {
            if (x.Error.Length >= 1)
                return "background-color:#d9574e";

            return "";
        };


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
