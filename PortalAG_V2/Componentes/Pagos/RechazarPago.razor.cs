using Microsoft.AspNetCore.Components;
using MudBlazor;
using PortalAG_V2.Shared.Model.AvisoDePago;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;

namespace PortalAG_V2.Componentes.Pagos
{
    public partial class RechazarPago
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public int idOperacion { get; set; } 
        [Parameter] public string idUsuario { get; set; }
        [Parameter] public Func<Task> FuncCloseDialog { get; set; }
        [Parameter] public Func<Task> funcRecargar { get; set; }

        private const string urlListado = "api/v2/AvisodePagos/ActualizaEstado";
        private ClientFactory conexion;

        string Comentario ;
        private async Task Enviar()
        {
            conexion = new MainServices().Pagos;
            ActulizaEstadoModel post = new ActulizaEstadoModel() 
            { 
                Comentario = Comentario,
                IDEstado =  9,
                IDOperacion = idOperacion,
                Usuario = idUsuario
            };
           
           var Rechazo = await conexion.HttpClientInstance.PostAsJsonAsync($"{urlListado}", post);
            if (Rechazo.IsSuccessStatusCode)
            {
                FuncCloseDialog();
                funcRecargar();
                Cancel();

            }
        }

        void Cancel() => MudDialog.Cancel();

    }
}
