using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Impresion
{
      public class ConsultarBodegasModel
    {
        public int IDBodega{get;set;}
        public string SiglaBodega { get; set; }
    }
    public class ConsultarSectorModel
    {
        public int IDSector { get; set; }
        public string Sector { get; set; }
    }
    public class ConsultarCalleModel
    {
        public int IDCalle { get; set; }
        public string Calle { get;set; }
    }          
    public class ConsultarTramoModel
    {
        public int idTramo { get; set; }
        public string Tramo { get; set; }
    }
    public class ConsultarNivelModel
    {
        public int IDNivel { get; set; }
        public string Nivel{get;set;}
    }
    public class ConsultarPosicionModel
    {
        public int idPosicion { get; set; }
        public string Posicion { get; set; }
    }
}
