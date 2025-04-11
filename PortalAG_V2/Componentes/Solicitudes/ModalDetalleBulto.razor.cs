using Microsoft.AspNetCore.Components;
using MudBlazor;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using Syncfusion.Blazor.Grids;
using static agDataAccess.Models.Solicitudes.AuditoriaPedidosModel;

namespace PortalAG_V2.Componentes.Solicitudes
{
    public partial class ModalDetalleBulto
    {
        SfGrid<Detalleauditoria> Grid;
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public List<Detalleauditoria> Detalle { get; set; }
        void Cancel() => MudDialog.Cancel();
        private List<string> ButtonsGrid = new List<string>() { "ExcelExport" };

        public async Task ToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {

            if (args.Item.Id == "Grid_excelexport")
            {
                if (Detalle.Count() >= 0)
                {
                    snakBarCreation("Exportando...", Defaults.Classes.Position.BottomStart, Severity.Info, 5000);
                    //_processing = true;
                    await Grid.ExportToExcelAsync();
                    //_processing = false;
                    snakBarCreation("Listo!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    snakBarCreation("No hay datos para exportar", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                }
            }
        }


        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion
    }
}
