using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class PagosPendienteDTO
    {
        public int annoProceso {get;set;}
        public double iDOperacion {get;set;}
        public double iDOperacionPedido {get;set;}
        public double nroDocumento {get;set;}
        public DateTime fecha {get;set;}
        public double monto {get;set;}
        public double montoPedido {get;set;}
        public double pagadoAFecha {get;set;}
        public double saldoAFecha {get;set;}
        public double valorAPagar {get;set;}
        public double saldoNuevo {get;set;}

        public string vendedor {get;set;}
        public double porcentajeDescuento {get;set;}
        public string formaPago {get;set;}
        public string condicionPago {get;set;}
        public string comentarios {get;set;}
        public int bultos {get;set;}
        public string numeroOC {get;set;}
        public string localidad {get;set;}
        public int siVentaBicicleta {get;set;}

        public int siAutorizaLiberacion {get;set;}
        public DateTime fechaAutorizaLiberacion {get;set;}
        public string iDUsuarioAutorizaLiberacion {get;set;}

        public int siProgramaDespacho {get;set;}
        public DateTime fechaProgramarLiberacion {get;set;}
        public string iDUsuarioProgramarLiberacion {get;set;}
        public string notaAutorizacionLiberacion {get;set;}


        public int iDEtapa {get;set;}
        public int iDEstado {get;set;}
        public int tipoEntrega {get;set;}
        public int siAutorizaDespacho {get;set;}
        public string notaAutorizacion {get;set;}
        public DateTime fechaAutorizaDespacho {get;set;}
        public string iDUsuarioAutorizaDespacho {get;set;}
        public string despachador {get;set;}
        public DateTime fechaInicioDespacho {get;set;}
        public DateTime fechaTerminoDespacho {get;set;}
        public DateTime fechaInicioRetiroBodega {get;set;}
        public DateTime fechaTerminoRetiroBodega {get;set;}
        public DateTime fechaInicioSalidaTransporte {get;set;}
        public DateTime fechaTerminoSalidaTransporte {get;set;}
        public DateTime fechaInicioEntregaEnTransporte {get;set;}
        public DateTime fechaTerminoEntregaEnTransporte {get;set;}
        public string dreccion {get;set;}
        public string region {get;set;}
        public string ciudad {get;set;}
        public string comuna {get;set;}
        public string fono {get;set;}
        public string eMail {get;set;}
        public string referencia {get;set;}
        public int iDTransporte {get;set;}
        public string transporte {get;set;}
    }
}
