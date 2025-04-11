using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.CargaMAsivaOfertas
{
     public class OfertaSistemaModel
    {
        public int IDListaDescuento {get; set;}
		public string ListaDescuento {get; set;}
		public double PorcentajeDescuento {get; set;}
		public string DescripcionWeb {get; set;}
		public int IDEstado {get; set;}
		public int Orden {get; set;}
		public string FechaActualizacion {get; set;}
		public string IDUsuario { get; set; }

    }
}
