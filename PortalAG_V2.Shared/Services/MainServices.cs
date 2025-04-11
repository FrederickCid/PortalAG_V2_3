using System;
using System.Collections.Generic;
using System.Text;

namespace PortalAG_V2.Shared.Services
{
    public class MainServices
    {
        #region Instancias de Clientes

        //https://localhost:7053/ - https://apicda.andesindustrial.cl/ - https://apicda.andesindustrial.cl/Test/ - //http://190.54.179.197:7095/Api_BV_Crar/
        public ClientFactory Baliza { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");
        public ClientFactory SolMovimiento { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/"); //https://localhost:44352/ //https://apicda.andesindustrial.cl/
        public ClientFactory PermisosAPP { get; set; } = new ClientFactory("https://apiportalctrl.andesindustrial.cl/");
        public ClientFactory PermisosAPPPublic { get; set; } = new ClientFactory("https://apiportalctrl.andesindustrial.cl/");
        public ClientFactory CrearGuia { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");
        public ClientFactory SAP { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");
        public ClientFactory Bodega { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");
        public ClientFactory EstadoPedido { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");
        public ClientFactory SolcitudInformes { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");
        public ClientFactory EstadoPedidoTest { get; set; } = new ClientFactory("https://localhost:44352/");
        public ClientFactory PrePacking { get; set; } = new ClientFactory("https://apicore.andesindustrial.cl");
        public ClientFactory PrePackingtest { get; set; } = new ClientFactory("https://localhost:44343/");
        public ClientFactory Formulario { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");
        public ClientFactory ConectionService { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");  //new ClientFactory("https://apifb.andesindustrial.cl/"); // new ClientFactory("http://190.54.179.197:8099/"); 
        public ClientFactory Prueba { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");

        public ClientFactory Pagos { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");

        public ClientFactory ConectionServicePublic { get; set; } = new ClientFactory("http://190.54.179.197:8099");

        public ClientFactory Formularios { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");

        public ClientFactory BackOffice { get; set; } = new ClientFactory("https://apiandes.andesindustrial.cl/backoffcie/");


        public ClientFactory FacturaPorServicio { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");

        public ClientFactory FacturaPorServicioLocal { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/");


        public ClientFactory ConectionServiceNotaCredito { get; set; } = new ClientFactory("https://apicda.andesindustrial.cl/"); 
        public ClientFactory SheriffCliente { get; set; } = new ClientFactory("https://localhost:7154/"); 

        #endregion

        private static MainServices instance;
        public static MainServices GetInstance()
        {
            if (instance == null)
            {
                return new MainServices();
            }

            return instance;
        }

        public MainServices()
        {
            instance = this;
        }

        
    }
}
