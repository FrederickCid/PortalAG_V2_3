

namespace PortalAG_V2.Shared.Model.CargaMasiva.Articulos
{
    public class AticuloPrecioModel
    {
        public int Linea { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int PrecioAnterior { get; set; }
        public int PrecioNuevo { get; set; }

    }  
    public class AticuloPrecioValidationModel
    {
        public int Linea { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int PrecioAnterior { get; set; }
        public int PrecioNuevo { get; set; }
        public string Error { get; set; }
    }
}
