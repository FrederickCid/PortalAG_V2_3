using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.Extensions.Options;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.EstadoPedidos;
using PortalAG_V2.Shared.Model.Impresion;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Pages.Impresiones
{
    public partial class ImpresionUbicacion
    {
        bool success;
        string[] errors = { };
        MudForm form;
        string Impresora = "CDA_Recepcion_1";
        MainServices service;
        List<ConsultarBodegasModel> consultarBodegasModel = new();
        ConsultarBodegasModel SelectedconsultarBodegasModel = new();
        List<ConsultarSectorModel> consultarSector = new();
        ConsultarSectorModel SelectedconsultarSector = new();
        List<ConsultarCalleModel> consultarCalle = new();
        ConsultarCalleModel SelectedConsultarCalle = new();
        List<ConsultarTramoModel> ConsultarTramoModel = new();
        ConsultarTramoModel SelectedConsultarTramoModel = new();
        List<ConsultarNivelModel> consultarNivelModel = new();
        ConsultarNivelModel SelectedConsultarNivelModel = new();
        List<ConsultarPosicionModel> consultarPosicionModel = new();
        ConsultarPosicionModel SelectedConsultarPosicionModel = new();
        int Copias;
        int Cantidad;
        string url = "/api/v2/ImpresionEtiquetas/ImpresionUbicacion/";
        string urlBodegas = "/api/v2/ImpresionEtiquetas/ConsultarBodega/";
        string urlConsultarSector = "/api/v2/ImpresionEtiquetas/ConsultarSector/";
        string urlConsultarCalle = "/api/v2/ImpresionEtiquetas/ConsultarCalle/";
        string urlConsultarTramo = "/api/v2/ImpresionEtiquetas/ConsultarTramo/";
        string urlConsultarNivel = "/api/v2/ImpresionEtiquetas/ConsultarNivel/";
        string urlConsultarPosicion = "/api/v2/ImpresionEtiquetas/ConsultarPosicion/";

        public string SelectedOption { get; set; } 
        protected override async Task OnInitializedAsync()
        {
            await ConsultarBodegas();
        }



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
                    service = new MainServices();
                    var result = await service.ConectionService.HttpClientInstance.GetAsync($"{url}{SelectedconsultarBodegasModel.IDBodega}/{SelectedconsultarSector.IDSector}/{SelectedConsultarCalle.IDCalle}/{SelectedOption}/{SelectedConsultarTramoModel.idTramo}/{SelectedConsultarNivelModel.IDNivel}/{SelectedConsultarPosicionModel.idPosicion}/{Impresora}/{Copias}");
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

        private async Task ConsultarBodegas()
        {
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlBodegas}");
            if (result.IsSuccessStatusCode)
            {
                consultarBodegasModel = new();
                consultarBodegasModel = JsonConvert.DeserializeObject<List<ConsultarBodegasModel>>(await result.Content.ReadAsStringAsync());
            }
        }

        private async Task ConsultarSector(int idBodega)
        {
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlConsultarSector}{idBodega}");
            if (result.IsSuccessStatusCode)
            {
                consultarSector = new();
                consultarSector = JsonConvert.DeserializeObject<List<ConsultarSectorModel>>(await result.Content.ReadAsStringAsync());
            }
        }
        private async Task ConsultarCalle(int idBodega, int idSector)
        {
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlConsultarCalle}{idBodega}/{idSector}");
            if (result.IsSuccessStatusCode)
            {
                consultarCalle = new();
                consultarCalle = JsonConvert.DeserializeObject<List<ConsultarCalleModel>>(await result.Content.ReadAsStringAsync());
            }
        }

        private async Task ConsultarTramo(int idBodega, int idSector, int idCalle, string lado)
        {
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlConsultarTramo}{idBodega}/{idSector}/{idCalle}/{lado}");
            if (result.IsSuccessStatusCode)
            {
                ConsultarTramoModel = new();
                ConsultarTramoModel = JsonConvert.DeserializeObject<List<ConsultarTramoModel>>(await result.Content.ReadAsStringAsync());
            }
        }   

        private async Task ConsultarNivel(int idBodega, int idSector, int idCalle, string lado, int idTramo)
        {
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlConsultarNivel}{idBodega}/{idSector}/{idCalle}/{lado}/{idTramo}");
            if (result.IsSuccessStatusCode)
            {
                consultarNivelModel = new();
                consultarNivelModel = JsonConvert.DeserializeObject<List<ConsultarNivelModel>>(await result.Content.ReadAsStringAsync());
            }
        } 

        private async Task ConsultarPosicion(int idBodega, int idSector, int idCalle, string lado, int idTramo, int idNivel)
        {
            service = new MainServices();
            var result = await service.ConectionService.HttpClientInstance.GetAsync($"{urlConsultarPosicion}{idBodega}/{idSector}/{idCalle}/{lado}/{idTramo}/{idNivel}");
            if (result.IsSuccessStatusCode)
            {
                consultarPosicionModel = new();
                consultarPosicionModel = JsonConvert.DeserializeObject<List<ConsultarPosicionModel>>(await result.Content.ReadAsStringAsync());
            }
        }

        private async Task OnChangeBodega(ConsultarBodegasModel bodega)
        {
            SelectedconsultarBodegasModel = bodega;
            await ConsultarSector(bodega.IDBodega);

        }
        private async Task OnChangeSector(ConsultarSectorModel Sector)
        {
            SelectedconsultarSector = Sector;
            await ConsultarCalle(SelectedconsultarBodegasModel.IDBodega, Sector.IDSector);

        }
        private async Task OnChangeLado(string lado)
        {
            SelectedOption = lado;
            await ConsultarTramo(SelectedconsultarBodegasModel.IDBodega,SelectedconsultarSector.IDSector, SelectedConsultarCalle.IDCalle, SelectedOption);

        }  
        private async Task OnChangeTramo(ConsultarTramoModel Tramo)
        {
            SelectedConsultarTramoModel = Tramo;
            await ConsultarNivel(SelectedconsultarBodegasModel.IDBodega,SelectedconsultarSector.IDSector, SelectedConsultarCalle.IDCalle, SelectedOption, Tramo.idTramo);

        }  
        private async Task OnChangeNivel(ConsultarNivelModel Nivel)
        {
            SelectedConsultarNivelModel = Nivel;
            await ConsultarPosicion(SelectedconsultarBodegasModel.IDBodega,SelectedconsultarSector.IDSector, SelectedConsultarCalle.IDCalle, SelectedOption, SelectedConsultarTramoModel.idTramo, Nivel.IDNivel);

        }

        Func<ConsultarBodegasModel, string> convertBodegas = p => p.SiglaBodega.ToString();
        Func<ConsultarSectorModel, string> convertSector = p => p.Sector.ToString();
        Func<ConsultarCalleModel, string> convertCalle = p => p.Calle.ToString();
        Func<ConsultarTramoModel, string> convertTramo = p => p.Tramo.ToString();
        Func<ConsultarNivelModel, string> convertNivel = p => p.Nivel.ToString();
        Func<ConsultarPosicionModel, string> convertPosicion = p => p.Posicion.ToString();


        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }

    }
}
