using AppForSEII2526.API.DTOs.CompraBonoDTOs;
using AppForSEII2526.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppForSEII2526.API.ComprarBonoBocadilloDTOs
{
    public class ComprarBonoBocadilloDetalle : ComprarBonoBocadilloPost
    {
     
        public ComprarBonoBocadilloDetalle(int id, ApplicationUser Cliente, MetodoPago metodo, DateTime fechaCompra,
                                        double precioTotal, IList<BonosCompradosDTO> bonoItem)
            : base(id, Cliente.Nombre, Cliente.Apellido1, Cliente.Apellido2, fechaCompra,metodo,bonoItem)
        {
            ID = id;
           
        }
        [Key]
        [JsonIgnore]
        public int ID { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not ComprarBonoBocadilloDetalle dTO) return false;

            return base.Equals(dTO) &&
                   ID == dTO.ID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ID);
        }
    }
}
