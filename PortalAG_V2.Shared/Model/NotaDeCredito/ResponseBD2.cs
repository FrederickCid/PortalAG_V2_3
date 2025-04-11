using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class ResponseBD2
    {
        public string msgResult { get; set; }
        public string msgMensaje { get; set; }
        public int IDOperacion { get; set; }
        public int UltimoNroDocumentoNC { get; set; }
    }
}
