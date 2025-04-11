using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.CargaMAsivaOfertas
{
    public class OfertasWebModel
    {
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        public int Correlativo { get; set; }
        public double PorcentajeDescuentoWeb { get; set; }
        public string FechaInicioDescuento { get; set; }
        public string FechaTerminoDescuento { get; set; }
        public int SiActivo { get; set; }
        public int Orden { get; set; }
        public string IDUsuario { get; set; }
        public string FechaActualizacion { get; set; }
    }
}
