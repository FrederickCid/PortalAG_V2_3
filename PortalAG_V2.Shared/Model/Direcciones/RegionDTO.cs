using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Direcciones
{
    public class RegionDTO
    {
        public int idRegion { get; set; }
        public string region { get; set; }
        public string codigo { get; set; }
        public string fechaActualizacion { get; set; }
        public char idUsuario { get; set; } 
    }

    public class ItemRegionDTO
    {
        public List<RegionDTO> Item1 { get; set; }
    }
}
