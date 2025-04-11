using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.SolicitudesInformes.InformeFacturaDespacho
{
    public class informeFacturaDespachoModel
    {
        public int NroPedido { get; set; }
        public int IDOperacion { get; set; }
        public string NroDocumetos { get; set; }
        public string IDCliente { get; set; }
        public string Docdate { get; set; }
        public int IDvendedorFactura { get; set; }
        public string VendedorFactura { get; set; }
        public int IDVendedorAsignado { get; set; }
        public string VendedorAsignado { get; set; }
        public int tipo { get; set; }
        public int NetoPedido { get; set; }
        public int totalPedido { get; set; }
        public string Estado { get; set; }

    }
}
