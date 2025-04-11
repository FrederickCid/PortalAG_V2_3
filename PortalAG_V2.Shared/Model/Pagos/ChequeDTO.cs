using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class ChequeDTO
    {
        public int numeroCheuqe { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public double importe { get; set; }
        public string sucursal { get; set; }
        public string cuenta { get; set; }
        public int idBanco { get; set; }
        public string banco { get; set; }
    }
}
