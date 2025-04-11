using PortalAG_V2.Shared.Model.NotaDeCredito;
using static PortalAG_V2.Pages.NotaDeCredito.NotaCredito;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using System.Collections.ObjectModel;
using PortalAG_V2.Shared.Model.Pagos;
using Syncfusion.Pdf.Grid;
using PortalAG_V2.Shared.Model.HojaDeRuta;
using PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaLineasPickingPacking;
using PortalAG_V2.Shared.Model.Prepacking;
using agDataAccess.Models.Solicitudes;
using agDataAccess.Models.Despacho1;

namespace PortalAG_V2.Services
{
    public class ExportService
    {
        public MemoryStream CreatePdfNC(List<DetalleClienteNCDTO> clientes, ObservableCollection<ProductoNCDTODevolver> ProductosPDF, SetDatosPDF datos, string respondable)
        {
            try
            {
                using (PdfDocument pdfDocument = new PdfDocument())
                {
                    // Add page
                    pdfDocument.PageSettings.Size = PdfPageSize.Letter;
                    pdfDocument.PageSettings.Orientation = PdfPageOrientation.Portrait;
                    PdfMargins margin = new PdfMargins();
                    margin.All = 20;
                    pdfDocument.PageSettings.Margins = margin;
                    PdfPage page = pdfDocument.Pages.Add();

                    // Estilos de letras
                    PdfStandardFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                    PdfStandardFont contentfont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Bold);
                    PdfStandardFont contentfontNormal = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Regular);
                    PdfStandardFont contentfontDetalle = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Regular);
                    PdfStandardFont contentfontLittle = new PdfStandardFont(PdfFontFamily.TimesRoman, 7, PdfFontStyle.Bold);
                    PdfStandardFont contentfontMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
                    PdfStandardFont contentfontNormalMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);

                    //Create a header 
                    RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 150);
                    PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

                    // Imagen Logo Andes
                    //PdfGraphics graphics = page.Graphics;
                    //PdfBitmap image = new PdfBitmap(img);
                    //header.Graphics.DrawImage(image, new PointF(0, 0), new SizeF(80, 30));

                    // Nombre 

                    header.Graphics.DrawString("ANDES INDUSTRIAL LTDA", fontTitle, PdfBrushes.Black, new PointF(170, 0));

                    //Giro Empresa
                    header.Graphics.DrawString("COMERCIAL ANDES INDUSTRIAL LTDA", contentfontLittle, PdfBrushes.Black, new PointF(80, 20));
                    header.Graphics.DrawString("Importadora y Comercializadora de Artículos Deportivos, Bicicletas y Repuestos", contentfontLittle, PdfBrushes.Black, new PointF(80, 30));
                    header.Graphics.DrawString("CASA MATRIZ: Casa Matriz: Santa Elena 1511 - Santiago ** Casilla 268-3", contentfontLittle, PdfBrushes.Black, new PointF(80, 40));
                    //header.Graphics.DrawString("SUCURSALES:  Av. La Dehesa 1766, Lo Barnechea - Tel: 222495156", contentfontLittle, PdfBrushes.Black, new PointF(80, 50));
                    //header.Graphics.DrawString("             Av. Paseo Colina Sur 14.500 - Loc 125, Mall Piedra Roja - Tel: 229464721", contentfontLittle, PdfBrushes.Black, new PointF(110, 60));
                    //header.Graphics.DrawString("             Padre Hurtado 1233, Vitacura - Tel: 223789811", contentfontLittle, PdfBrushes.Black, new PointF(110, 70));
                    header.Graphics.DrawString("PAGINA WEB:  www.andesindustrial.cl", contentfontLittle, PdfBrushes.Black, new PointF(80, 80));

                    // Nro Solicitud NC--------------------------

                    PdfPen pen = new PdfPen(Color.Red, 2);
                    RectangleF rect = new RectangleF(370, 0, 180, 90);
                    header.Graphics.DrawRectangle(pen, rect);

                    // RUT 
                    header.Graphics.DrawString("R.U.T. 85.390.800-2", fontTitle, PdfBrushes.Red, new PointF(410, 10));
                    header.Graphics.DrawString("Solicitud Nota de Credito", fontTitle, PdfBrushes.Red, new PointF(390, 30));
                    header.Graphics.DrawString("Nº " + datos.NumeroSolicitud, fontTitle, PdfBrushes.Red, new PointF(440, 50));
                    //header.Graphics.DrawString("S.I.I. - SANTIAGO CENTRO", fontTitle, PdfBrushes.Red, new PointF(390, 80));

                    // BARCODE
                    string codigoBarra = "C" + System.DateTime.Now.Year + "00" + datos.IdOperacion + "001";
                    PdfCode128ABarcode barcode = new PdfCode128ABarcode();
                    PdfQRBarcode pdfQR = new PdfQRBarcode();
                    pdfQR.Text = codigoBarra;
                    pdfQR.Size = new SizeF(60, 60);
                    pdfQR.Draw(page, new PointF(0, 20));

                    //Initialize pen to draw the line
                    PdfPen penLine = new PdfPen(PdfBrushes.Gray, 2.5f);
                    //Create the line points
                    PointF point1 = new PointF(280, 110);
                    PointF point2 = new PointF(700, 110);
                    header.Graphics.DrawLine(penLine, point1, point2);

                    pdfDocument.Template.Top = header;

                    // DATOS CLIENTE --------------------------

                    //Create Container
                    RectangleF rectangleF = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 100);
                    PdfTemplate dataCliente = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 100);

                    dataCliente.Graphics.DrawRectangle(PdfBrushes.White, rectangleF);

                    // Rut, Razon Social, Direccion, Giro
                    dataCliente.Graphics.DrawString("Rut", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                    dataCliente.Graphics.DrawString(":  " + clientes[0].IDCliente, contentfontNormal, PdfBrushes.Black, new PointF(50, 0));
                    dataCliente.Graphics.DrawString("R. Social", contentfont, PdfBrushes.Black, new PointF(0, 10));
                    dataCliente.Graphics.DrawString(":  " + clientes[0].RazonSocial, contentfont, PdfBrushes.Black, new PointF(50, 10));
                    dataCliente.Graphics.DrawString("Giro", contentfontNormal, PdfBrushes.Black, new PointF(0, 20));
                    dataCliente.Graphics.DrawString(":  ", contentfontNormal, PdfBrushes.Black, new PointF(50, 20));
                    dataCliente.Graphics.DrawString("Dirección", contentfontNormal, PdfBrushes.Black, new PointF(0, 30));
                    dataCliente.Graphics.DrawString(":  " + clientes[0].Direccion, contentfontNormal, PdfBrushes.Black, new PointF(50, 30));
                    dataCliente.Graphics.DrawString("Comuna", contentfontNormal, PdfBrushes.Black, new PointF(0, 40));
                    dataCliente.Graphics.DrawString(":  " + clientes[0].Comuna, contentfontNormal, PdfBrushes.Black, new PointF(50, 40));
                    dataCliente.Graphics.DrawString("Ciudad", contentfontNormal, PdfBrushes.Black, new PointF(0, 50));
                    dataCliente.Graphics.DrawString(":  " + clientes[0].Ciudad, contentfontNormal, PdfBrushes.Black, new PointF(50, 50));
                    dataCliente.Graphics.DrawString("Vendedor", contentfontNormal, PdfBrushes.Black, new PointF(0, 60));
                    dataCliente.Graphics.DrawString(":  " + clientes[0].Vendedor.ToUpper(), contentfontNormal, PdfBrushes.Black, new PointF(50, 60));
                    dataCliente.Graphics.DrawString("Solicitado x", contentfontNormal, PdfBrushes.Black, new PointF(150, 60));
                    dataCliente.Graphics.DrawString(":   " + respondable.ToUpper(), contentfontNormal, PdfBrushes.Black, new PointF(200, 60));


                    // Info de emision
                    dataCliente.Graphics.DrawString("Emisión: ", contentfontMonto, PdfBrushes.Black, new PointF(400, 0));
                    dataCliente.Graphics.DrawString(System.DateTime.Today.ToString("dd/MM/yyyy"), contentfontMonto, PdfBrushes.Black, new PointF(395, 20));

                    //Initialize pen to draw the line
                    PdfPen penLine2 = new PdfPen(PdfBrushes.Gray, 2.5f);
                    //Create the line points
                    PointF point11 = new PointF(280, 60);
                    PointF point22 = new PointF(700, 60);
                    dataCliente.Graphics.DrawLine(penLine2, point11, point22);


                    page.Graphics.DrawPdfTemplate(dataCliente, new PointF(0, 120));

                    // DATOS DE LOS PRODUCTOS A DEVOLVER

                    RectangleF rectangleDetalle = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 400);
                    PdfTemplate dataDetalle = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 400);
                    dataDetalle.Graphics.DrawRectangle(PdfBrushes.White, rectangleDetalle);

                    PdfPen pen4 = new PdfPen(Color.Black, 1);
                    RectangleF rect4 = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 20);
                    dataDetalle.Graphics.DrawRectangle(pen4, rect4);

                    PdfPen pen5 = new PdfPen(Color.Black, 1);
                    RectangleF rect5 = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 380);
                    dataDetalle.Graphics.DrawRectangle(pen5, rect5);

                    // Detalles  new PdfStringFormat(PdfTextAlignment.Right)

                    dataDetalle.Graphics.DrawString("Código", contentfont, PdfBrushes.Black, new PointF(8, 0));
                    dataDetalle.Graphics.DrawString("Descripción", contentfont, PdfBrushes.Black, new PointF(100, 0));
                    dataDetalle.Graphics.DrawString("Cantidad", contentfont, PdfBrushes.Black, new PointF(360, 0));
                    dataDetalle.Graphics.DrawString("P. Venta", contentfont, PdfBrushes.Black, new PointF(440, 0));
                    //dataDetalle.Graphics.DrawString("Sub Total", contentfont, PdfBrushes.Black, new PointF(360, 0));
                    //dataDetalle.Graphics.DrawString("Descuento", contentfont, PdfBrushes.Black, new PointF(440, 0));
                    dataDetalle.Graphics.DrawString("Total", contentfont, PdfBrushes.Black, new PointF(520, 0));

                    int y = 20;
                    foreach (var articulo in ProductosPDF)
                    {
                        y = y + 10;
                        dataDetalle.Graphics.DrawString(articulo.IDArticulo, contentfontDetalle, PdfBrushes.Black, new PointF(10, y));

                        if (articulo.Nombre.Length > 80)
                        {
                            dataDetalle.Graphics.DrawString(articulo.Nombre.Substring(0, 80), contentfontDetalle, PdfBrushes.Black, new PointF(80, y));
                        }
                        else
                        {
                            dataDetalle.Graphics.DrawString(articulo.Nombre, contentfontDetalle, PdfBrushes.Black, new PointF(80, y));
                        }

                        dataDetalle.Graphics.DrawString(articulo.CantidadADevolver.ToString("n0"), contentfontDetalle, PdfBrushes.Black, new PointF(360, y));
                        dataDetalle.Graphics.DrawString(articulo.PrecioVenta.ToString("n0"), contentfontDetalle, PdfBrushes.Black, new PointF(440, y));


                        dataDetalle.Graphics.DrawString((articulo.CantidadADevolver * articulo.PrecioVenta).ToString("n0"), contentfontDetalle, PdfBrushes.Black, new PointF(520, y));

                    }

                    page.Graphics.DrawPdfTemplate(dataDetalle, new PointF(0, 200));


                    //// MONTOS 

                    RectangleF rectangleMontos = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 200);
                    PdfTemplate dataMontos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 200);

                    dataMontos.Graphics.DrawRectangle(PdfBrushes.White, rectangleMontos);

                    PdfPen pen3 = new PdfPen(Color.Black, 1);
                    RectangleF rect3 = new RectangleF(360, 0, 210, 100);
                    dataMontos.Graphics.DrawRectangle(pen3, rect3);

                    PdfPen pen6 = new PdfPen(Color.Black, 1);
                    RectangleF rect6 = new RectangleF(0, 0, 300, 100);
                    dataMontos.Graphics.DrawRectangle(pen6, rect6);

                    dataMontos.Graphics.DrawString("Nota:" + clientes[0].Comentarios, contentfont, PdfBrushes.Black, new PointF(110, 0));
                    dataMontos.Graphics.DrawString("Referencia: " + (clientes[0].IDTipoOperacion == 33 ? "F/ " : "B/ ") + clientes[0].NroDocumento, contentfont, PdfBrushes.Black, new PointF(30, 20));
                    dataMontos.Graphics.DrawString("NroPedido: " + clientes[0].NroDocumentoAntes, contentfont, PdfBrushes.Black, new PointF(30, 40));
                    dataMontos.Graphics.DrawString("Comentarios: " + clientes[0].Comentarios, contentfont, PdfBrushes.Black, new PointF(30, 60));

                    // Detalles 

                    dataMontos.Graphics.DrawString("Subtotal", contentfont, PdfBrushes.Black, new PointF(380, 0));
                    dataMontos.Graphics.DrawString(":  " + datos.SubTotal.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 0), new PdfStringFormat(PdfTextAlignment.Left)); // SubTotal
                    dataMontos.Graphics.DrawString("Dcto " + datos.PorcentajeDescuento, contentfont, PdfBrushes.Black, new PointF(380, 20));
                    dataMontos.Graphics.DrawString(":  " + datos.Descuento.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 20), new PdfStringFormat(PdfTextAlignment.Left)); // SubTotal
                    dataMontos.Graphics.DrawString("Neto", contentfont, PdfBrushes.Black, new PointF(380, 40));
                    dataMontos.Graphics.DrawString(":  " + datos.Neto.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 40), new PdfStringFormat(PdfTextAlignment.Left)); // Neto
                    dataMontos.Graphics.DrawString("Iva 19%", contentfont, PdfBrushes.Black, new PointF(380, 60));
                    dataMontos.Graphics.DrawString(":  " + datos.Iva.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 60), new PdfStringFormat(PdfTextAlignment.Left)); // Iva
                    dataMontos.Graphics.DrawString("Total", contentfont, PdfBrushes.Black, new PointF(380, 80));
                    dataMontos.Graphics.DrawString(":  " + datos.Total.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 80), new PdfStringFormat(PdfTextAlignment.Left)); // Total
                                                                                                                                                                                        //dataMontos.Graphics.DrawString("Copyright: C. Salazar", contentfontLittle, PdfBrushes.Black, new PointF(380, 80));


                    page.Graphics.DrawPdfTemplate(dataMontos, new PointF(0, 595));

                    using (MemoryStream stream = new MemoryStream())
                    {
                        pdfDocument.Save(stream);
                        pdfDocument.Close();
                        return stream;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public MemoryStream CreatePdfPorValorConcepto(List<DetalleClienteNCDTO> clientes, List<ProductoNCDTODevolver> ProductosPDF, SetDatosPDF datos)
        {

            using (PdfDocument pdfDocument = new PdfDocument())
            {

                // Add page
                pdfDocument.PageSettings.Size = PdfPageSize.Letter;
                pdfDocument.PageSettings.Orientation = PdfPageOrientation.Portrait;
                PdfMargins margin = new PdfMargins();
                margin.All = 20;
                pdfDocument.PageSettings.Margins = margin;
                PdfPage page = pdfDocument.Pages.Add();

                // Estilos de letras
                PdfStandardFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormal = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Regular);
                PdfStandardFont contentfontDetalle = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Regular);
                PdfStandardFont contentfontLittle = new PdfStandardFont(PdfFontFamily.TimesRoman, 7, PdfFontStyle.Bold);
                PdfStandardFont contentfontMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormalMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);

                //Create a header 
                RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 150);
                PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

                // Imagen Logo Andes
                //PdfGraphics graphics = page.Graphics;
                //PdfBitmap image = new PdfBitmap(img);
                //header.Graphics.DrawImage(image, new PointF(0, 0), new SizeF(80, 30));

                // Nombre 

                header.Graphics.DrawString("ANDES INDUSTRIAL LTDA", fontTitle, PdfBrushes.Black, new PointF(170, 0));

                //Giro Empresa
                header.Graphics.DrawString("COMERCIAL ANDES INDUSTRIAL LTDA", contentfontLittle, PdfBrushes.Black, new PointF(80, 20));
                header.Graphics.DrawString("Importadora y Comercializadora de Artículos Deportivos, Bicicletas y Repuestos", contentfontLittle, PdfBrushes.Black, new PointF(80, 30));
                header.Graphics.DrawString("CASA MATRIZ: Casa Matriz: Santa Elena 1511 - Santiago ** Casilla 268-3", contentfontLittle, PdfBrushes.Black, new PointF(80, 40));
                //header.Graphics.DrawString("SUCURSALES:  Av. La Dehesa 1766, Lo Barnechea - Tel: 222495156", contentfontLittle, PdfBrushes.Black, new PointF(80, 50));
                //header.Graphics.DrawString("             Av. Paseo Colina Sur 14.500 - Loc 125, Mall Piedra Roja - Tel: 229464721", contentfontLittle, PdfBrushes.Black, new PointF(110, 60));
                //header.Graphics.DrawString("             Padre Hurtado 1233, Vitacura - Tel: 223789811", contentfontLittle, PdfBrushes.Black, new PointF(110, 70));
                header.Graphics.DrawString("PAGINA WEB:  www.andesindustrial.cl", contentfontLittle, PdfBrushes.Black, new PointF(80, 80));

                // Nro Solicitud NC--------------------------

                PdfPen pen = new PdfPen(Color.Red, 2);
                RectangleF rect = new RectangleF(370, 0, 180, 90);
                header.Graphics.DrawRectangle(pen, rect);

                // RUT 
                header.Graphics.DrawString("R.U.T. 85.390.800-2", fontTitle, PdfBrushes.Red, new PointF(410, 10));
                header.Graphics.DrawString("Solicitud Nota de Credito", fontTitle, PdfBrushes.Red, new PointF(390, 30));
                header.Graphics.DrawString("Nº " + datos.NumeroSolicitud, fontTitle, PdfBrushes.Red, new PointF(440, 50));
                //header.Graphics.DrawString("S.I.I. - SANTIAGO CENTRO", fontTitle, PdfBrushes.Red, new PointF(390, 80));

                // BARCODE
                string codigoBarra = "C" + System.DateTime.Now.Year + "00" + datos.IdOperacion + "001";
                PdfCode128ABarcode barcode = new PdfCode128ABarcode();
                PdfQRBarcode pdfQR = new PdfQRBarcode();
                pdfQR.Text = codigoBarra;
                pdfQR.Size = new SizeF(60, 60);
                pdfQR.Draw(page, new PointF(0, 20));

                //Initialize pen to draw the line
                PdfPen penLine = new PdfPen(PdfBrushes.Gray, 2.5f);
                //Create the line points
                PointF point1 = new PointF(280, 110);
                PointF point2 = new PointF(700, 110);
                header.Graphics.DrawLine(penLine, point1, point2);

                pdfDocument.Template.Top = header;

                // DATOS CLIENTE --------------------------

                //Create Container
                RectangleF rectangleF = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 100);
                PdfTemplate dataCliente = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 100);

                dataCliente.Graphics.DrawRectangle(PdfBrushes.White, rectangleF);

                // Rut, Razon Social, Direccion, Giro
                dataCliente.Graphics.DrawString("Rut", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                dataCliente.Graphics.DrawString(":  " + clientes[0].IDCliente, contentfontNormal, PdfBrushes.Black, new PointF(50, 0));
                dataCliente.Graphics.DrawString("R. Social", contentfont, PdfBrushes.Black, new PointF(0, 10));
                dataCliente.Graphics.DrawString(":  " + clientes[0].RazonSocial, contentfont, PdfBrushes.Black, new PointF(50, 10));
                dataCliente.Graphics.DrawString("Giro", contentfontNormal, PdfBrushes.Black, new PointF(0, 20));
                dataCliente.Graphics.DrawString(":  ", contentfontNormal, PdfBrushes.Black, new PointF(50, 20));
                dataCliente.Graphics.DrawString("Dirección", contentfontNormal, PdfBrushes.Black, new PointF(0, 30));
                dataCliente.Graphics.DrawString(":  " + clientes[0].Direccion, contentfontNormal, PdfBrushes.Black, new PointF(50, 30));
                dataCliente.Graphics.DrawString("Comuna", contentfontNormal, PdfBrushes.Black, new PointF(0, 40));
                dataCliente.Graphics.DrawString(":  " + clientes[0].Comuna, contentfontNormal, PdfBrushes.Black, new PointF(50, 40));
                dataCliente.Graphics.DrawString("Ciudad", contentfontNormal, PdfBrushes.Black, new PointF(0, 50));
                dataCliente.Graphics.DrawString(":  " + clientes[0].Ciudad, contentfontNormal, PdfBrushes.Black, new PointF(50, 50));
                dataCliente.Graphics.DrawString("Vendedor", contentfontNormal, PdfBrushes.Black, new PointF(0, 60));
                dataCliente.Graphics.DrawString(":  " + clientes[0].Vendedor.ToUpper(), contentfontNormal, PdfBrushes.Black, new PointF(50, 60));

                // Info de emision
                dataCliente.Graphics.DrawString("Emisión: ", contentfontMonto, PdfBrushes.Black, new PointF(400, 0));
                dataCliente.Graphics.DrawString(System.DateTime.Today.ToString("dd/MM/yyyy"), contentfontMonto, PdfBrushes.Black, new PointF(395, 20));

                //Initialize pen to draw the line
                PdfPen penLine2 = new PdfPen(PdfBrushes.Gray, 2.5f);
                //Create the line points
                PointF point11 = new PointF(280, 60);
                PointF point22 = new PointF(700, 60);
                dataCliente.Graphics.DrawLine(penLine2, point11, point22);


                page.Graphics.DrawPdfTemplate(dataCliente, new PointF(0, 120));

                // DATOS DE LOS PRODUCTOS A DEVOLVER

                RectangleF rectangleDetalle = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 400);
                PdfTemplate dataDetalle = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 400);
                dataDetalle.Graphics.DrawRectangle(PdfBrushes.AliceBlue, rectangleDetalle);

                PdfPen pen4 = new PdfPen(Color.Black, 1);
                RectangleF rect4 = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 25);
                dataDetalle.Graphics.DrawRectangle(pen4, rect4);

                PdfPen pen5 = new PdfPen(Color.Black, 1);
                RectangleF rect5 = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 380);
                dataDetalle.Graphics.DrawRectangle(pen5, rect5);

                // Detalles  new PdfStringFormat(PdfTextAlignment.Right)

                dataDetalle.Graphics.DrawString("Código", contentfont, PdfBrushes.Black, new PointF(8, 0));
                dataDetalle.Graphics.DrawString("Descripción", contentfont, PdfBrushes.Black, new PointF(100, 0));
                dataDetalle.Graphics.DrawString("Cantidad", contentfont, PdfBrushes.Black, new PointF(360, 0));
                dataDetalle.Graphics.DrawString("P. Venta", contentfont, PdfBrushes.Black, new PointF(440, 0));
                //dataDetalle.Graphics.DrawString("Sub Total", contentfont, PdfBrushes.Black, new PointF(360, 0));
                //dataDetalle.Graphics.DrawString("Descuento", contentfont, PdfBrushes.Black, new PointF(440, 0));
                dataDetalle.Graphics.DrawString("Total", contentfont, PdfBrushes.Black, new PointF(520, 0));
                int y = 20;
                foreach (var articulo in ProductosPDF)
                {
                    y = y + 20;
                    //if (articulo.Nombre.Length > 70)
                    //{
                    //    dataDetalle.Graphics.DrawString(articulo.Nombre.Substring(0, 70), contentfontDetalle, PdfBrushes.Black, new PointF(100, 15));
                    //}
                    //else
                    //{
                    //    dataDetalle.Graphics.DrawString(articulo.Nombre, contentfontDetalle, PdfBrushes.Black, new PointF(100, 15));
                    //}
                    dataDetalle.Graphics.DrawString(articulo.IDArticulo, contentfontDetalle, PdfBrushes.Black, new PointF(10, y));
                    dataDetalle.Graphics.DrawString(articulo.CantidadADevolver.ToString("n0"), contentfontDetalle, PdfBrushes.Black, new PointF(360, y));
                    dataDetalle.Graphics.DrawString(articulo.PrecioVenta.ToString("n0"), contentfontDetalle, PdfBrushes.Black, new PointF(440, y));
                    dataDetalle.Graphics.DrawString((articulo.CantidadADevolver * articulo.PrecioVenta).ToString("n0"), contentfontDetalle, PdfBrushes.Black, new PointF(520, y));

                }

                page.Graphics.DrawPdfTemplate(dataDetalle, new PointF(0, 200));



                //// MONTOS 

                RectangleF rectangleMontos = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 200);
                PdfTemplate dataMontos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 200);

                dataMontos.Graphics.DrawRectangle(PdfBrushes.White, rectangleMontos);

                PdfPen pen3 = new PdfPen(Color.Black, 1);
                RectangleF rect3 = new RectangleF(360, 0, 210, 100);
                dataMontos.Graphics.DrawRectangle(pen3, rect3);

                PdfPen pen6 = new PdfPen(Color.Black, 1);
                RectangleF rect6 = new RectangleF(0, 0, 300, 100);
                dataMontos.Graphics.DrawRectangle(pen6, rect6);

                dataMontos.Graphics.DrawString("Nota:" + clientes[0].Comentarios, contentfont, PdfBrushes.Black, new PointF(110, 0));
                dataMontos.Graphics.DrawString("Referencia: " + (clientes[0].IDTipoOperacion == 33 ? "F/ " : "B/ ") + clientes[0].NroDocumento, contentfont, PdfBrushes.Black, new PointF(30, 20));
                dataMontos.Graphics.DrawString("NroPedido: " + clientes[0].NroDocumentoAntes, contentfont, PdfBrushes.Black, new PointF(30, 40));
                dataMontos.Graphics.DrawString("Comentarios: " + clientes[0].Comentarios, contentfont, PdfBrushes.Black, new PointF(30, 60));

                // Detalles 

                dataMontos.Graphics.DrawString("Subtotal", contentfont, PdfBrushes.Black, new PointF(380, 0));
                dataMontos.Graphics.DrawString(":  " + datos.SubTotal.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 0), new PdfStringFormat(PdfTextAlignment.Left)); // SubTotal
                dataMontos.Graphics.DrawString("Dcto " + datos.PorcentajeDescuento, contentfont, PdfBrushes.Black, new PointF(380, 20));
                dataMontos.Graphics.DrawString(":  " + datos.Descuento.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 20), new PdfStringFormat(PdfTextAlignment.Left)); // SubTotal
                dataMontos.Graphics.DrawString("Neto", contentfont, PdfBrushes.Black, new PointF(380, 40));
                dataMontos.Graphics.DrawString(":  " + datos.Neto.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 40), new PdfStringFormat(PdfTextAlignment.Left)); // Neto
                dataMontos.Graphics.DrawString("Iva 19%", contentfont, PdfBrushes.Black, new PointF(380, 60));
                dataMontos.Graphics.DrawString(":  " + datos.Iva.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 60), new PdfStringFormat(PdfTextAlignment.Left)); // Iva
                dataMontos.Graphics.DrawString("Total", contentfont, PdfBrushes.Black, new PointF(380, 80));
                dataMontos.Graphics.DrawString(":  " + datos.Total.ToString("n0"), contentfont, PdfBrushes.Black, new PointF(480, 80), new PdfStringFormat(PdfTextAlignment.Left)); // Total
                                                                                                                                                                                    //dataMontos.Graphics.DrawString("Copyright: C. Salazar", contentfontLittle, PdfBrushes.Black, new PointF(380, 80));


                page.Graphics.DrawPdfTemplate(dataMontos, new PointF(0, 595));

                using (MemoryStream stream = new MemoryStream())
                {
                    pdfDocument.Save(stream);
                    pdfDocument.Close();
                    return stream;
                }

            }

        }
        public MemoryStream CreatePfdComprobanteDePago(PagosClienteDTO pago)
        {

            using (PdfDocument pdfDocument = new PdfDocument())
            {

                // Add page
                pdfDocument.PageSettings.Size = PdfPageSize.Letter;
                pdfDocument.PageSettings.Orientation = PdfPageOrientation.Portrait;
                PdfMargins margin = new PdfMargins();
                margin.All = 20;
                pdfDocument.PageSettings.Margins = margin;
                PdfPage page = pdfDocument.Pages.Add();

                // Estilos de letras
                int paragraphAfterSpacing = 8;
                PdfStandardFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold);
                PdfStandardFont contentfont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Regular);
                PdfStandardFont contentfontNormal = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);
                PdfStandardFont contentfontDetalle = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Regular);
                PdfStandardFont contentfontLittle = new PdfStandardFont(PdfFontFamily.TimesRoman, 7, PdfFontStyle.Bold);
                PdfStandardFont contentfontMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormalMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);

                //Create a header 
                RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 120);
                PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

                // Folio
                header.Graphics.DrawString($"Folio", contentfontNormal, PdfBrushes.Black, new PointF(400, 0));
                header.Graphics.DrawString($": {pago.NumeroCobranza.ToString()}", contentfontNormal, PdfBrushes.Black, new PointF(480, 0));

                //Fecha Impresion
                header.Graphics.DrawString($"F. Impresión", contentfontNormal, PdfBrushes.Black, new PointF(400, 15));
                header.Graphics.DrawString($": {DateTime.Now.ToString("dd/MM/yyyy")}", contentfontNormal, PdfBrushes.Black, new PointF(480, 15));

                // Titulo 
                header.Graphics.DrawString("COMPROBANTE DE PAGO", fontTitle, PdfBrushes.Black, new PointF(160, 50));

                //Datos cliente
                header.Graphics.DrawString($"Rut: {pago.IDCliente.ToUpper()}", contentfontNormal, PdfBrushes.Black, new PointF(0, 100));
                header.Graphics.DrawString($"Razon Social: {pago.RazonSocial.ToUpper()}", contentfontNormal, PdfBrushes.Black, new PointF(160, 100));
                //Linea
                PdfPen penLine = new PdfPen(PdfBrushes.Black, 1.0f);
                PointF point1 = new PointF(0, 120);
                PointF point2 = new PointF(pdfDocument.Pages[0].GetClientSize().Width, 120);
                header.Graphics.DrawLine(penLine, point1, point2);

                pdfDocument.Template.Top = header;

                //// ------------------------------

                ////Create Container
                RectangleF rectangleF = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 100);
                PdfTemplate dataCliente = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 100);

                dataCliente.Graphics.DrawRectangle(PdfBrushes.White, rectangleF);

                //// Fecha de pago
                dataCliente.Graphics.DrawString("Fecha Pago", contentfont, PdfBrushes.Black, new PointF(0, 0));
                dataCliente.Graphics.DrawString($": {pago.FechaContabilizacion.ToString()}", contentfont, PdfBrushes.Black, new PointF(80, 0));
                dataCliente.Graphics.DrawString("Monto Cancelado", contentfont, PdfBrushes.Black, new PointF(0, 20));
                dataCliente.Graphics.DrawString($": {pago.TotalPago.ToString("N0")} CLP", contentfont, PdfBrushes.Black, new PointF(80, 20));

                page.Graphics.DrawPdfTemplate(dataCliente, new PointF(0, 10));


                //// --------Lista de Pedidos pagados--------
                PdfTemplate listaPedidos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPedidos.Graphics.DrawString("Lista de pedidos pagados", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                page.Graphics.DrawPdfTemplate(listaPedidos, new PointF(0, 60));
                //page.Graphics.DrawPdfTemplate(listaPedidos, new PointF(0, pdfGridLayoutResult2.Bounds.Bottom + 15));

                PdfGrid pdfGrid3 = new PdfGrid();
                pdfGrid3.Style.CellPadding.Left = 8;
                pdfGrid3.Style.CellPadding.Right = 8;
                //Applying built-in style to the PDF grid.
                pdfGrid3.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid3.Columns.Add(7);
                pdfGrid3.Columns[0].Width = 30;
                pdfGrid3.Columns[1].Width = 86;
                pdfGrid3.Columns[2].Width = 86;
                pdfGrid3.Columns[3].Width = 86;
                pdfGrid3.Columns[4].Width = 86;
                pdfGrid3.Columns[5].Width = 86;
                pdfGrid3.Columns[6].Width = 90;

                //Add header.
                pdfGrid3.Headers.Add(1);
                PdfGridRow pdfGridHeader3 = pdfGrid3.Headers[0];
                pdfGridHeader3.Cells[0].Value = "#";
                pdfGridHeader3.Cells[1].Value = "Tipo";
                pdfGridHeader3.Cells[2].Value = "N° Documento";
                pdfGridHeader3.Cells[3].Value = "Fecha";
                pdfGridHeader3.Cells[4].Value = "Total";
                pdfGridHeader3.Cells[5].Value = "Pagado";
                pdfGridHeader3.Cells[6].Value = "Saldo";

                int i = 1;
                int saldoPedidosPagado = 0;
                List<Documeto> facturas = new List<Documeto>();
                if (pago.Pedidos != null) facturas = pago.Pedidos.FirstOrDefault().Documetos;

                foreach (var data in facturas)
                {
                    //Add rows.
                    PdfGridRow pdfGridRow = pdfGrid3.Rows.Add();
                    pdfGridRow.Cells[0].Value = i.ToString();
                    pdfGridRow.Cells[1].Value = data.TipoOperacion;
                    pdfGridRow.Cells[2].Value = data.NroDocumento.ToString();
                    pdfGridRow.Cells[3].Value = data.FechaDocumento.ToString("dd/MM/yyyy");
                    pdfGridRow.Cells[4].Value = data.MontoDocumento.ToString("N0");
                    pdfGridRow.Cells[5].Value = data.Pagar.ToString("N0");
                    pdfGridRow.Cells[6].Value = data.Saldo.ToString("N0");
                    saldoPedidosPagado = saldoPedidosPagado + data.Pagar;
                    i = i++;
                }


                //Assign data source.
                //pdfGrid.DataSource = pago.Pagos;
                //pdfGrid.Style.Font = contentfont;

                //Draw PDF grid into the PDF page.
                //PdfGridLayoutResult pdfGridLayoutResult3 = pdfGrid3.Draw(page, new PointF(0, pdfGridLayoutResult2.Bounds.Bottom + 30));
                PdfGridLayoutResult pdfGridLayoutResult3 = pdfGrid3.Draw(page, new Syncfusion.Drawing.PointF(0, listaPedidos.GetBounds().Bottom + 40 + paragraphAfterSpacing));

                //// --------Lista de Saldos--------
                PdfTemplate listaSaldos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaSaldos.Graphics.DrawString("Lista de saldos", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                page.Graphics.DrawPdfTemplate(listaSaldos, new PointF(0, pdfGridLayoutResult3.Bounds.Bottom + 15));

                PdfGrid pdfGrid2 = new PdfGrid();
                pdfGrid2.Style.CellPadding.Left = 8;
                pdfGrid2.Style.CellPadding.Right = 8;
                //Applying built-in style to the PDF grid.
                pdfGrid2.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid2.Columns.Add(4);
                pdfGrid2.Columns[0].Width = 30;
                pdfGrid2.Columns[1].Width = 150;
                pdfGrid2.Columns[2].Width = 185;
                pdfGrid2.Columns[3].Width = 185;

                //Add header.
                pdfGrid2.Headers.Add(1);
                PdfGridRow pdfGridHeader2 = pdfGrid2.Headers[0];
                pdfGridHeader2.Cells[0].Value = "#";
                pdfGridHeader2.Cells[1].Value = "N° Documento";
                pdfGridHeader2.Cells[2].Value = "Tipo";
                pdfGridHeader2.Cells[3].Value = "Monto";

                i = 1;
                int montosSaldos = 0;
                foreach (var data in pago.Saldos_A_Favors)
                {
                    //Add rows.
                    PdfGridRow pdfGridRow = pdfGrid2.Rows.Add();
                    pdfGridRow.Cells[0].Value = i.ToString();
                    pdfGridRow.Cells[1].Value = data.nroDocumentoSaldo.ToString();
                    pdfGridRow.Cells[2].Value = "";

                    switch (data.tipoDocumento)
                    {
                        case 56:
                            pdfGridRow.Cells[2].Value = "Nota Debito";
                            break;
                        case 61:
                            pdfGridRow.Cells[2].Value = "Nota Credito";
                            break;
                        case 11:
                            pdfGridRow.Cells[2].Value = "Saldo Favor";
                            break;
                        case 14:
                            pdfGridRow.Cells[2].Value = "Saldo Contra";
                            break;
                        case 8:
                            pdfGridRow.Cells[2].Value = "Pago Anticipado";
                            break;

                    }
                    pdfGridRow.Cells[2].Value = data.tipoDocumento.ToString();
                    pdfGridRow.Cells[3].Value = data.montoDocumentoSaldo.ToString("N0");
                    montosSaldos = montosSaldos + data.montoDocumentoSaldo;
                    i = i++;
                }


                //Assign data source.
                //pdfGrid.DataSource = pago.Pagos;
                //pdfGrid.Style.Font = contentfont;

                //Draw PDF grid into the PDF page.
                PdfGridLayoutResult pdfGridLayoutResult2 = pdfGrid2.Draw(page, new PointF(0, pdfGridLayoutResult3.Bounds.Bottom + 30));


                //// --------Lista de Pagos--------
                PdfTemplate listaPagos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPagos.Graphics.DrawString("Lista de pagos", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                page.Graphics.DrawPdfTemplate(listaPagos, new PointF(0, pdfGridLayoutResult2.Bounds.Bottom + 15));
                //page.Graphics.DrawPdfTemplate(listaPagos, new PointF(0, 60));

                PdfGrid pdfGrid = new PdfGrid();
                pdfGrid.Style.CellPadding.Left = 8;
                pdfGrid.Style.CellPadding.Right = 8;
                //Applying built-in style to the PDF grid.
                pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid.Columns.Add(8);
                pdfGrid.Columns[0].Width = 30;
                pdfGrid.Columns[1].Width = 80;
                pdfGrid.Columns[2].Width = 40;
                pdfGrid.Columns[3].Width = 80;
                pdfGrid.Columns[4].Width = 80;
                pdfGrid.Columns[5].Width = 80;
                pdfGrid.Columns[6].Width = 80;
                pdfGrid.Columns[7].Width = 80;
                //Add header.
                pdfGrid.Headers.Add(1);
                PdfGridRow pdfGridHeader = pdfGrid.Headers[0];
                pdfGridHeader.Cells[0].Value = "#";
                pdfGridHeader.Cells[1].Value = "Forma de pago";
                pdfGridHeader.Cells[2].Value = "DocNum";
                pdfGridHeader.Cells[3].Value = "Banco";
                pdfGridHeader.Cells[4].Value = "Cuenta";
                pdfGridHeader.Cells[5].Value = "Serie";
                pdfGridHeader.Cells[6].Value = "Fecha";
                pdfGridHeader.Cells[7].Value = "Importe";

                i = 1;
                int importeTotal = 0;
                foreach (var data in pago.Pagos)
                {
                    //Add rows.
                    PdfGridRow pdfGridRow = pdfGrid.Rows.Add();
                    pdfGridRow.Cells[0].Value = i.ToString();
                    pdfGridRow.Cells[1].Value = data.FormaPago;
                    pdfGridRow.Cells[2].Value = "";
                    pdfGridRow.Cells[3].Value = data.Banco;
                    pdfGridRow.Cells[4].Value = data.NroCtaCteBanco;
                    pdfGridRow.Cells[5].Value = data.NumeroSerie;
                    pdfGridRow.Cells[6].Value = data.FechaCancelacion.ToString("dd/MM/yyyy");
                    pdfGridRow.Cells[7].Value = data.Monto.ToString("N0");
                    importeTotal = importeTotal + Convert.ToInt32(data.Monto);
                    i++;
                }


                //Assign data source.
                //pdfGrid.DataSource = pago.Pagos;
                //pdfGrid.Style.Font = contentfont;
                float x = listaPagos.GetBounds().Bottom;
                //Draw PDF grid into the PDF page.
                PdfGridLayoutResult pdfGridLayoutResult = pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(0, pdfGridLayoutResult2.Bounds.Bottom + 30));


                PdfTemplate comentario = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 80);
                comentario.Graphics.DrawString("Comentarios:", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                comentario.Graphics.DrawString($" {pago.Comentarios.Trim()}", contentfontNormal, PdfBrushes.Black, new PointF(80, 0));

                comentario.Graphics.DrawString($"Total pedido", contentfont, PdfBrushes.Black, new PointF(320, 0));
                comentario.Graphics.DrawString($": {saldoPedidosPagado.ToString("N0")}", contentfont, PdfBrushes.Black, new PointF(420, 0));

                comentario.Graphics.DrawString($"Saldo usado", contentfont, PdfBrushes.Black, new PointF(320, 10));
                comentario.Graphics.DrawString($": {montosSaldos.ToString("N0")}", contentfont, PdfBrushes.Black, new PointF(420, 10));

                comentario.Graphics.DrawString($"Total pago", contentfont, PdfBrushes.Black, new PointF(320, 20));
                comentario.Graphics.DrawString($": {importeTotal.ToString("N0")}", contentfont, PdfBrushes.Black, new PointF(420, 20));

                page.Graphics.DrawPdfTemplate(comentario, new PointF(0, pdfGridLayoutResult.Bounds.Bottom + 15));


                PdfTemplate firma = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 80);
                //Linea
                PdfPen penLine2 = new PdfPen(PdfBrushes.Black, 1.0f);
                PointF point11 = new PointF(50, 0);
                PointF point22 = new PointF(200, 0);
                firma.Graphics.DrawLine(penLine2, point11, point22);
                firma.Graphics.DrawString($"Receptor pago", contentfontNormal, PdfBrushes.Black, new PointF(90, 10));

                PdfPen penLine3 = new PdfPen(PdfBrushes.Black, 1.0f);
                PointF point13 = new PointF(350, 0);
                PointF point23 = new PointF(500, 0);
                firma.Graphics.DrawLine(penLine3, point13, point23);
                firma.Graphics.DrawString($"Cliente", contentfontNormal, PdfBrushes.Black, new PointF(410, 10));

                page.Graphics.DrawPdfTemplate(firma, new PointF(0, pdfGridLayoutResult.Bounds.Bottom + 120));


                using (MemoryStream stream = new MemoryStream())
                {
                    pdfDocument.Save(stream);
                    pdfDocument.Close();
                    return stream;
                }

            }
        }
        public MemoryStream CreatePdfHojaderuta(HRReimpresionModel datos)
        {
            //Stream img;
            //DetalleClienteNCDTO detalleCliente = clientes[0];
            #region
            using (PdfDocument pdfDocument = new PdfDocument())
            {

                // Add page
                pdfDocument.PageSettings.Size = PdfPageSize.Letter;
                pdfDocument.PageSettings.Orientation = PdfPageOrientation.Portrait;
                PdfMargins margin = new PdfMargins();
                margin.All = 20;
                pdfDocument.PageSettings.Margins = margin;
                PdfPage page = pdfDocument.Pages.Add();

                // Estilos de letras
                int i = 1;
                int paragraphAfterSpacing = 8;
                PdfStandardFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormal = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Regular);
                PdfStandardFont contentfontDetalle = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Regular);
                PdfStandardFont contentfontLittle = new PdfStandardFont(PdfFontFamily.TimesRoman, 7, PdfFontStyle.Bold);
                PdfStandardFont contentfontMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormalMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);

                //Create a header 
                RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 60);
                PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

                // Cabecera
                header.Graphics.DrawString("COMPROBANTE HOJA DE RUTA", fontTitle, PdfBrushes.Black, new PointF(180, 20));
                header.Graphics.DrawString("ANDES LTDA.", fontTitle, PdfBrushes.Black, new PointF(240, 33));
                header.Graphics.DrawString("Fecha Impresion", contentfont, PdfBrushes.Black, new PointF(420, 0));
                header.Graphics.DrawString($": {DateTime.Today.ToShortDateString()}", contentfontNormal, PdfBrushes.Black, new PointF(500, 0));

                PdfPen StyleLine = new PdfPen(PdfBrushes.Gray, 1.5f);
                //Create the line points
                PointF Inicio = new PointF(0, 50);
                PointF Termino = new PointF(pdfDocument.Pages[0].GetClientSize().Width, 50);

                header.Graphics.DrawLine(StyleLine, Inicio, Termino);

                pdfDocument.Template.Top = header;

                //Create Container
                RectangleF rectangleF = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 150);
                PdfTemplate dataHR = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 150);

                dataHR.Graphics.DrawRectangle(PdfBrushes.White, rectangleF);

                dataHR.Graphics.DrawString("Nro. Hoja De Ruta", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                dataHR.Graphics.DrawString(":  " + datos.IDGuiaHojaRuta, contentfontNormal, PdfBrushes.Black, new PointF(100, 0));
                dataHR.Graphics.DrawString("Responsable", contentfontNormal, PdfBrushes.Black, new PointF(400, 0));
                dataHR.Graphics.DrawString(":  " + datos.IDUsuario, contentfontNormal, PdfBrushes.Black, new PointF(500, 0));
                dataHR.Graphics.DrawString("Fecha", contentfontNormal, PdfBrushes.Black, new PointF(0, 10));
                dataHR.Graphics.DrawString(":  " + datos.FechaInicio, contentfontNormal, PdfBrushes.Black, new PointF(100, 10));
                dataHR.Graphics.DrawString("Total Bultos HR", contentfontNormal, PdfBrushes.Black, new PointF(400, 10));
                dataHR.Graphics.DrawString(":  " + datos.Detalle.Where(x=> x.IDTipo == 1).Sum(x => x.NroCajas ), contentfontNormal, PdfBrushes.Black, new PointF(500, 10));
                dataHR.Graphics.DrawString("Zona", contentfontNormal, PdfBrushes.Black, new PointF(0, 20));
                dataHR.Graphics.DrawString(":  " + datos.ZonaD, contentfontNormal, PdfBrushes.Black, new PointF(100, 20));

                PdfPen StyleLine2 = new PdfPen(PdfBrushes.Gray, 1.5f);
                //Create the line points
                PointF Inicio2 = new PointF(0, 50);
                PointF Termino2 = new PointF(pdfDocument.Pages[0].GetClientSize().Width, 50);

                dataHR.Graphics.DrawLine(StyleLine2, Inicio2, Termino2);

                page.Graphics.DrawPdfTemplate(dataHR, new PointF(0, 0));


                //Create Grid  Pedidos             

                PdfTemplate listaPedidos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPedidos.Graphics.DrawString("Lista de pedidos", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                page.Graphics.DrawPdfTemplate(listaPedidos, new PointF(0, 60));

                PdfGrid pdfGrid3 = new PdfGrid();
                pdfGrid3.Style.CellPadding.Left = 8;
                pdfGrid3.Style.CellPadding.Right = 8;

                //Applying built-in style to the PDF grid.
                pdfGrid3.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid3.Columns.Add(8);
                pdfGrid3.Columns[0].Width = 30;
                pdfGrid3.Columns[1].Width = 73;
                pdfGrid3.Columns[2].Width = 73;
                pdfGrid3.Columns[3].Width = 111;
                pdfGrid3.Columns[4].Width = 73;
                pdfGrid3.Columns[5].Width = 95;
                pdfGrid3.Columns[6].Width = 20;
                pdfGrid3.Columns[7].Width = 80;

                //Add header.
                pdfGrid3.Headers.Add(1);
                PdfGridRow pdfGridHeader3 = pdfGrid3.Headers[0];
                pdfGridHeader3.Cells[0].Value = "#";
                pdfGridHeader3.Cells[1].Value = "Nro. Documento";
                pdfGridHeader3.Cells[2].Value = "Cliente";
                pdfGridHeader3.Cells[3].Value = "Razon Solcial";
                pdfGridHeader3.Cells[4].Value = "Comuna";
                pdfGridHeader3.Cells[5].Value = "Direccion";
                pdfGridHeader3.Cells[6].Value = "Nro. Bultos";
                pdfGridHeader3.Cells[7].Value = "ubicacion D";


                HRReimpresionModel HojaRutaPedidos = new HRReimpresionModel();
                if (datos.Detalle.Count != 0) HojaRutaPedidos = datos;

                foreach (var data in HojaRutaPedidos.Detalle)
                {
                    if (data.IDTipo == 1)
                    {
                        //Add rows.
                        PdfGridRow pdfGridRow = pdfGrid3.Rows.Add();
                        pdfGridRow.Cells[0].Value = i.ToString();
                        pdfGridRow.Cells[1].Value = data.DocReferencia.ToString();
                        pdfGridRow.Cells[2].Value = data.IDCliente.ToString();
                        pdfGridRow.Cells[3].Value = data.RazonSocial.Length > 20 ? data.RazonSocial.Substring(0, 20) : data.RazonSocial;
                        pdfGridRow.Cells[4].Value = data.Comuna;
                        pdfGridRow.Cells[5].Value = data.Direccion.Length > 20 ? data.Direccion.Substring(0, 20) : data.Direccion;
                        pdfGridRow.Cells[6].Value = data.NroCajas.ToString();
                        pdfGridRow.Cells[7].Value = data.UbicacionD;
                        i++;
                    }
                }

                PdfGridLayoutResult pdfGridLayoutResult3 = pdfGrid3.Draw(page, new Syncfusion.Drawing.PointF(0, listaPedidos.GetBounds().Bottom + 40 + paragraphAfterSpacing));

                //Formularios
                PdfTemplate listaSaldos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaSaldos.Graphics.DrawString("Lista de Formularios", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                page.Graphics.DrawPdfTemplate(listaSaldos, new PointF(0, pdfGridLayoutResult3.Bounds.Bottom + 15));

                PdfGrid pdfGrid2 = new PdfGrid();
                pdfGrid2.Style.CellPadding.Left = 8;
                pdfGrid2.Style.CellPadding.Right = 8;
                //Applying built-in style to the PDF grid.
                pdfGrid2.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                pdfGrid2.Columns.Add(7);
                pdfGrid2.Columns[0].Width = 30;
                pdfGrid2.Columns[1].Width = 86;
                pdfGrid2.Columns[2].Width = 86;
                pdfGrid2.Columns[3].Width = 86;
                pdfGrid2.Columns[4].Width = 86;
                pdfGrid2.Columns[5].Width = 86;
                pdfGrid2.Columns[6].Width = 90;

                //Add three columns.
                pdfGrid2.Headers.Add(1);
                PdfGridRow pdfGridHeader2 = pdfGrid2.Headers[0];
                pdfGridHeader2.Cells[0].Value = "#";
                pdfGridHeader2.Cells[1].Value = "Nro. Documento";
                pdfGridHeader2.Cells[2].Value = "Cliente";
                pdfGridHeader2.Cells[3].Value = "Razon Solcial";
                pdfGridHeader2.Cells[4].Value = "Comuna";
                pdfGridHeader2.Cells[5].Value = "Direccion";
                pdfGridHeader2.Cells[6].Value = "Nro. Bultos/Documentos";

                i = 1;

                if (datos.Detalle.Count != 0) HojaRutaPedidos = datos;

                foreach (var data in HojaRutaPedidos.Detalle)
                {
                    if (data.IDTipo == 2 || data.IDTipo == 3)
                    {
                        //Add rows.
                        PdfGridRow pdfGridRow = pdfGrid2.Rows.Add();
                        pdfGridRow.Cells[0].Value = i.ToString();
                        pdfGridRow.Cells[1].Value = data.DocReferencia.ToString();
                        pdfGridRow.Cells[2].Value = data.IDCliente.ToString();
                        pdfGridRow.Cells[3].Value = data.RazonSocial.Length > 20 ? data.RazonSocial.Substring(0, 20) : data.RazonSocial;
                        pdfGridRow.Cells[4].Value = data.Comuna;
                        pdfGridRow.Cells[5].Value = data.Direccion.Length > 20 ? data.Direccion.Substring(0, 20) : data.Direccion;
                        pdfGridRow.Cells[6].Value = data.NroCajas.ToString();


                        i++;
                    }
                }

                PdfGridLayoutResult pdfGridLayoutResult2 = pdfGrid2.Draw(page, new PointF(0, pdfGridLayoutResult3.Bounds.Bottom + 30));




                using (MemoryStream stream = new MemoryStream())
                {
                    pdfDocument.Save(stream);
                    pdfDocument.Close();
                    return stream;
                }


                #endregion
            }


        }
        public MemoryStream CreatePdfHojaderuta(List<ConsultaDisponiblesHojaDeRutaModel> HojaRuta, List<ResponseHojaruta> HojaRutaResponse, int zona)
        {
            //Stream img;
            //DetalleClienteNCDTO detalleCliente = clientes[0];
            #region
            using (PdfDocument pdfDocument = new PdfDocument())
            {

                // Add page
                pdfDocument.PageSettings.Size = PdfPageSize.Letter;
                pdfDocument.PageSettings.Orientation = PdfPageOrientation.Portrait;
                PdfMargins margin = new PdfMargins();
                margin.All = 20;
                pdfDocument.PageSettings.Margins = margin;
                PdfPage page = pdfDocument.Pages.Add();

                // Estilos de letras
                int i = 1;
                int paragraphAfterSpacing = 8;
                PdfStandardFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormal = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Regular);
                PdfStandardFont contentfontDetalle = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Regular);
                PdfStandardFont contentfontLittle = new PdfStandardFont(PdfFontFamily.TimesRoman, 7, PdfFontStyle.Bold);
                PdfStandardFont contentfontMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormalMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);

                //Create a header 
                RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 60);
                PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

                // Cabecera
                header.Graphics.DrawString("COMPROBANTE HOJA DE RUTA", fontTitle, PdfBrushes.Black, new PointF(180, 20));
                header.Graphics.DrawString("ANDES LTDA.", fontTitle, PdfBrushes.Black, new PointF(240, 33));
                header.Graphics.DrawString("Fecha Impresion", contentfont, PdfBrushes.Black, new PointF(420, 0));
                header.Graphics.DrawString($": {DateTime.Today.ToShortDateString()}", contentfontNormal, PdfBrushes.Black, new PointF(500, 0));

                PdfPen StyleLine = new PdfPen(PdfBrushes.Gray, 1.5f);
                //Create the line points
                PointF Inicio = new PointF(0, 50);
                PointF Termino = new PointF(pdfDocument.Pages[0].GetClientSize().Width, 50);

                header.Graphics.DrawLine(StyleLine, Inicio, Termino);

                pdfDocument.Template.Top = header;

                //Create Container
                RectangleF rectangleF = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 150);
                PdfTemplate dataHR = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 150);

                dataHR.Graphics.DrawRectangle(PdfBrushes.White, rectangleF);

                dataHR.Graphics.DrawString("Nro. Hoja De Ruta", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                dataHR.Graphics.DrawString(":  " + HojaRutaResponse.FirstOrDefault().IDGuiaHojaRuta, contentfontNormal, PdfBrushes.Black, new PointF(100, 0));
                dataHR.Graphics.DrawString("Responsable", contentfontNormal, PdfBrushes.Black, new PointF(400, 0));
                dataHR.Graphics.DrawString(":  " + HojaRutaResponse.FirstOrDefault().IDUsuario, contentfontNormal, PdfBrushes.Black, new PointF(500, 0));
                dataHR.Graphics.DrawString("Fecha", contentfontNormal, PdfBrushes.Black, new PointF(0, 10));
                dataHR.Graphics.DrawString(":  " + HojaRutaResponse.FirstOrDefault().FechaInicio.Remove(11), contentfontNormal, PdfBrushes.Black, new PointF(100, 10));
                dataHR.Graphics.DrawString("Total Bultos HR", contentfontNormal, PdfBrushes.Black, new PointF(400, 10));
                dataHR.Graphics.DrawString(":  " + HojaRuta.Where(x=> x.Tipo == 1).Sum(x => x.NroBultos), contentfontNormal, PdfBrushes.Black, new PointF(500, 10));
                dataHR.Graphics.DrawString("Zona", contentfontNormal, PdfBrushes.Black, new PointF(0, 20));
                dataHR.Graphics.DrawString(":  " + zona, contentfontNormal, PdfBrushes.Black, new PointF(100, 20));

                PdfPen StyleLine2 = new PdfPen(PdfBrushes.Gray, 1.5f);
                //Create the line points
                PointF Inicio2 = new PointF(0, 50);
                PointF Termino2 = new PointF(pdfDocument.Pages[0].GetClientSize().Width, 50);

                dataHR.Graphics.DrawLine(StyleLine2, Inicio2, Termino2);

                page.Graphics.DrawPdfTemplate(dataHR, new PointF(0, 0));


                //Create Grid  Pedidos             

                PdfTemplate listaPedidos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPedidos.Graphics.DrawString("Lista de pedidos", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                page.Graphics.DrawPdfTemplate(listaPedidos, new PointF(0, 60));

                PdfGrid pdfGrid3 = new PdfGrid();
                pdfGrid3.Style.CellPadding.Left = 8;
                pdfGrid3.Style.CellPadding.Right = 8;

                //Applying built-in style to the PDF grid.
                pdfGrid3.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid3.Columns.Add(8);
                pdfGrid3.Columns[0].Width = 30;
                pdfGrid3.Columns[1].Width = 73;
                pdfGrid3.Columns[2].Width = 73;
                pdfGrid3.Columns[3].Width = 111;
                pdfGrid3.Columns[4].Width = 73;
                pdfGrid3.Columns[5].Width = 95;
                pdfGrid3.Columns[6].Width = 20;
                pdfGrid3.Columns[7].Width = 80;

                //Add header.
                pdfGrid3.Headers.Add(1);
                PdfGridRow pdfGridHeader3 = pdfGrid3.Headers[0];
                pdfGridHeader3.Cells[0].Value = "#";
                pdfGridHeader3.Cells[1].Value = "Nro. Documento";
                pdfGridHeader3.Cells[2].Value = "Cliente";
                pdfGridHeader3.Cells[3].Value = "Razon Solcial";
                pdfGridHeader3.Cells[4].Value = "Comuna";
                pdfGridHeader3.Cells[5].Value = "Direccion";
                pdfGridHeader3.Cells[6].Value = "Nro. Bultos";
                pdfGridHeader3.Cells[7].Value = "Transporte";


                List<ConsultaDisponiblesHojaDeRutaModel> HojaRutaPedidos = new List<ConsultaDisponiblesHojaDeRutaModel>();
                if (HojaRuta.Count != 0) HojaRutaPedidos = HojaRuta;

                foreach (var data in HojaRutaPedidos)
                {
                    if (data.Tipo == 1)
                    {
                        //Add rows.
                        PdfGridRow pdfGridRow = pdfGrid3.Rows.Add();
                        pdfGridRow.Cells[0].Value = i.ToString();
                        pdfGridRow.Cells[1].Value = data.NroDocumento.ToString();
                        pdfGridRow.Cells[2].Value = data.IDCliente.ToString();
                        pdfGridRow.Cells[3].Value = data.RazonSocial.Length > 20 ? data.RazonSocial.Substring(0, 20) : data.RazonSocial;
                        pdfGridRow.Cells[4].Value = data.Comuna;
                        pdfGridRow.Cells[5].Value = data.Direccion.Length > 20 ? data.Direccion.Substring(0,20): data.Direccion;
                        pdfGridRow.Cells[6].Value = data.NroBultos.ToString();
                        pdfGridRow.Cells[7].Value = data.Transporte;
                        i++;
                    }
                }

                PdfGridLayoutResult pdfGridLayoutResult3 = pdfGrid3.Draw(page, new Syncfusion.Drawing.PointF(0, listaPedidos.GetBounds().Bottom + 40 + paragraphAfterSpacing));

                //Formularios
                PdfTemplate listaSaldos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaSaldos.Graphics.DrawString("Lista de Formularios", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                page.Graphics.DrawPdfTemplate(listaSaldos, new PointF(0, pdfGridLayoutResult3.Bounds.Bottom + 15));

                PdfGrid pdfGrid2 = new PdfGrid();
                pdfGrid2.Style.CellPadding.Left = 8;
                pdfGrid2.Style.CellPadding.Right = 8;
                //Applying built-in style to the PDF grid.
                pdfGrid2.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                pdfGrid2.Columns.Add(7);
                pdfGrid2.Columns[0].Width = 30;
                pdfGrid2.Columns[1].Width = 86;
                pdfGrid2.Columns[2].Width = 86;
                pdfGrid2.Columns[3].Width = 86;
                pdfGrid2.Columns[4].Width = 86;
                pdfGrid2.Columns[5].Width = 86;
                pdfGrid2.Columns[6].Width = 90;

                //Add three columns.
                pdfGrid2.Headers.Add(1);
                PdfGridRow pdfGridHeader2 = pdfGrid2.Headers[0];
                pdfGridHeader2.Cells[0].Value = "#";
                pdfGridHeader2.Cells[1].Value = "Nro. Documento";
                pdfGridHeader2.Cells[2].Value = "Cliente";
                pdfGridHeader2.Cells[3].Value = "Razon Solcial";
                pdfGridHeader2.Cells[4].Value = "Comuna";
                pdfGridHeader2.Cells[5].Value = "Direccion";
                pdfGridHeader2.Cells[6].Value = "Nro. Bultos/Documentos";

                i = 1;

                if (HojaRuta.Count != 0) HojaRutaPedidos = HojaRuta;

                foreach (var data in HojaRutaPedidos)
                {
                    if (data.Tipo == 2 || data.Tipo == 3)
                    {
                        //Add rows.
                        PdfGridRow pdfGridRow = pdfGrid2.Rows.Add();
                        pdfGridRow.Cells[0].Value = i.ToString();
                        pdfGridRow.Cells[1].Value = data.NroDocumento.ToString();
                        pdfGridRow.Cells[2].Value = data.IDCliente.ToString();
                        pdfGridRow.Cells[3].Value = data.RazonSocial.Length > 20 ? data.RazonSocial.Substring(0, 20) : data.RazonSocial;
                        pdfGridRow.Cells[4].Value = data.Comuna;
                        pdfGridRow.Cells[5].Value = data.Direccion.Length > 20 ? data.Direccion.Substring(0, 20) : data.Direccion;
                        pdfGridRow.Cells[6].Value = data.NroBultos.ToString();


                        i++;
                    }
                }

                PdfGridLayoutResult pdfGridLayoutResult2 = pdfGrid2.Draw(page, new PointF(0, pdfGridLayoutResult3.Bounds.Bottom + 30));




                using (MemoryStream stream = new MemoryStream())
                {
                    pdfDocument.Save(stream);
                    pdfDocument.Close();
                    return stream;
                }


                #endregion
            }


        }
        public MemoryStream CreatePdfLineas(List<Lineas> LineasPicking, List<Lineas> LineasPacking, List<Lineas> LineasReposicion, List<Lineas> LineasDevolucion)
        {
            //Stream img;
            //DetalleClienteNCDTO detalleCliente = clientes[0];            
            using (PdfDocument pdfDocument = new PdfDocument())
            {

                // Add page
                pdfDocument.PageSettings.Size = PdfPageSize.Letter;
                pdfDocument.PageSettings.Orientation = PdfPageOrientation.Portrait;
                PdfMargins margin = new PdfMargins();
                margin.All = 20;
                pdfDocument.PageSettings.Margins = margin;
                PdfPage page = pdfDocument.Pages.Add();

                // Estilos de letras
                int i = 1;
                int paragraphAfterSpacing = 8;
                PdfStandardFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormal = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Regular);
                PdfStandardFont contentfontDetalle = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Regular);
                PdfStandardFont contentfontLittle = new PdfStandardFont(PdfFontFamily.TimesRoman, 7, PdfFontStyle.Bold);
                PdfStandardFont contentfontMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormalMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);

                //Create a header 
                RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 60);
                PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

                // Cabecera

                header.Graphics.DrawString("Líneas Por Usuarios", fontTitle, PdfBrushes.Black, new PointF(230, 20));
                header.Graphics.DrawString("Fecha Impresion", contentfont, PdfBrushes.Black, new PointF(420, 0));
                header.Graphics.DrawString($": {DateTime.Today.ToShortDateString()}", contentfontNormal, PdfBrushes.Black, new PointF(500, 0));

                PdfPen StyleLine = new PdfPen(PdfBrushes.Gray, 1.5f);
                //Create the line points
                PointF Inicio = new PointF(0, 50);
                PointF Termino = new PointF(pdfDocument.Pages[0].GetClientSize().Width, 50);

                header.Graphics.DrawLine(StyleLine, Inicio, Termino);

                pdfDocument.Template.Top = header;

                //Create Grid  Pedidos             

                PdfTemplate listaPedidos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPedidos.Graphics.DrawString("Lista Picking", contentfontNormal, PdfBrushes.Black, new PointF(10, 0));
                page.Graphics.DrawPdfTemplate(listaPedidos, new PointF(0, 60));

                PdfGrid pdfGrid3 = new PdfGrid();
                pdfGrid3.Style.CellPadding.Left = 8;
                pdfGrid3.Style.CellPadding.Right = 8;

                //Applying built-in style to the PDF grid.
                pdfGrid3.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid3.Columns.Add(6);
                pdfGrid3.Columns[0].Width = 30;
                pdfGrid3.Columns[1].Width = 100;
                pdfGrid3.Columns[2].Width = 100;
                pdfGrid3.Columns[3].Width = 50;
                pdfGrid3.Columns[4].Width = 50;

                //Add header.
                pdfGrid3.Headers.Add(1);
                PdfGridRow pdfGridHeader3 = pdfGrid3.Headers[0];
                pdfGridHeader3.Cells[0].Value = "#";
                pdfGridHeader3.Cells[1].Value = "Sacador";
                pdfGridHeader3.Cells[2].Value = "Bodega";
                pdfGridHeader3.Cells[3].Value = "Líneas";
                //pdfGridHeader3.Cells[4].Value = "Bultos";

                List<Lineas> lineasPickingData = new List<Lineas>();
                if (LineasPicking.Count != 0) lineasPickingData = LineasPicking;
                i = 1;
                foreach (var data in lineasPickingData)
                {
                    //Add rows.
                    PdfGridRow pdfGridRow = pdfGrid3.Rows.Add();
                    pdfGridRow.Cells[0].Value = i.ToString();
                    pdfGridRow.Cells[1].Value = data.Sacador.ToString();
                    pdfGridRow.Cells[2].Value = data.IDBodega.ToString();
                    pdfGridRow.Cells[3].Value = data.LINEAS.ToString();
                    //pdfGridRow.Cells[4].Value = data.BULTOS.ToString();
                    i++;

                }
                PdfGridLayoutResult pdfGridLayoutResult3 = pdfGrid3.Draw(page, new Syncfusion.Drawing.PointF(10, listaPedidos.GetBounds().Bottom + 40 + paragraphAfterSpacing));

                PdfTemplate listaPedidos2 = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPedidos2.Graphics.DrawString("Lista Packing", contentfontNormal, PdfBrushes.Black, new PointF(10, 0));
                page.Graphics.DrawPdfTemplate(listaPedidos2, new PointF(0, pdfGridLayoutResult3.Bounds.Bottom + 15));

                PdfGrid pdfGrid32 = new PdfGrid();
                pdfGrid32.Style.CellPadding.Left = 8;
                pdfGrid32.Style.CellPadding.Right = 8;

                //Applying built-in style to the PDF grid.
                pdfGrid32.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid32.Columns.Add(5);
                pdfGrid32.Columns[0].Width = 30;
                pdfGrid32.Columns[1].Width = 100;
                pdfGrid32.Columns[2].Width = 100;
                pdfGrid32.Columns[3].Width = 100;

                //Add header.
                pdfGrid32.Headers.Add(1);
                PdfGridRow pdfGridHeader32 = pdfGrid32.Headers[0];
                pdfGridHeader32.Cells[0].Value = "#";
                pdfGridHeader32.Cells[1].Value = "Sacador";
                pdfGridHeader32.Cells[2].Value = "Líneas";
                //pdfGridHeader32.Cells[3].Value = "Bultos";

                List<Lineas> lineasPackingData = new List<Lineas>();
                if (LineasPacking.Count != 0) lineasPackingData = LineasPacking;
                i = 1;
                foreach (var data in lineasPackingData)
                {
                    //Add rows.
                    PdfGridRow pdfGridRow2 = pdfGrid32.Rows.Add();
                    pdfGridRow2.Cells[0].Value = i.ToString();
                    pdfGridRow2.Cells[1].Value = data.Sacador == "" ? "NADIE" : data.Sacador  ;
                    pdfGridRow2.Cells[2].Value = data.LINEAS.ToString();
                    //pdfGridRow2.Cells[3].Value = data.BULTOS.ToString();
                    i++;

                }
                PdfGridLayoutResult pdfGridLayoutResult33 = pdfGrid32.Draw(page, new PointF(10, pdfGridLayoutResult3.Bounds.Bottom + 30));


                //lista reposicion
                PdfTemplate listaPedidos3 = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPedidos3.Graphics.DrawString("Lista Reposicion", contentfontNormal, PdfBrushes.Black, new PointF(10, 0));
                page.Graphics.DrawPdfTemplate(listaPedidos3, new PointF(0, pdfGridLayoutResult33.Bounds.Bottom + 15));

                PdfGrid pdfGrid33 = new PdfGrid();
                pdfGrid33.Style.CellPadding.Left = 8;
                pdfGrid33.Style.CellPadding.Right = 8;

                //Applying built-in style to the PDF grid.
                pdfGrid33.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid33.Columns.Add(5);
                pdfGrid33.Columns[0].Width = 30;
                pdfGrid33.Columns[1].Width = 200;
                pdfGrid33.Columns[2].Width = 200;
                pdfGrid33.Columns[3].Width = 100;
                pdfGrid33.Columns[4].Width = 100;

                //Add header.
                pdfGrid33.Headers.Add(1);
                PdfGridRow pdfGridHeader33 = pdfGrid33.Headers[0];
                pdfGridHeader33.Cells[0].Value = "#";
                pdfGridHeader33.Cells[1].Value = "Sacador";
                pdfGridHeader33.Cells[2].Value = "Líneas";

                List<Lineas> lineasReposicionData = new List<Lineas>();
                if (LineasReposicion.Count != 0) lineasReposicionData = LineasReposicion;
                i = 1;
                foreach (var data in LineasReposicion)

                {
                    //Add rows.
                    PdfGridRow pdfGridRow3 = pdfGrid33.Rows.Add();
                    pdfGridRow3.Cells[0].Value = i.ToString();
                    pdfGridRow3.Cells[1].Value = data.Sacador.ToString();
                    pdfGridRow3.Cells[2].Value = data.LINEAS.ToString();
                    i++;

                }
                PdfGridLayoutResult pdfGridLayoutResult34 = pdfGrid33.Draw(page, new PointF(10, pdfGridLayoutResult33.Bounds.Bottom + 30));


                //Lista Devolucion
                PdfTemplate listaPedidos4 = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPedidos4.Graphics.DrawString("Lista Devolucion", contentfontNormal, PdfBrushes.Black, new PointF(10, 0));
                page.Graphics.DrawPdfTemplate(listaPedidos4, new PointF(0, pdfGridLayoutResult34.Bounds.Bottom + 15));

                PdfGrid pdfGrid34 = new PdfGrid();
                pdfGrid34.Style.CellPadding.Left = 8;
                pdfGrid34.Style.CellPadding.Right = 8;

                //Applying built-in style to the PDF grid.
                pdfGrid34.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid34.Columns.Add(5);
                pdfGrid34.Columns[0].Width = 30;
                pdfGrid34.Columns[1].Width = 200;
                pdfGrid34.Columns[2].Width = 200;
                pdfGrid34.Columns[3].Width = 100;
                pdfGrid34.Columns[4].Width = 100;

                //Add header.
                pdfGrid34.Headers.Add(1);
                PdfGridRow pdfGridHeader34 = pdfGrid34.Headers[0];
                pdfGridHeader34.Cells[0].Value = "#";
                pdfGridHeader34.Cells[1].Value = "Sacador";
                pdfGridHeader34.Cells[2].Value = "Líneas";

                List<Lineas> lineasDevolucionData = new List<Lineas>();
                if (LineasReposicion.Count != 0) lineasDevolucionData = LineasDevolucion;
                i = 1;
                foreach (var data in LineasDevolucion)

                {
                    //Add rows.
                    PdfGridRow pdfGridRow4 = pdfGrid34.Rows.Add();
                    pdfGridRow4.Cells[0].Value = i.ToString();
                    pdfGridRow4.Cells[1].Value = data.Sacador.ToString();
                    pdfGridRow4.Cells[2].Value = data.LINEAS.ToString();
                    i++;

                }
                PdfGridLayoutResult pdfGridLayoutResult35 = pdfGrid34.Draw(page, new PointF(10, pdfGridLayoutResult34.Bounds.Bottom + 30));

                using (MemoryStream stream = new MemoryStream())
                {
                    pdfDocument.Save(stream);
                    pdfDocument.Close();
                    return stream;
                }
            }
        }
        public MemoryStream CreatePdfPrePacking(List<ShowGuiaPrePackingDetalleDTOPDF> DetalleCabecera, double idGuia, string idUsuario, int DocEntryEM, int DocNumEM ,string ReferenciaEM)
        {
            #region
            using (PdfDocument pdfDocument = new PdfDocument())
            {

                // Add page
                pdfDocument.PageSettings.Size = PdfPageSize.Letter;
                pdfDocument.PageSettings.Orientation = PdfPageOrientation.Portrait;
                PdfMargins margin = new PdfMargins();
                margin.All = 20;
                pdfDocument.PageSettings.Margins = margin;
                PdfPage page = pdfDocument.Pages.Add();

                // Estilos de letras
                int i = 1;
                int paragraphAfterSpacing = 8;
                PdfStandardFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormal = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Regular);
                PdfStandardFont contentfontDetalle = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Regular);
                PdfStandardFont contentfontLittle = new PdfStandardFont(PdfFontFamily.TimesRoman, 7, PdfFontStyle.Bold);
                PdfStandardFont contentfontMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormalMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);

                //Create a header 
                RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 60);
                PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

                // Cabecera
                header.Graphics.DrawString("COMPROBANTE PRE PACKING", fontTitle, PdfBrushes.Black, new PointF(180, 20));
                header.Graphics.DrawString("Fecha Impresion", contentfont, PdfBrushes.Black, new PointF(420, 0));
                header.Graphics.DrawString($": {DateTime.Today.ToShortDateString()}", contentfontNormal, PdfBrushes.Black, new PointF(500, 0));

                PdfPen StyleLine = new PdfPen(PdfBrushes.Gray, 1.5f);
                //Create the line points
                PointF Inicio = new PointF(0, 50);
                PointF Termino = new PointF(pdfDocument.Pages[0].GetClientSize().Width, 50);

                header.Graphics.DrawLine(StyleLine, Inicio, Termino);

                pdfDocument.Template.Top = header;

                //Create Container
                RectangleF rectangleF = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 150);
                PdfTemplate dataHR = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 150);

                dataHR.Graphics.DrawRectangle(PdfBrushes.White, rectangleF);

                dataHR.Graphics.DrawString("Nro. Guia", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                dataHR.Graphics.DrawString(":  "+ idGuia, contentfontNormal, PdfBrushes.Black, new PointF(100, 0));
                dataHR.Graphics.DrawString("Responsable", contentfontNormal, PdfBrushes.Black, new PointF(400, 0));
                dataHR.Graphics.DrawString(":  "+ idUsuario, contentfontNormal, PdfBrushes.Black, new PointF(500, 0));
                dataHR.Graphics.DrawString("Fecha", contentfontNormal, PdfBrushes.Black, new PointF(0, 10));
                dataHR.Graphics.DrawString(":  "+ DetalleCabecera.FirstOrDefault().Fecha, contentfontNormal, PdfBrushes.Black, new PointF(100, 10));
                dataHR.Graphics.DrawString("DocNum EM", contentfontNormal, PdfBrushes.Black, new PointF(400, 10));
                dataHR.Graphics.DrawString(":  " + DocNumEM, contentfontNormal, PdfBrushes.Black, new PointF(500, 10));
                dataHR.Graphics.DrawString("Referencia EM;", contentfontNormal, PdfBrushes.Black, new PointF(0, 20));
                dataHR.Graphics.DrawString(":  " + ReferenciaEM, contentfontNormal, PdfBrushes.Black, new PointF(100, 20));

                PdfPen StyleLine2 = new PdfPen(PdfBrushes.Gray, 1.5f);
                PointF Inicio2 = new PointF(0, 50);
                PointF Termino2 = new PointF(pdfDocument.Pages[0].GetClientSize().Width, 50);

                dataHR.Graphics.DrawLine(StyleLine2, Inicio2, Termino2);

                page.Graphics.DrawPdfTemplate(dataHR, new PointF(0, 0));

                PdfTemplate listaPedidos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPedidos.Graphics.DrawString("Lista de Articulos", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));
                page.Graphics.DrawPdfTemplate(listaPedidos, new PointF(0, 60));

                PdfGrid pdfGrid3 = new PdfGrid();
                pdfGrid3.Style.CellPadding.Left = 8;
                pdfGrid3.Style.CellPadding.Right = 8;

                //Applying built-in style to the PDF grid.
                pdfGrid3.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid3.Columns.Add(7);
                pdfGrid3.Columns[0].Width = 30;
                pdfGrid3.Columns[1].Width = 73;
                pdfGrid3.Columns[2].Width = 230;
                pdfGrid3.Columns[3].Width = 40;
                pdfGrid3.Columns[4].Width = 50;
                pdfGrid3.Columns[5].Width = 90;
                pdfGrid3.Columns[6].Width = 50;              

                //Add header.
                pdfGrid3.Headers.Add(1);
                PdfGridRow pdfGridHeader3 = pdfGrid3.Headers[0];
                pdfGridHeader3.Cells[0].Value = "#";
                pdfGridHeader3.Cells[1].Value = "ID Articulo";
                pdfGridHeader3.Cells[2].Value = "Nombre Articulo";
                pdfGridHeader3.Cells[3].Value = "Cantidad EM";
                pdfGridHeader3.Cells[4].Value = "Cantidad Bulto";
                pdfGridHeader3.Cells[5].Value = "Nro Parte";
                pdfGridHeader3.Cells[6].Value = "Revision";            


                List<ShowGuiaPrePackingDetalleDTOPDF> Detalle = new List<ShowGuiaPrePackingDetalleDTOPDF>();
                if (DetalleCabecera.Count != 0) Detalle = DetalleCabecera;
                string Bultos  = "";
                foreach (var data in Detalle)
                {
                   
                    //Add rows.
                    PdfGridRow pdfGridRow = pdfGrid3.Rows.Add();
                    pdfGridRow.Cells[0].Value = data.Linea.ToString();
                    pdfGridRow.Cells[1].Value = data.IDArticulo.ToString();
                    pdfGridRow.Cells[2].Value = data.Nombre;
                    pdfGridRow.Cells[3].Value = data.CantidadEM.ToString();
                    foreach (var item in data.ListaBultos) {
                        Bultos = $"{item.Bultos} X {item.UnidadPorBulto}\n";
                        i++;
                    };
                    pdfGridRow.Cells[4].Value = Bultos;
                    //pdfGridRow.Cells[6].Value = data.NroBultos.ToString();
                    //if (data.Fecha.Length > 11)
                    //{
                    //    DateTime date = DateTime.ParseExact(data.Fecha, "yyyy-MM-ddTHH:mm:ss.ff", null);
                    //    pdfGridRow.Cells[5].Value = date.ToString("dd/MM/yyyy");
                    //}
                    pdfGridRow.Cells[5].Value = data.NroParte;

                    i = 1;
                }

                PdfGridLayoutResult pdfGridLayoutResult3 = pdfGrid3.Draw(page, new Syncfusion.Drawing.PointF(0, listaPedidos.GetBounds().Bottom + 40 + paragraphAfterSpacing));

                PdfPage lastPage = pdfDocument.Pages[pdfDocument.Pages.Count - 1];
                // Crea una plantilla para el comentario y dibuja el texto "Comentario:"
                PdfTemplate Comentario = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                Comentario.Graphics.DrawString("Comentario:", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));

                // Dibuja la plantilla del comentario en la página
                Comentario.Draw(lastPage, new PointF(20, pdfGridLayoutResult3.Bounds.Bottom + 20));

                // Crea una plantilla para un rectángulo y dibuja un rectángulo en ella
                PdfTemplate ComentarioRectangle = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 180);
                PdfPen pen = new PdfPen(Color.Black, 2);
                RectangleF rect = new RectangleF(0, 0, lastPage.GetClientSize().Width, 90);
                ComentarioRectangle.Graphics.DrawRectangle(pen, rect);

                // Dibuja la plantilla del rectángulo en la página
                ComentarioRectangle.Draw(lastPage, new Syncfusion.Drawing.PointF(0, pdfGridLayoutResult3.Bounds.Bottom + 15));

                PdfTemplate Firma = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                Firma.Graphics.DrawString("Firma:", contentfontNormal, PdfBrushes.Black, new PointF(0, 0));

                PdfPen StyleLine3 = new PdfPen(PdfBrushes.Gray, 2.5f);
                PointF Inicio3 = new PointF(0, 15);
                PointF Termino3 = new PointF(120, 15);

                Firma.Graphics.DrawLine(StyleLine3, Inicio3, Termino3);

                // Dibuja la plantilla del comentario en la página
                Firma.Draw(lastPage, new PointF(230, pdfGridLayoutResult3.Bounds.Bottom + 120));



                using (MemoryStream stream = new MemoryStream())
                {
                    pdfDocument.Save(stream);
                    pdfDocument.Close();
                    return stream;
                }


                #endregion
            }


        }
        public MemoryStream CreatePdfReposiciones(List<ReposicionesPendientesModel> ReposicionesPendientes)
        {
            //Stream img;
            //DetalleClienteNCDTO detalleCliente = clientes[0];            
            using (PdfDocument pdfDocument = new PdfDocument())
            {
                // Add page
                pdfDocument.PageSettings.Size = PdfPageSize.Letter;
                pdfDocument.PageSettings.Orientation = PdfPageOrientation.Portrait;
                PdfMargins margin = new PdfMargins();
                margin.All = 20;
                pdfDocument.PageSettings.Margins = margin;
                PdfPage page = pdfDocument.Pages.Add();

                // Estilos de letras
                int i = 1;
                int paragraphAfterSpacing = 8;
                PdfStandardFont fontTitle = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormal = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Regular);
                PdfStandardFont contentfontDetalle = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Regular);
                PdfStandardFont contentfontLittle = new PdfStandardFont(PdfFontFamily.TimesRoman, 7, PdfFontStyle.Bold);
                PdfStandardFont contentfontMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
                PdfStandardFont contentfontNormalMonto = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);

                //Create a header 
                RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 60);
                PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);

                // Cabecera

                header.Graphics.DrawString("Reposiciones Pendientes", fontTitle, PdfBrushes.Black, new PointF(230, 20));
                header.Graphics.DrawString("Fecha Impresion", contentfont, PdfBrushes.Black, new PointF(420, 0));
                header.Graphics.DrawString($": {DateTime.Today.ToShortDateString()}", contentfontNormal, PdfBrushes.Black, new PointF(500, 0));

                PdfPen StyleLine = new PdfPen(PdfBrushes.Gray, 1.5f);
                //Create the line points
                PointF Inicio = new PointF(0, 50);
                PointF Termino = new PointF(pdfDocument.Pages[0].GetClientSize().Width, 50);

                header.Graphics.DrawLine(StyleLine, Inicio, Termino);

                pdfDocument.Template.Top = header;

                //Create Grid  Pedidos             

                PdfTemplate listaPedidos = new PdfTemplate(pdfDocument.Pages[0].GetClientSize().Width, 30);
                listaPedidos.Graphics.DrawString("Lista Reposiciones Pendientes", contentfontNormal, PdfBrushes.Black, new PointF(70, 0));
                page.Graphics.DrawPdfTemplate(listaPedidos, new PointF(0, 60));

                PdfGrid pdfGrid3 = new PdfGrid();
                pdfGrid3.Style.CellPadding.Left = 8;
                pdfGrid3.Style.CellPadding.Right = 8;

                //Applying built-in style to the PDF grid.
                pdfGrid3.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                //Add three columns.
                pdfGrid3.Columns.Add(6);
                pdfGrid3.Columns[0].Width = 25;
                pdfGrid3.Columns[1].Width = 80;
                pdfGrid3.Columns[2].Width = 150;
                pdfGrid3.Columns[3].Width = 50;
                pdfGrid3.Columns[4].Width = 50;
                pdfGrid3.Columns[5].Width = 80;

                //Add header.
                pdfGrid3.Headers.Add(1);
                PdfGridRow pdfGridHeader3 = pdfGrid3.Headers[0];
                pdfGridHeader3.Cells[0].Value = "#";
                pdfGridHeader3.Cells[1].Value = "IDArticulo";
                pdfGridHeader3.Cells[2].Value = "Nombre";
                pdfGridHeader3.Cells[3].Value = "cantidad";
                pdfGridHeader3.Cells[4].Value = "Estado";
                pdfGridHeader3.Cells[5].Value = "Bodegas";

                List<ReposicionesPendientesModel> reposiciones = new List<ReposicionesPendientesModel>();
                if (ReposicionesPendientes.Count != 0) reposiciones = ReposicionesPendientes;
                i = 1;
                foreach (var data in reposiciones)
                {
                    //Add rows.
                    PdfGridRow pdfGridRow = pdfGrid3.Rows.Add();
                    pdfGridRow.Cells[0].Value = i.ToString();
                    pdfGridRow.Cells[1].Value = data.IDArticulo.ToString();
                    pdfGridRow.Cells[2].Value = data.NombreArticulo.ToString();
                    pdfGridRow.Cells[3].Value = data.CantidadSolicitada.ToString();
                    pdfGridRow.Cells[4].Value = data.Estado.ToString();
                    pdfGridRow.Cells[5].Value = data.Bodegas.ToString();
                    i++;
                }
                PdfGridLayoutResult pdfGridLayoutResult3 = pdfGrid3.Draw(page, new Syncfusion.Drawing.PointF(70, listaPedidos.GetBounds().Bottom + 40 + paragraphAfterSpacing));



                using (MemoryStream stream = new MemoryStream())
                {
                    pdfDocument.Save(stream);
                    pdfDocument.Close();
                    return stream;
                }
            }
        }

    }
}