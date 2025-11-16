using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.DTOsCrearResenya.ResenyaDTOs
{
    public class ResenyaDetallesDTO : ResenyaForCreacionDTO
    {
        public int Id { get; set; }

        public string Titulo { get; set; }


        public string Descripcion { get; set; }

        public EnumValoracion_General Valoracion_General { get; set; }

        public DateTime FechaPublicacion { get; set; }

        public string? NombreUsuario { get; set; }


        public List<ResenyaBocadilloDTO> ResenyaBocadillos { get; set; } = new List<ResenyaBocadilloDTO>();

        public ResenyaDetallesDTO() { }

        public ResenyaDetallesDTO(int idResenya, string titulo, string descripcion, EnumValoracion_General valoracion_General, DateTime fechaPublicacion, string? nombreUsuario, List<ResenyaBocadilloDTO> resenyaBocadillos)
        {
            Id = idResenya;
            Titulo = titulo;
            Descripcion = descripcion;
            Valoracion_General = valoracion_General;
            FechaPublicacion = fechaPublicacion;
            NombreUsuario = nombreUsuario;
            ResenyaBocadillos = resenyaBocadillos;
        }

        public override bool Equals(object? obj)
        {
            return obj is ResenyaDetallesDTO dto &&
                   base.Equals(obj) &&
                   Id == dto.Id &&
                   FechaPublicacion == dto.FechaPublicacion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, FechaPublicacion);
        }
    }
}
