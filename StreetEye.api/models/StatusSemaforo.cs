using System.ComponentModel.DataAnnotations.Schema;

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
        public Semaforo? Semaforo { get; set; }
    }
}