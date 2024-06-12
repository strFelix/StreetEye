using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StreetEye.models
{
    public class HistoricoUsuario
    {
        //foreign de usuario
        public int IdUsuario { get; set; }
        [JsonIgnore]
        public DateTime Momento { get; set; }
        public int IdSemaforo { get; set; }
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;

        [NotMapped]
        [JsonIgnore]
        public Usuario? Usuario { get; set; }
        [NotMapped]
        [JsonIgnore]
        public Semaforo? Semaforo { get; set; }
    }
}