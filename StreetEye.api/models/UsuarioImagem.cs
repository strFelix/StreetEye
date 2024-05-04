using StreetEye.models;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreetEye.Models
{
    public class UsuarioImagem
    {
        public int IdUsuario { get; set; }
        public byte[]? Imagem { get; set; }

        [NotMapped]
        public Usuario? Usuario { get; set; }
    }
}