using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agDataAccess.Models.Despacho1
{

    public class HRReimpresionModel
    {
        public string MsgResult { get; set; }
        public string MsgMensaje { get; set; }
        public int IDAllGestEmpresa { get; set; }
        public int IDEmpresa { get; set; }
        public int AnnoProceso { get; set; }
        public int IDGuiaHojaRuta { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }
        public int ZonaD { get; set; }
        public int IDEstadoHR { get; set; }
        public string IDUsuario { get; set; }
        public int Dispositivo { get; set; }
        public List<DetalleHojaRuta> Detalle { get; set; }
    }

    public class DetalleHojaRuta
    {
        public int Correlativo { get; set; }
        public int AnnoProceso { get; set; }
        public int IDTipo { get; set; }
        public long IDOperacion { get; set; }
        public string UbicacionD { get; set; }
        public DateTime FechaUbicacionD { get; set; }
        public int IDEstadoDetalle { get; set; }
        public DateTime FechaIngresoHoja { get; set; }
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
        public DateTime FechaActualizacion { get; set; }
    }
}
