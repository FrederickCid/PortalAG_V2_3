using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Requests.SolicitudMovimiento
{
    public class UbicacionArticulo
    {
        public string IDArticulo { get; set; }
        public int NroPallet { get; set; }
        public int IDUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public int Cantidad { get; set; }
    }
}
