using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PortalAG_V2.Shared.Models.ClienteEvaluacion
{
    public class ClienteAdicionalModel
    {

        public string IDCliente { get; set; }
        public List<Comportamientospago> ComportamientosPago { get; set; }
        public int Protestos { get; set; }
        public int TotalDeuda { get; set; }
        public string CondicionPago { get; set; }
        public string FormaPago { get; set; }
        public int CréditoTotal { get; set; }
        public int CréditoDisponible { get; set; }
        public int CréditoUtilizado { get; set; }
        public string CondicionDespacho { get; set; }
        public float UltimoDescuento { get; set; }
        public float DescuentoCliente { get; set; }
        public string RFM { get; set; }
        public int Ranking { get; set; }
        public List<Ventasporsubcastegoria> VentasPorSubCastegoria { get; set; }
        public List<Ventasporgama> VentasPorGama { get; set; }
        public string VentasPorSubModelo { get; set; }
        public string UltimaCompraEnMeses { get; set; }
        public string UltimaCompraEnSemanas { get; set; }
        public string Transporte { get; set; }
        public string Observaciones { get; set; }
        public string Usuario { get; set; }
        public string FechaActualizacion { get; set; }


        public class Comportamientospago
        {
            public int FolioNum { get; set; }
            public double DocTotal { get; set; }
            public double PaidToDate { get; set; }
            public string DocDate { get; set; }
            public string Estado { get; set; }
        }

        public class Ventasporsubcastegoria
        {
            public string SubCategoria { get; set; }
            public double Num { get; set; }
            public double suma { get; set; }
            public double NumP { get; set; }
            public double sumaP { get; set; }
        }

        public class Ventasporgama
        {
            public string Gama { get; set; }
            public double Num { get; set; }
            public double suma { get; set; }
            public double NumP { get; set; }
            public double sumaP { get; set; }
        }
    }
    
}
