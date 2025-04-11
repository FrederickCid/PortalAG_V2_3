using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.CargaMAsivaOfertas
{
    public class OfertaResultModel
    {
        public string IDArticulo { get; set; }
        public int TipoOferta { get; set; }
        public string Result { get; set; }
        public int IDEstado { get; set; }
    }
    public class validarArticuloRepetidoModel
    {
        public string IDArticulo { get; set; }
        public int Cantidad { get; set; }
       
    }
}
