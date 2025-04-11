using Microsoft.JSInterop;
using agDataAccess.Models.Solicitudes;
using PortalAG_V2.Shared.Models.Cheques;

namespace PortalAG_V2.Services
{
    public static class JSExtension
    {
        #region Método de exportación a PDF
      
        public static async Task<object> GenerarPdfReposicionesPendientes(this IJSRuntime js,string Nombre ,List<ReposicionesPendientesModel> ReposicionesPendientes)
        {
             return await js.InvokeAsync<object>("GenerarPDF", Nombre, ReposicionesPendientes);
        }

        public static async Task<object> GenerarPDFCheques(this IJSRuntime js, List<ChequesModel> Cheques)
        {
            return await js.InvokeAsync<object>("MyLib.GenerarPDFCheques", Cheques);
        }
        #endregion


        #region Alertas

        public static async Task<object> MensajeAlertaRefrescar(this IJSRuntime js)
        {
            return await js.InvokeAsync<object>("MensajeAlertaRefrescar");
        }

        public static async Task<object> MensajeAlertaSimple(this IJSRuntime js, string titulo, string texto, string icono)
        {
            return await js.InvokeAsync<object>("MensajeAlertaSimple", titulo, texto, icono);
        }

        public static async Task<object> MensajeAlertaError(this IJSRuntime js, string titulo, string texto, string icono)
        {
            return await js.InvokeAsync<object>("MensajeAlertaError", titulo, texto, icono);
        }

        public static async Task<object> MensajeConfirmacion(this IJSRuntime js, string position, string icon, string title)
        {
            return await js.InvokeAsync<object>("MensajeConfirmacion", position, icon, title);
        }

        public static async Task<bool> MensajeVadidacion(this IJSRuntime js, string titulo, string texto, string icono)
        {
            return await js.InvokeAsync<bool>("MensajeVadidacion", titulo, texto, icono);
        }

        public static async Task<bool> MensajeAlertaValidacion(this IJSRuntime js, string title, string text, string icon)
        {
            return await js.InvokeAsync<bool>("MensajeAlertaValidacion", title, text, icon);
        }

        public static async Task<bool> MensajeAlertaEditar(this IJSRuntime js, string title, string text, string icon)
        {
            return await js.InvokeAsync<bool>("MensajeAlertaEditar", title, text, icon);
        }

        public static async Task<object> MensajeAlertaToast(this IJSRuntime js)
        {
            return await js.InvokeAsync<object>("MensajeAlertaToast");
        }

        #endregion

        #region Cookie's
        public static async Task<object> WriteCookie(this IJSRuntime js, string name, string value, DateTime days)
        {
            return await js.InvokeAsync<object>("MyLib.WriteCookie",name,   value,  days);
        }
        public static async Task<string> ReadCookie(this IJSRuntime js, string cname)
        {
            return await js.InvokeAsync<string>("MyLib.ReadCookie", cname);
        }
        public static async Task<string> ReloadWasm(this IJSRuntime js)
        {
            return await js.InvokeAsync<string>("MyLib.WasmReload");
        }
        #endregion
    }
}
