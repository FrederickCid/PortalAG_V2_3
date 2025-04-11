using agDataAccess.Models;
using Microsoft.AspNetCore.Components;

namespace PortalAG_V2.Componentes.EstadoPedido
{
    public partial class TextComponent
    {
        [Parameter]
        public int IDEtapaPedido { get; set; }
        [Parameter]
        public int IDEstadoPedido { get; set; }
        [Parameter]
        public int IDEtapaOriginalPedido { get; set; }
        [Parameter]
        public int IDEstadoOriginalPedido { get; set; }
        [Parameter]
        public EstadoPedidosNoMOD estadoPedido { get; set; }
    }
}
