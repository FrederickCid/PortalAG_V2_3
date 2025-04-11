using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    

    public class ModeloProcesarNCsap
    {
        public List<RespuestaProcesarNCsap> Respuesta { get; set; }
    }

    public class RespuestaProcesarNCsap
    {
        public string msgResult { get; set; }
        public string msgMessage { get; set; }
        public string msgValor { get; set; }
    }



}
