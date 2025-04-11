using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.SolicitudesInformes
{
    public class inventarioModel
    {
        public int IDUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public string IDArticulo { get; set; }
        public int UnidadxBulto { get; set; }
        public int StockAG { get; set; }
        public int StockSap { get; set; }
        public int ConteoUno { get; set; }
        public int ConteoDos { get; set; }
        public int ConteoTres { get; set; }
        public int TotalInventario { get; set; }
        public string Observacion { get; set; }

    }
    public class InventarioVizualisadorModel
    {
        public int Calle { get; set; }
        public int Tramo { get; set; }
        public string Comentario { get; set; }
        public int Estado { get; set; }
        public string UltimaActualizacion { get; set; }
        public string Lado { get; set; }
    }
}
