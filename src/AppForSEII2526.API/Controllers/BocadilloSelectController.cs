using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.Models; 
using System;
using System.Linq; 

namespace AppForSEII2526.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BocadilloSelectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BocadilloSelectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<BocadilloSelectDTO>>> GetBocadillosDisponibles(
            [FromQuery] string? tamano,
            [FromQuery] int? tipoPanId)
        {
            try
            {
                var query = _context.Bocadillos
                    .Where(b => b.Stock > 0)
                    .Include(b => b.TipoPan)
                    .AsQueryable();

               
                if (!string.IsNullOrEmpty(tamano))
                {
                   
                    bool isValidEnum = Enum.TryParse<EnumTamaño>(tamano, true, out EnumTamaño tamanoEnum);

                    if (isValidEnum)
                    {
                       
                        query = query.Where(b => b.Tamano == tamanoEnum);
                    }
                    else
                    {
                       
                        query = query.Where(b => false); 
                    }
                }

             
                if (tipoPanId.HasValue)
                {
                    query = query.Where(b => b.TipoPan.PanId == tipoPanId.Value);
                }

                
                var bocadillos = await query.Select(b => new BocadilloSelectDTO
                {
                    Id = b.Id,
                    Nombre = b.Nombre,
                    Precio = b.PVP,
                    TipoPan = b.TipoPan.Nombre,
                    Tamano = b.Tamano.ToString(),
                    Stock = b.Stock
                })
                    .ToListAsync();

                if (!bocadillos.Any())
                {
                    return NotFound(new { message = "No hay bocadillos disponibles que cumplan los criterios" });
                }

                return Ok(bocadillos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener bocadillos: " + ex.Message });
            }
        }
    }
}