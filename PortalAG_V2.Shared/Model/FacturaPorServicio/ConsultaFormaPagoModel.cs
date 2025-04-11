using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.FacturaPorServicio
{
    public class TuplaFS
    {
        public List<ConsultaFormaPagoModel> item1 { get; set; }
    }

    public class ConsultaFormaPagoModel
    {
        public int idFormaPago { get; set; }
        public string? descripcion { get; set; }
        public string? texto { get; set; }
    }
}
