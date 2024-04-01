using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using StreetEye.models.enums;
using StreetEye.models;
using Newtonsoft.Json;

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