using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.CargaMAsivaOfertas
{
    public class OfertasListadoModel
    {
        public string IDArticulo { get; set; }
        public int SiOutlet { get; set; }
        public int SoloAplicaWEB { get; set; }
        public double Porcentaje { get; set; }
        public int ListaDescuneto { get; set; }
        public int SiActivo { get; set; }
        public int SiAplicaWEB { get; set; } // 0 no aplica >> fecha inicio termino null 
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
    }
    public class OfertasListadoModelExcel
    {
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        public int SiOutlet { get; set; }
        public int SoloAplicaWEB { get; set; }
        public double Porcentaje { get; set; }
        public int ListaDescuneto { get; set; }
        public int SiActivo { get; set; }
        public int SiAplicaWEB { get; set; } // 0 no aplica >> fecha inicio termino null 
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public string Alerta { get; set; }

    }
}
