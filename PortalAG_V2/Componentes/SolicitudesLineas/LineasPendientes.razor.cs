using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaLineasPickingPacking;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Componentes.SolicitudesLineas
{
    public partial class LineasPendientes
    {

        [Parameter]
        public Func<Task> funcionActualizar { get; set; }
        MainServices service;
        private List<CantidadLineadModel> Lineas = new List<CantidadLineadModel>();
        string UrlLineas = "Lineas/LineasPorPickear";
        private bool Loading = false;

        public async Task CargarDatosLineas()
        {
            try
            {
                service = new MainServices();
                var lista = await service.SolcitudInformes.HttpClientInstance.GetAsync($"api/v2/{UrlLineas}");
                if (lista.IsSuccessStatusCode)
                {
                    Lineas = JsonConvert.DeserializeObject<List<CantidadLineadModel>>(await lista.Content.ReadAsStringAsync());
                    Loading = false;
                }
                else
                {
                    Lineas = new List<CantidadLineadModel>();
                    Loading = false;
                    snakBarCreation("Error!", Defaults.Classes.Position.BottomStart, Severity.Error, 1000);

                }
                StateHasChanged();
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await CargarDatosLineas();
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
