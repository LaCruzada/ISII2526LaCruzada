using AppForSEII2526.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppForSEII2526.API.ComprarBonoBocadilloDTOs
{
    public class ComprarBonoBocadilloPost
    {
        public ComprarBonoBocadilloPost()
        {
            BonosCompra = new List<BonosCompradosDTO>();
        }

        [ValidateComplexType]
        [JsonPropertyName("BonosCompra")]
        public IList<BonosCompradosDTO> BonosCompra { get; set; }

        [Display(Name = "Precio Total")]
        [JsonPropertyName("precioTotal")]
        public double PrecioTotal
        {
            get
            {
                return BonosCompra.Sum(pi => pi.Cantidad * pi.PrecioUnitario);
            }
        }

       
        [Required]
        [JsonPropertyName("usuario")]
        public UsuarioCompraDTO usuario { get; set; }

        [Required]
        [JsonPropertyName("metodoDePago")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MetodoPago MetodoPago { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ComprarBonoBocadilloPost post &&
                   EqualityComparer<IList<BonosCompradosDTO>>.Default.Equals(BonosCompra, post.BonosCompra) &&
                   PrecioTotal == post.PrecioTotal &&
                   EqualityComparer<UsuarioCompraDTO>.Default.Equals(usuario, post.usuario) &&
                   MetodoPago == post.MetodoPago;
        }
    }
}
