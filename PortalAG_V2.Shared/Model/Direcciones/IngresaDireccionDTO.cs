using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Direcciones
{
    public class IngresaDireccionDTO
    {
        public string IDCliente { get; set; }
        public int Linea { get; set; }
        public string IDDireccion { get; set; }
        public string Direccion { get; set; }
        public string NroDireccion { get; set; }
        public string CodigoPostal { get; set; }
        public string IDRegion { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
        public string Localidad { get; set; }
        public string IDUsuario { get; set; }
    }
}
