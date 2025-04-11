using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.AvisoDePago;

public class ConfirmacionDespachoModel
{
    public string RazonSocial { get; set; }
    public string IDCliente { get; set; }
    public int NroPedido { get; set; }
    public string Fono { get; set; }
    public string Celular { get; set; }
    public string Transporte { get; set; }
    public int Bultos { get; set; }
}

public class Transportes
{
    public int IDTransporte { get; set; }
    public string Transporte { get; set; }
}

public class Direcciones
{
    public string Direccion { get; set; }
    public string NroDireccion { get; set; }
    public string Region { get; set; }
    public int IDComuna { get; set; }
    public string Comuna { get; set; }
    public string Ciudad { get; set; }
}

