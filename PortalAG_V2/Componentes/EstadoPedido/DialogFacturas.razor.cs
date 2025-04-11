using agDataAccess.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PortalAG_V2.Componentes.EstadoPedido
{
    public partial class DialogFacturas
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public List<FacturaEstadoPedidoModel> Detalle { get; set; }
        [Parameter] public int nroDocumento { get; set; }
        [Parameter] public string idCliente { get; set; }
        [Parameter] public string razonSocial { get; set; }
        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}
