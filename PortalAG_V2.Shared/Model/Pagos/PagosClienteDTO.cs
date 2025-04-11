using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class PagosClienteDTO
    {
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public string FechaContabilizacion { get; set; }
        public string FechaVencimiento { get; set; }
        public string FechaDocumento { get; set; }
        public string ReferenciaPago { get; set; }
        public int NumeroCobranza { get; set; }
        public string Comentarios { get; set; }
        public int SumaImporte { get; set; }
        public int TotalPago { get; set; }
        public int SaldoPendiente { get; set; }
        public int DeudaCliente { get; set; }
        public List<PedidosDTO> Pedidos { get; set; }
        public List<SaldosFavorDTO> Saldos_A_Favors { get; set; }
        public bool Check { get; set; } = false;
        public List<PagoCancelacion> Pagos { get; set; }
    }
    public class PedidosDTO
    {
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int NroDocumento { get; set; }
        public string Fecha { get; set; }
        public double Monto { get; set; }
        public double PagadoAFecha { get; set; }
        public double SaldoAFecha { get; set; }
        public double ValorAPagar  { get; set; }
        public double SaldoNuevo { get; set; }
        public string Vendedor { get; set; }
        public double PorcentajeDescuento { get; set; }
        public string FormaPago { get; set; }
        public string CondicionPago { get; set; }
        public string Comentarios { get; set; }
        public int Bultos { get; set; }
        public string NumeroOC { get; set; }
        public string Localidad { get; set; }
        public int SiVentaBicicleta { get; set; }
        public int SiAutorizaLiberacion { get; set; }
        public int SiProgramaDespacho { get; set; }
        public string NotaAutorizacionLiberacion { get; set; }
        public string IDUsuarioAutorizaLiberacion { get; set; }
        public int IDEtapa { get; set; }
        public int IDEstado { get; set; }
        public int TipoEntrega { get; set; }
        public int SiAutorizaDespacho { get; set; }
        public string NotaAutorizacion { get; set; }
        public string IDUsuarioAutorizaDespacho { get; set; }
        public string Despachador { get; set; }
        public string FechaInicioDespacho { get; set; }
        public string FechaTerminoDespacho { get; set; }
        public string FechaInicioRetiroBodega { get; set; }
        public string FechaTerminoRetiroBodega { get; set; }
        public string Direccion { get; set; }
        public string Region { get; set; }
        public string Ciudad { get; set; }
        public string Comuna { get; set; }
        public string Fono { get; set; }
        public string EMail { get; set; }
        public string Referencia { get; set; }
        public int IDTransporte { get; set; }
        public string Transporte { get; set; }
        public int Orden { get; set; }
        public List<Documeto> Documetos { get; set; }
        public bool Check { get; set; } = false;
    }

    public class Documeto
    {
        public int Linea { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public string TipoOperacion { get; set; }
        public int NroDocumento { get; set; }
        public DateTime FechaDocumento { get; set; }
        public int MontoDocumento { get; set; }
        public int Pagar { get; set; }
        public int Saldo { get; set; }
        public int DocEntry { get; set; }
    }

    public class PagoCancelacion
    {
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Correlativo { get; set; }
        public int CorrelativoCancelacion { get; set; }
        public DateTime FechaCancelacion { get; set; }
        public int IDFormaPago { get; set; }
        public string FormaPago { get; set; }
        public int IDBanco { get; set; }
        public string Banco { get; set; }
        public string NumeroSerie { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string NroCtaCteBanco { get; set; }
        public float Monto { get; set; }
        public int IDBancoAndes { get; set; }
        public int IDAvisoPago { get; set; }
        public int IDEstado { get; set; }
        public int DocEntry { get; set; }
        public DateTime FechaActualizacion { get; set; }


    }
}
