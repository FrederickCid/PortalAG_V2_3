using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.PermisosAPPDTO
{
    public class PermisosAPPModel
    {
        public string msgError { get; set; }
        public string msgResult { get; set; }
    }

    public class ConsultarIDAPPModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
    }


    public class ConsultarPermisosUsuario
    {
        public int IDAplicacion { get; set; }
        public string NombreAPP { get; set; }
        public int IDSubAplicacion { get; set; }
        public string NombreSubAPP { get; set; }
        public string IDEstado { get; set; }
    }

    public class MostrarNombresModel
    {
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string IDUsuario { get; set; }
    }

    public class MostrarAccesosDTO
    {

        public int IDModulo { get; set; }
        public int IDSubModulo { get; set; }
        public int IDPantalla { get; set; }
        public int IDSubPantalla { get; set; }
        public int IDAcceso { get; set; }
        public int IDSubAcceso { get; set; }
        public string DescripcionHelp { get; set; }
    }
}
