using ApexCharts;
using Microsoft.AspNetCore.Components;
using static PortalAG_V2.Shared.Models.ClienteEvaluacion.ClienteAdicionalModel;

namespace PortalAG_V2.LazyLoad.Componentes
{
    public partial class ChartSubCategoria
    {
        [Parameter]
        public List<Ventasporsubcastegoria> ListSub { get; set; } = new List<Ventasporsubcastegoria>();
        public ApexChartOptions<Ventasporsubcastegoria> options;
        public ApexChart<Ventasporsubcastegoria> chart;

        protected override void OnInitialized()
        {
            options = new ApexChartOptions<Ventasporsubcastegoria>
            {
                PlotOptions = new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        Horizontal = true
                    }
                }
            };
        }
        public async Task UpdateChartSeriesCategorias()
        {
            StateHasChanged();
            //await chartGamaComponent.chart.ResetSeriesAsync(true,false);
            //await chartSubCategoriaComponent.chart.ResetSeriesAsync(true,false);
            await chart.UpdateSeriesAsync(true);
            //await chartGamaComponent.chart.RenderAsync();
            //await chartSubCategoriaComponent.chart.RenderAsync();


        }
    }
}
