using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class FormaDePagoDTO
    {
        public int IdFormaPago { get; set; }
        public string? Descripcion { get; set; }
        public string? Texto { get; set; }
    }
}
