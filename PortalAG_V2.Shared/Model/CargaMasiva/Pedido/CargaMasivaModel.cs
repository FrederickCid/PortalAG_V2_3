using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Models.CargaMasiva.Pedidos
{
    public class CargaMasivaModel
    {

        // Datos de Pedido
        public string NroOrden { get; set; }
        public string NroF12 { get; set; }
        public string FechaEmision { get; set; }
        public string RutCliente { get; set; }
        public int IDFormaPago { get; set; }  // POR DEFINIR
        public int IDCondicionPago { get; set; } // POR DEFINIR
        public int TipoEntrega { get; set; } // POR DEFINIR
        public int SiUrgencia { get; set; }// 0
        public int IDTransporte { get; set; }// POR DEFINIR
        public string Referencia { get; set; }// REFERENCIA MANDAR EN BLANCO
        public string Comentarios { get; set; }// AGREGAR EXCEL(VER SI HAY QUE AGREGAR MAS COMENTARIOS)
        public int TipoDocumento { get; set; } // 1 CHECK OCULTO PARA TI HABILITAR EL SI URGENCIA


        public List<CargaMasivaDetalleModel> DetallePedido { get; set; } // detalle de pedido

    }
    public class CargaMasivaModelValidada
    {

        public string NroOrden { get; set; }
        public string NroF12 { get; set; }
        public string FechaEmision { get; set; }
        public string RutCliente { get; set; }
        public int IDFormaPago { get; set; }  // POR DEFINIR
        public int IDCondicionPago { get; set; } // POR DEFINIR
        public int TipoEntrega { get; set; } // POR DEFINIR
        public int SiUrgencia { get; set; }// 0
        public int IDTransporte { get; set; }// POR DEFINIR
        public string Referencia { get; set; }// REFERENCIA MANDAR EN BLANCO
        public string Comentarios { get; set; }// AGREGAR EXCEL(VER SI HAY QUE AGREGAR MAS COMENTARIOS)
        public int TipoDocumento { get; set; } // 1 CHECK OCULTO PARA TI HABILITAR EL SI URGENCIA
        public List<CargaMasivaDetalleModelValidada> DetallePedido { get; set; } // detalle de pedido

        public int Estado { get; set; }
        public int Linea { get; set; }
        public string Error { get; set; }

    }
    public class CargaMasivaDetalleModel
    {
        public string SKU { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int precio { get; set; }
        public int TotalProducto { get; set; }
    }
    public class CargaMasivaDetalleModelValidada
    {
        public string SKU { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int precio { get; set; }
        public int TotalProducto { get; set; }
        public int Estado { get; set; }
        public int Linea { get; set; }
        public string Error { get; set; }
        public int IDEstado { get; set; }
        public string ErrDescripcion { get; set; }

    }
    public class CargaMasivaModelResponse
    {

        public string NroOrden { get; set; }
        public string NroF12 { get; set; }
        public string FechaEmision { get; set; }
        public string RutCliente { get; set; }
        public int IDFormaPago { get; set; }  // POR DEFINIR
        public int IDCondicionPago { get; set; } // POR DEFINIR
        public int TipoEntrega { get; set; } // POR DEFINIR
        public int SiUrgencia { get; set; }// 0
        public int IDTransporte { get; set; }// POR DEFINIR
        public string Referencia { get; set; }// REFERENCIA MANDAR EN BLANCO
        public string Comentarios { get; set; }// AGREGAR EXCEL(VER SI HAY QUE AGREGAR MAS COMENTARIOS)
        public int TipoDocumento { get; set; } // 1 CHECK OCULTO PARA TI HABILITAR EL SI URGENCIA
        public List<CargaMasivaDetalleModelValidada> DetallePedido { get; set; } // detalle de pedido

        // Pedido Andes
        public int NroPedido { get; set; }
        public int IDOperacion { get; set; }
        public string NotasPedido { get; set; }

    }
}
