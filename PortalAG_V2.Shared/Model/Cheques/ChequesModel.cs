using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Models.Cheques
{
    public class ChequesModel
    {
        public int IDOperacion { get; set; }
        public int AnnoProceso { get; set; }
        public string FechaCancelacion { get; set; }
        public string NroCtaCteBanco { get; set; }
        public string NroComprobante { get; set; }
        public string NumeroSerie { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public int Monto { get; set; }
        public string FechaVencimiento { get; set; }
        public int IDBanco { get; set; }
        public string Banco { get; set; }
        public int DocEntry { get; set; }
        public int Si_Created { get; set; }
        public int Si_Sent { get; set; }
        public int Si_Deposited { get; set; }
        public int Si_Paid { get; set; }
        public int Si_Cancelled { get; set; }
        public int Si_Closed { get; set; }
        public int Si_Failed { get; set; }
        public string IDUsuario { get; set; }
    }
    public class ChequeExcelModel
    {
        public string NroCtaCteBanco { get; set; }
        public string NroComprobante { get; set; }
        public string NumeroSerie { get; set; }
        public int Monto { get; set; }
        public string IDCliente { get; set; }
        public string Alerta { get; set; }
    }
}
