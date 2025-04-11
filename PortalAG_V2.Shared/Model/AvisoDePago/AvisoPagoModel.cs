using Microsoft.AspNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.AvisoDePago;

public class AvisoPagoModel
{
    public int IDOperacion { get; set; }
    public string IDCliente { get; set; }
    public string RazonSocial { get; set; }
    public DateTime Fecha { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    public int Valor { get; set; }
    public int? IDBanco { get; set; }
    public int? IDBancoOrigen { get; set; }
    public int IDTipoPago { get; set; }
    public string NroCuenta { get; set; }
    public string NroCuentaOrigen { get; set; }
    public string UsuarioOrigen { get; set; }
    public string NroComprobante { get; set; }
    public string IDVendedor { get; set; }
    public DateTime FechaDocumento { get; set; } = DateTime.Now;
    public DateTime FechaVencimiento { get; set; } = DateTime.Now;
    public string Comentarios { get; set; } = string.Empty;
    public string Referencia { get; set; } = string.Empty;
    public DateTime FechaContabilizacion { get; set; } = DateTime.Now;
    public List<Archivo> Imagenes { get; set; } 
}

public class ActulizaEstadoModel
{
    public int IDOperacion { get; set; }
    public int IDEstado { get; set; }
    public string Usuario { get; set; }
    public string Comentario { get; set; }
}

public class Archivo
{
    public string Stream { get; set; }
    public string FileInfo { get; set; }
}