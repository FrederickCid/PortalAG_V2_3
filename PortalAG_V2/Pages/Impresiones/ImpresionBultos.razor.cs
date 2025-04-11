using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.Extensions.Options;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Model.Impresion;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Pages.Impresiones
{
    public partial class ImpresionBultos
    {
        bool success;
        string[] errors = { };
        MudForm form;
        string IDArtiuclo;
        string Impresora = "CDA_Recepcion_1";
        MainServices service;
        List<ResponseBultos> responseBultos = new();
        int Copias;
        int Cantidad ;
        string url = "/api/v2/ImpresionEtiquetas/ImpresionBultoArticulo/";
        string urlArticulo = "/api/v2/ImpresionEtiquetas/ConsultaBultos/";
        public int SelectedOption { get; set; } = 22;

        public async Task Imprimir()
        {
            await form.Validate();
            if (form.IsValid)
            {
                try
                {
                    if (Copias < 1 || Copias > 100)
                    {
                        snakBarCreation("Copias No puede ser 0 o mayor a 100", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                        return;
                    }
                    if (Cantidad < 1)
                    {
                        snakBarCreation("Cantidad No puede ser 0", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                        return;
                    }
                    service = new MainServices();
                    var result = await service.ConectionService.HttpClientInstance.GetAsync($"{url}{IDArtiuclo}/{SelectedOption}/{Cantidad}/{Impresora}/{Copias}");
                    if (result.IsSuccessStatusCode)
                    {
                        snakBarCreation("Imprimiendo", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                    }
                    else
                    {
                        snakBarCreation("Error Al Imprimir.", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private async Task ConsultarArticulo()
        {
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlArticulo}{IDArtiuclo}/{SelectedOption}");
            if (result.IsSuccessStatusCode)
            {
                responseBultos = new();
                responseBultos = JsonConvert.DeserializeObject<List<ResponseBultos>>(await result.Content.ReadAsStringAsync());
                Cantidad = responseBultos.FirstOrDefault().CantidadPorBulto;
            }
        }

        private async Task ConsultarArticulo(int Bodega)
        {
            SelectedOption = Bodega;
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlArticulo}{IDArtiuclo}/{SelectedOption}");
            if (result.IsSuccessStatusCode)
            {
                responseBultos = new();
                responseBultos = JsonConvert.DeserializeObject<List<ResponseBultos>>(await result.Content.ReadAsStringAsync());
                Cantidad = responseBultos.FirstOrDefault().CantidadPorBulto;
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
