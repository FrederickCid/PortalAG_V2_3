using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class CuentaDTO
    {
        public int Codigo { get; set; } 
        public string Descripcion { get; set; }

        public CuentaDTO()
        {

        }
        public CuentaDTO(int codigo, string descripcion)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }
    }
    
}
