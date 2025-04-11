using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.VisualzadorFacturacion.VisualzadorFacturacionDTO
{
    public class DetalleVentaDTO
    {
        public int Correlativo { get; set; }
        public int Linea { get; set; }
        public string IDArticulo { get; set; }
        public int NroDocumento { get; set; }
        public int DocEntry { get; set; }
        public int SiPedidoWeb { get; set; }
        public int PrecioCosto { get; set; }
        public int PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public int StockActualBVN { get; set; }
        public int StockActualBPM { get; set; }
        public int ConStockDisponibleVentas { get; set; }
        public string IDEstado { get; set; }
        public string EstadoLinea { get; set; }
        public string Color { get; set; }
    }
}
