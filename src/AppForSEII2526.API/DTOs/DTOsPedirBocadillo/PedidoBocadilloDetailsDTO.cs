using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.API.DTOs.DTOsPedirBocadillo
{
    public class PedidoBocadilloDetailsDTO : PedirBocadilloCreateDTO
    {
        public int PedidoId { get; set; }

        public DateTime FechaCompra { get; set; }
        public decimal PrecioTotal { get; set; }

        public new List<BocadilloItemDTO> Bocadillos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PedidoBocadilloDetailsDTO dto &&
                   PedidoId == dto.PedidoId &&
                   FechaCompra == dto.FechaCompra &&
                   PrecioTotal == dto.PrecioTotal &&
                   NombreCliente == dto.NombreCliente &&
                   Apellido1Cliente == dto.Apellido1Cliente &&
                   MetodoPago == dto.MetodoPago &&
                   (Bocadillos != null && dto.Bocadillos != null && Bocadillos.SequenceEqual(dto.Bocadillos));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PedidoId, FechaCompra, PrecioTotal, NombreCliente, Apellido1Cliente);
        }
    }
}