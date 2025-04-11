using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.CargaMAsivaOfertas
{
    public class OfertaBultoModel
    {
        [Key]
        public string IDArticulo { get; set; }
        public int CantidadOfeta { get; set; }
        public int PrecioOferta { get; set; }
        public int SiActivo { get; set; } // 0 borrar descuento

    }
    public class OfertaBultoModelExecelModel
    {
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        public int CantidadOfeta { get; set; }
        public int PrecioOferta { get; set; }
        public int SiActivo { get; set; } // 0 borrar descuento
        public string Alerta { get; set; }

    }



}
