using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PortalAG_V2.Componentes.Solicitudes
{
    public partial class AyudaLineasUsuarioModal
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    }
}
