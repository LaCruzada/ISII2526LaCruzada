using RabbitMQ.Client;
using System;
using System.Dynamic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.Models
{
    public class TipoBocadillo
    {

        public TipoBocadillo()
        {
        }

        public TipoBocadillo(int idTipo, string nombreTipo, List<BonoBocadillo> bonoBocadillo)
        {
            this.idTipo = idTipo;
            this.nombreTipo = nombreTipo;
            BonoBocadillo = bonoBocadillo;
        }

        [Key]
        public int idTipo { get; set; }

        [Required]
        [StringLength(100)]
        public string nombreTipo { get; set; }

        public List<BonoBocadillo> BonoBocadillo { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TipoBocadillo bocadillo &&
                   idTipo == bocadillo.idTipo &&
                   nombreTipo == bocadillo.nombreTipo &&
                   EqualityComparer<List<BonoBocadillo>>.Default.Equals(BonoBocadillo, bocadillo.BonoBocadillo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(idTipo, nombreTipo, BonoBocadillo);
        }
    }
}
