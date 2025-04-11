using Microsoft.AspNetCore.Components;

namespace PortalAG_V2.Componentes.EstadoPedido
{
    public partial class CardEstadoPedidos
    {
        [Parameter]
        public string titulo { get; set; }
        [Parameter]
        public string inicio { get; set; }
        [Parameter]
        public string termino { get; set; }
        [Parameter]
        public string responsable { get; set; }
        [Parameter]
        public double sla { get; set; }
    }
}
