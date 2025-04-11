using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace agDataAccess.Models
{


    public class EstadoPedidosNoMOD
    {
        public int NroDocumento { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int IDTipoOperacion { get; set; }
        public int MesProceso { get; set; }
        public string IDVendedor { get; set; }
        public string Vendedor { get; set; }
        public int SiPedidoWeb { get; set; }
        public int MontoTotal { get; set; }
        public int LineasSolicitud { get; set; }
        public int LineasCalculoBase { get; set; }
        public int LineaPickingStaHelena { get; set; }
        public int LineaPickingCDA { get; set; }
        public int LineasPickingCDABVN { get; set; }
        public int LineasPickingCDABIT { get; set; }
        public int LineasPickingCDABPM { get; set; }
        public int LineaPackingStaHelena { get; set; }
        public int LineaPackingCDA { get; set; }
        public int LineasPackingCDABVN { get; set; }
        public int LineasPackingCDABIT { get; set; }
        public int LineasPackingCDABPM { get; set; }
        public int IDEtapa { get; set; }
        public int IDEstado { get; set; }
        public int IDEtapaOriginal { get; set; }
        public int IDEstadoOriginal { get; set; }
        public string FechaInicioCoticacion { get; set; }
        public string FechaTemrinoCotizacion { get; set; }
        public string FechaInicioAutorizacion { get; set; }
        public string FechaTerminoAutorizacion { get; set; }
        public string IDUsuarioAutorizacion { get; set; }
        public string FechaInicioReposicionBPM { get; set; }
        public string FechaTerminoReposicionBPM { get; set; }
        public string IDUsuarioReposicionBPM { get; set; }
        public string FechaInicioReposicionBIT { get; set; }
        public string FechaTerminoReposicionBIT { get; set; }
        public string IDUsuarioReposicionBIT { get; set; }
        public int IDSacadorPickingBU { get; set; }
        public string SacadorPickingBU { get; set; }
        public string FechaInicioPickingBU { get; set; }
        public string FechaTerminoPickingBU { get; set; }
        public int IDEstadoPickingBU { get; set; }
        public int IDRevisadorPackingBU { get; set; }
        public string RevisadorPackingBU { get; set; }
        public string FechaInicioPackingBU { get; set; }
        public string FechaTerminoPackingBU { get; set; }
        public int IDEstadoPackingBU { get; set; }
        public int IDSacadorPickingBC { get; set; }
        public string SacadorPickingBC { get; set; }
        public string FechaInicioPickingBC { get; set; }
        public string FechaTerminoPickingBC { get; set; }
        public int IDEstadoPickingBC { get; set; }
        public int IDRevisadorPackingBC { get; set; }
        public string RevisadorPackingBC { get; set; }
        public string FechaInicioPackingBC { get; set; }
        public string FechaTerminoPackingBC { get; set; }
        public int IDEstadoPackingBC { get; set; }
        public int IDSacadorPickingBV_BVN { get; set; }
        public string SacadorPickingBV_BVN { get; set; }
        public string FechaInicioPickingBV_BVN { get; set; }
        public string FechaTerminoPickingBV_BVN { get; set; }
        public int IDEstadoPickingBV_BVN { get; set; }
        public int IDRevisadorPackingBV_BVN { get; set; }
        public string RevisadorPackingBV_BVN { get; set; }
        public string FechaInicioPackingBV_BVN { get; set; }
        public string FechaTerminoPackingBV_BVN { get; set; }
        public int IDEstadoPackingBV_BVN { get; set; }
        public int IDSacadorPickingBV_BIT { get; set; }
        public string SacadorPickingBV_BIT { get; set; }
        public string FechaInicioPickingBV_BIT { get; set; }
        public string FechaTerminoPickingBV_BIT { get; set; }
        public int IDEstadoPickingBV_BIT { get; set; }
        public int IDRevisadorPackingBV_BIT { get; set; }
        public string RevisadorPackingBV_BIT { get; set; }
        public string FechaInicioPackingBV_BIT { get; set; }
        public string FechaTerminoPackingBV_BIT { get; set; }
        public int IDEstadoPackingBV_BIT { get; set; }
        public int IDSacadorPickingBV_BPM { get; set; }
        public string SacadorPickingBV_BPM { get; set; }
        public string FechaInicioPickingBV_BPM { get; set; }
        public string FechaTerminoPickingBV_BPM { get; set; }
        public int IDEstadoPickingBV_BPM { get; set; }
        public int IDRevisadorPackingBV_BPM { get; set; }
        public string RevisadorPackingBV_BPM { get; set; }
        public string FechaInicioPackingBV_BPM { get; set; }
        public string FechaTerminoPackingBV_BPM { get; set; }
        public int IDEstadoPackingBV_BPM { get; set; }
        public List<BultosModel> Bultos { get; set; }
        public string FechaRecibeCaja { get; set; }
        public string FechaFacturacion { get; set; }
        public List<FacturaEstadoPedidoModel> Facturas { get; set; }
        public string FechaLiberacion { get; set; }
        public string UsuarioLiberacion { get; set; }
        public string Transporte { get; set; }
        public string FechaIngresoHR { get; set; }
        public string FechaIngresoBodegaD { get; set; }
        public string FechaIngresadoACamion { get; set; }
        public string FechaEntregado { get; set; }
        public int NroHR { get; set; }
        public int NroPedidoFB { get; set; }
        public int IDOperacionFB { get; set; }
        public string NroOrden { get; set; }
        public int SiPedidoMKT { get; set; }
    }

    

}





