using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.NotaDeCredito
{
    public class DetalleClienteNCDTO
    {
        public string IDCliente { get; set; }
        public string Fecha { get; set; }
        public string FechaVencimiento { get; set; }
        public string Rut { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public int IDComuna { get; set; }
        public string  Comuna { get; set; }
        public int IDCiudad { get; set; }
        public string  Ciudad { get; set; }
        public string  Fono { get; set; }
        public int Descuentos { get; set; }
        public int Interes { get; set; }
        public float OtrosValores { get; set; } // Porcentaje descuento
        public int Neto { get; set; }
        public float IVAFecha { get; set; }
        public int IVA { get; set; }
        public int Total { get; set; }
        public int TotalFactura { get; set; }
        public string FechaDocumento { get; set; }
        public int IDTransporte { get; set; }
        public string Transporte { get; set; }
        //public int IDFormaPago { get; set; }
        //public string FormaPago { get; set; }
        //public int CantidadFormaPago { get; set; }
        public int IDVendedor { get; set; }
        public string  Vendedor { get; set; }
        public string  FechaActualizacion { get; set; }
        public string  IDUsuario { get; set; }
        //public int SiEmbalaje { get; set; }
        public int IDTipoOperacion { get; set; }
        //public int Folio { get; set; }
        public int IDEstado { get; set; }
        public int NroDocumento { get; set; }
        public int Correlativo { get; set; }
        //public string Observacion { get; set; }
        //public int IDRevisador { get; set; }
        //public string Revisador { get; set; }
        //public int IDSacador { get; set; }
        //public string Sacador { get; set; }
        public int IDFacturador { get; set; }
        public string  Facturador { get; set; }
        //public string FechaAnterior { get; set; }
        public int SaldoAnterior { get; set; }
        public int SaldoDocumento { get; set; }
        //public int IDCondicionPago { get; set; }
        //public string CondicionPago { get; set; }
        public string  Comentarios { get; set; }
        public string  AddressDef { get; set; }
        //public int PuntosUsar { get; set; }
        //public string NumeroOC { get; set; }
        //public int SiVentaBicicleta { get; set; }
        //public string Localidad { get; set; }
        public int SiPedidoWeb { get; set; }
        public int NumeroWeb { get; set; }
        //public int SiVentaDetalle { get; set; }
        public int NroDocumentoAntes { get; set; }
        //public string PorcentajeDescuento { get; set; }
        //public string RutaImagen { get; set; }
        public string  msgResult { get; set; }
    }
}
