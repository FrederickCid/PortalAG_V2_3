using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Direcciones
{
    public class ResponseActualizaDireccionDTO
    {
        public int IDOperacion { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Region { get; set; }
        public string Ciudad { get; set; }
        public string Comuna { get; set; }
        public string EstadoPedido { get; set; }
    }
}
