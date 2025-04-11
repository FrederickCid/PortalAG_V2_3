using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class ProductosNCDTO
    {
        public int Linea { get; set; }
        public string? Fecha { get; set; }
        public string? IDArticulo { get; set; }
        public int CorrelativoArticulo { get; set; }
        public string? Nombre { get; set; }
        public int Cantidad { get; set; }
        public double PrecioVenta { get; set; }
        public int StockAlComprar { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string? IDUsuario { get; set; }
        public string? DescripcionUbicacion { get; set; }
        public string? IDUbicacion { get; set; }
        public int IDOrigen { get; set; }
        public int IDArticuloNumber { get; set; }
    }
}
