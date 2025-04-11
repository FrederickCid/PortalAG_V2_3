using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Models.CargaMasiva.Articulos
{
    public class ArticuloExcel
    {
        public int Linea { get; set; }
        public bool Valido { get; set; }//necesario
        public string Codigo { get; set; }//necesario (validar)
        public string Rango { get; set; }//necesario (validar)
        public string NombreFactura { get; set; }//necesario (validar)
        public string NombreInterno { get; set; }//necesario (validar)
        public string UnidadVenta { get; set; }//necesario (validar)
        public string UnidadCompra { get; set; }//necesario (validar)
        public string Provedor { get; set; } //necesario (validar)
        public string GrupoContable { get; set; }//necesario (validar)
        public string Marca { get; set; }//necesario (validar)
        public string NroParte { get; set; }//necesario (opcional)
        public string ComentarioSAP { get; set; }//necesario (validar)
        public int SiDesactivado { get; set; }//necesario (validar)
        public int SiBloqueado { get; set; }//necesario (validar)
        public int Estado { get; set; }//necesario (Aviso)
    }

    public class ArticuloExcelValidada
    {
        public int Linea { get; set; }
        public bool Valido { get; set; }//necesario
        public string Codigo { get; set; }//necesario (validar)
        public string Rango { get; set; }//necesario (validar)
        public string NombreFactura { get; set; }//necesario (validar)
        public string NombreInterno { get; set; }//necesario (validar)
        public string UnidadVenta { get; set; }//necesario (validar)
        public string UnidadCompra { get; set; }//necesario (validar)
        public string Provedor { get; set; } //necesario (validar)
        public string GrupoContable { get; set; }//necesario (validar)
        public string Marca { get; set; }//necesario (validar)
        public string NroParte { get; set; }//necesario (opcional)
        public string ComentarioSAP { get; set; }//necesario (validar)
        public int SiDesactivado { get; set; }//necesario (validar)
        public int SiBloqueado { get; set; }//necesario (validar)
        public int Estado { get; set; }//necesario (Aviso)
        public string Error { get; set; }//necesario (Aviso)
    }

}
