
namespace AppForSEII2526.API.Models
{
    public class ResenyaBocadillo
    {
        [Key]
        public int ResenyaBocadilloId { get; set; }

        public int BocadilloId { get; set; }

        public int ResenyaId { get; set; }

        [Range(1, 5)]
        public int Puntuacion { get; set; }

        public Bocadillo Bocadillo { get; set; }
        public Resenya Resenya { get; set; }
        public ResenyaBocadillo()
        {
            Bocadillo = new Bocadillo();
            Resenya = new Resenya();
        }

        public ResenyaBocadillo(int resenyaBocadilloId, int bocadilloId, int resenyaId, int puntuacion, Bocadillo bocadillo, Resenya resenya)
        {
            ResenyaBocadilloId = resenyaBocadilloId;
            BocadilloId = bocadilloId;
            ResenyaId = resenyaId;
            Puntuacion = puntuacion;
            Bocadillo = bocadillo;
            Resenya = resenya;
        }

        public override bool Equals(object? obj)
        {
            return obj is ResenyaBocadillo bocadillo &&
                   ResenyaBocadilloId == bocadillo.ResenyaBocadilloId &&
                   BocadilloId == bocadillo.BocadilloId &&
                   ResenyaId == bocadillo.ResenyaId &&
                   Puntuacion == bocadillo.Puntuacion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ResenyaBocadilloId, BocadilloId, ResenyaId, Puntuacion);
        }
    }
}
