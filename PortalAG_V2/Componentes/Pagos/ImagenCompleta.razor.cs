using Microsoft.AspNetCore.Components;
using MudBlazor;
using PortalAG_V2.Shared.Model.AvisoDePago;


namespace PortalAG_V2.Componentes.Pagos
{
    public partial class ImagenCompleta
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string imagen { get; set; }

    }
}
