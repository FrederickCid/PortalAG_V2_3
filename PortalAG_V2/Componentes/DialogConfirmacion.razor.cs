using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PortalAG_V2.Componentes
{
    public partial class DialogConfirmacion
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string Titulo { get; set; }
        [Parameter] public string TextDialog { get; set; }
        [Parameter] public string nombreBoton { get; set; }
        [Parameter] public bool ocultarcancelar { get; set; } = true;

        private void Cancel()
        {
            MudDialog.Close(DialogResult.Ok(false));
        }

        private void Confirmacion()
        {
            //In a real world scenario this bool would probably be a service to delete the item from api/database           
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
}
