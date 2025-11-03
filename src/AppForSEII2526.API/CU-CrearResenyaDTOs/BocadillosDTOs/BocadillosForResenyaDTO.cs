using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.BocadilloDTOs
{
    public class BocadillosForResenyaDTO
    {
        public BocadillosForResenyaDTO() { }

        public BocadillosForResenyaDTO(int id, string nombre, decimal pvp, EnumTamaño tamano, int tipoPanId, int stock, int comprasDelBocadillo)
        {
            Id = id;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            PVP = pvp;
            Tamano = tamano;
            TipoPanId = tipoPanId;
            Stock = stock;
            ComprasDelBocadillo = comprasDelBocadillo;
        }

        public int Id { get; set; }

        [Required, StringLength(20, ErrorMessage = "El nombre no puede ocupar más de 20 caracteres.", MinimumLength = 1)]
        public string Nombre { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal PVP { get; set; }

        [Required]
        public EnumTamaño Tamano { get; set; }

        [Required]
        public int TipoPanId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Las compras no pueden ser negativas.")]
        public int ComprasDelBocadillo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is BocadillosForResenyaDTO dto &&
                   Id == dto.Id &&
                   Nombre == dto.Nombre &&
                   PVP == dto.PVP &&
                   Tamano == dto.Tamano &&
                   TipoPanId == dto.TipoPanId &&
                   Stock == dto.Stock &&
                   ComprasDelBocadillo == dto.ComprasDelBocadillo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, PVP, Tamano, TipoPanId, Stock, ComprasDelBocadillo);
        }
    }
}
