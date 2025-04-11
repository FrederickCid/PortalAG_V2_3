using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.AutorizarPedidos;

public class PedidoModel
{
    public int AnnoProceso { get; set; }
    public int IDOperacion { get; set; }
    public int Correlativo { get; set; }
    public string IDCliente { get; set; }
    public DateTime Fecha { get; set; }
    public string RazonSocial { get; set; }
    public int Neto { get; set; }
    public double IVAFecha { get; set; }
    public int IVA { get; set; }
    public int Total { get; set; }
    public int TotalFactura { get; set; }
    public string FechaDocumento { get; set; }
    public string Direccion { get; set; }
    public string Comuna { get; set; }
    public string Ciudad { get; set; }
    public int IDVendedor { get; set; }
    public string Vendedor { get; set; }
    public int NroDocumento { get; set; }
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
    public string IDUsuario { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public string FormaPago { get; set; }
    public string CondicionPago { get; set; }
    public string Comentarios { get; set; }
    public int Copias { get; set; }
    public int TipoDocumento { get; set; }
    public int SiAgregadoDespacho { get; set; }
    public int SiAdjunto { get; set; }
    public string NombreAdjunto { get; set; }
    public string Icon { get; set; }
    public string ToolTip { get; set; }
    public string Estado { get; set; }
    public int Bultos { get; set; }
    public int BultosOrigen { get; set; }
    public int SiPedidoWeb { get; set; }
    public string ComentariosSacador { get; set; }
    public string NombreVendedor { get; set; }
    public int TotalBultos { get; set; }
    public int BultoActual { get; set; }
    public object FechaTerminoRevision { get; set; }
    public string EstadoFactura { get; set; }
    public int TotalFacturas { get; set; }
    public int IDTipoOperacion { get; set; }
    public string msgResult { get; set; }
    public int LineasPedido { get; set; }
    public string FechaHoraEntregaOriginal { get; set; }
    public int SiPausado { get; set; }
    public double PorcentajeDescuento { get; set; }
}

public class RequestPedidoModel
{
    public int AnnoProceso { get; set; }
    public int IDOperacion { get; set; }
    public int Correlativo { get; set; }
    public int IDEtapa { get; set; }
    public int IDEstado { get; set; }
    public string IDUsuario { get; set; }
}

public class ResponsePedidoModel
{
    public string msgResult { get; set; }
    public string msgError { get; set; }

}