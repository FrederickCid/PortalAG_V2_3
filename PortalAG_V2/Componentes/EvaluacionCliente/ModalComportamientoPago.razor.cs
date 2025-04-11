using Microsoft.AspNetCore.Components;
using MudBlazor;
using static PortalAG_V2.Shared.Models.ClienteEvaluacion.ClienteAdicionalModel;

namespace PortalAG_V2.Componentes.EvaluacionCliente
{
    public partial class ModalComportamientoPago
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public List<Comportamientospago> compPag { get; set; } =  new List<Comportamientospago>();

        public bool Loading = false;

    }
}
