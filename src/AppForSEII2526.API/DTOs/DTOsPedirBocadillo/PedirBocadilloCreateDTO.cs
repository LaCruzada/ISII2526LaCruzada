using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.DTOsPedirBocadillo
{
    public class PedirBocadilloCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        public string Apellido1Cliente { get; set; }

        public string? Apellido2Cliente { get; set; }

        [EmailAddress]
        public string? EmailCliente { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public string MetodoPago { get; set; }

        public virtual List<BocadilloPedidoItemDTO> Bocadillos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PedirBocadilloCreateDTO dTO &&
                   NombreCliente == dTO.NombreCliente &&
                   Apellido1Cliente == dTO.Apellido1Cliente &&
                   Apellido2Cliente == dTO.Apellido2Cliente &&
                   EmailCliente == dTO.EmailCliente &&
                   MetodoPago == dTO.MetodoPago &&
                   EqualityComparer<List<BocadilloPedidoItemDTO>>.Default.Equals(Bocadillos, dTO.Bocadillos);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreCliente, Apellido1Cliente, Apellido2Cliente, EmailCliente, MetodoPago, Bocadillos);
        }
    }
}