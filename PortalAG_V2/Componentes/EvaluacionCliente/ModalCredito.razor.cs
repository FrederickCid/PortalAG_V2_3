using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PortalAG_V2.Componentes.EvaluacionCliente
{
    public partial class ModalCredito
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public int creditoTotal { get; set; } = 0;
        [Parameter]
        public int creditoUtilizado { get; set; } = 0;
        [Parameter]
        public int creditoDisponible { get; set; } = 0;

        private bool Loading = false;
    }
}
