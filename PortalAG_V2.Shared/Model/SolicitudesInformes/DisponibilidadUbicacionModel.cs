
namespace PortalAG_V2.Shared.Model.SolicitudesInformes
{

    public class DisponibilidadUbicacionModel
    {      
            public string IDArticulo { get; set; }
            public string IDBodega { get; set; }
            public int NroPallet { get; set; }
            public int UnidadPorBulto { get; set; }
            public int IDUbicacionHasta { get; set; }
            public string Ubicacion { get; set; }
            public int CantidadDisponibleUbicacion { get; set; }
            public int CantidadSolicitadoPedidos { get; set; }       

    }
}
