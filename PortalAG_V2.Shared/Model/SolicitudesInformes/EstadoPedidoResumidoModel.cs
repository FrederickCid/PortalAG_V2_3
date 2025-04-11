namespace PortalAG_V2.Shared.Model.SolicitudesInformes
{
    public class EstadoPedidoResumidoModel
    {
        public int NumeroPedido { get; set; }
        public string RazonSocial { get; set; }
        public string Rut { get; set; }
        public string Fecha { get; set; }
        public int CantidadBultos { get; set; }
        public int CantidadLineas { get; set; }
        public string CondicionDespacho { get; set; }
        public string Estado { get; set; }

    }
}
