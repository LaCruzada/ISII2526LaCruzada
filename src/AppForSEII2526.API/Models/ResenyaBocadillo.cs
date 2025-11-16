public class ResenyaBocadillo
{
    [Key]
    public int BocadilloId { get; set; }

    public int ResenyaId { get; set; }

    [Range(1, 5)]
    public int Puntuacion { get; set; }

    public Bocadillo Bocadillo { get; set; }
    public Resenya Resenya { get; set; }

    public ResenyaBocadillo() { }

    public ResenyaBocadillo(int bocadilloId, int resenyaId, int puntuacion, Bocadillo bocadillo, Resenya resenya)
    {
        BocadilloId = bocadilloId;
        ResenyaId = resenyaId;
        Puntuacion = puntuacion;
        Bocadillo = bocadillo;
        Resenya = resenya;
    }
}
