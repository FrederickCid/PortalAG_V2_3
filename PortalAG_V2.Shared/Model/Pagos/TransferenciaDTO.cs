using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class TransferenciaDTO
    {
        public int lineas;
        public DateTime fecha;
        public int numeroCuenta;
        public double montoTarjeta;
        public int numeroComprobante;
        public int idBanco;
        public string banco;
        public int idBancoAndes;
        public string bancoAndes;
        public string cuentaAndes;
    }
}
