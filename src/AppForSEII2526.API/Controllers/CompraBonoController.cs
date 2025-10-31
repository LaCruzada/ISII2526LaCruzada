using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppForSEII2526.API.ComprarBonoBocadilloDTOs;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraBonoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompraBonoController> _logger;

        public CompraBonoController(ApplicationDbContext context, ILogger<CompraBonoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ComprarBonoBocadilloDetalle), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBonoCompra(int id)
        {
            if (_context.CompraBono == null)
            {
                _logger.LogError("Error: la tabla CompraBono no existe");
                return NotFound();
            }       

            var compraDto = await _context.CompraBono
                .Where(compra => compra.CompraBonoId == id)
                    .Include(compra => compra.bonosComprados)
                        .ThenInclude(bc => bc.Bono)
                            .ThenInclude(bono => bono.tipoBocadillos)
                    .Include(compra => compra.usuario)
                .Select(compra => new ComprarBonoBocadilloDetalle(
                    compra.CompraBonoId,
                    compra.bonosComprados
                        .Select(bc => new BonosCompradosDTO(
                            bc.Bono.BonoId,
                            bc.Bono.nombre,
                            bc.PrecioBono,
                            bc.Cantidad,
                            bc.Bono.tipoBocadillos.nombreTipo
                        ))
                        .ToList<BonosCompradosDTO>(),
                    compra.usuario.FirstOrDefault(),
                    compra.metodoPago,
                    compra.FechaCompraBono
                ))
                .FirstOrDefaultAsync();

            if (compraDto == null)
            {
                _logger.LogError($"Error: La compra con id {id} no existe.");
                return NotFound();
            }

            return Ok(compraDto);
        }

        // POST: api/CompraBono/CrearCompraBono
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ComprarBonoBocadilloDetalle), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CrearCompraBono(ComprarBonoBocadilloPost compraPorCrear)
        {
            if (_context.CompraBono == null)
            {
                _logger.LogError("Error: la tabla CompraBono no existe.");
                return Problem("Entity set 'ApplicationDbContext.CompraBono' is null.");
            }

            if (compraPorCrear?.BonosCompra == null || compraPorCrear.BonosCompra.Count == 0)
            {
                ModelState.AddModelError("BonosCompra", "Error! Debes incluir al menos un bono para comprarlo");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (compraPorCrear.usuario == null ||
                string.IsNullOrWhiteSpace(compraPorCrear.usuario.Nombre) ||
                string.IsNullOrWhiteSpace(compraPorCrear.usuario.Apellido1))
            {
                ModelState.AddModelError("usuario", "Error! Debes proporcionar Nombre y Apellido1 del cliente (campos obligatorios).");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var bonoIDs = compraPorCrear.BonosCompra.Select(b => b.BonoID).ToList<int>();

            var bonos = await _context.BonoBocadillo
                .Include(b => b.bonosComprados)
                .Include(b => b.tipoBocadillos)
                .Where(b => bonoIDs.Contains(b.BonoId))
                .ToListAsync();

          
            foreach (var lineaBono in compraPorCrear.BonosCompra)
            {
                var b = bonos.FirstOrDefault(b => b.BonoId == lineaBono.BonoID);
                if (b != null && lineaBono.PrecioUnitario == 0)
                {
                    lineaBono.PrecioUnitario = b.PVP;
                }
            }

            ApplicationUser usuarioEnBd = null;
            if (!string.IsNullOrWhiteSpace(compraPorCrear.usuario.UserName))
            {
                usuarioEnBd = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == compraPorCrear.usuario.UserName);
            }
            else if (!string.IsNullOrWhiteSpace(compraPorCrear.usuario.Id))
            {
                usuarioEnBd = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == compraPorCrear.usuario.Id);
            }

            if (usuarioEnBd == null)
            {
                usuarioEnBd = new ApplicationUser
                {
                    UserName = $"{compraPorCrear.usuario.Nombre}.{compraPorCrear.usuario.Apellido1}.{Guid.NewGuid():N}",
                    Nombre = compraPorCrear.usuario.Nombre,
                    Apellido1 = compraPorCrear.usuario.Apellido1,
                    Apellido2 = compraPorCrear.usuario.Apellido2
                };
                _context.Users.Add(usuarioEnBd);
            }

            CompraBono compra = new CompraBono(
                0,
                DateTime.Today,
                compraPorCrear.BonosCompra.Sum(b => b.Cantidad),
                (float)compraPorCrear.PrecioTotal,
                compraPorCrear.MetodoPago,
                new List<BonosComprados>(),
                new List<ApplicationUser> { usuarioEnBd }
            );

            BonoBocadillo bono;
            foreach (var item in compraPorCrear.BonosCompra)
            {
                bono = await _context.BonoBocadillo
                    .Include(b => b.tipoBocadillos)
                    .FirstOrDefaultAsync(b => b.BonoId == item.BonoID);
                
                if (bono == null)
                {
                    ModelState.AddModelError("BonosCompra", $"Error! El bono con nombre {item.Nombre} y con ID {item.BonoID} no existe en la base de datos.");
                }
                else
                {
                    if (bono.cantidadDisponible < item.Cantidad)
                    {
                        ModelState.AddModelError("BonosCompra", $"Error! El bono con nombre {item.Nombre} solo tiene {bono.cantidadDisponible} unidades disponibles, pero has seleccionado {item.Cantidad} unidades para comprar.");
                    }
                    else
                    {
                        bono.cantidadDisponible -= item.Cantidad;
                        
                        var bonoComprado = new BonosComprados
                        {
                            BonoId = bono.BonoId,
                            Cantidad = item.Cantidad,
                            PrecioBono = (float)item.PrecioUnitario,
                            Bono = bono,
                            Compra = compra
                        };
                        
                        compra.bonosComprados.Add(bonoComprado);
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.CompraBono.Add(compra);
            await _context.SaveChangesAsync();

          
            var compraGuardada = await _context.CompraBono
                .Where(c => c.CompraBonoId == compra.CompraBonoId)
                .Include(c => c.bonosComprados)
                    .ThenInclude(bc => bc.Bono)
                        .ThenInclude(b => b.tipoBocadillos)
                .Include(c => c.usuario)
                .FirstOrDefaultAsync();

            var bonosDto = compraGuardada.bonosComprados
                .Select(bc => new BonosCompradosDTO(
                    bc.Bono.BonoId,
                    bc.Bono.nombre,
                    bc.PrecioBono,
                    bc.Cantidad,
                    bc.Bono.tipoBocadillos?.nombreTipo ?? string.Empty
                ))
                .ToList();

            var detalleCompra = new ComprarBonoBocadilloDetalle(
                compraGuardada.CompraBonoId,
                bonosDto,
                compraGuardada.usuario.FirstOrDefault(),
                compraGuardada.metodoPago,
                compraGuardada.FechaCompraBono
            );

            return CreatedAtAction("GetBonoCompra", new { id = compra.CompraBonoId }, detalleCompra);
        }
    }
}