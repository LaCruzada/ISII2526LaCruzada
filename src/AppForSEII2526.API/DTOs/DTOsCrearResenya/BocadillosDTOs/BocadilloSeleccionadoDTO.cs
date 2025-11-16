using System;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.DTOsCrearResenya.BocadillosDTOs
{
    public class BocadilloSeleccionadoDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public decimal PVP { get; set; }

        public EnumTamaño Tamano { get; set; }

        public int TipoPanId { get; set; }

        public BocadilloSeleccionadoDTO() { }

        public BocadilloSeleccionadoDTO(int id, string nombre, decimal pvp, EnumTamaño tamano, int tipoPanId)
        {
            Id = id;
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            PVP = pvp;
            Tamano = tamano;
            TipoPanId = tipoPanId;
        }

        public override bool Equals(object? obj)
        {
            return obj is BocadilloSeleccionadoDTO dto &&
                   Id == dto.Id &&
                   Nombre == dto.Nombre &&
                   PVP == dto.PVP &&
                   Tamano == dto.Tamano &&
                   TipoPanId == dto.TipoPanId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, PVP, Tamano, TipoPanId);
        }
    }
}
