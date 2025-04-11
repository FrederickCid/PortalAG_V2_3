using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Formularios
{
    public class DetalleFacturaModel
    {
        public int Linea { get; set; }
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser menor que 0.")]
        public int Cantidad { get; set; }
        public int PrecioVenta { get; set; }
    }

    public class DetalleFacturaModelRequest
    {
        public int Linea { get; set; }
        public string IDArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public int Precio { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser menor que 0.")]
        public int Cantidad { get; set; }
        public int Total { get; set; }
    }

    public class DetalleFacturaModelDTO
    {
        public int Linea { get; set; }
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser menor que 0.")]
        public int Cantidad { get; set; }
        public int PrecioVenta { get; set; }
        public int Total { get; set; }
    }

    public class DetalleFacturaImagenModelDTO
    {
        public int Linea { get; set; }
        public string NombreImagen { get; set; }
        public string UrlImagen { get; set; }
    }
}
