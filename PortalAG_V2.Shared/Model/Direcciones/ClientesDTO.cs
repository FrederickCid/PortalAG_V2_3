using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Direcciones
{
    public class ClientesDTO
    {
            public string idCliente { get; set; }
            public int correlativo { get; set; }
            public int rut { get; set; }
            public string digito { get; set; }
            public string razonSocial { get; set; }
            public string eMail { get; set; }
            public string fono { get; set; }
            public string areaCelular { get; set; }
            public string celuOriginal { get; set; }
            public object celuOK { get; set; }
            public string celular { get; set; }
            public string rutContacto { get; set; }
            public string digitoContacto { get; set; }
            public string contacto { get; set; }
            public string giro { get; set; }
            public string direccion { get; set; }
            public string nroDireccion { get; set; }
            public string direccionComercial { get; set; }
            public string codigoPostal { get; set; }
            public int idRegion { get; set; }
            public int idComuna { get; set; }
            public string comuna { get; set; }
            public int idCiudad { get; set; }
            public string ciudad { get; set; }
            public string localidad { get; set; }
            public int siDatosActualizados { get; set; }
            public string fax { get; set; }
            public int tipoCliente { get; set; }
            public int limiteCredito { get; set; }
            public int idTransporte { get; set; }
            public string transporte { get; set; }
            public string oficina { get; set; }
            public int siObsequio { get; set; }
            public int siEtiqueta { get; set; }
            public int siListaPrecio { get; set; }
            public int idFormaPago { get; set; }
            public string formaPago { get; set; }
            public string comentarioFormaPago { get; set; }
            public int idCodigoAsociado { get; set; }
            public string nombreAsociado { get; set; }
            public int idEstado { get; set; }
            public DateTime fechaActualizacion { get; set; }
            public string idUsuario { get; set; }
            public int cantidadFormaPago { get; set; }
            public int comienzoDiasFormaPago { get; set; }
            public int siClientePremium { get; set; }
            public int porcentajePremium { get; set; }
            public int siClienteMesuca { get; set; }
            public int siReajuste { get; set; }
            public int porcentajeReajuste { get; set; }
            public int siClienteWeb { get; set; }
            public string claveWeb { get; set; }
            public int idCodigoDeuda { get; set; }
            public int idVendedor { get; set; }
            public int siClienteMoto { get; set; }
            public int siClienteDeportes { get; set; }
            public int siClienteBicicletas { get; set; }
            public int sinEmail { get; set; }
            public int siEnviarEmail { get; set; }
            public int siVentaEspecial { get; set; }
            public int factorVentaEspecial { get; set; }
            public object fechaInicioAP { get; set; }
            public int porcentajeDescuento { get; set; }
            public int descuentoSobre { get; set; }
            public int siMKT { get; set; }
            public int siClienteEstablecido { get; set; }
            public int siClienteInternet { get; set; }
            public int siClienteAmbulante { get; set; }
            public int siClienteProductora { get; set; }
            public int siClienteGimnasio { get; set; }
            public int siClienteHotel { get; set; }
            public int siClienteInstitucion { get; set; }
            public int siClienteTrabajador { get; set; }
            public int siPortalPagoWeb { get; set; }
            public string tokenPagoWeb { get; set; }
            public int siClienteScott { get; set; }
            public int siEnvioSAP { get; set; }
            public string facebook { get; set; }
            public object twitter { get; set; }
            public string instagram { get; set; }
            public string paginaWeb { get; set; }
            public string tipoSiCliente { get; set; }
            public int condicionPago { get; set; }
            public int idZona { get; set; }
        

    }
}
