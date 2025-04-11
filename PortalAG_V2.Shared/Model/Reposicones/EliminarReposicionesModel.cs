using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Reposicones
{
    public class EliminarReposicionesModel
    {

            public int idSolicitud { get; set; }
            public int idOperacion { get; set; }
            public int nroPallet { get; set; }
            public string idArticulo { get; set; }
            public int unidadPorBulto { get; set; }
            public int cantidad { get; set; }
            public string desde { get; set; }
            public string hasta { get; set; }
            public string fechaActualizacion { get; set; }
        

    }
}
