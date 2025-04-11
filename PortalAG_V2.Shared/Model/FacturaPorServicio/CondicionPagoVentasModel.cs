using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.FacturaPorServicio
{
    public class TuplaFS1
    {
        public List<CondicionPagoVentasModel> item1 { get; set; }
    }

    public class CondicionPagoVentasModel
    {
        public int idCondicionPago { get; set; }
        public string? descripcion { get; set; }
        public string? texto { get; set; }
    }
}
