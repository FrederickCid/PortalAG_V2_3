using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Model.Pagos
{
    public class FormaPagoDTO
    {

        public int crTypeCode { get; set; }
        public string crTypeName { get; set; }
        public int creditCard { get; set; }
        public string dueTerms { get; set; }
        public int minCredit { get; set; }
        public int minToPay { get; set; }
        public int maxValid { get; set; }
        

    }
}
