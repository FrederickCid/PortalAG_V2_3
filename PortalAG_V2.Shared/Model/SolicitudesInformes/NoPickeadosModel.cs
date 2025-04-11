using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.SolicitudesInformes
{
    public class NoPickeadosModel
    {
        public int IDOperacion { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public int NroPedido { get; set; }
        public string IDArticulo { get; set; }
        public string IDBodega { get; set; }
        public int UnidadPorBulto { get; set; }
        public int CantidadBultos { get; set; }
        public int Total { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Vendedor { get; set; }

    }

}

