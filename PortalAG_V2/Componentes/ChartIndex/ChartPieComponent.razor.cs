using ApexCharts;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Components;
using PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaLineasPickingPacking;


namespace PortalAG_V2.Componentes.ChartIndex
{
    public partial class ChartPieComponent : ComponentBase
    {
        [Parameter]
        public List<lineasPickingModel> Lista { get; set; } = new List<lineasPickingModel>();
        [Parameter]
        public List<lineasPickingModel> Lista2 { get; set; } = new List<lineasPickingModel>();
        [Parameter]
        public List<lineasPickingModel> Lista3 { get; set; } = new List<lineasPickingModel>();
      
        [Parameter]
        public List<lineasPackingModel> ListLineasPacking { get; set; } = new List<lineasPackingModel>();
        [Parameter]
        public List<lineasDespachoModel> ListLineasDespacho { get; set; } = new List<lineasDespachoModel>();
        [Parameter]
        public List<lineasReposicionModel> ListLineasReposicion { get; set; } = new List<lineasReposicionModel>();

        public ApexChartOptions<lineasPackingModel> options { get; set; } = new();
        public ApexChartOptions<lineasReposicionModel> optionsChartReposicion { get; set; } = new();
        public ApexChartOptions<lineasPickingModel> optionsChartlineas { get; set; } = new();
        public ApexChart<lineasPickingModel> chartPickeadas;
        public ApexChart<lineasPickingModel> chartPickeadas2;
        public ApexChart<lineasPickingModel> chartPickeadas3;
        public ApexChart<lineasPackingModel> chartPacking;
        public ApexChart<lineasReposicionModel> chartReposicion;

        protected override void OnInitialized()
        {
            optionsChartReposicion.DataLabels = new ApexCharts.DataLabels
            {
                Formatter = @"function(value, { seriesIndex, w }) {
            return w.config.series[seriesIndex];}"
            };
            //optionsChartlineas.DataLabels = new ApexCharts.DataLabels
            //{
            //    Formatter = @"function(value, { seriesIndex, w }) {
            //return w.config.series[seriesIndex];}"
            //};
            options.DataLabels = new ApexCharts.DataLabels
            {
                Formatter = @"function(value, { seriesIndex, w }) {
            return w.config.series[seriesIndex];}"
            };
        }      
    }
}



