using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.AvisoDePago
{
    public class BancoModel
    {
        public int IDBanco { get; set; }
        public string Descripcion { get; set; }
    }
    public class BancoAndesModel
    {
        public int IDBanco { get; set; }
        public string Descripcion { get; set; }
        public string CuentaCorriete { get; set; }
        public int CuentaContable { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string Fono { get; set; }
    }
}
