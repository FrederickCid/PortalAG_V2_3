using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using PortalAG_V2.Shared.Model.Impresion;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Services;

namespace PortalAG_V2.Pages.GrillaInventario
{

    public partial class VisualizadorInventario
    {
        private int _filterValue = 0;
        MainServices service = new MainServices();
        string URLConteo = "/api/v2/VizualisadorConteo/";

        public class LegendItem
        {
            public string Color { get; set; }
            public string Label { get; set; }
            public string Icon { get; set; }
        }

        protected override async Task OnInitializedAsync()
        {
            await ConsultarBodegas();
        }

        #region data
        List<InventarioVizualisadorModel> Data = new() {};
        #endregion

        private async Task ConsultarBodegas()
        {
            service = new MainServices();
            var result = await service.EstadoPedidoTest.HttpClientInstance.GetAsync($"{URLConteo}BV_BIT/3");
            if (result.IsSuccessStatusCode)
            {
                Data = new();
                Data = JsonConvert.DeserializeObject<List<InventarioVizualisadorModel>>(await result.Content.ReadAsStringAsync());
            }
        }

        List<LegendItem> _legendItems = new()
    {
        new() { Color = "#ffffff", Label = "En espera", Icon = Icons.Material.Filled.HourglassEmpty },
        new() { Color = "#d8f3dc", Label = "Primera revisión", Icon = Icons.Material.Filled.PlayCircleOutline },
        new() { Color = "#b7e4c7", Label = "En espera 2da revisión", Icon = Icons.Material.Filled.HourglassFull },
        new() { Color = "#95d5b2", Label = "2da revisión terminada", Icon = Icons.Material.Filled.DoneAll },
        new() { Color = "#74c69d", Label = "En espera 3ra revisión", Icon = Icons.Material.Filled.HourglassFull },
        new() { Color = "#ff758f", Label = "Error", Icon = Icons.Material.Filled.Warning }
    };

        private List<int> Calles => Data
        .Select(d => d.Calle)
        .Distinct()
        .OrderBy(c => c)
        .ToList();

        private List<int> Tramos => Data
            .Select(d => d.Tramo)
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        private List<int> FilteredCalles =>
            _filterValue == 0 ? Calles : Calles
                .Where(c => Data.Any(d => d.Calle == c && FilterCondition(d.Estado)))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

        private List<int> FilteredTramos =>
            _filterValue == 0 ? Tramos : Tramos
                .Where(t => Data.Any(d => d.Tramo == t && FilterCondition(d.Estado)))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

        private bool FilterCondition(int estado)
        {
            return _filterValue switch
            {
                1 => estado is 1 or 2 or 4,    // En progreso
                2 => estado is 3,              // Completados
                3 => estado == 5,              // Con errores
                _ => true                      // Todos
            };
        }

        private bool ShouldDisplayCell(int calle, int tramo)
        {
            if (_filterValue == 0) return true;

            var item = Data.FirstOrDefault(d => d.Calle == calle && d.Tramo == tramo);
            return item != null && FilterCondition(item.Estado);
        }

        private string GetComentario(int calle, int tramo)
        {
            var item = Data.FirstOrDefault(d => d.Calle == calle && d.Tramo == tramo);
            return item?.Comentario ?? "Estado desconocido";
        }

        private string GetHeatmapColor(int estado)
        {
            return estado switch
            {
                0 => "background-color: #ffffff;",    // Blanco
                1 => "background-color: #d8f3dc;",   // Verde muy claro
                2 => "background-color: #b7e4c7;",   // Verde claro
                3 => "background-color: #95d5b2;",   // Verde medio
                4 => "background-color: #74c69d;",   // Verde oscuro
                5 => "background-color: #ff758f;",   // Rojo claro
                _ => "background-color: #ffffff;"
            };
        }

        private Color GetBadgeColor(int estado)
        {
            return estado switch
            {
                5 => Color.Error,
                3 => Color.Success,
                _ => Color.Default
            };
        }

        private string GetIcon(int estado)
        {
            return estado switch
            {
                0 => Icons.Material.Filled.HourglassEmpty,
                1 => Icons.Material.Filled.PlayCircleOutline,
                2 => Icons.Material.Filled.HourglassFull,
                3 => Icons.Material.Filled.DoneAll,
                4 => Icons.Material.Filled.HourglassFull,
                5 => Icons.Material.Filled.Warning,
                _ => Icons.Material.Filled.Help
            };
        }

        private int GetEstado(int calle, int tramo)
        {
            var item = Data.FirstOrDefault(d => d.Calle == calle && d.Tramo == tramo);
            return item?.Estado ?? 0;
        }


    }
}
