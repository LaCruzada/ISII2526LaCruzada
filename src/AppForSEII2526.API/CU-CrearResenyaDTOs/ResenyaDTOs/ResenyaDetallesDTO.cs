using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.ResenyaDTOs
{
    public class ResenyaDetallesDTO : ResenyaForCreacionDTO
    {
        public ResenyaDetallesDTO() { }

        public ResenyaDetallesDTO(int idResenya,string? nombreUsuario,string titulo,string descripcion,EnumValoracion_General valoracionGeneral,DateTime fechaPublicacion,List<ResenyaBocadilloDTO> resenyaBocadillos)
            : base(nombreUsuario, titulo, descripcion, valoracionGeneral, resenyaBocadillos)
        {
            IdResenya = idResenya;
            FechaPublicacion = fechaPublicacion;
        }

        public int IdResenya { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime), Display(Name = "Fecha Reseña")]
        [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaPublicacion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ResenyaDetallesDTO dto &&
                   base.Equals(obj) &&
                   IdResenya == dto.IdResenya &&
                   FechaPublicacion == dto.FechaPublicacion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), IdResenya, FechaPublicacion);
        }
    }
}
