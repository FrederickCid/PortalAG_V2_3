using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.StockCyber
{
    public class StockCyberModel
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int StockBV_BPM { get; set; }
        public int StockBV_BIT { get; set; }
        public int StockBV_BVN { get; set; }
        public int StockSE_BPM { get; set; }
        public int StockSE_BVN { get; set; }
        public int Stock_Disponible { get; set; }
        public int Stock_EnPedido { get; set; }
    }
}
