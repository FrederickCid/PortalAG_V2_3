using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pallet
{
    public class PalletModels
    {

        public class IDUbicacionModel
        {
            public string IDArticulo { get; set; }
            public string Nombre { get; set; }
            public int UnidadPorBulto { get; set; }
            public int IDUbicacionHasta { get; set; }
            public string Ubicacion { get; set; }
        }

        public class DespalletizarModel
        {
            public string msgError { get; set; }
            public string msgResult { get; set; }
        }
    }
}
