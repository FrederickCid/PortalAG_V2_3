using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.CargaMAsivaOfertas
{
    public class OfertasBultosModel
    {
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        public int CorrelativoOferta { get; set; }
        public int BultoOferta { get; set; }
        public int PrecioOferta { get; set; }
        public string FechaActualizacion { get; set; }
        public string IDUsuario { get; set; }
    }
}
