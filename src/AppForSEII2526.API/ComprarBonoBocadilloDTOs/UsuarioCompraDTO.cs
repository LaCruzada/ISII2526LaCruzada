using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppForSEII2526.API.ComprarBonoBocadilloDTOs
{
    public class UsuarioCompraDTO
    {
        [JsonPropertyName("nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [JsonPropertyName("apellido1")]
        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [StringLength(50)]
        public string Apellido1 { get; set; }

        [JsonPropertyName("apellido2")]
        [StringLength(50)]
        public string Apellido2 { get; set; }

        [JsonPropertyName("userName")]
        public string? UserName { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}