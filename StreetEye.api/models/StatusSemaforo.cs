using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StreetEye.models
{
    public class StatusSemaforo
    {
        // foreign de semaforo
        public int IdSemaforo { get; set; }
        public DateTime Momento { get; set; }
        public bool StatusVisor { get; set; }
        public bool StatusAudio { get; set; }
        public bool Estado { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Semaforo? Semaforo { get; set; }
    }
}