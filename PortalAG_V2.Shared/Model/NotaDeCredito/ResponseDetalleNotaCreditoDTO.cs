using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class ResponseDetalleNotaCreditoDTO
    {
        public DetalleClienteNCDTO Cabecera { get; set; }
        public ProductoNCDTODevolver Detalle { get; set; }
    }
}
