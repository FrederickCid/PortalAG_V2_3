using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agDataAccess.Models.Solicitudes
{
    public class ReposicionesPendientesModel
    {
        public string IDArticulo { get; set; }
        public string NombreArticulo { get; set; }
        public int NroPallet { get; set; }
        public string CantidadSolicitada { get; set; }
        public string Estado { get; set; }
        public string Bodegas { get; set; }
    }
}
