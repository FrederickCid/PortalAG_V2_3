using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.LiberarPedidos;

public class RequestConsultaPedidos
{
    public int nroPedido { get; set; }
    public string fechaInicio { get; set; }
    public string fechaFin { get; set; }
    public string idCliente { get; set; }
    public string idUsuario { get; set; }
    public int idOperacion { get; set; }
}

public class ResponseConsultaPedidos
{
    public int nroDocumento { get; set; }
    public int idOperacion { get; set; }
    public string idCliente { get; set; }
    public string razonSocial { get; set; }
    public int nroFacturas { get; set; }
    public List<ResponseListFacturas> documentos { get; set; }
    public string CondicionPago { get; set; }
    public string Vendedor { get; set; }
    public string TipoDespacho { get; set; }
    public string FechaSolicitud { get; set; }
    public int TipoDocumento { get; set; }
    public int estado { get; set; }

}

public class ResponseListFacturas
{
    public int nroDocumeto { get; set; }
    public int total { get; set; }
    public int pagadoAfecha { get; set; }
}

public class RequestLiberarPedido
{
    public int IDOperacion { get; set; }
    public string idUsuario { get; set; }
}