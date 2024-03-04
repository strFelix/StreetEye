using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StreetEye.models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int IdUtilizador { get; set; }
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password {get; set;} = string.Empty;
        public byte[]? PasswordHash {get; set;}
        public byte[]? PasswordSalt {get; set;}

        [NotMapped]
        public Utilizador? Utilizador {get; set;}
    }
}