
namespace AppForSEII2526.API.Models
{
    public class ResenyaBocadillo
    {
        [Key]
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

        public ResenyaBocadillo(int bocadilloId, int resenyaId, int puntuacion, Bocadillo bocadillo, Resenya resenya)
        {
            BocadilloId = bocadilloId;
            ResenyaId = resenyaId;
            Puntuacion = puntuacion;
            Bocadillo = bocadillo;
            Resenya = resenya;
        }

        public ResenyaBocadillo(Bocadillo bocadillo, int puntuacion)
        {
            Bocadillo = bocadillo;
            Puntuacion = puntuacion;
        }

        public override bool Equals(object? obj)
        {
            return obj is ResenyaBocadillo bocadillo &&
                   BocadilloId == bocadillo.BocadilloId &&
                   ResenyaId == bocadillo.ResenyaId &&
                   Puntuacion == bocadillo.Puntuacion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BocadilloId, ResenyaId, Puntuacion);
        }
    }
}
