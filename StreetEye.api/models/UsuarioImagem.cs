using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StreetEye.models
{
    public class UsuarioImagem
    {
        public int IdUsuario { get; set; }
        public byte[]? Imagem {get; set;}

        [NotMapped]
        public Usuario? Usuario { get; set; }
    }
}