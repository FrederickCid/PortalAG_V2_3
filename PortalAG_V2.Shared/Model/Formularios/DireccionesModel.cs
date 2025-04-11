using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Formularios
{
    public class DireccionesModel
    {

        public int Linea { get; set; }
        public string IDDireccion { get; set; }
        public string TipoDireccion { get; set; }
        public string Direccion { get; set; }
        public string NroDireccion { get; set; }
        public int IDRegion { get; set; }
        public int IDComuna { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
      
    }
}
