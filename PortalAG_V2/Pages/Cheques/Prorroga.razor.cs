using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes.Cheques;
using PortalAG_V2.Shared.Models.Cheques;
using PortalAG_V2.Shared.Services;
using System.Globalization;

namespace PortalAG_V2.Pages.Cheques
{
    public partial class Prorroga

    {
        //URls
        string UrlBscar = "Cheques/ListadoCheque/";

        string SerieCheque = "";
        bool _processingProtesto = false;
        bool Loading = false;
        //Borrar despues
        public string NroSerie = " 2008KK ";
        public int NroSerie2 = 2524582;
        public string Serial = "0XX-XXXXXXXXX";
        public int Monto = 0;
        public string Mes = "";
        public int Dia = 0;
        public int annoProceso = 0;
        public string RazonSocial = "NOMBRE APELLIDO";
        public bool Show = false;
        ChequeComponent componente = new ChequeComponent();
        public List<ChequesModel> ListCheques = new();
        public ChequesModel Cheques = new ChequesModel();

        MainServices service;


        private async Task GetCheque()
        {
            await BuscarChecque(SerieCheque);
        }

        //ToDo: HAcer boton de busqueda para que cargue los datos del cheque
        private async Task BuscarChecque(string serial)
        {
            try
            {
                Loading = true;
                Show = false;
                Cheques = new();
                ListCheques = new();
                service = new MainServices();
                var lista = await service.EstadoPedido.HttpClientInstance.GetAsync($"api/v2/{UrlBscar}{serial}");
                if (lista.IsSuccessStatusCode)
                {
                    ListCheques = JsonConvert.DeserializeObject<List<ChequesModel>>(await lista.Content.ReadAsStringAsync());
                    if (ListCheques?.Count > 1)
                    {
                        _processingProtesto = false;
                        Loading = false;
                        StateHasChanged();
                        var parameters = new DialogParameters<ModalMultipleCheques> {
                            { x => x.List, ListCheques },
                            { x => x.numeroSerie , serial}
                        };
                        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraExtraLarge };
                        var dialog = await DialogService.ShowAsync<ModalMultipleCheques>("Seleccionar Cliente", parameters, options);
                        var result = await dialog.Result;
                        var Data = (ChequesModel)result.Data;
                        if (Data != null)
                        {
                            Cheques = Data;
                            DateTime dateTime = DateTime.ParseExact(Cheques.FechaVencimiento, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            Dia = dateTime.Day;
                            Mes = dateTime.ToString("MMMM");
                            annoProceso = dateTime.Year;
                            Loading = false;
                            Show = true;
                            snakBarCreation("Encontrado!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                            StateHasChanged();
                        }

                    }
                    if (ListCheques.Count == 1 && ListCheques.FirstOrDefault().IDOperacion != null)
                    {
                        Cheques = ListCheques.FirstOrDefault();
                        DateTime dateTime = DateTime.ParseExact(Cheques.FechaVencimiento, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        Dia = dateTime.Day;
                        Mes = dateTime.ToString("MMMM");
                        annoProceso = dateTime.Year;
                        Loading = false;
                        Show = true;
                        snakBarCreation("Encontrado!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);

                        StateHasChanged();
                    }
                    if(ListCheques.Count == 0) 
                    {
                        Cheques = new ChequesModel();
                        ListCheques = new();
                        Loading = false;
                        Show = false;
                        //componente.Cheque = Cheques;
                        snakBarCreation("Error - Cheque no existes", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                        StateHasChanged();

                    }
                    StateHasChanged();

                }
                else
                {
                    Cheques = new ChequesModel();
                    ListCheques = new();
                    Loading = false;
                    Show = false;
                    //componente.Cheque = Cheques;
                    snakBarCreation("Error - Cheque no existes", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);
                    StateHasChanged();

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                Loading = false;
                string mensaje = e.Message;
            }
        }
        public async Task Limpiar()
        {
            Cheques = new();
            SerieCheque = "";
            Show = false;
            StateHasChanged();
        }

        #region SnackBar
        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }
        #endregion
    }
}
