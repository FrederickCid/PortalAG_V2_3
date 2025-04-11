using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Indicadores
{
    public class IndicadoresDTO
    {

        public string bodega { get; set; }
        public int nroPedido { get; set; }
        public int nroPedidoRojo { get; set; }
        public int totalPicking { get; set; }
        public int totalRojoPicking { get; set; }
        public int totalVerdePicking { get; set; }
        

    }
}
