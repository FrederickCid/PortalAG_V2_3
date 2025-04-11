using Microsoft.AspNetCore.Components;
using MudBlazor;
using PortalAG_V2.Shared.Models.Cheques;
using SheriffDataAccess.Models.SheriffModel;

namespace PortalAG_V2.Componentes.Cheques
{
    public partial class ModalMultipleCheques
    {
        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }
        public ChequesModel cheque = new ChequesModel();
        [Parameter]
        public List<ChequesModel> List { get; set; } = new List<ChequesModel> { };
        [Parameter]
        public string numeroSerie { get; set; }



        private void Submit(ChequesModel e) => MudDialog.Close(DialogResult.Ok(e));

        private void Cancel() => MudDialog.Cancel();
    }
}
