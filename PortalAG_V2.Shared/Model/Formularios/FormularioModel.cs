using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Formularios
{
    public class FormularioModel
    {

        public string? msgResult { get; set; }
        public int NroSolicitud { get; set; }
        public string IDCliente { get; set; }
        public string? RazonSocial { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public int NroDocumento { get; set; }
        public int IDTipoDocumento { get; set; }
        public string? FechaDocumento { get; set; }
        public List<DireccionesModel> Direcciones { get; set; }
        public List<DetalleFacturaModel> DetalleFactura { get; set; }

    }
    public class FormularioEnvio
    {
        public DocumentoRequestActuaCheque Cabecera { get; set; }
        public List<DetalleFacturaModelRequest> Detalle { get; set; }
        public List<DetalleFacturaImagenModelDTO> Imagen { get; set; }

    }

    public class DocumentoRequestActuaCheque
    {
        public int IDtipo { get; set; }
        public int NroFactura { get; set; }
        public string IDCliente { get; set; }
        public string IDDireccion { get; set; }
        public string Observacion { get; set; }
        public int TotalCheques { get; set; }
        public int NroSolicitud { get; set; }
        public string Comentarios { get; set; }
        public int nroCajas { get; set; }
        public string IDUsuario { get; set; }

    }

    public class ResponseActuaCheque
    {
        public string? msgResult { get; set; }
    }
}
