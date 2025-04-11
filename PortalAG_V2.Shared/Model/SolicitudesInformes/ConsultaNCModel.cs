using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.SolicitudesInformes
{
    public class ConsultaNCModel
    {
        public float NroSolicitudNC { get; set; }
        public string RazonSocial { get; set; }
        public float Lineas { get; set; }
        public string Fecha { get; set; }
        public string Usuario { get; set; }
        public float NroNotaCredito { get; set; }
        public int NroDocumentoRef { get; set; }
        public List<NCDetalleModel> DetalleNC { get; set; }
        public int IDOperacion { get; set; }
        public int Observacion { get; set; }
    }

    public class NCDetalleModel
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Correlativo { get; set; }
        public int Linea { get; set; }
        public int LineaRef { get; set; }
        public string IDArticulo { get; set; }
        public string Descripcion { get; set; }
        public int CantidadRef { get; set; }
        public int CantDevolver { get; set; }
        public int PrecioUnidad { get; set; }
        public int Total { get; set; }
        public string IDUsuario { get; set; }
        public string FechaActualizacion { get; set; }
    }
}
