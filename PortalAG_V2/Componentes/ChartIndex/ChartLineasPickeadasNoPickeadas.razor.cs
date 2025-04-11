using ApexCharts;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Components;
using PortalAG_V2.Shared.Model.SolicitudesInformes;
using System.Reflection.Metadata;

namespace PortalAG_V2.Componentes.ChartIndex
{
    public partial class ChartLineasPickeadasNoPickeadas
    {
        [Parameter]
        public List<LineasPickeadasNoPickeadasModel> Lista { get; set; } = new List<LineasPickeadasNoPickeadasModel> { };

        public ApexChart<LineasPickeadasNoPickeadasModel> chartPickeadasNoPickeadas;

        private string GetPointColor(LineasPickeadasNoPickeadasModel Lista)
        {
            switch (Lista.Cabecera)
            {
                case "NoPickeadas":
                    return "#ff0000";
                case "Pickeadas":
                    return "#00ff00";
                default:
                    return "#87775d";
            }
        }
    }
}
