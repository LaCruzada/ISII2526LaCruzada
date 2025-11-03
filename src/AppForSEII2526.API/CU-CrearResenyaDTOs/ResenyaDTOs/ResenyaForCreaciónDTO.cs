using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.ResenyaDTOs
{
    public class ResenyaForCreacionDTO
    {
        public ResenyaForCreacionDTO() { }

        public ResenyaForCreacionDTO(
            string? nombreUsuario,
            string titulo,
            string descripcion,
            EnumValoracion_General valoracionGeneral,
            List<ResenyaBocadilloDTO> resenyaBocadillos)
        {
            NombreUsuario = nombreUsuario;
            Titulo = titulo ?? throw new ArgumentNullException(nameof(titulo));
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            ValoracionGeneral = valoracionGeneral;
            ResenyaBocadillos = resenyaBocadillos ?? new List<ResenyaBocadilloDTO>();
        }

        [StringLength(20, ErrorMessage = "El nombre no puede ocupar más de 20 caracteres.", MinimumLength = 1)]
        public string? NombreUsuario { get; set; }

        [Required, StringLength(20, ErrorMessage = "El título no puede ocupar más de 20 caracteres.", MinimumLength = 1)]
        public string Titulo { get; set; }

        [Required, StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres.", MinimumLength = 1)]
        public string Descripcion { get; set; }

        [Required]
        public EnumValoracion_General ValoracionGeneral { get; set; }

        public List<ResenyaBocadilloDTO> ResenyaBocadillos { get; set; } = new List<ResenyaBocadilloDTO>();
        public override bool Equals(object? obj)
        {
            return obj is ResenyaForCreacionDTO dto &&
                   NombreUsuario == dto.NombreUsuario &&
                   Titulo == dto.Titulo &&
                   Descripcion == dto.Descripcion &&
                   ValoracionGeneral == dto.ValoracionGeneral &&
                   EqualityComparer<List<ResenyaBocadilloDTO>>.Default.Equals(ResenyaBocadillos, dto.ResenyaBocadillos);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(NombreUsuario, Titulo, Descripcion, ValoracionGeneral, ResenyaBocadillos);
        }
    }
}
