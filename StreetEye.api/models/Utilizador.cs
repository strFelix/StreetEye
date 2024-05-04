using Newtonsoft.Json;
using StreetEye.models.enums;
using StreetEye.Models;

namespace StreetEye.models
{
    public class Utilizador
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public TipoUtilizador Tipo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string NumeroEndereco { get; set; } = string.Empty;
        public string? Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string? Latitude { get; set; } = string.Empty;
        public string? Longitude { get; set; } = string.Empty;


        //navigation
        [JsonIgnore]
        public ICollection<Usuario>? Usuarios { get; set; }
        [JsonIgnore]
        public ICollection<Responsavel>? Responsaveis { get; set; }

    }
}