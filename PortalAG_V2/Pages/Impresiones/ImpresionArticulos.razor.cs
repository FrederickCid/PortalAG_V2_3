using DocumentFormat.OpenXml.Office.CoverPageProps;
using MudBlazor;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Pages.Impresiones
{
    public partial class ImpresionArticulos
    {
        bool success;
        string[] errors = { };
        MudForm form;
        string IDArtiuclo;
        MainServices service;
        string Impresora = "CDA_Recepcion_1";
        int Copias;
        string url = "/api/v2/ImpresionEtiquetas/ImpresionArticulo/";
        public int SelectedOption { get; set; }
        

        public async Task Imprimir()
        {   
            await form.Validate();
            if (form.IsValid)
            {
                if (Copias < 1 || Copias > 100)
                {
                    snakBarCreation("Copias No puede ser 0 o mayor a 100", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                    return;
                }
                service = new MainServices();
                var result = await service.ConectionService.HttpClientInstance.GetAsync($"{url}{IDArtiuclo}/{Impresora}/{Copias}");
                if (result.IsSuccessStatusCode)
                {
                    snakBarCreation("Imprimiendo", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    snakBarCreation("Error Al Imprimir.", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                }
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
