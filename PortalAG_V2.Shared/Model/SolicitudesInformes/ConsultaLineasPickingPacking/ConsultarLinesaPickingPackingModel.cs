using MudBlazor.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.SolicitudesInformes.ConsultaLineasPickingPacking
{
    public class ConsultarLinesaPickingPackingModel
    {
        public int AnnoProceso { get; set; }
        public int IDOperacion { get; set; }
        public int Correlativo { get; set; }
        public int NroDocumento { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public int TipoEntrega { get; set; }
        public string Transporte { get; set; }
        public int NroOrden { get; set; }
        public int IDEtapa { get; set; }
        public int IDEstado { get; set; }
        public string SiUrgencia { get; set; }
        public string Sacador { get; set; }
        public string FechaSolicitud { get; set; }
        public string FechaAutorizacion { get; set; }
        public string FechaEntregaEstimada { get; set; }
        public string FechaInicioSacado { get; set; }
        public string Region { get; set; }
        public string Comuna { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public int MontoTotal { get; set; }
        public int LineasBU { get; set; }
        public int LineasBC { get; set; }
        public int LineasBV_BVN { get; set; }
        public int LineasBV_BIT { get; set; }
        public int LineasBV_BPM { get; set; }
        public string YaPicking { get; set; }
        public string YaPacking { get; set; }
        public string YaPicking_BV { get; set; }
        public string YaPacking_BV { get; set; }
        public string Vendedor { get; set; }
    }


    public class CantidadLineadModel
    {
        public int LineasBC { get; set; }
        public int LineasBU { get; set; }
        public int LineasBV_BVN { get; set; }
        public int LineasBV_BIT { get; set; }
        public int LineasBV_BPM { get; set; }
    }

    public class LineasModel
    {
        public List<LineaspickingModel> LineasPicking { get; set; }
        public List<LineaspackingModel> LineasPacking { get; set; }
    }

    public class LineaspickingModel
    {
        public string Revisador { get; set; }
        public string Lineas { get; set; }
        public string Bodega { get; set; }
    }

    public class LineaspackingModel
    {
        public string Sacador { get; set; }
        public string IDBodega { get; set; }
        public string Lineas { get; set; }
        public string Bodega { get; set; }
    }


    public class lineasPickingModel
    {
        public int idSacador { get; set; }
        public string sacador { get; set; }
        public string IDBodega { get; set; }
        public int lineas { get; set; }
        public int bultos { get; set; }
        public int monto { get; set; }
    }
    public class lineasPackingModel
    {
        public int idSacador { get; set; }
        public string sacador { get; set; }
        public string IDBodega { get; set; }
        public int lineas { get; set; }
        public int bultos { get; set; }
        public int monto { get; set; }
    }
    public class lineasDespachoModel
    {
        public int idSacador { get; set; }
        public string sacador { get; set; }
        public int lineas { get; set; }
        public int bultos { get; set; }
        public int monto { get; set; }
    }
    public class lineasReposicionModel
    {
        public int idSacador { get; set; }
        public string sacador { get; set; }
        public int lineas { get; set; }
        public int bultos { get; set; }
        public int monto { get; set; }
    }
    
    public class LineasDevolucionModel
    {
        public int idSacador { get; set; }
        public string sacador { get; set; }
        public int lineas { get; set; }
        public int bultos { get; set; }
        public int monto { get; set; }
    }
    
    public class LineasTodas
    {
        public List<lineasPickingModel> lineasPicking { get; set; }
        public List<lineasPackingModel> lineasPacking { get; set; }
        public List<lineasDespachoModel> lineasDespacho { get; set; }
        public List<lineasReposicionModel> lineasReposicion { get; set; }
        public List<LineasDevolucionModel> LineasDevolucion { get; set; }

    }
    public class LineasUsuarioModel
    {
        public List<Lineas> LineasPicking { get; set; }
        public List<Lineas> LineasPacking { get; set; }
        public List<Lineas> LineasDespacho { get; set; }
        public List<Lineas> LineasReposicion { get; set; }
        public List<Lineas> LineasDevolucion { get; set; }
    }

    public class Lineas
    {
        public int IDSacador { get; set; }
        public string Sacador { get; set; }
        public string IDBodega { get; set; }
        public int LINEAS { get; set; }
        public int BULTOS { get; set; }
        public int MONTO { get; set; }
    }


}
