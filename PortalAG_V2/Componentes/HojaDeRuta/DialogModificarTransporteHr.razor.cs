using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.HojaDeRuta;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;
using static FullBikePos.Shared.Models.HojaDeRuta.CambioTransporteModel;

namespace PortalAG_V2.Componentes.HojaDeRuta
{
    public partial class DialogModificarTransporteHr
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        MainServices service;
        private string urlEnviaHoja = "Despacho/HojaRutaDesktop/ActualizaTransporte";
        private List<ResponseTransporteModel> responseHojaRuta = new List<ResponseTransporteModel>() { };
        RequestTransporteModel ModificarHRPost = new RequestTransporteModel();
        bool Loading = false;
        bool showCallAlert = true;
        public string HojaRuta { get; set; }
        public string Transporte { get; set; }

        async Task Submit()
        {
            try
            {
                if (!String.IsNullOrEmpty(HojaRuta))
                {
                    if (!String.IsNullOrEmpty(Transporte))
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
                ModificarHRPost = new RequestTransporteModel();
                ModificarHRPost.Transporte = Transporte;
                ModificarHRPost.IDGuiaHojaruta = int.Parse(HojaRuta);

                var HojaDeRutapost = await service.Formulario.HttpClientInstance.PostAsJsonAsync($"api/v2/{urlEnviaHoja}", ModificarHRPost);
                if (HojaDeRutapost.IsSuccessStatusCode)
                {
                    responseHojaRuta = JsonConvert.DeserializeObject<List<ResponseTransporteModel>>(await HojaDeRutapost.Content.ReadAsStringAsync());
                    return true;
                }
                else
                {
                    responseHojaRuta = JsonConvert.DeserializeObject<List<ResponseTransporteModel>>(await HojaDeRutapost.Content.ReadAsStringAsync());
                    return false;
                }
            }
            catch (Exception ex)
            {
                responseHojaRuta = new();
                responseHojaRuta.FirstOrDefault().msgMensaje = "ER - " + ex.Message;
                responseHojaRuta.FirstOrDefault().msgResult = "ER";
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

