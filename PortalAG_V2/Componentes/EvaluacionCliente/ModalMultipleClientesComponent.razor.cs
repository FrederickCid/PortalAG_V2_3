using Microsoft.AspNetCore.Components;
using MudBlazor;
using SheriffDataAccess.Models.SheriffModel;

namespace PortalAG_V2.Componentes.EvaluacionCliente
{
    public partial class ModalMultipleClientesComponent
    {
        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }
        public ClienteEvaluacionModel Cliente = new ClienteEvaluacionModel();
        [Parameter]
        public List<ClienteEvaluacionModel> List { get; set; } = new List<ClienteEvaluacionModel> { };



        private void Submit(ClienteEvaluacionModel e) => MudDialog.Close(DialogResult.Ok(e));

        private void Cancel() => MudDialog.Cancel();
    }
}
