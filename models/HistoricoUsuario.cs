using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StreetEye.models
{
    public class HistoricoUsuario
    {
        //foreign de usuario
        public int IdUsuario { get; set; }
        public DateTime Momento { get; set; }
        public int IdSemaforo { get; set; }
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;

        [NotMapped]
        public Usuario? Usuario { get; set; }
    
    }
}