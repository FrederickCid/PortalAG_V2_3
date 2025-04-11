

using agDataAccess.Models.Solicitudes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Componentes;
using PortalAG_V2.Services;
using PortalAG_V2.Shared.Helpers;
using PortalAG_V2.Shared.Services;
using SheriffDataAccess.Models.SheriffModel;
using System;

namespace PortalAG_V2.Pages.Reposiciones
{
    public partial class ReposicionesPendientes
    {
        private string URLGetReposiciones = "api/v2/Solicitud/Reposiciones/Pendientes/";
        public List<ReposicionesPendientesModel> ReposicionesGet = new List<ReposicionesPendientesModel> { };
        public MainServices? service;
        private Timer timer;
        private int Count = 120;
        bool _Loading = false;
        private string _searchString;
        [Inject] ExportService exportService { get; set; }
        [Inject] IJSRuntime js { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await GetReposiciones();
            StartCountdown();
        }
        public async Task GetReposiciones()
        {
            try
            {
                service = new MainServices();
                var lista = await service.ConectionServiceNotaCredito.HttpClientInstance.GetAsync($"{URLGetReposiciones}");
                if (lista.IsSuccessStatusCode)
                {
                    ReposicionesGet = JsonConvert.DeserializeObject<List<ReposicionesPendientesModel>>(await lista.Content.ReadAsStringAsync());
                    // snakBarCreation("Actualizado!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                else
                {
                    //snakBarCreation("Sin Reposcionoes!", Defaults.Classes.Position.BottomStart, Severity.Success, 1000);
                }
                StateHasChanged();

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }
        public async Task Actualizar()
        {
            try
            {
                _Loading = true;
                await GetReposiciones();
                Count = 120;
                _Loading = false;

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }


        public void StartCountdown()
        {
            try
            {
                timer = new Timer(new TimerCallback(async (e) =>
                {
                    if (Count <= 0)
                    {
                        await Actualizar();
                        Count = 120;
                    }
                    else
                    {
                        Count--;
                    }
                    StateHasChanged();
                }), null, 1000, 1000);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
        }

        #region SnackBar


        private void snakBarCreation(string msj, string position, MudBlazor.Severity style, int duration)
        {
            _snackBar.Configuration.VisibleStateDuration = duration;
            _snackBar.Configuration.PositionClass = position;
            _snackBar.Add(msj, style);
        }

        #endregion SnackBar
        private Func<ReposicionesPendientesModel, bool> QuickFilter => x =>
        {
            //si no encuetra nada no manda nada
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            //Buscar por NRO de Documento 
            if (x.IDArticulo.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Buscar Por CLiente
            if (x.IDArticulo.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Busqueda mas amplia de los dos anterirore y agregando al vendedor, para mostrar multiples
            if ($"{x.IDArticulo} ".Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        public async Task GenerarPDFPrePacking()
        {
            using (MemoryStream memory = exportService.CreatePdfReposiciones(ReposicionesGet))
            {
                await js.SaveAs($"Guia Reposiciones {DateTime.Today.ToString("dd/MM/yyyy")}.pdf", memory.ToArray());
            }
        }


        async Task exportPdf()
        {
            await javaScript.GenerarPdfReposicionesPendientes($"Reposiciones {DateTime.Today.ToString("dd/MM/yyyy")}", ReposicionesGet);
        }
       
    }
}
