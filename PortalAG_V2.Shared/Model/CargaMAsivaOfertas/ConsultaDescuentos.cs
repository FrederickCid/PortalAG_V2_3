using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.CargaMAsivaOfertas
{
    public class OfertasDescuentos
    {
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        public int SiListaDescuento { get; set; }
        public string ListaDescuento { get; set; }
        public double PorcentajeDescuento { get; set; }
        public int SiWeb { get; set; }
        public string FechaInicioDescuento { get; set; }
        public string FechaTerminoDescuento { get; set; }
    }

}
