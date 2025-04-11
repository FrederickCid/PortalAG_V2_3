using agDataAccess.Models.Solicitudes;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PortalAG_V2.Shared.Models.HubSpotModels;

namespace PortalAG_V2.Componentes.EvaluacionCliente
{
    public partial class ModalHubSpotComponent
    {
        private string _searchString;

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public ResponseSearchModel tickets { get; set; } = new ResponseSearchModel();
        [Parameter]
        public OwnerModel OwnerLista {get; set;} = new OwnerModel();
        [Parameter]
        public StagesModels StagesLista { get; set; } = new StagesModels();

        private bool Loading = false;

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
        private Func<ResponseSearchModel.Result, bool> QuickFilter => x =>
        {
            //si no encuetra nada no manda nada
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            //Buscar por NRO de Documento 
            if (x.id.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Buscar Por CLiente
            if (x.id.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Busqueda mas amplia de los dos anterirore y agregando al vendedor, para mostrar multiples
            if ($"{x.id} ".Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };
    }
}
