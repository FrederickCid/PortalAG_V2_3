using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Direcciones
{
    public class ComunaDTO
    {
        public int idComuna { get; set; }
        public string comuna { get; set; }
        public int idCiudad { get; set; }
        public int idProvincia { get; set; }
        public string provincia { get; set; }
        public int idRegion { get; set; }
    }

    public class ItemComunaDTO
    {
        public List<ComunaDTO> Item1 { get; set; }
    }
}
