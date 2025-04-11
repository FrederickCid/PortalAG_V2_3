using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Auth;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Model.Prepacking;
using PortalAG_V2.Shared.Model.SolicitudMovimiento;
using PortalAG_V2.Shared.Services;
using System.Net.Http.Json;
using static PortalAG_V2.Shared.Model.Pallet.PalletModels;

namespace PortalAG_V2.Pages.Pallet
{
    partial class SepararPallet
    {
        [CascadingParameter]
        public AppState appSatate { get; set; }
        public int pallet { get; set; } = 0;
        public int cantidadExt { get; set; } = 0;
        public bool Loading = false;
        public bool habilitar_procesar = true;
        public string Enter = "Enter";
        public string URLIDUbicacionPallet = "/api/v1/Despalletizar/IDUbicacionPallet/";
        public string URLDespaletizar = "/api/v1/Despalletizar/Despalletizar/";
        public string urlCrearGuia = "/api/v2/MovimientoBodegas/CrearGuia";
        public string UrlTraspasoSAP = "api/v2/MovimientoBodegas/EnvioSAP";
        MainServices service;
        ClientFactory conexionGuia;
        List<IDUbicacionModel> iDUbicacionModel = new List<IDUbicacionModel>();
        List<DespalletizarModel> resultDespaletizar = new List<DespalletizarModel>();
        public List<ItemsGrilla> listaDetalleSAP = new List<ItemsGrilla>();


        public async Task BlurPalletAsync()
        {
            
            if (pallet != 0) {
                service = new MainServices();
                var lista = await service.PrePacking.HttpClientInstance.GetAsync($"{URLIDUbicacionPallet}{pallet}");
                if (lista.IsSuccessStatusCode)
                {
                    
                    iDUbicacionModel = JsonConvert.DeserializeObject<List<IDUbicacionModel>>(await lista.Content.ReadAsStringAsync());
                    if(iDUbicacionModel.Count > 1)
                    {
                        snakBarCreation("El pallet tiene mas de una ubicación", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    }
                    else
                    {
                        snakBarCreation("Pallet consultado correctamente", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                        habilitar_procesar = false;
                    }
                }
                else
                {
                    snakBarCreation("No se pudo obtener la ubicacion del pallet", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                }
            }
            else
            {
                snakBarCreation("Nro de pallet no puede ser cero!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
            }
            
        }

        public async Task Digito(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                if (pallet != 0)
                {

                    var lista = await service.PrePacking.HttpClientInstance.GetAsync($"api/v1/{URLIDUbicacionPallet}{pallet}");
                    if (lista.IsSuccessStatusCode)
                    {

                        iDUbicacionModel = JsonConvert.DeserializeObject<List<IDUbicacionModel>>(await lista.Content.ReadAsStringAsync());
                        if (iDUbicacionModel.Count > 1)
                        {
                            snakBarCreation("El pallet tiene mas de una ubicación", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                        }
                        else
                        {
                            snakBarCreation("Pallet consultado correctamente", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                        }
                    }
                    else
                    {
                        snakBarCreation("No se pudo obtener la ubicacion del pallet", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    }
                }
                else
                {
                    snakBarCreation("Nro de pallet no puede ser cero!", Defaults.Classes.Position.BottomStart, Severity.Warning, 1000);
                }

            }
            StateHasChanged();

        }

        public async Task Procesar()
        {
            Loading = true;
            service = new MainServices();
            var user = await _authenticationManager.CurrentUser();
            var lista = await service.PrePacking.HttpClientInstance.GetAsync($"{URLDespaletizar}{pallet}/{iDUbicacionModel.FirstOrDefault().IDUbicacionHasta}/{cantidadExt}/{user.GetUserId()}");
            if (lista.IsSuccessStatusCode)
            {

                resultDespaletizar = JsonConvert.DeserializeObject<List<DespalletizarModel>>(await lista.Content.ReadAsStringAsync());
                if (resultDespaletizar.FirstOrDefault().msgError.Contains("ER"))
                {
                    snakBarCreation(resultDespaletizar.FirstOrDefault().msgResult, Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                }
                else
                {
                    try
                    {
                        CabeceraTraspasoDTO cabeceraGuia = new CabeceraTraspasoDTO
                        {
                            IDBodegaDesde = 21,
                            BodegaDesde = "BV_BPM",
                            IDBodegaHasta = 24,
                            BodegaHasta = "BV_BRE",
                            Comentario = "Despaletizado",
                            IDEstado = 9,
                            Fecha = DateTime.Now.ToString(),
                            IDTipoGuia = 101,
                            IDGuia = 0,
                            NumeroGuia = 0
                        };

                        List<DetalleTraspasoDTO> detalleGuia;
                        detalleGuia = new List<DetalleTraspasoDTO>();

                        detalleGuia.Add(
                        new DetalleTraspasoDTO()
                        {
                            Linea = 0,
                            Valido = false,
                            Estado = 0,
                            Codigo = iDUbicacionModel.FirstOrDefault().IDArticulo,
                            Nombre = iDUbicacionModel.FirstOrDefault().Nombre,
                            BodegaDes = "BV_BPM",
                            CantDes = cantidadExt.ToString(),
                            UnidadBultoDes = iDUbicacionModel.FirstOrDefault().UnidadPorBulto.ToString(),
                            BodegaHast = "BV_BRE",
                            CantHast = cantidadExt.ToString(),
                            UnidadBultoHast = iDUbicacionModel.FirstOrDefault().UnidadPorBulto.ToString(),
                            Total = (iDUbicacionModel.FirstOrDefault().UnidadPorBulto * cantidadExt).ToString(),
                            Comentario = $"Despaletizado x {user.GetUserId()}"
                        });

                        GuiaTraspasoDTO data = new GuiaTraspasoDTO()
                        {
                            Cabecera = cabeceraGuia,
                            Detalle = detalleGuia
                        };


                        var agGuia = await service.CrearGuia.HttpClientInstance.PostAsJsonAsync<GuiaTraspasoDTO>(urlCrearGuia, data);
                        if (agGuia.IsSuccessStatusCode)
                        {

                            ResultGuiaMovimiento idGuia = JsonConvert.DeserializeObject<ResultGuiaMovimiento>(await agGuia.Content.ReadAsStringAsync());
                            listaDetalleSAP = new List<ItemsGrilla>();
                            ItemsGrilla itemsGrilla = new ItemsGrilla
                            {
                                ItemCode = iDUbicacionModel.FirstOrDefault().IDArticulo,
                                Quantity = (iDUbicacionModel.FirstOrDefault().UnidadPorBulto * cantidadExt).ToString(),
                                WarehouseCode = "BV_BRE"
                            };
                            listaDetalleSAP.Add(itemsGrilla);


                            EnvioTraspaso envioTraspaso = new EnvioTraspaso
                            {
                                Comments = "Despaletizado",
                                JournalMemo = "Guia N° " + idGuia.NumeroGuia + " - Despaletizado",
                                FromWarehouse = "BV_BPM",
                                ToWarehouse = "BV_BRE",
                                StockTransferLines = listaDetalleSAP
                            };

                            var respuesta = await service.SAP.HttpClientInstance.PostAsJsonAsync<EnvioTraspaso>(UrlTraspasoSAP, envioTraspaso);
                            if (respuesta.IsSuccessStatusCode)
                            {
                                snakBarCreation(resultDespaletizar.FirstOrDefault().msgResult, Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                                //snakBarCreation("Proceso terminado con exito", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                            }
                            else
                            {
                                
                                snakBarCreation("Error al enviar a SAP", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                            }
                        }
                        else
                        {
                            
                            snakBarCreation("Error al ingresar guia", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                        }
                        Loading = false;
                        Cancelar();
                        //
                    }
                    catch (Exception ex) 
                    {
                        Loading = false;
                        Cancelar();
                        snakBarCreation(ex.Message, Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    }

                    
                }

                Loading = false;
                Cancelar();
            }
            else
            {
                Loading = false;
                snakBarCreation("Error desconocido - Consultar a TI", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
            }

        }

        public void Cancelar()
        {
            pallet = 0;
            cantidadExt = 0;
        }

        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
    }
}
