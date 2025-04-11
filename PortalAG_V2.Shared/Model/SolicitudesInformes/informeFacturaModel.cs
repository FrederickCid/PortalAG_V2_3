using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.SolicitudesInformes
{
    public class informeFacturaModel
    {
        public int IDOperacion { get; set; }
        public int NroPedido { get; set; }
        public string IDCliente { get; set; }
        public string Cliente { get; set; }
        public string NroTelefono { get; set; }
        public string NroCelular { get; set; }
        public int NroFacturas { get; set; }
        public string Facturas { get; set; }
        public string FormaPago { get; set; }
        public string CondicionPago { get; set; }
        public string FechaFacturacion { get; set; }
        public string Estado { get; set; }
        public int AvisoPago { get; set; }
    }
}
