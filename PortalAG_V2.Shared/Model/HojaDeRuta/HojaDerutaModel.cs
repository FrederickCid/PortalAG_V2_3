using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.HojaDeRuta
{
    public class HojaDerutaModel
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int Correlativo { get; set; }
        public int AnnoProceso { get; set; }
        public int IDGuiaHojaRuta { get; set; }
        public string FechaInicio { get; set; }
        public string FechaTermino { get; set; }
        public string ZonaD { get; set; }
        public int IDEstado { get; set; }
        public string IDUsuario { get; set; }
        public int Dispositivo { get; set; }
    }

    public class HojaDerutaDetalleModel
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int Correlativo { get; set; }
        public int AnnoProceso { get; set; }
        public int IDGuiaHojaRuta { get; set; }
        public int IDTipo { get; set; }
        public int IDOperacion { get; set; }
        public string UbicacionD { get; set; }
        public object FechaUbicacionD { get; set; }
        public int IDEstado { get; set; }
        public DateTime FechaIngresoHoja { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string IDUsuario { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Region { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
    }
    public class HojaRutaDetalleDevolucion
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int Correlativo { get; set; }
        public int AnnoProceso { get; set; }
        public int IDGuiaHojaRuta { get; set; }
        public int IDTipo { get; set; }
        public int NroSolicitud { get; set; }
        public string DocReferencia { get; set; }
        public int IDEstado { get; set; }
        public int NroCajas { get; set; }
        public int TotalCheques { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaActualizacion { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Region { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
        public string Observaciones { get; set; }

    }
    public class HojaRutaDetalleCompleto
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int Correlativo { get; set; }
        public int AnnoProceso { get; set; }
        public int IDGuiaHojaRuta { get; set; }
        public int IDTipo { get; set; }
        public int IDOperacion { get; set; }
        public string UbicacionD { get; set; }
        public object FechaUbicacionD { get; set; }
        public int IDEstado { get; set; }
        public DateTime FechaIngresoHoja { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string IDUsuario { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Region { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
        public int NroSolicitud { get; set; }
        public string DocReferencia { get; set; }
        public int NroCajas { get; set; }
        public int TotalCheques { get; set; }
        public string FechaIngreso { get; set; }
        public string Observaciones { get; set; }
    }
    public class RequestHojaRuta
    {
        public int IDOperacion { get; set; }
        public string CodigoBarra { get; set; }
        public int Correlativo { get; set; }
        public int IDHojaRuta { get; set; }
        public int Zona { get; set; }
        public int TipoConsulta { get; set; }
        public string IDUsuario { get; set; }
        public int Dispositivo { get; set; }
    }

    public class RequestHojaRutaTermino
    {
        public int IDHojaRuta { get; set; }
        public int Zona { get; set; }
    } 

    public class ConsultaDisponiblesHojaDeRutaModel
    {
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int Correlativo { get; set; }
        public int IDOperacion { get; set; }
        public int NroDocumento { get; set; }
        public int NroBultos { get; set; }
        public int Tipo { get; set; }
        public int TipoDoc { get; set; }
        public string Descripcion { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
        public string Region { get; set; }
        public string Transporte { get; set; }
    }

    public class ResponseHojaruta
    {
        public string? msgResult { get; set; }
        public string? msgMensaje { get; set; }
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int IDGuiaHojaRuta { get; set; }
        public string? FechaInicio { get; set; }
        public string? Fechatermino { get; set; }
        public int ZonaD { get; set; }
        public int IDEstadoHR { get; set; }
        public string? IDUsuario { get; set; }
        public int Dispositivo { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string detail { get; set; }
        public List<RutaDetalle> Detalle { get; set; }
    }

    public class RutaDetalle
    {
        public int Correlativo { get; set; }
        public int AnnoProceso { get; set; }
        public int IDTipo { get; set; }
        public int IDOperacion { get; set; }
        public string UbicacionD { get; set; }
        public int IDEstadoDetalle { get; set; }
        public string FechaIngresoHoja { get; set; }
        public int DocReferencia { get; set; }
        public int NroCajas { get; set; }
        public int TotalCheques { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Region { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
        public string Observaciones { get; set; }
        public string FechaActualizacion { get; set; }
    }

    public class RequestModificarHR
    {
        public int nroPedido { get; set; }
        public int IDHojaDeRuta { get; set; }
        public string IDusuario { get; set; }

    }


}
