using Microsoft.AspNetCore.Components;
using System.Security;

namespace PortalAG_V2.Componentes.EstadoPedido
{
    public partial class CardTiempoEspera
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