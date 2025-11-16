using AppForSEII2526.API.DTOs.DTOsCrearResenya.BocadillosDTOs;
using AppForSEII2526.API.DTOs.DTOsCrearResenya.ResenyaDTOs;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers.ControllerCrearResenya
{
    [ApiController]
    [Route("api/[controller]")]
    public class BocadilloResenyaSelectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BocadilloResenyaSelectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<BocadilloSeleccionadoDTO>>> GetBocadillosParaResenya(
        [FromQuery] string? nombre,
        [FromQuery] decimal? minPvp,
        [FromQuery] decimal? maxPvp)
        {
            try
            {
                var query = _context.Bocadillos.AsQueryable();

                // Filtro por nombre del bocadillo
                if (!string.IsNullOrEmpty(nombre))
                {
                    query = query.Where(b => b.Nombre.ToLower().Contains(nombre.ToLower()));
                }

                // Filtro por PVP mínimo
                if (minPvp.HasValue)
                {
                    query = query.Where(b => b.PVP >= minPvp.Value);
                }

                // Filtro por PVP máximo
                if (maxPvp.HasValue)
                {
                    query = query.Where(b => b.PVP <= maxPvp.Value);
                }

                var bocadillos = await query
                    .Select(b => new BocadilloSeleccionadoDTO
                    {
                        Id = b.Id,
                        Nombre = b.Nombre,
                        PVP = b.PVP,
                        Tamano = b.Tamano,
                        TipoPanId = b.TipoPanId
                    })
                    .ToListAsync();

                if (!bocadillos.Any())
                {
                    return NotFound(new { message = "No se encontraron bocadillos disponibles para crear reseñas." });
                }

                return Ok(bocadillos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener los bocadillos: " + ex.Message });
            }
        }
    }
}
