using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.SolicitudesInformes
{
    public class ConsultaPedidosEnJaulaModel
    {
        public int NroDocumento { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public string RUT { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string TipoEntrega { get; set; }
        public string Transporte { get; set; }
        public List<GrupocontableModel> GrupoContable { get; set; }
        public string FechaPedido { get; set; }
        public string TerminoRevision { get; set; }
        public string FechaPasoJaula { get; set; }
        public int Bultos { get; set; }
        public string Ubicacion { get; set; }
        public string Revisador { get; set; }
        public string Despachador { get; set; }
        public string Facturas { get; set; }
        public int TotalFacturas { get; set; }
        public int SaldoxPagar { get; set; }
        public string NotasDeCreditos { get; set; }
        public int MontoNC { get; set; }
        public string Estado { get; set; }
        public int ReceiptNum { get; set; }
        public string Vendedor { get; set; }


        public class GrupocontableModel
        {
            public string GrupoContableAgrupa { get; set; }
        }

    }
}
