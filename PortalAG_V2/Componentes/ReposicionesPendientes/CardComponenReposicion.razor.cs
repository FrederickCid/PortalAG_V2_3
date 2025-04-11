using Microsoft.AspNetCore.Components;

namespace PortalAG_V2.Componentes.ReposicionesPendientes
{
    public partial class CardComponenReposicion
    {
        [Parameter]
        public string IDArticulo { get; set; }
        [Parameter]
        public string Nombre { get; set; }
        [Parameter]
        public string Estado { get; set; }
        [Parameter]
        public string Hasta { get; set; }
        [Parameter]
        public string Cantidad { get; set; }
    }
}
