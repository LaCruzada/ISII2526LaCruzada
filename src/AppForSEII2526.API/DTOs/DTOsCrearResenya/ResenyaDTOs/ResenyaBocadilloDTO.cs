using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.DTOsCrearResenya.ResenyaDTOs
{
    public class ResenyaBocadilloDTO
    {
        public int BocadilloId { get; set; }
        public string Nombre { get; set; }
        public decimal PVP { get; set; }
        public EnumTamaño Tamano { get; set; }
        public int Puntuacion { get; set; }

        public ResenyaBocadilloDTO() { }

        public ResenyaBocadilloDTO(int bocadilloId, string nombre, decimal pvp, EnumTamaño tamano, int puntuacion)
        {
            BocadilloId = bocadilloId;
            Nombre = nombre;
            PVP = pvp;
            Tamano = tamano;
            Puntuacion = puntuacion;
        }

        public override bool Equals(object? obj)
        {
            return obj is ResenyaBocadilloDTO dto &&
                   BocadilloId == dto.BocadilloId &&
                   Nombre == dto.Nombre &&
                   PVP == dto.PVP &&
                   Tamano == dto.Tamano &&
                   Puntuacion == dto.Puntuacion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BocadilloId, Nombre, PVP, Tamano, Puntuacion);
        }
    }
}
