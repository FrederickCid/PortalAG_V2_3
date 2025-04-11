using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Direcciones
{
    public class DireccionesDTO
    {

            public string idCliente { get; set; }
            public int linea { get; set; }
            public string idDireccion { get; set; }
            public string tipoDireccion { get; set; }
            public string direccion { get; set; }
            public string nroDireccion { get; set; }
            public string codigoPostal { get; set; }
            public int idRegion { get; set; }
            public int idComuna { get; set; }
            public string comuna { get; set; }
            public int idCiudad { get; set; }
            public string ciudad { get; set; }
            public string localidad { get; set; }
            public int siPorDefecto { get; set; }
            public int siEnvioSAP { get; set; }
            public DateTime fechaActualizacion { get; set; }
            public string idUsuario { get; set; }
        

    }
}
