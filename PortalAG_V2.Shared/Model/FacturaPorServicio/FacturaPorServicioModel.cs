using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.FacturaPorServicio
{
    #region Cabecera
    public class ResponseCabeceraFS
    {
        public string msgError { get; set; }
        public string msgMensaje { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Correlativo { get; set; }
        public int Folio { get; set; }
        public int NroDocumento { get; set; }
    }

    public class RequestCabeceraFS
    {
        public int AnnoProceso { get; set; }
        public int Correlativo { get; set; }
        public int IDTipoOperacion { get; set; }
        public string Fecha { get; set; }
        public int IDEstado { get; set; }
        public string IDCliente { get; set; }
        public double Neto { get; set; }
        public int IVAFecha { get; set; }
        public double IVA { get; set; }
        public double Total { get; set; }
        public double TotalFactura { get; set; }
        public string FechaDocumento { get; set; }
        public int IDFormaPago { get; set; }
        public string FormaPago { get; set; }
        public int IDVendedor { get; set; }
        public string Vendedor { get; set; }
        public int IDTransporte { get; set; }
        public string Transporte { get; set; }
        public int CantidadFormaPago { get; set; }
        public string IDUsuario { get; set; }
        public int SiEmbalaje { get; set; }
        public string FechaVencimiento { get; set; }
        public string Observacion { get; set; }
        public int IDFacturador { get; set; }
        public string Facturador { get; set; }
        public int IDCondicionPago { get; set; }
        public string CondicionPago { get; set; }
        public string Comentarios { get; set; }
        public string NumeroOC { get; set; }
        public string Localidad { get; set; }
    }
    #endregion

    #region Detalle
    public class ResponseDetalleFS
    {
        public string msgError { get; set; }
        public string msgMensaje { get; set; }
    }

    public class RequestDetalleFS
    {
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int IDTipoOperacion { get; set; }
        public string Fecha { get; set; }
        public int CorrelativoArticulo { get; set; }
        public string Nombre { get; set; }
        public int CorrelativoBulto { get; set; }
        public int Bulto { get; set; }
        public int CantidadBulto { get; set; }
        public int Cantidad { get; set; }
        public string IDUsuario { get; set; }
        public string Nota { get; set; }
    }
    #endregion

    #region Consulta factura
    public class BusquedaNroFactura
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int Correlativo { get; set; }
        public int IDTipoOperacion { get; set; }
        public string Fecha { get; set; }
        public string FechaVencimiento { get; set; }
        public int IDEstado { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public int NroDocumento { get; set; }
        public int DocEntry { get; set; }
        public string Direccion { get; set; }
        public int IDregion { get; set; }
        public string Region { get; set; }
        public int IDComuna { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
        public string Giro { get; set; }
        public int TotalVenta { get; set; }
        public int PorcentajeDescuento { get; set; }
        public int Descuentos { get; set; }
        public int Interes { get; set; }
        public int OtrosValores { get; set; }
        public int Neto { get; set; }
        public int IVAFecha { get; set; }
        public int IVA { get; set; }
        public int Total { get; set; }
        public int TotalFactura { get; set; }
        public int SaldoFactura { get; set; }
        public int IDFormaPago { get; set; }
        public string FormaPago { get; set; }
        public int IDCondicionPago { get; set; }
        public string CondicionPago { get; set; }
        public string NumeroOC { get; set; }
        public string Localidad { get; set; }
        public string Vendedor { get; set; }
        public List<BusquedaNroFacturaDetalle> Detalle { get; set; }
    }

    public class BusquedaNroFacturaDetalle
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Correlativo { get; set; }
        public int Linea { get; set; }
        public string Fecha { get; set; }
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        public int CorrelativoOferta { get; set; }
        public int BultoOferta { get; set; }
        public int CantidadOferta { get; set; }
        public int CorrelativoBulto { get; set; }
        public int Bulto { get; set; }
        public int CantidadBulto { get; set; }
        public int Cantidad { get; set; }
        public int CantidadBV_BPM { get; set; }
        public int CantidadBV_BIT { get; set; }
        public int CantidadBV_BVN { get; set; }
        public int PrecioVenta { get; set; }
        public int IDColor { get; set; }
        public string DescripcionColor { get; set; }
        public int StockAlComprar { get; set; }
        public string FechaActualizacion { get; set; }
        public string IDUsuario { get; set; }
        public int IDUbicacion { get; set; }
        public string DescripcionUbicacion { get; set; }
        public int StockAlComprarMayor { get; set; }
        public int StockAlAnular { get; set; }
        public int StockAlAnularMayor { get; set; }
        public int SiVentaSet { get; set; }
        public double PorcDescuento { get; set; }
        public int PrecioDescuento { get; set; }
        public int IDFamilia { get; set; }
        public int GrupoDescuento { get; set; }
        public string ModeloVIN { get; set; }
        public string NumeroMotor { get; set; }
        public string NumeroVIN { get; set; }
        public string ColorVIN { get; set; }
        public int IDEstadoVIN { get; set; }

    }
    #endregion

    #region SAP
    public class ActualizarDocEntryDTO
    {
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Correlativo { get; set; }
        public int IDTipoOperacion { get; set; }
        public int NroFactura { get; set; }
        public int DocEntry { get; set; }
        public int SiError { get; set; }
        public string Error { get; set; }
        public string IDUsuario { get; set; }
    }

    public partial class Respuestas
    {
        [JsonProperty("Respuesta")]
        public List<Respuesta> Respuesta { get; set; }
    }

    public partial class Respuesta
    {
        [JsonProperty("msgResult")]
        public string MsgResult { get; set; }

        [JsonProperty("msgMessage")]
        public string MsgMessage { get; set; }
    }
    #endregion
}
