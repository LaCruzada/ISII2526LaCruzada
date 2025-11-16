using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.DTOsCrearResenya.ResenyaDTOs
{
    public class ResenyaForCreacionDTO
    {
        public string NombreUsuario { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public EnumValoracion_General ValoracionGeneral { get; set; }
        public DateTime FechaPublicacion { get; set; }

        public List<ResenyaBocadilloDTO> ResenyaBocadillos { get; set; } = new List<ResenyaBocadilloDTO>();
        public ResenyaForCreacionDTO() { }

        public ResenyaForCreacionDTO(
            string nombreUsuario,
            string titulo,
            string descripcion,
            EnumValoracion_General valoracionGeneral,
            List<ResenyaBocadilloDTO> resenyaBocadillos,
            DateTime fechaPublicacion)
        {
            NombreUsuario = nombreUsuario;
            Titulo = titulo ?? throw new ArgumentNullException(nameof(titulo));
            Descripcion = descripcion ?? throw new ArgumentNullException(nameof(descripcion));
            ValoracionGeneral = valoracionGeneral;
            ResenyaBocadillos = resenyaBocadillos ?? new List<ResenyaBocadilloDTO>();
            FechaPublicacion = fechaPublicacion;
        }

        public override bool Equals(object? obj)
        {
            return obj is ResenyaForCreacionDTO dto &&
                   NombreUsuario == dto.NombreUsuario &&
                   Titulo == dto.Titulo &&
                   Descripcion == dto.Descripcion &&
                   ValoracionGeneral == dto.ValoracionGeneral &&
                   EqualityComparer<List<ResenyaBocadilloDTO>>.Default.Equals(ResenyaBocadillos, dto.ResenyaBocadillos) &&
                   FechaPublicacion == dto.FechaPublicacion;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(NombreUsuario, Titulo, Descripcion, ValoracionGeneral, ResenyaBocadillos, FechaPublicacion);
        }
    }
}
