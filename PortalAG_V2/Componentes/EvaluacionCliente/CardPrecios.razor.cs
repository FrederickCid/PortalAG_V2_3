using Microsoft.AspNetCore.Components;

namespace PortalAG_V2.Componentes.EvaluacionCliente
{
    public partial class CardPrecios
    {
        [Parameter]
        public string titulo { get; set; } = "0";
        [Parameter]
        public string Imagen { get; set; } = "https://imgs.andesindustrial.cl/footer/marcas/andes.svg";
        [Parameter]
        public long Precio { get; set; } = 0;  
        [Parameter]
        public long dias { get; set; } = 0;
        [Parameter]
        public int opcion { get; set; } = 1;
        [Parameter]
        public int Ranting { get; set; } = 0;


        public string transformShortValue(int num)
        {

            if (num.ToString().Length == 6)
            {
                return num.ToString().Substring(0, 3) + " K";
            }
            if (num.ToString().Length == 7)
            {
                return num.ToString().Substring(0, 1) + " M";
            }
            if (num.ToString().Length == 8)
            {
                return num.ToString().Substring(0, 2) + " M";
            }
            if (num.ToString().Length == 9)
            {
                return num.ToString().Substring(0, 3) + " M";
            }
            if (num.ToString().Length == 10)
            {
                return num.ToString().Substring(0, 4) + " M";
            }
            if (num.ToString().Length < 6 && num.ToString().Length > 1)
            {
                return num.ToString("###,###,###");
            }
            else 
            {             
                return null;
            }
        }
        public string transformShortValue(long num)
        {

            if (num.ToString().Length == 6)
            {
                return num.ToString().Substring(0, 3) + " K";
            }
            if (num.ToString().Length == 7)
            {
                return num.ToString().Substring(0, 1) + " M";
            }
            if (num.ToString().Length == 8)
            {
                return num.ToString().Substring(0, 2) + " M";
            }
            if (num.ToString().Length == 9)
            {
                return num.ToString().Substring(0, 3) + " M";
            }
            if (num.ToString().Length == 10)
            {
                return num.ToString().Substring(0, 4) + " M";
            }
            if (num.ToString().Length < 6 && num.ToString().Length > 1)
            {
                return num.ToString("###,###,###");
            }
            else
            {
                return null;
            }
        }
    }
}
