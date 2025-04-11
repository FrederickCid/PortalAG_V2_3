using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.EstadoPedidos
{
    public class EstadoPedidoDespachoImgModels
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int CorrelativoImagen { get; set; }
        public int IDTipoImagen { get; set; }
        public string NombreImagen { get; set; }
        public int IDEstado { get; set; }
        public string IDUsuario { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaDelete { get; set; }
        public string Msg { get; set; }
        public string MsgResult { get; set; }
    }
}
