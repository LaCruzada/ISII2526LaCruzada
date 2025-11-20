using AppForSEII2526.API.DTOs.CompraBonoDTOs;
using AppForSEII2526.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppForSEII2526.API.ComprarBonoBocadilloDTOs
{
    public class ComprarBonoBocadilloDetalle : ComprarBonoBocadilloPost
    {
        //7. El sistema muestra los datos de la compra realizada, mostrando el nombre y apellidos al que están los bonos adquiridos,
        //el método de pago y la fecha en la que se hizo, así como el precio total. De cada bono se muestra el nombre, tipo, precio y cantidad.
        public ComprarBonoBocadilloDetalle(int id, ApplicationUser Cliente, MetodoPago metodo, DateTime fechaCompra,
                                        double precioTotal, IList<BonosCompradosDTO> bonoItem)
            : base(id, Cliente.Nombre, Cliente.Apellido1, Cliente.Apellido2, fechaCompra,metodo,bonoItem)
        {
            ID = id;
           
        }
        [Key]
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
