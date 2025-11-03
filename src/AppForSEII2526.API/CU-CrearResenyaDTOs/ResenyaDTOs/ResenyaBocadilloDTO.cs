using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.ResenyaDTOs
{
    public class ResenyaBocadilloDTO
    {
        public ResenyaBocadilloDTO() { }

        public ResenyaBocadilloDTO(int bocadilloId, int resenyaId, int puntuacion)
        {
            BocadilloId = bocadilloId;
            ResenyaId = resenyaId;
            Puntuacion = puntuacion;
        }

        [Key]
        public int BocadilloId { get; set; }

        public int ResenyaId { get; set; }

        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5.")]
        public int Puntuacion { get; set; }

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
