using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class MontosNCDTO
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Subtotal { get; set; }
        public int Neto { get; set; }
        public int PorcentajeDesc { get; set; }
        public int Descto { get; set; }
        public int IVA { get; set; }
        public int Total { get; set; }
        public string Nota { get; set; }
    }
}
