using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.HojaDeRuta;
using PortalAG_V2.Shared.Model.NotaDeCredito;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;

namespace PortalAG_V2.Componentes.HojaDeRuta
{
    public partial class DialogModificarHR
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public string IDusuario { get; set; }
        MainServices service;
        private string urlEnviaHoja = "Despacho/HojaRutaDesktop/Modificar";
        private List<ResponseHojaruta> responseHojaRuta = new List<ResponseHojaruta>() { };
        RequestModificarHR ModificarHRPost = new RequestModificarHR();

        bool Loading = false;
        bool showCallAlert = true;
        public string HojaRuta { get; set; }
        public string NroPedido { get; set; }
        async Task Submit()
        {
            try
            {
                if (!String.IsNullOrEmpty(HojaRuta))
                {
                    if (!String.IsNullOrEmpty(NroPedido))
                    {
                        if (await ModificarHR() && responseHojaRuta.FirstOrDefault().msgResult == "OK")
                        {
                            snakBarCreation(responseHojaRuta.FirstOrDefault().msgMensaje, Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                            MudDialog.Close(DialogResult.Ok(true));
                        }
                        else
                        {
                            snakBarCreation(responseHojaRuta.FirstOrDefault().msgMensaje, Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                        }
                    }
                    else
                    {
                        snakBarCreation("Falta El Número de pedido", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    }
                }
                else
                {
                    snakBarCreation("Falta el Número DE la hoja De ruta", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                }
            }
            catch (Exception ex)
            {
                snakBarCreation(ex.Message, Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }
        }

        public async Task<bool> ModificarHR()
        {
            try
            {
                service = new MainServices();
                ModificarHRPost = new RequestModificarHR();
                ModificarHRPost.nroPedido = int.Parse(NroPedido);
                ModificarHRPost.IDHojaDeRuta = int.Parse(HojaRuta);
                ModificarHRPost.IDusuario = IDusuario;
                var HojaDeRutapost = await service.Formulario.HttpClientInstance.PostAsJsonAsync($"api/v2/{urlEnviaHoja}", ModificarHRPost);
                if (HojaDeRutapost.IsSuccessStatusCode)
                {
                    responseHojaRuta = JsonConvert.DeserializeObject<List<ResponseHojaruta>>(await HojaDeRutapost.Content.ReadAsStringAsync());
                    return true;
                }
                else
                {
                    responseHojaRuta = JsonConvert.DeserializeObject<List<ResponseHojaruta>>(await HojaDeRutapost.Content.ReadAsStringAsync());
                    return false;
                }
            }
            catch (Exception ex)
            {
                responseHojaRuta.Add(new ResponseHojaruta { msgMensaje = "ER - " + ex.Message, msgResult = "ER" });
                return false;
            }
        }
        void Cancel() 
        {
            snakBarCreation("Se Cancelo La Operación", Defaults.Classes.Position.BottomStart, Severity.Info, 1000);
            MudDialog.Cancel(); 
        }

        private void CerrarAlerta() => showCallAlert = !showCallAlert;
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }

    }
}
