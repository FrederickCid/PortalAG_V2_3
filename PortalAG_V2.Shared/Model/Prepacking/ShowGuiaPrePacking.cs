using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Prepacking
{
    public class ShowGuiaPrePacking
    {
        public int IDTipoGuia { get; set; }
        public double IDGuia { get; set; }
        public string Fecha { get; set; }
        public int DocEntryEM { get; set; }
        public int DocNumEM { get; set; }
        public string ReferenciaEM { get; set; }
        public string FechaEM { get; set; }
        public string IDProveedor { get; set; }
        public string Proveedor { get; set; }
        public int NumeroGuia { get; set; }
        public string Observaciones { get; set; }
        public int IDEstadoGuia { get; set; }
        public int IDEtapa { get; set; }
        public int IDEstado { get; set; }
        public int IDEtapaOriginal { get; set; }
        public int IDEstadoOriginal { get; set; }
        public string IDUsuarioCreacion { get; set; }
        public string FechaInicioCreacion { get; set; }
        public string FechaTerminoCreacion { get; set; }
        public string IDUsuarioRecepcion { get; set; }
        public string FechaInicioRecepcion { get; set; }
        public string FechaTerminoRecepcion { get; set; }
        public string IDUsuarioControlCalidad { get; set; }
        public string FechaInicioControlCalidad { get; set; }
        public string FechaTerminoControlCalidad { get; set; }
        public string IDUsuarioRevisionBultos { get; set; }
        public string FechaInicioRevisionBultos { get; set; }
        public string FechaTerminoRevisionBultos { get; set; }
        public string FechaAnulacion { get; set; }
        public string IDUsuarioAnulacion { get; set; }
        public string? FechaActualizacion { get; set; }
        public string IDUsuario { get; set; }
        public bool IsFiltro { get; set; }
    }

    public class ShowGuiaPrePackingDetalle 
    { 
     public double IDGuia { get; set; }
        public int Linea { get; set; }
        public int LineNum { get; set; }
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public int CantidadEM { get; set; }
        public int IDEstado { get; set; }
        public string Fecha { get; set; }
        public string IDUsuario { get; set; }
        public string NroParte { get; set; }

    }


    public class ShowGuiaPrePackingDetalleBulto
    {
        public string msgResult { get; set; }
        public string msgError { get; set; }
        public double IDGuia { get; set; }
        public int Linea { get; set; }
        public int LineaBulto { get; set; }
        public string IDArticulo { get; set; }
        public double UnidadPorBulto { get; set; }
        public int Bultos { get; set; }
        public int IDEstado { get; set; }
        public double Faltan { get; set; }
    }

    public class ShowGuiaPrePackingDetalleDTO
    {
        public double IDGuia { get; set; }
        public int Linea { get; set; }
        public int LineNum { get; set; }
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public int CantidadEM { get; set; }
        public int IDEstado { get; set; }
        public string Fecha { get; set; }
        public string IDUsuario { get; set; }
        public double Faltan { get; set; }
        public string NroParte { get; set; }


    }
    public class ShowGuiaPrePackingDetalleDTOPDF
    {
        public double IDGuia { get; set; }
        public int Linea { get; set; }
        public int LineNum { get; set; }
        public string IDArticulo { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public int CantidadEM { get; set; }
        public int IDEstado { get; set; }
        public string Fecha { get; set; }
        public string IDUsuario { get; set; }
        public double Faltan { get; set; }
        public string NroParte { get; set; }

        public List<ShowGuiaPrePackingDetalleBulto> ListaBultos { get; set; }
    }
}
