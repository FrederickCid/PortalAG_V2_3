using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Helpers
{
    public static class JSExtencion
    {
        #region Método de exportación a excel

        public static async Task<object> GenerarExcel(this IJSRuntime js, string nombreArchivo, byte[] archivo)
        {
            return await js.InvokeAsync<object>("GenerarExcel", nombreArchivo, Convert.ToBase64String(archivo));
        }

        public static async Task<object> GenerarPdf(this IJSRuntime js, string nombreArchivo, byte[] archivo)
        {
            return await js.InvokeAsync<object>("GenerarPdf", nombreArchivo, Convert.ToBase64String(archivo));
        }

        public static ValueTask<object> SaveAs(this IJSRuntime js, string filename, byte[] data)
           => js.InvokeAsync<object>("saveAsFile", filename, Convert.ToBase64String(data));
        #endregion

       
        public static async Task<string> getUsuario(this IJSRuntime js)
        {
            return await js.InvokeAsync<string>("GetUser");
        }
    }
}
