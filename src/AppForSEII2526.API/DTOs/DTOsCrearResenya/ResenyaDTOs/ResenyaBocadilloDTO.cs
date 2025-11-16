using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.DTOsCrearResenya.ResenyaDTOs
{
    public class ResenyaBocadilloDTO
    {
        private string nombre;
        private decimal pVP;
        private EnumTamaño tamano;

        public int BocadilloId { get; set; }

        public int ResenyaId { get; set; }

        public int Puntuacion { get; set; }

        public ResenyaBocadilloDTO() { }

        public ResenyaBocadilloDTO(int bocadilloId, int resenyaId, int puntuacion)
        {
            BocadilloId = bocadilloId;
            ResenyaId = resenyaId;
            Puntuacion = puntuacion;
        }

        public ResenyaBocadilloDTO(string nombre, decimal pVP, EnumTamaño tamano, int puntuacion)
        {
            this.nombre = nombre;
            this.pVP = pVP;
            this.tamano = tamano;
            Puntuacion = puntuacion;
        }

        public override bool Equals(object? obj)
        {
            return obj is ResenyaBocadilloDTO dto &&
                   BocadilloId == dto.BocadilloId &&
                   ResenyaId == dto.ResenyaId &&
                   Puntuacion == dto.Puntuacion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BocadilloId, ResenyaId, Puntuacion);
        }
    }
}
