using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AppForSEII2526.API.Models
{
    public class TipoProducto
    {
        public TipoProducto()
        {
            Productos = new List<Producto>();
        }

        public TipoProducto(int tipoProductoId, string nombre)
        {
            TipoProductoId = tipoProductoId;
            Nombre = nombre;
            Productos = new List<Producto>();
        }

        [Key]
        public int TipoProductoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Relación: Un TipoProducto puede tener muchos Productos
        public ICollection<Producto> Productos { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TipoProducto tipoProducto &&
                   TipoProductoId == tipoProducto.TipoProductoId &&
                   Nombre == tipoProducto.Nombre;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TipoProductoId, Nombre);
        }
    }
}
