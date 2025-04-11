using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agDataAccess.Models.Solicitudes
{
    public class AuditoriaPedidosModel
    {
        public string RazonSocial { get; set; }
        public int NroPedido { get; set; }
        public int NroBulto { get; set; }
        public string UbicacionBulto { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaInicioRevision { get; set; }
        public string FechaTerminoRevision { get; set; }
        public string IDUsuario { get; set; }
        public List<Detalleauditoria> DetalleAuditoria { get; set; }
        public string Estado { get; set; }

        public class Detalleauditoria
        {
            public int NroBulto { get; set; }
            public string IDArticulo { get; set; }
            public string Nombre { get; set; }
            public int UnidadxBulto { get; set; }
            public int Cantidad { get; set; }
            public string FechaActualizacion { get; set; }
            public string IDUsuario { get; set; }
        }

    }
}
