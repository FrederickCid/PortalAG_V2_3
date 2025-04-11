using MudBlazor;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Pages.Pallet
{
    partial class ImpresionPallet
    {

        public string URLxxxx = "api/v2/ImpresionPallet/ImprimirEtiquetaPallet/";
        MainServices service;
        private int pallet { get; set; }

        public async Task Imprimir()
        {
            if (pallet != 0)
            {
                service = new MainServices();
                var result = await service.ConectionService.HttpClientInstance.GetAsync($"{URLxxxx}{pallet}/CDA_OficinaBodega");
                if (result.IsSuccessStatusCode)
                {
                    snakBarCreation("Impresion generada", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    snakBarCreation("Error al imprimir", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                }

            }
            else
            {
                snakBarCreation("Nro pallet no puede ser cero", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }
        }

        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
    }
}
