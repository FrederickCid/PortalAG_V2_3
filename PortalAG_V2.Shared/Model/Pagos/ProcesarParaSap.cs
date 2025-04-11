using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class ProcesarParaSap
    {
        
        public string idCliente { get; set; }
        public string razonSocial { get; set; }
        public int numeroCobranza { get; set; }
        public DateTime fechaContabilizacion { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public DateTime fechaDocumento { get; set; }
        public string referenciaPago { get; set; }
        public string comentarios { get; set; }
        public int sumaImporte { get; set; }
        public int saldoPendiente { get; set; }
        public int totalPago { get; set; }
        public int deudaCliente { get; set; }
        public List<Ventaspagopedido> ventasPagoPedidos { get; set; }
        public Efectivo efectivo { get; set; }
        public Tarjeta tarjeta { get; set; }
        public List<Transferencia> transferencias { get; set; }
        public List<Deposito> depositos { get; set; }
        public List<Cheque> cheques { get; set; }
        public List<Saldos_A_Favor> saldos_A_Favor { get; set; }
    }

    public class Efectivo
    {
        public string fechaEfectivo { get; set; }
        public string cuentaContable { get; set; }
        public int montoEfectivo { get; set; }
    }

    public class Tarjeta
    {
        public Tarjetas tarjetas { get; set; }
        public int nroTarjeta { get; set; }
        public DateTime fechaValidoHasta { get; set; }
        public int nroID { get; set; }
        public string nroTelefono { get; set; }
        public int importeVencido { get; set; }
        public int cantidadPagos { get; set; }
        public int primerPago { get; set; }
        public int pagoAdicional { get; set; }
        public string certificado { get; set; }
        public string claseOperacionSelect { get; set; }
    }

    public class Tarjetas
    {
        public int creditCard { get; set; }
        public string cardName { get; set; }
        public string acctCode { get; set; }
        public string phone { get; set; }
        public string companyId { get; set; }
    }

    public class Ventaspagopedido
    {
        public int idOperacionPedido { get; set; }
        public int idTipoOperacion { get; set; }
        public int nroDocumento { get; set; }
        public int docEntry { get; set; }
        public int montoPedido { get; set; }
        public int pagadoAFecha { get; set; }
        public int saldoAFecha { get; set; }
        public int valorAPagar { get; set; }
        public int saldoNuevo { get; set; }
        public int siAutorizaLiberacion { get; set; }
        public DateTime fechaAutorizaLiberacion { get; set; }
        public string idUsuarioAutorizaLiberacion { get; set; }
        public int siProgramaDespacho { get; set; }
        public DateTime fechaProgramarLiberacion { get; set; }
        public string idUsuarioProgramarLiberacion { get; set; }
        public string notaAutorizacionLiberacion { get; set; }
    }

    public class Transferencia
    {
        public int lineas { get; set; }
        public DateTime fecha { get; set; }
        public int numeroCuenta { get; set; }
        public int montoTarjeta { get; set; }
        public int numeroComprobante { get; set; }
        public int idBanco { get; set; }
        public string banco { get; set; }
        public int idBancoAndes { get; set; }
        public string bancoAndes { get; set; }
        public string cuentaAndes { get; set; }
        public int idAvisoPago { get; set; }
    }

    public class Deposito
    {
        public int lineas { get; set; }
        public DateTime fecha { get; set; }
        public int numeroCuenta { get; set; }
        public int montoTarjeta { get; set; }
        public int numeroComprobante { get; set; }
        public int idBanco { get; set; }
        public string banco { get; set; }
        public int idBancoAndes { get; set; }
        public string bancoAndes { get; set; }
        public string cuentaAndes { get; set; }
        public int idAvisoPago { get; set; }
    }

    public class Cheque
    {
        public int numeroCheque { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public int importe { get; set; }
        public string sucursal { get; set; }
        public string cuenta { get; set; }
        public int idBanco { get; set; }
        public string banco { get; set; }
    }

    public class Saldos_A_Favor
    {
        public int linea { get; set; }
        public int annoProceso { get; set; }
        public int idOperacion { get; set; }
        public string tipoOperacionSaldo { get; set; }
        public string nroDocumentoSaldo { get; set; }
        public string fechaDocumentoSaldo { get; set; }
        public int montoDocumentoSaldo { get; set; }
        public int pagarSaldo { get; set; }
        public int saldo { get; set; }
        public bool check { get; set; }
    }

    
}
