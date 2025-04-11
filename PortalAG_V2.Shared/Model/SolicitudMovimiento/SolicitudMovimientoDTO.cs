using Microsoft.Extensions.Logging.Abstractions;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.SolicitudMovimiento;

//public class PrincipalModel
//{
//    public List<BodegasModel> Bodegas { get; set; }
//    public List<TipoOperacionModel> Operacion { get; set; }
//}

public class BodegasModel
{
    public int IDBodega { get; set; }
    public string SiglaBodega { get; set; }
}

public class TipoOperacionModel
{
    public int IDTipoOperacion { get; set; }
    public string Descripcion { get; set; }
}

public class UbicacionArticulo
{
    public string IDArticulo { get; set; }
    public string Nombre { get; set; }
    public int NroPallet { get; set; }
    public int IDUbicacion { get; set; }
    public int UnidadPorBulto { get; set; }
    public int Cantidad { get; set; }
    public string Ubicacion { get; set; }
}

public class UbicacionArticuloMayor

{
    public string IDArticulo { get; set; }
    public string Nombre { get; set; }
    public int NroPallet { get; set; }
    public int IDUbicacion { get; set; }
    public int UnidadPorBulto { get; set; }
    public int Cantidad { get; set; }
    public string Ubicacion { get; set; }
}

public class UbicacionDesde
{
    public string IDArticulo { get; set; }
    public string Nombre { get; set; }
    public int NroPallet { get; set; }
    public int IDUbicacion { get; set; }
    public int UnidadPorBulto { get; set; }
    public int Cantidad { get; set; }
    public string Ubicacion { get; set; }
}

public class UbicacionHasta
{
    public string IDArticulo { get; set; }
    public string Nombre { get; set; }
    public int NroPallet { get; set; }
    public int IDUbicacion { get; set; }
    public int UnidadPorBulto { get; set; }
    public int Cantidad { get; set; }
    public string Ubicacion { get; set; }
}

public class ListaDetalle310
{
    List<DetalleRecepcion310> Lista310 { get; set; }
}

public class DetalleRecepcion310
{
    public int TipoConsulta { get; set; }
    public int Solicitud { get; set; }
    public string IDArticulo { get; set; }
    public string Nombre { get; set; }
    public int NroPallet { get; set; }
    public int IDUbicacionDesde { get; set; }
    public int IDUbicacionHasta { get; set; }
    public string BodegaDesde { get; set; }
    public int Bultos { get; set; }
    public int UnidadxBultos { get; set; }
    public string IDUsuario { get; set; }
}

public class ResultGuiaMovimiento
{
    public string Estado { get; set; }
    public string Descripcion { get; set; }
    public int IDGuias { get; set; }
    public int NumeroGuia { get; set; }
}

public class GuiaCabecera
{
    public int IDTipoGuia { get; set; }
    public int IDGuia { get; set; }
    public int NumeroGuia { get; set; }
    public int IDBodegaDesde { get; set; }
    public int IDBodegaHasta { get; set; }
    public string Observaciones { get; set; }
    public string Fecha { get; set; }
    public int IDEstado { get; set; }
    public string IDUsuario { get; set; }

}

public class GuiaCabeceraDetalle
{
    public int IDTipoGuia { get; set; }
    public int IDGuia { get; set; }
    public int CorrelativoGuia { get; set; }
    public string Fecha { get; set; }
    public string IDArticulo { get; set; }
    public int CorrelativoArticulo { get; set; }
    public string Nombre { get; set; }
    public int IDBodegaDesde { get; set; }
    public int CantidadPorBultoDesde { get; set; }
    public int BultosDesde { get; set; }
    public int TotalDesde { get; set; }
    public int IDBodegaHasta { get; set; }
    public int CantidadPorBultoHasta { get; set; }
    public int BultosHasta { get; set; }
    public int TotalHasta { get; set; }
    public int IDEstado { get; set; }
    public string IDUsuario { get; set; }

}


#region AG
public class GuiaTraspasoDTO
{
    public CabeceraTraspasoDTO Cabecera { get; set; }
    public List<DetalleTraspasoDTO> Detalle { get; set; }
}
public class CabeceraTraspasoDTO
{
    public int IDBodegaDesde { get; set; }
    public string BodegaDesde { get; set; }
    public int IDBodegaHasta { get; set; }
    public string BodegaHasta { get; set; }
    public string Comentario { get; set; }
    public int IDEstado { get; set; }
    public string Fecha { get; set; }
    public int IDTipoGuia { get; set; }
    public int IDGuia { get; set; }
    public int NumeroGuia { get; set; }

}
public class DetalleTraspasoDTO
{
    public int Linea { get; set; }
    public bool Valido { get; set; }
    public int Estado { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string BodegaDes { get; set; }
    public string CantDes { get; set; }
    public string UnidadBultoDes { get; set; }
    public string BodegaHast { get; set; }
    public string CantHast { get; set; }
    public string UnidadBultoHast { get; set; }
    public string Total { get; set; }
    public string Comentario { get; set; }
}
#endregion

#region SAP
public class EnvioTraspaso
{
    [Required]
    public string Comments { get; set; }
    [Required]
    public string JournalMemo { get; set; }
    [Required]
    public string FromWarehouse { get; set; }
    [Required]
    public string ToWarehouse { get; set; }
    [Required]
    public List<ItemsGrilla> StockTransferLines { get; set; }
}
public class ItemsGrilla
{
    public string ItemCode { get; set; }
    public string Quantity { get; set; }
    public string WarehouseCode { get; set; }
    //public int correlativo { get; set; }
    //public string codigo { get; set; }
    //public string nombre { get; set; }
    //public int unidadPorBulto { get; set; }
    //public int cantidad { get; set; }
    //public int total { get; set; }
}
public class Response
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public object Result { get; set; }
}
#endregion

#region DetalleRecepcion300
public class ListaDetalle300
{
    List<DetalleRecepcion300> Lista300 { get; set; }
}

public class DetalleRecepcion300
{
    public int SubTipoSolicitud { get; set; }
    public string IDArticulo { get; set; }
    public int NroPallet { get; set; }
    public int Bultos { get; set; }
    public int UnidadxBulto { get; set; }
    public int IDUbicacion { get; set; }
    public int DocEntry { get; set; }
    public int IDGuia { get; set; }
    public string IDUsuario { get; set; }
}
#endregion

#region Traspaso
public class Bodegas
{
    public string SiglaBodega { get; set; }
    public string Descripcion { get; set; }
}

public class DetalleRecepcion123
{
    public int TipoConsulta { get; set; }
    public int Solicitud { get; set; }
    public string IDArticulo { get; set; }
    public string Nombre { get; set; }
    public int NroPallet { get; set; }
    public int IDUbicacionDesde { get; set; }
    public int IDUbicacionHasta { get; set; }
    public string BodegaDesde { get; set; }
    public int Bultos { get; set; }
    public int UnidadxBultos { get; set; }
    public string IDUsuario { get; set; }
}

public class DetalleEnvio123
{
    public int SubTipoSolicitud { get; set; }
    public string IDArticulo { get; set; }
    public int NroPallet { get; set; }
    public int Bultos { get; set; }
    public int UnidadxBulto { get; set; }
    public int IDUbicacion { get; set; }
    public int DocEntry { get; set; }
    public int IDGuia { get; set; }
    public string IDUsuario { get; set; }
}
#endregion

public class ConsultaBultosDTO
{
    public string IDArticulo { get; set; }
    public int Correlativo { get; set; }
    public int CorrelativoEnbodega { get; set; }
    public int CantidadPorBulto { get; set; }
    public int Bultos { get; set; }
    public string FechaActualizacion { get; set; }
    public string IDUsuario { get; set; }
}

public class ConsultaArticulo
{
    public string ItemName { get; set; }
}
