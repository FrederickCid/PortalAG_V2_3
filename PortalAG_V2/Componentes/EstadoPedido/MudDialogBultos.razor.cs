using agDataAccess.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Syncfusion.Blazor.TreeMap;

namespace PortalAG_V2.Componentes.EstadoPedido
{
    public partial class MudDialogBultos
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public List<BultosModel> Bultos { get; set; }      
        void Cancel() => MudDialog.Cancel();

        void onClickBultosDetalle(BultosModel args)
        {
            var parameters = new DialogParameters<MudDialogBultosDetalle>();
            parameters.Add(x => x.Detalle, args.DetalleBultos);

            var options = new MudBlazor.DialogOptions() { CloseButton = false, FullWidth = true, MaxWidth = MaxWidth.Large };

            DialogService.Show<MudDialogBultosDetalle>($"Bulto Numero: {args.NroBulto}", parameters, options);
        }
    }
}
