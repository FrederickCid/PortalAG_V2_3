namespace PortalAG_V2.Shared.Model.SolicitudesInformes
{
    public class InformeDiferenciaModel
    {
        public int NroPedido { get; set; }
        public int IDOperacion { get; set; }
        public int Linea { get; set; }
        public string IDArticulo { get; set; }
        public int Cantidad { get; set; }
        public int PrecioVenta { get; set; }
        public string Fecha { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public string IDUsuario { get; set; }
        public int IDEtapa { get; set; }
        public int IDEstado { get; set; }
    }
}

