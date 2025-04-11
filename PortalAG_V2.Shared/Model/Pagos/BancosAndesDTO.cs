using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class BancosAndesDTO
    {

            public int idBanco { get; set; }
            public string descripcion { get; set; }
            public string cuentaCorriete { get; set; }
            public int cuentaContable { get; set; }
            public object direccion { get; set; }
            public object contacto { get; set; }
            public object fono { get; set; }
        

    }
}
