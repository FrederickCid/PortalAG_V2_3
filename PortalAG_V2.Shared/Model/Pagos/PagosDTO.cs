using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class PagosDTO
    {
        public int annoProceso { get; set; }
        public int iDOperacion { get; set; }
        public int numeroCobranza { get; set; }
        public string iDCliente { get; set; }
        public string razonSocial { get; set; }
        public string fechaPago { get; set; }
        public double valorCobranza { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public int cantidadPedidos { get; set; }
        public double totalPedidos { get; set; }
        public double notasCredio { get; set; }
        public double notaDebito { get; set; }
        public double saldosFavor { get; set; }
        public double saldoEnContra { get; set; }
        public double totalApagar { get; set; }
        public double efectivo { get; set; }
        public double tarjetaCredito { get; set; }
        public double tarjetaDebito { get; set; }
        public double transferencia { get; set; }
        public double deposito { get; set; }
        public double cheque { get; set; }
        public double letra { get; set; }
        public double pagoAnticipado { get; set; }
        public string idUsuario { get; set; }
        public List<PagosDTO> lista { get;set;}
        public List<PagosDTO> listadoPagos { get; set; }
    }
}


