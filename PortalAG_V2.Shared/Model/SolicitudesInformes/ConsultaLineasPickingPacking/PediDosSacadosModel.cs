
namespace agDataAccess.Models.ConsultaLineasPickingPacking
{
    public class PediDosSacadosModel
    {
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Correlativo { get; set; }
        public int NroDocumento { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public int TipoEntrega { get; set; }
        public string Transporte { get; set; }
        public int NroOrden { get; set; }
        public int IDEtapa { get; set; }
        public int IDEstado { get; set; }
        public string Sacador { get; set; }
        public string FechaAutorizacion { get; set; }
        public string FechaEntregaEstimada { get; set; }
        public string FechaInicioSacado { get; set; }
        public string FechaTerminoSacado { get; set; }
        public string Region { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public int MontoTotal { get; set; }
        public int Lineas { get; set; }
        public string Vendedor { get; set; }
    }
}
