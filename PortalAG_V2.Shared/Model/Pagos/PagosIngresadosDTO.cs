using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class PagosIngresadosDTO
    {
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public int TotalPago { get; set; }
        public EfectivoPago Efectivo { get; set; }
        public List<TransferenciaDTO> Transferencias { get; set; }
        public List<TransferenciaDTO> Depositos { get; set; }
        public TarjetasPago Tarjeta { get; set; }
        public List<ChequeDTO> Cheques { get; set; }

    }

    public class EfectivoPago
    {
        public string FechaEfectivo { get; set; }
        public int MontoEfectivo { get; set;}
    }

    public class TarjetasPago
    {
        public TarjetasDTO tarjetas { get; set; }
        public int NroTarjeta { get; set; }
        public DateTime FechaValidoHasta { get; set; }
        public int NroID { get; set; }
        public string NroTelefono { get; set; }
        public int ImporteVencido { get; set; }
        public int CantidadPagos { get; set; }
        public int PrimerPago { get; set; }
        public int PagoAdicional { get; set; }
        public string Certificado { get; set; }
        public string ClaseOperacionSelect { get; set; }
    }
}
