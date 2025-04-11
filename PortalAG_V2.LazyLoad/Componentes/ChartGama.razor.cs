using ApexCharts;
using Microsoft.AspNetCore.Components;
using static PortalAG_V2.Shared.Models.ClienteEvaluacion.ClienteAdicionalModel;

namespace PortalAG_V2.LazyLoad.Componentes
{
    public partial class ChartGama
    {
        [Parameter]
        public List<Ventasporgama> LisGama { get; set; } = new List<Ventasporgama>();
        public ApexChartOptions<Ventasporgama> options;
        public ApexChart<Ventasporgama> chart;

    }
}
