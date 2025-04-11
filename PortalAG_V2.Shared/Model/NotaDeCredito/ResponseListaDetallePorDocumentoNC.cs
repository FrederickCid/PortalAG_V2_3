using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class ResponseListaDetallePorDocumentoNC
    {
        private RequestNCDetalle request;
        private List<ProductosNCDTO> listaProductosEnviada; // Comentario

        public ResponseListaDetallePorDocumentoNC(RequestNCDetalle request, List<ProductosNCDTO> listaProductosEnviada)
        {
            this.request = request;
            this.listaProductosEnviada = listaProductosEnviada;
        }

        public RequestNCDetalle Request { get => request; set => request = value; }
        public List<ProductosNCDTO> ListaProductosEnviada { get => listaProductosEnviada; set => listaProductosEnviada = value; }
    }
}
