using AppForSEII2526.API.DTOs.DTOsCrearResenya.BocadillosDTOs;
using AppForSEII2526.API.DTOs.DTOsCrearResenya.ResenyaDTOs;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.DTOs.DTOsCompraMerchandising;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AppForSEII2526.API.Controllers.ControllerCrearResenya
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrearResenyaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CrearResenyaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResenyaDetallesDTO>> GetResenya(int id)
        {
            if (_context.Resenyas == null)
                return NotFound(new { message = "Reseña no encontrada" });

            var resenyaDTO = await _context.Resenyas
                .Where(resenya => resenya.Id == id)
                    .Include(resenya => resenya.ResenyaBocadillos)
                        .ThenInclude(bocadillo => bocadillo.Bocadillo)
                .Select(resenya => new ResenyaDetallesDTO(
                        resenya.Id,
                        resenya.Titulo,
                        resenya.Descripcion,
                        resenya.Valoracion_General,
                        resenya.FechaPublicacion,
                        resenya.NombreUsuario,
                        resenya.ResenyaBocadillos
                            .Select(rb => new ResenyaBocadilloDTO(
                                rb.BocadilloId,
                                rb.Bocadillo.Nombre,
                                rb.Bocadillo.PVP,
                                rb.Bocadillo.Tamano,
                                rb.Puntuacion
                            ))
                            .ToList()
                    ))

                .FirstOrDefaultAsync();
            if (resenyaDTO == null)
            {
                return NotFound(new { message = "Reseña no encontrada" });
            }
            return Ok(resenyaDTO);
    }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ResenyaDetallesDTO), (int)System.Net.HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CrearResenya([FromBody] ResenyaForCreacionDTO resenyaforcreacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (resenyaforcreacion.ResenyaBocadillos == null || !resenyaforcreacion.ResenyaBocadillos.Any())
            {
                ModelState.AddModelError("ResenyaBocadillos", "Error! Debes añadir al menos un bocadillo para reseñar.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (string.IsNullOrWhiteSpace(resenyaforcreacion.Titulo) ||
                string.IsNullOrWhiteSpace(resenyaforcreacion.Descripcion))
            {
                ModelState.AddModelError("CamposObligatorios", "Error! Debes proporcionar título y descripción.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (!Enum.IsDefined(typeof(EnumValoracion_General), resenyaforcreacion.ValoracionGeneral))
            {
                ModelState.AddModelError("ValoracionGeneral", "La valoración general no es válida.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            ApplicationUser usuarioEnBd = null;
            if (!string.IsNullOrWhiteSpace(resenyaforcreacion.NombreUsuario))
            {
                usuarioEnBd = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == resenyaforcreacion.NombreUsuario);

                if (usuarioEnBd == null)
                {
                    usuarioEnBd = new ApplicationUser
                    {
                        UserName = resenyaforcreacion.NombreUsuario,
                        Email = resenyaforcreacion.NombreUsuario,
                        Nombre = "NombrePorDefecto",
                        Apellido1 = "Apellido1PorDefecto",
                        Apellido2 = "" // SQLite NO acepta null
                    };
                    _context.Users.Add(usuarioEnBd);
                    await _context.SaveChangesAsync();
                }
            }

            var resenya = new Resenya(
                resenyaforcreacion.Titulo,
                resenyaforcreacion.Descripcion,
                resenyaforcreacion.ValoracionGeneral,
                DateTime.Now,
                usuarioEnBd?.UserName
            );

            foreach (var item in resenyaforcreacion.ResenyaBocadillos)
            {
                var bocadillo = await _context.Bocadillos.FindAsync(item.BocadilloId);
                if (bocadillo == null)
                {
                    ModelState.AddModelError("ResenyaBocadillos", $"Bocadillo {item.BocadilloId} no encontrado.");
                }
                else
                {
                    var resenyaBocadillo = new ResenyaBocadillo(
                        item.BocadilloId,
                        resenya.Id,
                        item.Puntuacion,
                        bocadillo,
                        resenya
                    );
                    resenya.ResenyaBocadillos.Add(resenyaBocadillo);
                }
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Resenyas.Add(resenya);
            await _context.SaveChangesAsync();

            var resenyaDetalles = await _context.Resenyas
                .Where(r => r.Id == resenya.Id)
                .Include(r => r.ResenyaBocadillos)
                    .ThenInclude(rb => rb.Bocadillo)
                .FirstOrDefaultAsync();

            var resenyaDto = new ResenyaDetallesDTO(
                resenyaDetalles.Id,
                resenyaDetalles.Titulo,
                resenyaDetalles.Descripcion,
                resenyaDetalles.Valoracion_General,
                resenyaDetalles.FechaPublicacion,
                resenyaDetalles.NombreUsuario,
                resenyaDetalles.ResenyaBocadillos
                    .Select(rb => new ResenyaBocadilloDTO(
                        rb.BocadilloId,
                        rb.Bocadillo.Nombre,
                        rb.Bocadillo.PVP,
                        rb.Bocadillo.Tamano,
                        rb.Puntuacion
                    )).ToList()
            );

            return CreatedAtAction(nameof(GetResenya), new { id = resenyaDto.Id }, resenyaDto);
        }

    }
}
