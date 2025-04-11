using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.SolicitudesInformes
{
    public class ConsultaLiberados
    {
        public int NroPedido { get; set; } = 0;
        public int IDOperacion { get; set; } = 0;
        public string FechaInicio { get; set; } = "";
        public string FechaFin { get; set; } = "";
        public string IDCliente { get; set; } = "";
        public string IDUsuario { get; set; } 

    }

    public class ConsultaLiberadosLista
    {
        public int idOperacion { get; set; } 
        public int nroDocumento { get; set; } 
        public string idCliente { get; set; } 
        public string razonSocial { get; set; }
        public string idUsuarioAutorizaLiberacion { get; set; } 
        public string fechaAutorizaLiberacion { get; set; } 
        public string TipoEntrega { get; set; } 
        public string CondicionPago { get; set; } 
        public string Vendedor { get; set; }       
        public string estado { get; set; } 

    }
}
