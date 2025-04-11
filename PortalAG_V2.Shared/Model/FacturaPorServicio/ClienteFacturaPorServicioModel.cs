using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.FacturaPorServicio
{
    public class ClienteFacturaPorServicioModel
    {
        public string? IDCliente { get; set; }
        public int Correlativo { get; set; }
        public string? Rut { get; set; }
        public string? Digito { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Address { get; set; }
        public int IDCiudad { get; set; }
        public string? Ciudad { get; set; }
        public int IDComuna { get; set; }
        public string? Comuna { get; set; }
        public string? Nose { get; set; }
        public string? Phone1 { get; set; }
        public string? Cellular { get; set; }
        public string? Fax { get; set; }
        public string? EMail { get; set; }
        public string? Contacto { get; set; }
        public int TipoCliente { get; set; }
        public int LimiteCredito { get; set; }
        public int IDTransporte { get; set; }
        public string? Transporte { get; set; }
        public string? Oficina { get; set; }
        public int SiObsequio { get; set; }
        public int SiEtiqueta { get; set; }
        public int SiListaPrecio { get; set; }
        public int IDFormaPago { get; set; }
        public string? FormaPago { get; set; }
        public string? ComentarioFormaPago { get; set; }
        public string? IDCodigoAsociado { get; set; }
        public string? NombreAsociado { get; set; }
        public int IDEstado { get; set; }
        public string? FechaActualizacion { get; set; }
        public string? IDUsuario { get; set; }
        public int CantidadFormaPago { get; set; }
        public int ComienzoDiasFormaPago { get; set; }
        public int Otro { get; set; }
        public string? Otro2 { get; set; }
        public int SiClientePremium { get; set; }
        public int PorcentajePremium { get; set; }
        public int SiClienteMesuca { get; set; }
        public int SiReajuste { get; set; }
        public int PorcentajeReajuste { get; set; }
        public int SiClienteWeb { get; set; }
        public string? Otro3 { get; set; }
        public int CondicionPago { get; set; }
        public string? DireccionPorDefecto { get; set; }
        public int PuntosAcumulados { get; set; }
        public int SiClientePuntos { get; set; }
        public string? Giro { get; set; }
        public int SiDatosActualizados { get; set; }
        public string? IDPais { get; set; }
        public string? Pais { get; set; }
        public int SiVentaEspecial { get; set; }
        public int FactorVentaEspecial { get; set; }
        public string? FechaUltimoImpago { get; set; }
        public int DeudaAnterior { get; set; }
        public int DeudaTotal { get; set; }
        public int IDEstadoBloqueado { get; set; }
        public string? TextoTipoCliente { get; set; }
        public int IDVendedor { get; set; }
        public int IDRegion { get; set; }
        public int PorcentajeDescuento { get; set; }
        public int DescuentoSobre { get; set; }
        public string? DireccionFacturaDefecto { get; set; }
        public string? DireccionDespachoDefecto { get; set; }
        public string? TipoSiCliente { get; set; }
        public string? AreaCelular { get; set; }
        public string? NroCelular { get; set; }
        public string? RutContacto { get; set; }
        public string? DigitoContacto { get; set; }
        public string? MailVendedor { get; set; }
        public int SiClienteEstablecido { get; set; }
        public int SiClienteinternet { get; set; }
        public int SiClienteAmbulante { get; set; }
        public int SiClienteProductora { get; set; }
        public int SiClienteGimnasio { get; set; }
        public int SiClienteHotel { get; set; }
        public int SiClienteInstitucion { get; set; }
        public int SiClienteTrabajador { get; set; }
    }
}
