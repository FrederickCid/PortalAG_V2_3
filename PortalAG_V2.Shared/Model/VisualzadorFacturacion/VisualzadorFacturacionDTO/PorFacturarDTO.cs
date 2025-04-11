using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.VisualzadorFacturacion.VisualzadorFacturacionDTO
{
    public class PorFacturarDTO
    {
        public int Linea { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public string NroOpe { get; set; }
        public int NroDocumento { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public string FechaDocumento { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public int IDVendedor { get; set; }
        public string Vendedor { get; set; }
        public int TotalVenta { get; set; }
        public double PorcentajeDescuento { get; set; }
        public string Fecha { get; set; }
        public int Neto { get; set; }
        public double IvaFecha { get; set; }
        public int Iva { get; set; }
        public int Total { get; set; }
        public int TotalFactura { get; set; }
        public string FechaSolicitud { get; set; }
        public int TipoEntrega { get; set; }
        public string FechaHoraEntrega { get; set; }
        public string FechaAsignacion { get; set; }
        public int IDSacador { get; set; }
        public string Sacador { get; set; }
        public int IDTransporte { get; set; }
        public string Transporte { get; set; }
        public string FechaAutorizacion { get; set; }
        public string IDUsuarioAutorizacion { get; set; }
        public int IDEtapa { get; set; }
        public int IDEstado { get; set; }
        public string FechaTermino { get; set; }
        public string FormaPago { get; set; }
        public string CondicionPago { get; set; }
        public int Copias { get; set; }
        public int TipoDocumento { get; set; }
        public string Estado { get; set; }
        public int Bultos { get; set; }
        public int SiPedidoWeb { get; set; }
        public string EstadoFactura { get; set; }
        public int TotalFacturas { get; set; }
        public int IDTipoOperacion { get; set; }
        public int LineasPedido { get; set; }
        public string ErrorUltimoProceso { get; set; }
    }
}
