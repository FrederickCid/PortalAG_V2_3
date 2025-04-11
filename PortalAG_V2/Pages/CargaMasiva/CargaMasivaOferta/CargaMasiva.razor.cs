using ClosedXML.Excel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Componentes.Cheques;
using PortalAG_V2.Shared.Model.Cheques;
using PortalAG_V2.Shared.Models.Cheques;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Model.CargaMAsivaOfertas;
using static PortalAG_V2.Shared.Models.HubSpotModels.StagesModels;
using PortalAG_V2.Componentes.CargaMasiva;
using static MudBlazor.CategoryTypes;
using Humanizer.Localisation.TimeToClockNotation;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using PortalAG_V2.Componentes;
using System;

namespace PortalAG_V2.Pages.CargaMasiva.CargaMasivaOferta
{
    public partial class CargaMasiva
    {
        [Inject] IJSRuntime js { get; set; }
        MainServices service;

        private IBrowserFile fileComplete = null;
        IList<IBrowserFile> files = new List<IBrowserFile>();
        private string NameFile = "";
        private bool Loading = false;
        private bool EnviarBtn = true;
        private int errorCount = 0;
        string UrlBscarArticulo = "CargaExcel/ValidarOferta";
        //Bulto
        string UlrPostArticulosBulto = "CargaExcel/OfertasBultos";
        //Ofertas
        string UlrPostArticulosOfertas = "CargaExcel/OfertasLista";
        //Outlet
        string UlrPostArticulosOutlet = "CargaExcel/Outlet";
        //
        string UlrPostArticulosFB = "CargaExcel/OfertaFullBike";
        //Web
        string UlrPostArticulosWeb = "CargaExcel/OfertasWeb";
        //Listado de ofertas 
        string UrlListadoOfertas = "Ofertas/Listado";
        //export bultos
        string UrlExportBultos = "Ofertas/Bultos";
        //export Ofertas
        string UrlExportOfertas = "Ofertas/lista";
        //export Outlet
        string UrlExportOutlet = "Ofertas/Outlet";
        //export web
        string UrlExportWeb = "Ofertas/Web";
        //export web
        string UrlExportFB = "Ofertas/FB";

        public int Opcion { get; set; } = 1;

        public OfertaResultModel ItemBulto = new OfertaResultModel();

        //Bultos
        public List<OfertaBultoModelExecelModel> dataTableBultos = new List<OfertaBultoModelExecelModel>();
        public List<OfertaBultoModelExecelModel> dataTableBultosValidadas = new List<OfertaBultoModelExecelModel>();
        public List<OfertaBultoModel> BultosOfertasPost = new List<OfertaBultoModel>();
        public List<OfertasBultosModel> OfertaListaExportBultos = new List<OfertasBultosModel>();

        //Ofertas
        public List<OfertasListadoModelExcel> dataTableOfertas = new List<OfertasListadoModelExcel>();
        public List<OfertasListadoModelExcel> dataTableOfertasValidadas = new List<OfertasListadoModelExcel>();
        public List<OfertasListadoModel> OfertasPost = new List<OfertasListadoModel>();
        public List<OfertasDescuentos> OfertaListaExportofertas = new List<OfertasDescuentos>();
        public List<OfertasWebModel> OfertaListaExportWeb = new List<OfertasWebModel>();

        //fullbike
        public List<OfertaFBModelExcel> dataTableOfertasFB = new() { };
        public List<OfertaFBModelExcel> dataTableOfertasValidadasFB = new() { };
        public List<OfertaFBModel> OfertasFBPost = new() { };
        public List<OfertaFBGETModel> OfertaListaExportFB = new() { };

        //validarLista
        public List<OfertaSistemaModel> OfertaLista = new() { };
        public List<OfertaSistemaModel> OfertaListaItem = new() { };


        //validar post
        public List<OfertaResultModel> ofertaVaslidaPost = new();


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

        #region Descarga ejemplos
        //Bulstos
        private async Task DownloadFileOfertaBulto()
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("Bulto");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Codigo";
                worksheet.Row(1).Cell(2).Value = "Nombre";
                worksheet.Row(1).Cell(3).Value = "Bulto Oferta";
                worksheet.Row(1).Cell(4).Value = "Precio Oferta";
                worksheet.Row(1).Cell(5).Value = "Accion (0 - Crear, 1 - Eliminar)";

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
                    await js.SaveAs("Ejemplo Bulto.xlsx", memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task onclickDownloadFileOfertaBulto()
        {
            try
            {
                Loading = true;
                service = new MainServices();
                OfertaListaExportBultos = new();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlExportBultos}");
                if (lista.IsSuccessStatusCode)
                {
                    Loading = false;
                    OfertaListaExportBultos = JsonConvert.DeserializeObject<List<OfertasBultosModel>>(await lista.Content.ReadAsStringAsync());
                    XLWorkbook workBook = new XLWorkbook();
                    var worksheet = workBook.Worksheets.Add("Bulto");

                    // Agregar encabezado
                    worksheet.Row(1).Cell(1).Value = "ID Articulo";
                    worksheet.Row(1).Cell(2).Value = "Nombre";
                    worksheet.Row(1).Cell(3).Value = "Correlativo Oferta";
                    worksheet.Row(1).Cell(4).Value = "Bulto Oferta";
                    worksheet.Row(1).Cell(5).Value = "Precio Oferta";
                    worksheet.Row(1).Cell(6).Value = "Fecha Actualizacion";
                    worksheet.Row(1).Cell(7).Value = "Usuario";
                    // Aplicar formato al encabezado
                    for (int col = 1; col <= 7; col++)
                    {
                        var cell = worksheet.Cell(1, col);
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                    }
                    int row = 2;
                    foreach (OfertasBultosModel item in OfertaListaExportBultos)
                    {
                        worksheet.Row(row).Cell(1).Value = item.IDArticulo;
                        worksheet.Row(row).Cell(2).Value = item.Nombre;
                        worksheet.Row(row).Cell(3).Value = item.CorrelativoOferta;
                        worksheet.Row(row).Cell(4).Value = item.BultoOferta;
                        worksheet.Row(row).Cell(5).Value = item.PrecioOferta;
                        worksheet.Row(row).Cell(6).Value = item.FechaActualizacion;
                        worksheet.Row(row).Cell(7).Value = item.IDUsuario;
                        row++;
                    }


                    worksheet.Columns().AdjustToContents();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workBook.SaveAs(memoryStream);
                        await js.SaveAs("Articulos Ofertas Bultos.xlsx", memoryStream.ToArray());
                    }
                }
                else
                {
                    Loading = false;
                    snakBarCreation("ERROR AL EXPORTAR!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                Loading = false;
                Console.WriteLine(ex.Message);
            }
        }
        //Ofertas
        private async Task DownloadFileOferta()
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("Oferta");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Codigo";//si
                worksheet.Row(1).Cell(2).Value = "Nombre";//si
                worksheet.Row(1).Cell(3).Value = "Lista Descuento";//si id
                //worksheet.Row(1).Cell(4).Value = "Aplicar Web";////si
                worksheet.Row(1).Cell(4).Value = "Fecha Inicio";//si solo si aplica web
                worksheet.Row(1).Cell(5).Value = "Fecha Termino";//si solo si aplica web
                //worksheet.Row(1).Cell(7).Value = "Porcentaje";
                //worksheet.Row(1).Cell(8).Value = "Outlet";
                worksheet.Row(1).Cell(6).Value = "Accion (0 - Crear, 1 - Eliminar)";
                // Aplicar formato al encabezado
                for (int col = 1; col <= 6; col++)
                {
                    var cell = worksheet.Cell(1, col);
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                }
                worksheet.Columns().AdjustToContents();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workBook.SaveAs(memoryStream);
                    await js.SaveAs("Ejemplo Oferta.xlsx", memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task onclickDownloadFileOferta()
        {
            try
            {
                Loading = true;
                service = new MainServices();
                OfertaListaExportofertas = new();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlExportOfertas}");
                if (lista.IsSuccessStatusCode)
                {
                    Loading = false;
                    OfertaListaExportofertas = JsonConvert.DeserializeObject<List<OfertasDescuentos>>(await lista.Content.ReadAsStringAsync());
                    XLWorkbook workBook = new XLWorkbook();
                    var worksheet = workBook.Worksheets.Add("Bulto");

                    // Agregar encabezado
                    worksheet.Row(1).Cell(1).Value = "ID Articulo";
                    worksheet.Row(1).Cell(2).Value = "Nombre";
                    worksheet.Row(1).Cell(3).Value = "Lista Descuento";
                    worksheet.Row(1).Cell(4).Value = "FechaInicio Descuento";
                    worksheet.Row(1).Cell(5).Value = "FechaTermino Descuento";
                    // Aplicar formato al encabezado
                    for (int col = 1; col <= 5; col++)
                    {
                        var cell = worksheet.Cell(1, col);
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                    }
                    int row = 2;
                    foreach (OfertasDescuentos item in OfertaListaExportofertas)
                    {
                        worksheet.Row(row).Cell(1).Value = item.IDArticulo;
                        worksheet.Row(row).Cell(2).Value = item.Nombre;
                        worksheet.Row(row).Cell(3).Value = item.ListaDescuento;
                        worksheet.Row(row).Cell(4).Value = item.FechaInicioDescuento;
                        worksheet.Row(row).Cell(5).Value = item.FechaTerminoDescuento;
                        row++;
                    }
                    worksheet.Columns().AdjustToContents();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workBook.SaveAs(memoryStream);
                        await js.SaveAs("Articulos Ofertas.xlsx", memoryStream.ToArray());
                    }
                }
                else
                {
                    Loading = false;
                    snakBarCreation("ERROR AL EXPORTAR!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                Loading = false;
                Console.WriteLine(ex.Message);
            }
        }
        //Outel
        private async Task DownloadFileOfertaOutlet()
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("Outlet");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Codigo";
                worksheet.Row(1).Cell(2).Value = "Nombre";
                worksheet.Row(1).Cell(3).Value = "Lista Descuento";
                worksheet.Row(1).Cell(4).Value = "Accion (0 - Crear, 1 - Eliminar)";

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
                    await js.SaveAs("Ejemplo Outlet.xlsx", memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task onclickDownloadFileOfertaOutlet()
        {
            try
            {
                Loading = true;
                service = new MainServices();
                OfertaListaExportofertas = new();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlExportOutlet}");
                if (lista.IsSuccessStatusCode)
                {
                    Loading = false;
                    OfertaListaExportofertas = JsonConvert.DeserializeObject<List<OfertasDescuentos>>(await lista.Content.ReadAsStringAsync());
                    XLWorkbook workBook = new XLWorkbook();
                    var worksheet = workBook.Worksheets.Add("Bulto");

                    // Agregar encabezado
                    worksheet.Row(1).Cell(1).Value = "ID Articulo";
                    worksheet.Row(1).Cell(2).Value = "Nombre";
                    worksheet.Row(1).Cell(3).Value = "Si Lista Descuento";
                    worksheet.Row(1).Cell(4).Value = "Lista Descuento";
                    worksheet.Row(1).Cell(5).Value = "Porcentaje Descuento";
                    worksheet.Row(1).Cell(6).Value = "Si Web";

                    // Aplicar formato al encabezado
                    for (int col = 1; col <= 6; col++)
                    {
                        var cell = worksheet.Cell(1, col);
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                    }
                    int row = 2;
                    foreach (OfertasDescuentos item in OfertaListaExportofertas)
                    {
                        worksheet.Row(row).Cell(1).Value = item.IDArticulo;
                        worksheet.Row(row).Cell(2).Value = item.Nombre;
                        worksheet.Row(row).Cell(3).Value = item.SiListaDescuento;
                        worksheet.Row(row).Cell(4).Value = item.ListaDescuento;
                        worksheet.Row(row).Cell(5).Value = item.PorcentajeDescuento;
                        worksheet.Row(row).Cell(6).Value = "SI";


                        row++;
                    }


                    worksheet.Columns().AdjustToContents();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workBook.SaveAs(memoryStream);
                        await js.SaveAs("Articulos Ofertas Outlet.xlsx", memoryStream.ToArray());
                    }
                }
                else
                {
                    Loading = false;
                    snakBarCreation("ERROR AL EXPORTAR!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                Loading = false;
                Console.WriteLine(ex.Message);
            }
        }
        //web
        private async Task DownloadFileWeb()
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("Oferta");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Codigo";//si
                worksheet.Row(1).Cell(2).Value = "Nombre";//si
                worksheet.Row(1).Cell(3).Value = "Porcentaje";
                worksheet.Row(1).Cell(4).Value = "Fecha Inicio";
                worksheet.Row(1).Cell(5).Value = "Fecha Termino";
                worksheet.Row(1).Cell(6).Value = "Accion (0 - Crear, 1 - Eliminar)";
                // Aplicar formato al encabezado
                for (int col = 1; col <= 6; col++)
                {
                    var cell = worksheet.Cell(1, col);
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                }
                worksheet.Columns().AdjustToContents();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workBook.SaveAs(memoryStream);
                    await js.SaveAs("Ejemplo Web.xlsx", memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //web
        private async Task DownloadFileFullbike()
        {
            try
            {
                XLWorkbook workBook = new XLWorkbook();
                var worksheet = workBook.Worksheets.Add("Oferta");

                // Agregar encabezado
                worksheet.Row(1).Cell(1).Value = "Codigo";//si
                worksheet.Row(1).Cell(2).Value = "Precio Normal";
                worksheet.Row(1).Cell(3).Value = "Precio Oferta";
                worksheet.Row(1).Cell(4).Value = "Nombre";//si
                worksheet.Row(1).Cell(5).Value = "Accion (0 - Crear, 1 - Eliminar)";
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
                    await js.SaveAs("Ejemplo FullBike.xlsx", memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task onclickDownloadFileFB()
        {
            try
            {
                Loading = true;
                service = new MainServices();
                OfertaListaExportofertas = new();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlExportFB}");
                if (lista.IsSuccessStatusCode)
                {
                    Loading = false;
                    OfertaListaExportFB = JsonConvert.DeserializeObject<List<OfertaFBGETModel>>(await lista.Content.ReadAsStringAsync());
                    if (!String.IsNullOrEmpty(OfertaListaExportFB.FirstOrDefault().IDCliente))
                    {
                        XLWorkbook workBook = new XLWorkbook();
                        var worksheet = workBook.Worksheets.Add("Bulto");

                        // Agregar encabezado
                        worksheet.Row(1).Cell(1).Value = "ID Articulo";
                        worksheet.Row(1).Cell(2).Value = "Precio Normal";
                        worksheet.Row(1).Cell(3).Value = "Precio Oferta";
                        worksheet.Row(1).Cell(4).Value = "Nombre";
                        worksheet.Row(1).Cell(5).Value = "usuario";
                        // Aplicar formato al encabezado
                        for (int col = 1; col <= 5; col++)
                        {
                            var cell = worksheet.Cell(1, col);
                            cell.Style.Font.Bold = true;
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                        }
                        int row = 2;
                        foreach (OfertaFBGETModel item in OfertaListaExportFB)
                        {
                            worksheet.Row(row).Cell(1).Value = item.IDArticulo;
                            worksheet.Row(row).Cell(2).Value = item.PrecioVentaNormal;
                            worksheet.Row(row).Cell(3).Value = item.PrecioVenta;
                            worksheet.Row(row).Cell(4).Value = item.Nombre;
                            worksheet.Row(row).Cell(5).Value = item.IDUsuario;

                            row++;
                        }


                        worksheet.Columns().AdjustToContents();

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            workBook.SaveAs(memoryStream);
                            await js.SaveAs("Articulos Oferta FB.xlsx", memoryStream.ToArray());
                        }
                    }
                    else
                    {
                        Loading = false;
                        snakBarCreation("ERROR AL EXPORTAR!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    }
                    StateHasChanged();
                }

            }
            catch (Exception ex)
            {
                Loading = false;
                Console.WriteLine(ex.Message);
            }
        }
        private async Task onclickDownloadFileWeb()
        {
            try
            {
                Loading = true;
                service = new MainServices();
                OfertaListaExportofertas = new();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlExportWeb}");
                if (lista.IsSuccessStatusCode)
                {
                    Loading = false;
                    OfertaListaExportWeb = JsonConvert.DeserializeObject<List<OfertasWebModel>>(await lista.Content.ReadAsStringAsync());
                    XLWorkbook workBook = new XLWorkbook();
                    var worksheet = workBook.Worksheets.Add("Bulto");

                    // Agregar encabezado
                    worksheet.Row(1).Cell(1).Value = "ID Articulo";
                    worksheet.Row(1).Cell(2).Value = "Nombre";
                    worksheet.Row(1).Cell(3).Value = "Porcentaje Descuento";
                    worksheet.Row(1).Cell(4).Value = "FechaInicio Descuento";
                    worksheet.Row(1).Cell(5).Value = "FechaTermino Descuento";
                    // Aplicar formato al encabezado
                    for (int col = 1; col <= 5; col++)
                    {
                        var cell = worksheet.Cell(1, col);
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#FFD700");
                    }
                    int row = 2;
                    foreach (OfertasWebModel item in OfertaListaExportWeb)
                    {
                        worksheet.Row(row).Cell(1).Value = item.IDArticulo;
                        worksheet.Row(row).Cell(2).Value = item.Nombre;
                        worksheet.Row(row).Cell(3).Value = item.PorcentajeDescuentoWeb;
                        worksheet.Row(row).Cell(4).Value = item.FechaInicioDescuento;
                        worksheet.Row(row).Cell(5).Value = item.FechaTerminoDescuento;

                        row++;
                    }


                    worksheet.Columns().AdjustToContents();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        workBook.SaveAs(memoryStream);
                        await js.SaveAs("Articulos Web.xlsx", memoryStream.ToArray());
                    }
                }
                else
                {
                    Loading = false;
                    snakBarCreation("ERROR AL EXPORTAR!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                Loading = false;
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Procesar Archivo
        private async Task ProcesarFiles()
        {
            try
            {

                if (fileComplete == null)
                {
                    snakBarCreation("Debe seleccionar archivo", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                    Loading = false;
                    dataTableBultos = new List<OfertaBultoModelExecelModel>();
                    fileComplete = null;
                    NameFile = "";
                    return;
                }

                Loading = true;
                switch (Opcion)
                {
                    case 1:
                        dataTableBultos = await GetDataTableFromExcel(fileComplete);
                        await ValidarDataTable(dataTableBultos);
                        Loading = false;
                        StateHasChanged();
                        break;
                    case 2:
                        dataTableOfertas = await GetDataTableFromExcelOfertas(fileComplete);
                        await ValidarDataTableOfertas(dataTableOfertas);
                        Loading = false;
                        StateHasChanged();
                        break;
                    case 3:
                        dataTableOfertas = await GetDataTableFromExcelOutlet(fileComplete);
                        await ValidarDataTableOutlet(dataTableOfertas);
                        Loading = false;
                        StateHasChanged();
                        break;
                    case 4:
                        dataTableOfertas = await GetDataTableFromExcelWeb(fileComplete);
                        await ValidarDataTableWeb(dataTableOfertas);
                        Loading = false;
                        StateHasChanged();
                        break;
                    case 5:
                        dataTableOfertasFB = await GetDataTableFromExcelFB(fileComplete);
                        await ValidarDataTableFB(dataTableOfertasFB);
                        Loading = false;
                        StateHasChanged();
                        break;
                    default:
                        Loading = false;
                        StateHasChanged();
                        break;
                }

            }
            catch (Exception ex)
            {
                snakBarCreation($"Error - {ex.Message}", Defaults.Classes.Position.BottomStart, Severity.Error, 5000);
                Loading = false;
            }
        }
        public static async Task<List<OfertaBultoModelExecelModel>> GetDataTableFromExcel(IBrowserFile file)
        {
            List<OfertaBultoModelExecelModel> BultoList = new List<OfertaBultoModelExecelModel>();
            // Validar la extensión del archivo
            // Validar la extensión del archivo
            var allowedExtensions = new[] { ".xlsx" };
            var fileExtension = Path.GetExtension(file.Name);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("Formato de archivo no válido. Solo se permiten archivos .xlsx.");
            }

            MemoryStream memStream = new MemoryStream();
            await file.OpenReadStream(file.Size).CopyToAsync(memStream);

            XLWorkbook workbook = null;
            try
            {
                workbook = new XLWorkbook(memStream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);
                foreach (IXLRow row in rows)
                {
                    if (row.Cell(1).IsEmpty() || row.Cell(2).IsEmpty() || row.Cell(5).IsEmpty())
                    {
                        // Salta la fila si falta información en las primeras dos
                        throw new Exception("Error Al Leer Los Datos");
                    }

                    OfertaBultoModelExecelModel BultoItem = new OfertaBultoModelExecelModel
                    {
                        IDArticulo = row.Cell(1).Value.ToString(),
                        Nombre = row.Cell(2).Value.ToString(),
                        CantidadOfeta = int.Parse(row.Cell(3).Value.ToString()),
                        PrecioOferta = int.Parse(row.Cell(4).Value.ToString()),
                        SiActivo = int.Parse(row.Cell(5).Value.ToString()),
                        Alerta = ""
                    };
                    BultoList.Add(BultoItem);
                }
                return BultoList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Dispose();
                }
                memStream.Dispose();
            }

        }
        public static async Task<List<OfertasListadoModelExcel>> GetDataTableFromExcelOfertas(IBrowserFile file)
        {
            List<OfertasListadoModelExcel> BultoList = new List<OfertasListadoModelExcel>();
            // Validar la extensión del archivo
            var allowedExtensions = new[] { ".xlsx" };
            var fileExtension = Path.GetExtension(file.Name);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("Formato de archivo no válido. Solo se permiten archivos .xlsx.");
            }

            MemoryStream memStream = new MemoryStream();
            await file.OpenReadStream(file.Size).CopyToAsync(memStream);

            XLWorkbook workbook = null;
            try
            {
                workbook = new XLWorkbook(memStream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);
                foreach (IXLRow row in rows)
                {
                    if (row.Cell(1).IsEmpty() || row.Cell(2).IsEmpty())
                    {
                        // Salta la fila si falta información en las primeras dos
                        throw new Exception("Excel Formato incorrecto");
                    }

                    OfertasListadoModelExcel OfertaItem = new OfertasListadoModelExcel
                    {
                        IDArticulo = row.Cell(1).Value.ToString(),
                        Nombre = row.Cell(2).Value.ToString(),
                        ListaDescuneto = int.Parse(row.Cell(3).Value.ToString()),
                        SiAplicaWEB = 0,
                        FechaInicio = Convert.ToDateTime(row.Cell(4).Value.ToString()),
                        FechaTermino = Convert.ToDateTime(row.Cell(5).Value.ToString()),
                        SiActivo = int.Parse(row.Cell(6).Value.ToString()),
                        Alerta = ""
                    };
                    BultoList.Add(OfertaItem);
                }
                return BultoList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Dispose();
                }
                memStream.Dispose();
            }
        }
        public static async Task<List<OfertasListadoModelExcel>> GetDataTableFromExcelOutlet(IBrowserFile file)
        {
            List<OfertasListadoModelExcel> BultoList = new List<OfertasListadoModelExcel>();
            // Validar la extensión del archivo
            var allowedExtensions = new[] { ".xlsx" };
            var fileExtension = Path.GetExtension(file.Name);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("Formato de archivo no válido. Solo se permiten archivos .xlsx.");
            }

            MemoryStream memStream = new MemoryStream();
            await file.OpenReadStream(file.Size).CopyToAsync(memStream);

            XLWorkbook workbook = null;
            try
            {
                workbook = new XLWorkbook(memStream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);
                foreach (IXLRow row in rows)
                {
                    if (row.Cell(1).IsEmpty() || row.Cell(2).IsEmpty())
                    {
                        // Salta la fila si falta información en las primeras dos
                        throw new Exception("Excel Formato incorrecto");
                    }

                    OfertasListadoModelExcel OfertaItem = new OfertasListadoModelExcel
                    {
                        IDArticulo = row.Cell(1).Value.ToString(),
                        Nombre = row.Cell(2).Value.ToString(),
                        SiAplicaWEB = 1,
                        ListaDescuneto = int.Parse(row.Cell(3).Value.ToString()),
                        SiActivo = int.Parse(row.Cell(4).Value.ToString()),
                        Alerta = ""
                    };
                    BultoList.Add(OfertaItem);
                }
                return BultoList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Dispose();
                }
                memStream.Dispose();
            }

        }
        public static async Task<List<OfertasListadoModelExcel>> GetDataTableFromExcelWeb(IBrowserFile file)
        {
            List<OfertasListadoModelExcel> BultoList = new List<OfertasListadoModelExcel>();
            // Validar la extensión del archivo
            var allowedExtensions = new[] { ".xlsx" };
            var fileExtension = Path.GetExtension(file.Name);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("Formato de archivo no válido. Solo se permiten archivos .xlsx.");
            }

            MemoryStream memStream = new MemoryStream();
            await file.OpenReadStream(file.Size).CopyToAsync(memStream);

            XLWorkbook workbook = null;
            try
            {
                workbook = new XLWorkbook(memStream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);
                foreach (IXLRow row in rows)
                {
                    if (row.Cell(1).IsEmpty() || row.Cell(2).IsEmpty())
                    {
                        // Salta la fila si falta información en las primeras dos
                        throw new Exception("Excel Formato incorrecto");
                    }

                    OfertasListadoModelExcel OfertaItem = new OfertasListadoModelExcel
                    {
                        IDArticulo = row.Cell(1).Value.ToString(),
                        Nombre = row.Cell(2).Value.ToString(),
                        Porcentaje = Convert.ToDouble(row.Cell(3).Value.ToString()),
                        SiAplicaWEB = 1,
                        FechaInicio = Convert.ToDateTime(row.Cell(4).Value.ToString()),
                        FechaTermino = Convert.ToDateTime(row.Cell(5).Value.ToString()),
                        SiActivo = int.Parse(row.Cell(6).Value.ToString()),
                        Alerta = ""
                    };
                    BultoList.Add(OfertaItem);
                }
                return BultoList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Dispose();
                }
                memStream.Dispose();
            }
        }
        //fullbike ofertas
        public static async Task<List<OfertaFBModelExcel>> GetDataTableFromExcelFB(IBrowserFile file)
        {
            List<OfertaFBModelExcel> OfertaFBList = new List<OfertaFBModelExcel>();
            // Validar la extensión del archivo
            var allowedExtensions = new[] { ".xlsx" };
            var fileExtension = Path.GetExtension(file.Name);
            if (!allowedExtensions.Contains(fileExtension.ToLower()))
            {
                throw new Exception("Formato de archivo no válido. Solo se permiten archivos .xlsx.");
            }

            MemoryStream memStream = new MemoryStream();
            await file.OpenReadStream(file.Size).CopyToAsync(memStream);

            XLWorkbook workbook = null;
            try
            {
                workbook = new XLWorkbook(memStream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1);
                foreach (IXLRow row in rows)
                {
                    if (row.Cell(1).IsEmpty() || row.Cell(2).IsEmpty())
                    {
                        // Salta la fila si falta información en las primeras dos
                        throw new Exception("Excel Formato incorrecto");
                    }

                    OfertaFBModelExcel OfertaItem = new OfertaFBModelExcel
                    {
                        IDCliente = "76809030-0",
                        IDArticulo = row.Cell(1).Value.ToString(),
                        PrecioNormal = Convert.ToInt32(row.Cell(2).Value.ToString()),
                        PrecioCliente = Convert.ToInt32(row.Cell(3).Value.ToString()),
                        Nombre = row.Cell(4).Value.ToString(),
                        IDEstado = int.Parse(row.Cell(5).Value.ToString()),
                        Alerta = ""
                    };
                    OfertaFBList.Add(OfertaItem);
                }
                return OfertaFBList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Dispose();
                }
                memStream.Dispose();
            }
        }

        public async Task ValidarDataTable(List<OfertaBultoModelExecelModel> datatable)
        {
            dataTableBultosValidadas = new List<OfertaBultoModelExecelModel>();
            BultosOfertasPost = new List<OfertaBultoModel>();
            errorCount = 0;

            foreach (OfertaBultoModelExecelModel item in datatable)
            {
                ItemBulto = await BuscarArticulo(item.IDArticulo, Opcion);

                if (ItemBulto == null)
                {
                    item.Alerta = "Error - Validacion del articulo erronea o articulo no existe";
                    dataTableBultosValidadas.Add(item);
                    errorCount++;
                }
                else if (ItemBulto.IDEstado != item.SiActivo)
                {
                    if (item.IDArticulo != ItemBulto.IDArticulo)
                    {
                        item.Alerta = "Error - en el IDArticulo de Articulo";
                        dataTableBultosValidadas.Add(item);
                        errorCount++;
                    }
                    // se puede usar any para que si existe uno en la lista debuelva un bool para validar el if
                    else if (dataTableBultosValidadas.Any(x => x.IDArticulo == item.IDArticulo && x.CantidadOfeta == item.CantidadOfeta))
                    {
                        item.Alerta = "Error - IDarticulo repetido con la misma cantidad de bultos";
                        dataTableBultosValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.Nombre.Length <= 0 || string.IsNullOrEmpty(item.Nombre))
                    {
                        item.Alerta = "Error - en el Nombre Del Articulo";
                        dataTableBultosValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.CantidadOfeta <= 0)
                    {
                        item.Alerta = "Error -  Cantidad Ofeta del Articulo";
                        dataTableBultosValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.PrecioOferta <= 0)
                    {
                        item.Alerta = "Error - en el Precio Oferta del Articulo";
                        dataTableBultosValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.SiActivo != datatable.FirstOrDefault().SiActivo)
                    {
                        item.Alerta = "Error - Solo puede ser un tipo de acción (0 o 1)";
                        dataTableBultosValidadas.Add(item);
                        errorCount++;
                    }
                    else
                    {
                        item.Alerta = "Sin Error";
                        dataTableBultosValidadas.Add(item);

                        var Bulto = new OfertaBultoModel()
                        {
                            IDArticulo = ItemBulto.IDArticulo,
                            CantidadOfeta = item.CantidadOfeta,
                            PrecioOferta = item.PrecioOferta,
                            SiActivo = item.SiActivo

                        };
                        BultosOfertasPost.Add(Bulto);
                    }
                }
                else
                {
                    item.Alerta = $"Error - {ItemBulto.Result}";
                    dataTableBultosValidadas.Add(item);
                    errorCount++;
                }
            }

            if (errorCount == 0)
            {
                EnviarBtn = false;
            }
        }
        public async Task ValidarDataTableOfertas(List<OfertasListadoModelExcel> datatable)
        {
            dataTableOfertasValidadas = new List<OfertasListadoModelExcel>();
            OfertasPost = new List<OfertasListadoModel>();
            errorCount = 0;
            HashSet<string> idArticulos = new HashSet<string>();
            OfertaListaItem = new();
            foreach (OfertasListadoModelExcel item in datatable)
            {
                //validando lista  y articulo
                ItemBulto = await BuscarArticulo(item.IDArticulo, Opcion);
                OfertaListaItem = await ValidarLista(item.ListaDescuneto);

                if (ItemBulto == null)
                {
                    item.Alerta = ItemBulto.Result;
                    dataTableOfertasValidadas.Add(item);
                    errorCount++;
                }
                else if (ItemBulto.IDEstado != item.SiActivo)
                {
                    if (item.IDArticulo != ItemBulto.IDArticulo)
                    {
                        item.Alerta = "Error - en el IDArticulo de Articulo";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (!idArticulos.Add(item.IDArticulo))
                    {
                        item.Alerta = "Error - IDarticulo repetido";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.Nombre.Length <= 0 || string.IsNullOrEmpty(item.Nombre))
                    {
                        item.Alerta = "Error - en el Nombre Del Articulo";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.FechaInicio > item.FechaTermino)
                    {
                        item.Alerta = "Error - Fecha inicio no puede se mayor a fecha termino";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.SiActivo != datatable.FirstOrDefault().SiActivo)
                    {
                        item.Alerta = "Error - Solo puede ser un tipo de acción (0 o 1)";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.ListaDescuneto == null)
                    {
                        item.Alerta = "Error - La lista de descuento no puede estar vacía";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (OfertaListaItem.FirstOrDefault().DescripcionWeb == "OUTLET")
                    {
                        item.Alerta = "Error - La lista de descuento es solamente para OUTLET";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else
                    {
                        item.Alerta = "Sin Error";
                        dataTableOfertasValidadas.Add(item);
                        var oferta = new OfertasListadoModel()
                        {
                            IDArticulo = ItemBulto.IDArticulo,
                            ListaDescuneto = item.ListaDescuneto,
                            SiAplicaWEB = 0,
                            FechaInicio = DateTime.Today,
                            FechaTermino = DateTime.Today,
                            SiActivo = item.SiActivo
                        };
                        OfertasPost.Add(oferta);
                    }
                }
                else
                {
                    item.Alerta = $"Error - {ItemBulto.Result}";
                    dataTableOfertasValidadas.Add(item);
                    errorCount++;
                }
            }
            if (errorCount == 0)
            {
                EnviarBtn = false;
            }
        }
        public async Task ValidarDataTableOutlet(List<OfertasListadoModelExcel> datatable)
        {
            dataTableOfertasValidadas = new List<OfertasListadoModelExcel>();
            OfertasPost = new List<OfertasListadoModel>();
            errorCount = 0;
            HashSet<string> idArticulos = new HashSet<string>();
            OfertaListaItem = new();
            foreach (OfertasListadoModelExcel item in datatable)
            {
                ItemBulto = await BuscarArticulo(item.IDArticulo, Opcion);
                OfertaListaItem = await ValidarLista(item.ListaDescuneto);

                if (ItemBulto == null)
                {
                    item.Alerta = ItemBulto.Result;
                    dataTableOfertasValidadas.Add(item);
                    errorCount++;
                }
                else if (ItemBulto.IDEstado != item.SiActivo)
                {
                    if (item.IDArticulo != ItemBulto.IDArticulo)
                    {
                        item.Alerta = "Error - en el IDArticulo de Articulo";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (!idArticulos.Add(item.IDArticulo))
                    {
                        item.Alerta = "Error - IDarticulo repetido";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.Nombre.Length <= 0 || string.IsNullOrEmpty(item.Nombre))
                    {
                        item.Alerta = "Error - en el Nombre Del Articulo";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.SiActivo != datatable.FirstOrDefault().SiActivo)
                    {
                        item.Alerta = "Error - Solo puede ser un tipo de acción (0 o 1)";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.ListaDescuneto == null)
                    {
                        item.Alerta = "Error - La lista de descuento no puede estar vacía";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (OfertaListaItem.FirstOrDefault().DescripcionWeb != "OUTLET")
                    {
                        item.Alerta = "Error - La lista de descuento es solamente para OFERTAS";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else
                    {
                        item.Alerta = "Sin Error";
                        dataTableOfertasValidadas.Add(item);

                        var oferta = new OfertasListadoModel()
                        {
                            IDArticulo = ItemBulto.IDArticulo,
                            ListaDescuneto = item.ListaDescuneto,
                            SiAplicaWEB = item.SiAplicaWEB,
                            FechaInicio = DateTime.Now,
                            FechaTermino = DateTime.Now,
                            SiActivo = item.SiActivo,

                        };
                        OfertasPost.Add(oferta);
                    }
                }
                else
                {
                    item.Alerta = $"Error - {ItemBulto.Result}";
                    dataTableOfertasValidadas.Add(item);
                    errorCount++;
                }
            }
            if (errorCount == 0)
            {
                EnviarBtn = false;
            }
        }
        public async Task ValidarDataTableWeb(List<OfertasListadoModelExcel> datatable)
        {
            dataTableOfertasValidadas = new List<OfertasListadoModelExcel>();
            OfertasPost = new List<OfertasListadoModel>();
            errorCount = 0;
            HashSet<string> idArticulos = new HashSet<string>();
            OfertaListaItem = new();
            foreach (OfertasListadoModelExcel item in datatable)
            {
                //validando lista  y articulo
                ItemBulto = await BuscarArticulo(item.IDArticulo, Opcion);


                if (ItemBulto == null)
                {
                    item.Alerta = ItemBulto.Result;
                    dataTableOfertasValidadas.Add(item);
                    errorCount++;
                }
                else if (ItemBulto.IDEstado != item.SiActivo)
                {
                    if (item.IDArticulo != ItemBulto.IDArticulo)
                    {
                        item.Alerta = "Error - en el IDArticulo de Articulo";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (!idArticulos.Add(item.IDArticulo))
                    {
                        item.Alerta = "Error - IDarticulo repetido";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.Nombre.Length <= 0 || string.IsNullOrEmpty(item.Nombre))
                    {
                        item.Alerta = "Error - en el Nombre Del Articulo";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.FechaInicio > item.FechaTermino)
                    {
                        item.Alerta = "Error - Fecha inicio no puede se mayor a fecha termino";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.SiActivo != datatable.FirstOrDefault().SiActivo)
                    {
                        item.Alerta = "Error - Solo puede ser un tipo de acción (0 o 1)";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else if (item.Porcentaje < 1.0 && item.Porcentaje > 99.0)
                    {
                        item.Alerta = "Error - El porcentaje no puede ser 0";
                        dataTableOfertasValidadas.Add(item);
                        errorCount++;
                    }
                    else
                    {
                        item.Alerta = "Sin Error";
                        dataTableOfertasValidadas.Add(item);
                        var oferta = new OfertasListadoModel()
                        {
                            IDArticulo = ItemBulto.IDArticulo,
                            Porcentaje = item.Porcentaje,
                            SiAplicaWEB = item.SiAplicaWEB,
                            FechaInicio = item.FechaInicio,
                            FechaTermino = item.FechaTermino,
                            SiActivo = item.SiActivo
                        };
                        OfertasPost.Add(oferta);
                    }
                }
                else if (ItemBulto.Result != "Codigo Existe")
                {
                    item.Alerta = $"Error - {ItemBulto.Result}";
                    dataTableOfertasValidadas.Add(item);
                    errorCount++;
                }
                else if (ItemBulto.Result == "Codigo Existe" && item.SiActivo == 1)
                {
                    item.Alerta = "Sin Error";
                    dataTableOfertasValidadas.Add(item);
                    var oferta = new OfertasListadoModel()
                    {
                        IDArticulo = ItemBulto.IDArticulo,
                        Porcentaje = item.Porcentaje,
                        SiAplicaWEB = item.SiAplicaWEB,
                        FechaInicio = item.FechaInicio,
                        FechaTermino = item.FechaTermino,
                        SiActivo = item.SiActivo
                    };
                    OfertasPost.Add(oferta);

                }

            }
            if (errorCount == 0)
            {
                EnviarBtn = false;
            }
        }

        public async Task ValidarDataTableFB(List<OfertaFBModelExcel> datatable)
        {
            dataTableOfertasValidadasFB = new List<OfertaFBModelExcel>();
            OfertasFBPost = new List<OfertaFBModel>();
            errorCount = 0;
            HashSet<string> idArticulos = new HashSet<string>();
            OfertaListaItem = new();
            foreach (OfertaFBModelExcel item in datatable)
            {
                //ItemBulto = await BuscarArticulo(item.IDArticulo, 1);
                //if (ItemBulto == null)
                //{
                //    item.Alerta = ItemBulto.Result;
                //    dataTableOfertasValidadasFB.Add(item);
                //    errorCount++;
                //}
                // if (ItemBulto.IDEstado != item.IDEstado)
                //{
                //if (item.IDArticulo != ItemBulto.IDArticulo)
                //{
                //    item.Alerta = "Error - en el IDArticulo de Articulo";
                //    dataTableOfertasValidadasFB.Add(item);
                //    errorCount++;
                //}
                if (!idArticulos.Add(item.IDArticulo))
                {
                    item.Alerta = "Error - IDarticulo repetido";
                    dataTableOfertasValidadasFB.Add(item);
                    errorCount++;
                }
                else if (item.Nombre.Length <= 0 || string.IsNullOrEmpty(item.Nombre))
                {
                    item.Alerta = "Error - en el Nombre Del Articulo";
                    dataTableOfertasValidadasFB.Add(item);
                    errorCount++;
                }
                else if (item.IDEstado != datatable.FirstOrDefault().IDEstado)
                {
                    item.Alerta = "Error - Solo puede ser un tipo de acción (0 o 1)";
                    dataTableOfertasValidadasFB.Add(item);
                    errorCount++;
                }
                //else if (OfertaListaItem.FirstOrDefault().DescripcionWeb != "OUTLET")
                //{
                //    item.Alerta = "Error - La lista de descuento es solamente para OFERTAS";
                //    dataTableOfertasValidadasFB.Add(item);
                //    errorCount++;
                //}
                else
                {
                    item.Alerta = "Sin Error";
                    dataTableOfertasValidadasFB.Add(item);

                    var oferta = new OfertaFBModel()
                    {
                        IDCliente = "76809030-0",
                        IDArticulo = item.IDArticulo,
                        PrecioNormal = item.PrecioNormal,
                        Nombre = item.Nombre,
                        PrecioCliente = item.PrecioCliente,
                        IDEstado = item.IDEstado,

                    };
                    OfertasFBPost.Add(oferta);
                }
                //}
                //else
                //{
                //    item.Alerta = $"Error - {ItemBulto.Result}";
                //    dataTableOfertasValidadasFB.Add(item);
                //    errorCount++;
                //}
            }
            if (errorCount == 0)
            {
                EnviarBtn = false;
            }
        }

        #endregion

        #region Consulta a Endpoint 
        private async Task<OfertaResultModel> BuscarArticulo(string IDarticulo, int TipoOption)
        {
            try
            {
                OfertaResultModel ItemBultoValidacion = new();
                ItemBultoValidacion.IDArticulo = IDarticulo;
                ItemBultoValidacion.TipoOferta = TipoOption;

                service = new MainServices();
                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var lista = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UrlBscarArticulo}", ItemBultoValidacion);
                if (lista.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<OfertaResultModel>(await lista.Content.ReadAsStringAsync());
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
        private async Task<List<OfertaSistemaModel>> ValidarLista(int idlista)
        {
            try
            {
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlListadoOfertas}/{idlista}");
                if (lista.IsSuccessStatusCode)
                {
                    string var = await lista.Content.ReadAsStringAsync();
                    OfertaLista = JsonConvert.DeserializeObject<List<OfertaSistemaModel>>(var);
                    return OfertaLista;
                }
                else
                {
                    StateHasChanged();
                    return null;
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                return null;
            }

        }
        private async Task PostBultot(List<OfertaBultoModel> Post)
        {
            try
            {
                service = new MainServices();

                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostArticulosBulto}", Post);
                if (Response.IsSuccessStatusCode)
                {
                    var Data = Response.Content.ReadFromJsonAsync<List<OfertaResultModel>>();


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
        private async Task PostOfertas(List<OfertasListadoModel> Post)
        {
            try
            {
                service = new MainServices();
                ofertaVaslidaPost = new();
                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostArticulosOfertas}", Post);
                if (Response.IsSuccessStatusCode)
                {
                    string textoError = "";
                    var Data = JsonConvert.DeserializeObject<List<OfertaResultModel>>(await Response.Content.ReadAsStringAsync());
                    if (Data.Count > 0)
                    {
                        foreach (OfertaResultModel item in Data)
                        {
                            if (item.Result.Contains("Error"))
                            {
                                textoError = textoError + $"<li> {item.Result} - Articculo: {item.IDArticulo} \r\n </li>";
                            }
                        }

                    }

                    if (textoError.Contains("Error"))
                    {
                        snakBarCreation($"<ul>{textoError}</ul>", Defaults.Classes.Position.BottomStart, Severity.Error, 10000);
                    }
                    else
                    {
                        snakBarCreation($"ingresado son exito", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);
                    }

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
        private async Task PostOutlet(List<OfertasListadoModel> Post)
        {
            try
            {
                service = new MainServices();

                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostArticulosOutlet}", Post);
                if (Response.IsSuccessStatusCode)
                {
                    string textoError = "";
                    var Data = JsonConvert.DeserializeObject<List<OfertaResultModel>>(await Response.Content.ReadAsStringAsync());
                    if (Data.Count > 0)
                    {
                        foreach (OfertaResultModel item in Data)
                        {
                            if (item.Result.Contains("Error"))
                            {
                                textoError = textoError + $"<li> {item.Result} - Articculo: {item.IDArticulo} \r\n </li>";
                            }
                        }

                    }

                    if (textoError.Contains("Error"))
                    {
                        snakBarCreation($"<ul>{textoError}</ul>", Defaults.Classes.Position.BottomStart, Severity.Error, 10000);
                    }
                    else
                    {
                        snakBarCreation($"ingresado son exito", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);
                    }

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
        private async Task PostWeb(List<OfertasListadoModel> Post)
        {
            try
            {
                service = new MainServices();

                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostArticulosWeb}", Post);
                if (Response.IsSuccessStatusCode)
                {
                    string textoError = "";
                    var Data = JsonConvert.DeserializeObject<List<OfertaResultModel>>(await Response.Content.ReadAsStringAsync());
                    if (Data.Count > 0)
                    {
                        foreach (OfertaResultModel item in Data)
                        {
                            if (item.Result.Contains("Error"))
                            {
                                textoError = textoError + $"<li> {item.Result} - Articculo: {item.IDArticulo} \r\n </li>";
                            }
                        }

                    }

                    if (textoError.Contains("Error"))
                    {
                        snakBarCreation($"<ul>{textoError}</ul>", Defaults.Classes.Position.BottomStart, Severity.Error, 10000);
                    }
                    else
                    {
                        snakBarCreation($"ingresado son exito", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);
                    }

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

        private async Task PostFB(List<OfertaFBModel> Post)
        {
            try
            {
                service = new MainServices();

                var user = await _authenticationManager.CurrentUser();
                var IDuse = user.GetFirstName();
                string _IDUser = user.GetUserId();

                service.EstadoPedido.HttpClientInstance.DefaultRequestHeaders.Add("ID", _IDUser);
                var Response = await service.EstadoPedido.HttpClientInstance.PostAsJsonAsync($"api/v2/{UlrPostArticulosFB}", Post);
                if (Response.IsSuccessStatusCode)
                {
                    string textoError = "";
                    var Data = JsonConvert.DeserializeObject<List<OfertaResultModel>>(await Response.Content.ReadAsStringAsync());
                    if (Data.Count > 0)
                    {
                        foreach (OfertaResultModel item in Data)
                        {
                            if (item.Result.Contains("Error"))
                            {
                                textoError = textoError + $"<li> {item.Result} - Articculo: {item.IDArticulo} \r\n </li>";
                            }
                        }

                    }

                    if (textoError.Contains("Error"))
                    {
                        snakBarCreation($"<ul>{textoError}</ul>", Defaults.Classes.Position.BottomStart, Severity.Error, 10000);
                    }
                    else
                    {
                        snakBarCreation($"ingresado son exito", Defaults.Classes.Position.BottomStart, Severity.Success, 3000);
                    }

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
            var parameters = new DialogParameters<DialogCargaMasivaOfertas> { };
            var options = new MudBlazor.DialogOptions() { CloseButton = false, DisableBackdropClick = true, };
            var dialog = await DialogService.ShowAsync<DialogCargaMasivaOfertas>("Question", parameters, options);

            var result = await dialog.Result;
            if ((bool)result.Data)
            {
                switch (Opcion)
                {
                    case 1:
                        await PostBultot(BultosOfertasPost);
                        limpiar();
                        break;
                    case 2:
                        await PostOfertas(OfertasPost);
                        limpiar();
                        break;
                    case 3:
                        await PostOutlet(OfertasPost);
                        limpiar();
                        break;
                    case 4:
                        await PostWeb(OfertasPost);
                        limpiar();
                        break;
                    case 5:
                        await PostFB(OfertasFBPost);
                        limpiar();
                        break;
                    default: break;
                }


            }
        }
        private void limpiar()
        {
            fileComplete = null;
            NameFile = "";
            dataTableBultos = new List<OfertaBultoModelExecelModel>();
            dataTableBultosValidadas = new List<OfertaBultoModelExecelModel>();
            BultosOfertasPost = new List<OfertaBultoModel>();
            dataTableOfertas = new List<OfertasListadoModelExcel>();
            dataTableOfertasValidadas = new List<OfertasListadoModelExcel>();
            OfertasPost = new List<OfertasListadoModel>();
            dataTableOfertasFB = new() { };
            dataTableOfertasValidadasFB = new() { };
            OfertasFBPost = new() { };
            OfertaListaExportFB = new() { };
            EnviarBtn = true;
            errorCount = 0;
        }
        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion
        private void OnChangedClear(int selected)
        {
            Opcion = selected;
            limpiar();
        }
    }
}
