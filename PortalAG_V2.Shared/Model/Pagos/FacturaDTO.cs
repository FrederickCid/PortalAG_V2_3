using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class FacturaDTO
    {
        public int Linea { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public string TipoOperacion { get; set; }
        public string NumeroDocumento { get; set; }
        public string FechaDocumento { get; set; }
        public double MontoDocumento { get; set; }
        public double Pagar { get; set; }
        public double Saldo { get; set; }
        public int DocEntry { get; set; }
    }
}
