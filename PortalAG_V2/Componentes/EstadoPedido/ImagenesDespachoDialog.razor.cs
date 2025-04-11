using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes.Pagos;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Models.HubSpotModels;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Componentes.EstadoPedido
{
    public partial class ImagenesDespachoDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter]
        public List<EstadoPedidoDespachoImgModels> Imagenes { get; set; } = new();
        [Parameter]
        public int IDOperacion { get; set; }

        bool Loading = false;

        async Task Abrir(string imagen, string nombre)
        {
            var parameters = new DialogParameters<ImagenCompleta>();
            parameters.Add(x => x.imagen, imagen);
            var options = new MudBlazor.DialogOptions() { CloseButton = true, FullWidth = false, MaxWidth = MaxWidth.ExtraLarge };

            DialogService.Show<ImagenCompleta>($"{nombre}", parameters, options);
        }
    }
}
