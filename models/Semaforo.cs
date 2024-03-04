using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetEye.models
{
    public class Semaforo
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int IntervaloAberto { get; set; }
        public int IntervaloFechado { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string ViaCruzamento { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;

        // navigation
        public ICollection<StatusSemaforo> Status { get; set; }
    } 
}