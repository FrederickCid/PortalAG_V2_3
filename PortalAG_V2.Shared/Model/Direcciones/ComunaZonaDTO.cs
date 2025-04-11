using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Direcciones
{
    public class ComunaZonaDTO
    {
        public int IDZona { get; set; }
        public string Zona { get; set; }
        public int IDRegion { get; set; }
        public string Region { get; set; }
        public string Codigo { get; set; }
        public int IDProvincia { get; set; }
        public string Provincia { get; set; }
        public int IDComuna { get; set; }
        public string Comuna { get; set; }
    }
}
