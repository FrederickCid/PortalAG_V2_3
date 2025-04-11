namespace agDataAccess.Models
{
    public class BultosModel
    {
        public string IDBodega { get; set; }
        public int NroBulto { get; set; }
        public string FechaPasoJaula { get; set; }
        public string FechaUbicacion { get; set; }
        public string IDUsuarioPasoJaula { get; set; }
        public string UbicacionBultos { get; set; }
        public string FechaEntregado { get; set; }
        public string SalidaJaula { get; set; }
        public List<DetalleBultosModel> DetalleBultos { get; set; }
    }

    public class DetalleBultosModel
    {
        public int LineaDetalle { get; set; }
        public string IDArticulo { get; set; }
        public string IDBodega { get; set; }
        public int UnidadxBulto { get; set; }
        public int CantidadEnBulto { get; set; }
        public int IDEstado { get; set; }
        public string FechaActualizacion { get; set; }
    }

    public class FacturaEstadoPedidoModel
    {
        public int nroDocumeto { get; set; }
        public double Total { get; set; }
        public double PagadoAfecha { get; set; }
        public string Estado { get; set; }
    }
}
