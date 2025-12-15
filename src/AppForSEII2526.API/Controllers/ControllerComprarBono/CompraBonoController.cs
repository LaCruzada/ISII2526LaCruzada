using AppForSEII2526.API.ComprarBonoBocadilloDTOs;

using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AppForSEII2526.API.Controllers.ControllerComprarBono
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
        public async Task<ActionResult> GetCompra(int id)
        {

            if(id <= 0)
            {
                return NotFound(new { message = "Pedido no encontrado por id igual o menor que cero" });
            }

            // Cargar la entidad completa con includes y luego mapear en memoria.
            var compra = await _context.CompraBono
                .Include(cb => cb.bonosComprados!)
                    .ThenInclude(bc => bc.Bono!)
                        .ThenInclude(b => b.tipoBocadillos)
                .Include(cb => cb.usuarios) // cargar usuarios relacionados
                .FirstOrDefaultAsync(cb => cb.CompraBonoId == id);

            if (_context.CompraBono == null)
            {
                _logger.LogError("Error: No hay compras disponibles");
                return NotFound();
            }

            if (compra == null)
            {
                _logger.LogError($"Error: Compra con id {id} no existe");
                return NotFound();
            }

            var cliente = compra.usuarios?.FirstOrDefault();

            var bonosDto = compra.bonosComprados?
                .Select(bc => new BonosCompradosDTO
                {
                    BonoID = bc.Bono != null ? bc.Bono.BonoId : 0,
                    Nombre = bc.Bono != null ? bc.Bono.nombre : string.Empty,
                    PrecioUnitario = bc.PrecioBono,
                    Cantidad = bc.Cantidad,
                    Tipo = bc.Bono?.tipoBocadillos?.nombreTipo ?? string.Empty
                })
                .ToList() ?? new List<BonosCompradosDTO>();

            var detalle = new ComprarBonoBocadilloDetalle(
                compra.CompraBonoId,
                cliente ?? new ApplicationUser(),
                compra.metodoPago,
                compra.FechaCompraBono,
                compra.PrecioTotalBono,
                bonosDto
            );

            return Ok(detalle);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ComprarBonoBocadilloDetalle), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CrearCompra(ComprarBonoBocadilloPost crearCompra)
        {
            if (crearCompra == null)
            {
                ModelState.AddModelError("CrearCompra", "Error! El cuerpo de la petición no puede ser vacío");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (crearCompra.BonoItem == null || crearCompra.BonoItem.Count == 0)
            {
                ModelState.AddModelError("Bonos", "Error! Debes seleccionar algún bono");
            }

            if (string.IsNullOrWhiteSpace(crearCompra.NombreCliente))
                ModelState.AddModelError("NombreCliente", "Error! El nombre es obligatorio");

            if (string.IsNullOrWhiteSpace(crearCompra.ApellidoCliente1) || string.IsNullOrWhiteSpace(crearCompra.ApellidoCliente2))
                ModelState.AddModelError("Apellidos", "Error! Los apellidos son obligatorios");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Buscar cliente por nombre/apellidos
            ApplicationUser? cliente = _context.Users
                .FirstOrDefault(u => u.Nombre == crearCompra.NombreCliente
                                  && u.Apellido1 == crearCompra.ApellidoCliente1
                                  && u.Apellido2 == crearCompra.ApellidoCliente2);

   
            if (cliente == null)
            {
                cliente = new ApplicationUser
                {
                    Nombre = crearCompra.NombreCliente,
                    Apellido1 = crearCompra.ApellidoCliente1,
                    Apellido2 = crearCompra.ApellidoCliente2,
                
                };
                _context.Users.Add(cliente);
            }

            // Crear la entidad CompraBono (usamos ahora la fecha actual)
            var com = new CompraBono
            {
                FechaCompraBono = DateTime.Now,
                nBonos = 0,
                PrecioTotalBono = 0f,
                metodoPago = crearCompra.pago,
                bonosComprados = new List<BonosComprados>(),
                usuarios = new List<ApplicationUser> { cliente }
            };

            // Procesar items (usar BonosCompra)
            foreach (var item in crearCompra.BonoItem)
            {
                if (item == null)
                {
                    ModelState.AddModelError("Bonos", "Error! Item de bono inválido");
                    continue;
                }

                // Cargar bono con su tipo
                var bonoEntity = await _context.BonoBocadillo
                    .Include(b => b.tipoBocadillos)
                    .FirstOrDefaultAsync(b => b.BonoId == item.BonoID);

                if (bonoEntity == null)
                {
                    ModelState.AddModelError("Bonos", $"Error, el bono '{item?.BonoID}' no existe en nuestra tienda");
                    continue;
                }

                if (item.Cantidad <= 0)
                {
                    ModelState.AddModelError("Bonos", $"Error, la cantidad solicitada para '{bonoEntity.nombre}' debe ser mayor que 0");
                    continue;
                }

                if (bonoEntity.cantidadDisponible < item.Cantidad)
                {
                    ModelState.AddModelError("Bonos", $"Error, no hay suficiente stock del bono '{bonoEntity.nombre}'. Disponibles: {bonoEntity.cantidadDisponible}, solicitados: {item.Cantidad}");
                    continue;
                }

                // Actualizar stock y añadir el item a la compra
                bonoEntity.cantidadDisponible -= item.Cantidad;

                var precioUnidad = bonoEntity.PVP;
                var bonosComprados = new BonosComprados
                {
                    Cantidad = item.Cantidad,
                    PrecioBono = (float)precioUnidad,
                    Bono = bonoEntity,
                    Compra = com
                };

                com.bonosComprados.Add(bonosComprados);
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Actualizar totales
            com.nBonos = com.bonosComprados.Sum(bc => bc.Cantidad);
            com.PrecioTotalBono = com.bonosComprados.Sum(bc => bc.Cantidad * bc.PrecioBono);

            _context.CompraBono.Add(com);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando la compra");
                ModelState.AddModelError("Compra", "Error! Ocurrió un error al guardar la compra, inténtalo más tarde");
                return Conflict("Error: " + ex.Message);
            }

            // Construir DTO de respuesta (detalle)
            var compraDetalles = new ComprarBonoBocadilloDetalle(
                com.CompraBonoId,
                cliente!,
                com.metodoPago,
                com.FechaCompraBono,
                com.PrecioTotalBono,
                com.bonosComprados.Select(bc => new BonosCompradosDTO(
                    bc.Bono?.BonoId ?? 0,
                    bc.Bono?.nombre ?? string.Empty,
                    bc.PrecioBono,
                    bc.Cantidad,
                    bc.Bono?.tipoBocadillos?.nombreTipo ?? string.Empty
                )).ToList()
            );

            return CreatedAtAction(nameof(GetCompra), new { id = com.CompraBonoId }, compraDetalles);
        }
    }
}
