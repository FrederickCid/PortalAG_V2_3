using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class RequestNCDetalle
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Correlativo { get; set; }
        public int IDTipoOperacion { get; set; }

    }
}
