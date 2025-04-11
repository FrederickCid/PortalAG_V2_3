using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.DispositivosModel;



public class MostrarDispositivos
{
    public int IDTipo { get; set; }
    public string Descripcion { get; set; }

}

public class BuscarPorTipo
{
    public int IDDispositivo { get; set; }
    public int IDTipo { get; set; }
    public string Identificador { get; set; }
    public string IDBodegaEnPicking { get; set; }
    public int IDEstadoEnPicking { get; set; }
    public string IDBodegaEnPacking { get; set; }
    public int IDEstadoEnPacking { get; set; }
    public int IDEstado { get; set; }
    public string FechaActualizacion { get; set; }
    public string IDUsuario { get; set; }
    public string IDConexionHub { get; set; }
    public string IDUsuarioEnUso { get; set; }
}

public class Bodegas
{
    public string SiglaBodega { get; set; }
    public string Descripcion { get; set; }
}