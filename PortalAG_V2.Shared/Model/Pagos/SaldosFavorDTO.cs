using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class SaldosFavorDTO
    {
        public int linea { get; set; }
        public int annoProceso { get; set; }
        public int idOperacion { get; set; }
        public int balDueDeb { get; set; }
        public int balDueCred { get; set; }
        public int debe { get; set; }
        public int haber { get; set; }
        public int tipoDocumento { get; set; }
        public int nroDocumentoSaldo { get; set; }
        public string fechaDocumentoSaldo { get; set; }
        public int montoDocumentoSaldo { get; set; }
        public int pagarSaldo { get; set; }
        public int saldo { get; set; }
        public bool check { get; set; }
    }
}
