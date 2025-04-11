using agDataAccess.Models.Despacho1;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Model.AvisoDePago;
using PortalAG_V2.Shared.Model.HojaDeRuta;
using PortalAG_V2.Shared.Services;
using static PortalAG_V2.Pages.Movimientos.Dialog123;

namespace PortalAG_V2.Componentes.HojaDeRuta
{
    public partial class DialogReimpresionHR
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string IDusuario { get; set; }
        MainServices service;
        private string urlGetDatosReimpresion = "Despacho/HojaRutaDesktop/ReimpresionHR/";
        HRReimpresionModel DatosHr = new HRReimpresionModel();
        [Inject] IJSRuntime js { get; set; }
        [Inject] ExportService exportService { get; set; }



        bool Loading = false;
        bool showCallAlert = true;
        public string HojaRuta { get; set; }

        void Cancel()
        {
            snakBarCreation("Se Cancelo La Operación", Defaults.Classes.Position.BottomStart, Severity.Info, 1000);
            MudDialog.Cancel();
        }

        public async Task<HRReimpresionModel> CargarDato(string IDHoja)
        {
            try
            {
                HRReimpresionModel DatosHrresponse = new();
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{urlGetDatosReimpresion}{IDHoja}");
                if (lista.IsSuccessStatusCode)
                {
                    DatosHrresponse = JsonConvert.DeserializeObject<List<HRReimpresionModel>>(await lista.Content.ReadAsStringAsync()).FirstOrDefault();
                    if (DatosHrresponse != null) { 
                        return DatosHrresponse;
                    }
                    return new HRReimpresionModel();
                }
                else
                {
                    return new HRReimpresionModel();
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                return new HRReimpresionModel();

            }
        }

        async Task Submit()
        {
            try
            {
                if (!String.IsNullOrEmpty(HojaRuta))
                {
                    DatosHr = new();
                    DatosHr = await CargarDato(HojaRuta);
                    if (DatosHr.IDUsuario != null)
                    {
                        await GenerarPDFHojaRuta(DatosHr);
                        snakBarCreation("Procesando Reimpresion", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                        MudDialog.Close(DialogResult.Ok(true));
                    }
                    else
                    {
                        snakBarCreation("Hoja de ruta no existe", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                    }
                }
            }
            catch (Exception ex)
            {
                snakBarCreation(ex.Message, Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }
        }

        public async Task GenerarPDFHojaRuta(HRReimpresionModel datos)
        {
            using (MemoryStream memory = exportService.CreatePdfHojaderuta(datos))
            {
                await js.SaveAs($"Hoja de Ruta {datos.IDGuiaHojaRuta}.pdf", memory.ToArray());
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
