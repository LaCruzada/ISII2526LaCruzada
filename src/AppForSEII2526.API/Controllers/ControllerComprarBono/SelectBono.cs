using AppForSEII2526.API.DTOs.CompraBonoDTOs;
using AppForSEII2526.API.Models;
using AppForSEII2526.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppForSEII2526.API.ComprarBonoBocadilloDTOs;

namespace AppForSEII2526.API.Controllers.ControllerComprarBono
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BonoController> _logger;

        public BonoController(ApplicationDbContext context, ILogger<BonoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Bono/GetBonoParaCompra?filtroNombre=...&tipoBocadillo=...
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ComprarBonosDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBonoParaCompra(string? filtroNombre, string? tipoBocadillo)
        {
            var query = _context.BonoBocadillo
                .Include(b => b.tipoBocadillos)
                .Include(b => b.bonosComprados)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtroNombre))
            {
                query = query.Where(b => b.nombre.Contains(filtroNombre));
            }

            if (!string.IsNullOrWhiteSpace(tipoBocadillo))
            {
                query = query.Where(b => b.tipoBocadillos!= null && b.tipoBocadillos.nombreTipo == tipoBocadillo);
            }

            var bonos = await query
                .OrderBy(b => b.nombre)
                .Select(b => new ComprarBonosDTO(
                    b.BonoId,
                    b.nombre,
                    b.PVP,
                    b.cantidadDisponible,
                    b.tipoBocadillos!= null ? b.tipoBocadillos.nombreTipo : string.Empty
                ))
                .ToListAsync();

            if (bonos == null || !bonos.Any())
            {
                _logger.LogInformation("No hay bonos que cumplan los requisitos (filtroNombre='{filtro}', tipo='{tipo}').", filtroNombre, tipoBocadillo);
                return NotFound("No hay bonos que cumplan los requisitos");
            }

            return Ok(bonos);
        }
    }
}