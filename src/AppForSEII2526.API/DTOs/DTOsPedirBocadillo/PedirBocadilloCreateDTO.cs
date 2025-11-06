using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.DTOsPedirBocadillo
{
    public class PedirBocadilloCreateDTO
    {
        /*
        Version anterior antes de separar en pasos:
        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public string MetodoPago { get; set; } // "Tarjeta", "PayPal", "GPay"

        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un bocadillo")]
        public List<BocadilloPedidoItemDTO> Bocadillos { get; set; }*/


        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        public string Apellido1Cliente { get; set; }

        public string? Apellido2Cliente { get; set; } 

        [Required(ErrorMessage = "El Email es obligatorio")]
        [EmailAddress]
        public string EmailCliente { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio")]
        public string MetodoPago { get; set; }

        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un bocadillo")]
        public List<BocadilloPedidoItemDTO> Bocadillos { get; set; }
    }
}