﻿using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PortalAG_V2.Componentes.Cheques
{
    public partial class DialogRescate
    {
        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; }
        string Respuesta = "";


        private void Cancel()
        {
            MudDialog.Close();
        }

        private void Confirmacion()
        {
            if (Respuesta.Length <= 8 || String.IsNullOrEmpty(Respuesta)) { snakBarCreation("error! - mensaje vacio o demasiado corto", Defaults.Classes.Position.BottomStart, Severity.Error, 1000); return; }
            MudDialog.Close(DialogResult.Ok(Respuesta));
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
