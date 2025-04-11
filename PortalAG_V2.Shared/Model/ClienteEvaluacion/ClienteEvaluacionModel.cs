using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheriffDataAccess.Models.SheriffModel
{
    public class ClienteEvaluacionModel
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public string IDCliente { get; set; }
        public int DICOM { get; set; }
        public string RazonSocial { get; set; }
        public string RiesgoDICOM { get; set; }
        public long MontoCompraAnual     { get; set; }
        public int MontoCompraMensual { get; set; }
        public int MontoCompraPromedio { get; set; }
        public int AntiguedadClienteMeses { get; set; }
        public int NCAnual { get; set; }
        public string FechaUltimaCompra { get; set; }
        public string UltimoTipoEntrega { get; set; }
        public string Zona { get; set; }
        public string Vendedor { get; set; }
        public int CantidadFacturasMensual { get; set; }
        public int CantidadPedidosWeb { get; set; }
        public int MontoPedidosWeb { get; set; }
        public int CompraGCBicicletas { get; set; }
        public int CompraGCRepuestosImp { get; set; }
        public int CompraGCRepuestosMotos { get; set; }
        public int CompraGCPro { get; set; }
        public int CompraGCShimano { get; set; }
        public int CompraGCMotos { get; set; }
        public int CompraGCDisney { get; set; }
        public int CompraGCMaquinas { get; set; }
        public int CompraMarcaElite { get; set; }
        public int CompraMarcaLazer { get; set; }
        public int CompraMarcaScott { get; set; }
        public int CompraMarcaMerida { get; set; }
        public int CompraMarcaPro { get; set; }
        public int CompraMarcaShimano { get; set; }
        public int CompraMarcaBest { get; set; }
        public int CompraMarcaContinental { get; set; }
        public int CompraMarcaVittoria { get; set; }
        public string ClienteContraFactura { get; set; }
        public string UltimaFormaPago { get; set; }
        public string CorreoCliente { get; set; }
        public string CelularCliente { get; set; }
        public string DireccionCliente { get; set; }
        public int Rating { get; set; }
        public int RatingMensual { get; set; }
        public int RatingWeb { get; set; }
        public int RatingGCBicicletas { get; set; }
        public int RatingGCRepuestosMotos { get; set; }
        public int RatingGCPro { get; set; }
        public int RatingGCShimano { get; set; }
        public int RatingGCDisney { get; set; }
        public int RatingGCMaquinas { get; set; }
        public int RatinMarcaElite { get; set; }
        public int RatingMarcaLazer { get; set; }
        public int RatingMarcaScott { get; set; }
        public int RatingMarcaMerida { get; set; }
        public int RatingMarcaPro { get; set; }
        public int RatingMarcaShimano { get; set; }
        public int RatingMarcaBest { get; set; }
        public int MarcaContinental { get; set; }
        public int RatingMarcaVittoria { get; set; }

    }
}
