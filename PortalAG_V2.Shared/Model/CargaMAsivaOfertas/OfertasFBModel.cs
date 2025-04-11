using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.CargaMAsivaOfertas
{
    public class OfertaFBModel
    {
        public int IDAllgestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public string IDCliente { get; set; }
        public string IDArticulo { get; set; }
        public int PrecioNormal { get; set; }
        public string Nombre { get; set; }
        public int PrecioCliente { get; set; }
        public int IDEstado { get; set; }
        public string IDUsuario { get; set; }

    }
    public class OfertaFBModelExcel
    {
        public int IDAllgestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public string IDCliente { get; set; }
        public string IDArticulo { get; set; }
        public int PrecioNormal { get; set; }
        public string Nombre { get; set; }
        public int PrecioCliente { get; set; }
        public int IDEstado { get; set; }
        public string IDUsuario { get; set; }
        public string Alerta { get; set; }

    }
    public class OfertaFBGETModel
    {
        public int IDAllgestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public string IDCliente { get; set; }
        public string IDArticulo { get; set; }
        public int PrecioVentaNormal { get; set; }
        public string Nombre { get; set; }
        public int PrecioVenta { get; set; }
        public int IDEstado { get; set; }
        public string IDUsuario { get; set; }

    }
}
