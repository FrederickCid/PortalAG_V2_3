using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class ProductoNCDTODevolver 
    {
        public int Linea { get; set; }
        public string? IDArticulo { get; set; }
        public int Cantidad { get; set; }
        public string? Nombre { get; set; }
        public int PrecioVenta { get; set; }
        public int CantidadADevolver { get; set; }
        public int Total { get; set; }
    }
}
