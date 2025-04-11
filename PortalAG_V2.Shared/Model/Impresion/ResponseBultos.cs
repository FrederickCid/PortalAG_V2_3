using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Impresion
{
    public class ResponseBultos
    {
        public string IDArticulo { get; set; }
        public int Correlativo { get; set; }
        public int CorrelativoEnBodega { get; set; }
        public int CantidadPorBulto { get; set; }
        public int Bultos { get; set; }
        public string FechaActualizacion { get; set; }
        public string IDUsuario { get; set; }
    }
}
