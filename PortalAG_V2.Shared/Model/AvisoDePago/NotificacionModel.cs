using PortalAG_V2.Shared.Model.Direcciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.AvisoDePago
{
    public class NotificacionModel
    {
        public int Tipo { get; set; } = 1;
        public string Titulo { get; set; } = "Liberacion";

        #region Despacho
        public int NroDocumento { get; set; }
        public string IDCliente { get; set; }
        public string RazonSocial { get; set; }
        public int IDOperacion { get; set; }
        public string FechaAutorizaLiberacion { get; set; }
        public string Vendedor { get; set; }

        #endregion

        #region Aviso de pago
        public  string TipoPago { get; set; }

        #endregion


    }
}
