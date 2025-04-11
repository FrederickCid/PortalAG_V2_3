using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class RequestIngresaSolicitudNC
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Correlativo { get; set; }
        public int TipoOperacion { get; set; }
        public int IDTipo { get; set; }
        public int IDTipoSiDevolucion { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public int Subtotal { get; set; }
        public int Neto { get; set; }
        public int Descto { get; set; }
        public int IVA { get; set; }
        public int Total { get; set; }
        public int AnnoProcesoRef { get; set; }
        public int IDOperacionRef { get; set; }
        public int CorrelativoRef { get; set; }
        public int TipoOperacionRef { get; set; }
        public int NroFacturaRef { get; set; }
        public string FechaFacturaRef { get; set; }
        public int MontoFacturaRef { get; set; }
        public int IDGuia { get; set; }
        public int IDOperacionNuevo { get; set; }
        public int IDEstado { get; set; }
        public int TipoConsulta { get; set; }
        public string IDUsuario { get; set; }
    }
}
