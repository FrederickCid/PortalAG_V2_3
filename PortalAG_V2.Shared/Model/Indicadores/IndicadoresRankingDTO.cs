using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Indicadores
{
    public class IndicadoresRankingDTO
    {

        public List<lineasSacadorDTO> actual { get; set; }
        public List<lineasSacadorDTO> m3 { get; set; }
        public List<lineasSacadorDTO> m2 { get; set; }
        public List<lineasSacadorDTO> m1 { get; set; }
        

        public class lineasSacadorDTO
        {
            public int idSacador { get; set; }
            public string sacador { get; set; }
            public int bV_BVN { get; set; }
            public int bV_BIT { get; set; }
            public int bV_BPM { get; set; }
            public int packing { get; set; }
            public int devolucion { get; set; }
            public int reposicion { get; set; }
        }

        
    }
}
