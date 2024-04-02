using System.ComponentModel.DataAnnotations.Schema;

namespace StreetEye.models
{
    public class UsuarioImagem
    {
        public int IdUsuario { get; set; }
        public byte[]? Imagem { get; set; }

        [NotMapped]
        public Usuario? Usuario { get; set; }
    }
}