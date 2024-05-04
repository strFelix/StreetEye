using Newtonsoft.Json;
using StreetEye.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreetEye.models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int IdUtilizador { get; set; }
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        [NotMapped]
        public string Token { get; set; } = string.Empty;

        [NotMapped]
        [JsonIgnore]
        public UsuarioImagem? UsuarioImagem { get; set; }

        [NotMapped]
        public Utilizador? Utilizador { get; set; }

        [JsonIgnore]
        // navigation
        public ICollection<HistoricoUsuario>? Historicos { get; set; }
    }
}