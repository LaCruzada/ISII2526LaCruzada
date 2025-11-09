// File: Controllers/ProductoSelectController.cs
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DTOsCompraMerchandising;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoSelectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductoSelectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductoSelectDTO>>> GetProductosDisponibles(
            [FromQuery] string? tipo,
            [FromQuery] decimal? minPrecio,
            [FromQuery] decimal? maxPrecio)
        {
            try
            {
                var query = _context.Producto
                    .Where(p => p.Stock > 0)
                    .AsQueryable();

                // Filtro por tipo (si tu TipoProducto es enum)
                if (!string.IsNullOrEmpty(tipo))
                {
                    // Buscar el tipo por nombre en la base de datos
                    var tipoProductoDb = await _context.TipoProducto
                        .FirstOrDefaultAsync(t => t.Nombre.ToLower() == tipo.ToLower());

                    if (tipoProductoDb != null)
                    {
                        query = query.Where(p => p.TipoProducto.TipoProductoId == tipoProductoDb.TipoProductoId);
                    }
                    else
                    {
                        return BadRequest(new { message = $"El tipo '{tipo}' no es un filtro válido." });
                    }
                }

                // Filtro por rango de precio mínimo
                if (minPrecio.HasValue)
                {
                    query = query.Where(p => p.PVP >= minPrecio.Value);
                }

                // Filtro por rango de precio máximo
                if (maxPrecio.HasValue)
                {
                    query = query.Where(p => p.PVP <= maxPrecio.Value);
                }

                var productos = await query
                    .Select(p => new ProductoSelectDTO
                    {
                        Id = p.ProductoId,  // O p.Productold según tu modelo exacto
                        Nombre = p.Nombre,
                        Precio = p.PVP,
                        Tipo = p.TipoProducto.ToString(),
                        Stock = p.Stock
                    })
                    .ToListAsync();

                if (!productos.Any())
                {
                    return NotFound(new { message = "No hay merchandising disponible que cumplan los criterios." });
                }

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener productos: " + ex.Message });
            }
        }
    }
}