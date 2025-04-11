using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.AsientoContable
{
    public class AsientosContablesModel
    {
        public string fecha { get; set; }
        public string fechaContable { get; set; }
        public string fechaVencimiento { get; set; }
        public string Comentario { get; set; }
        public string Codigo { get; set; }
        public string Referencia { get; set; }
        public string Referencia2 { get; set; }
        public string Referencia3 { get; set; }
        public ArchivoAsiento file { get; set; }

        public class ArchivoAsiento
        {
            public string Stream { get; set; }
            public string FileInfo { get; set; }
        }
        public List<DetalleAsientos> Detalle { get; set; }

        public class DetalleAsientos {
            public int Id { get; set; }
            public string Cuenta { get; set; }
            public string Cliente { get; set; }

            public int SiCuentaCliente { get; set; }
            public string nombreCuenta { get; set; }

            public long Debito { get; set; }
            public long Credito { get; set; }
            public string Comentario { get; set; }
            public string Referencia1 { get; set; }
            public string Referencia2 { get; set; }
        }
    }
}
