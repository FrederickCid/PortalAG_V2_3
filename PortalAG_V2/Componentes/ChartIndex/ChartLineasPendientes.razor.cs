using ApexCharts;
using Microsoft.AspNetCore.Components;
using PortalAG_V2.Shared.Model.ChartsModel;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaLineasPickingPacking;

namespace PortalAG_V2.Componentes.ChartIndex
{
    public partial class ChartLineasPendientes
    {
        private ApexChartOptions<LineasPendientesModelDTO> options = new();
        public ApexChart<LineasPendientesModelDTO> chartLineasPendientes;
        [Parameter]
        public string nombre { get; set; }
        [Parameter]
        public List<LineasPendientesModelDTO> ListaLineasPendientes { get; set; } = new List<LineasPendientesModelDTO>();

        protected override void OnInitialized()
        {
            //options.PlotOptions = new PlotOptions { Pie = new PlotOptionsPie { StartAngle = -90, EndAngle = 90 } };
            options.Legend = new Legend { Position = LegendPosition.Top, FontSize = "18px", HorizontalAlign = Align.Center };
            options.PlotOptions = new PlotOptions
            {
                
                Pie = new PlotOptionsPie
                {
                    StartAngle = -90,
                    EndAngle = 90,
                    Donut = new PlotOptionsDonut
                    {     
                        
                        //Labels = new DonutLabels
                        //{
                        //    Show = true,
                        //    Total = new DonutLabelTotal
                        //    {
                               
                        //        FontSize = "20px",
                        //        Color = "#000000",
                        //        Formatter = @"function (w) {return w.globals.seriesTotals.reduce((a, b) => { return (a + b) }, 0)}"
                        //    }
                        //}
                    }
                }
            };
            //options.Fill = new Fill
            //{
            //    Type = FillType.Gradient,
            //};            
            options.Stroke = new Stroke
            {
                Width = 0,
            };
        }


        private string GetPointColor(LineasPendientesModelDTO Lista)
        {
            switch (Lista.label)
            {
                case "Total Rojo":
                    return "#ff0000";
                case "Total Verde":
                    return "#00ff00";
                default:
                    return "#87775d";
            }
        }

    }
}
