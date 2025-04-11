using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalAG_V2.Shared.Requests.User
{
    public class TokenRequest
    {
        [Required(ErrorMessage = "El campo Usuario es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es requerido")]
        public string Password { get; set; }
    }
}
