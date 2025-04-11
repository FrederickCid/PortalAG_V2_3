using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PortalAG_V2.Componentes.CargaMasivaArticulos
{
    public partial class DialogCargaMasiva
    {
        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }
        string Respuesta = "";


        private void Cancel()
        {
            MudDialog.Close(DialogResult.Ok(false));
        }

        private void Confirmacion()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }


        #region SnackBar
        private void snakBarCreation(string msj, string position, Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion
    }
}

