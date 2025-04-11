using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullBikePos.Shared.Models.HojaDeRuta
{
    public class CambioTransporteModel
    {
        public class RequestTransporteModel
        {
            public int IDGuiaHojaruta { get; set; }
            public string Transporte { get; set; }
        }
        public class ResponseTransporteModel
        {
            public string msgResult { get; set; }
            public int IDGuiaHojaruta { get; set; }
            public string Transporte { get; set; }
            public string msgMensaje { get; set; }

        }

    }
}
