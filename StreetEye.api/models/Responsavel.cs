using StreetEye.models;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreetEye.Models
{
    public class Responsavel
    {
        public int IdUtilizador { get; set; }
        public int IdResponsavel { get; set; }

        [NotMapped]
        public Utilizador? Utilizador { get; set; }

        [NotMapped]
        public Utilizador? ResponsavelUtilizador { get; set; }
    }
}