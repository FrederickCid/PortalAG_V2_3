using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Requests.SolicitudMovimiento
{
    public class BodegasOperacionesResponse
    {
        public List<BodegasModel> Bodegas { get; set; }
        public List<TipoOperacionModel> Operacion { get; set; }
    }
    public class BodegasModel { public int IDBodega { get; set; } public string SiglaBodega { get; set; } }
    public class TipoOperacionModel { public int IDTipoOperacion { get; set; } public string Descripcion { get; set; } }
}
