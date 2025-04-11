using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.ChartsModel
{
    public class LineasPendientesModel
    {
        public string Bodega { get; set; }
        public int TotalPicking { get; set; }
        public int TotalRojoPicking { get; set; }
        public int TotalVerdePicking { get; set; }
    }
    public class LineasPendientesModelDTO
    {
        public string label { get; set; }
        public int Cantidad { get; set; }
    }
}
