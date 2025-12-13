using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
using System.Linq;
using AppForSEII2526.API.DTOs.DTOsPedirBocadillo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppForSEII2526.API.Controllers.ControllerPedirBocadillo
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedirBocadilloController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PedirBocadilloController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoBocadilloDetailsDTO>> GetPedido(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound(new { message = "Pedido no encontrado por id igual o menor que cero" });
                }

                var compra = await _context.Compra
                    .Include(c => c.Cliente) 
                    .Include(c => c.BocadillosComprados)
                        .ThenInclude(bc => bc.Bocadillo)
                            .ThenInclude(b => b.TipoPan)
                    .FirstOrDefaultAsync(c => c.CompraId == id);

                if (compra == null)
                {
                    return BadRequest(new { message = "Pedido no encontrado" });
                }

                string nombre = "Desconocido";
                string apellido1 = "";
                string apellido2 = "";
                string email = "";

                if (compra.Cliente != null && compra.Cliente.Any())
                {
                    var primerCliente = compra.Cliente.First();
                    nombre = primerCliente.Nombre;
                    apellido1 = primerCliente.Apellido1;
                    apellido2 = primerCliente.Apellido2;
                    email = primerCliente.Email;
                }

                var detalles = new PedidoBocadilloDetailsDTO
                {
                    PedidoId = compra.CompraId,
                    NombreCliente = nombre,
                    Apellido1Cliente = apellido1,
                    Apellido2Cliente = apellido2,
                    EmailCliente = email,
                    MetodoPago = compra.MetodoPago.ToString(),
                    FechaCompra = compra.FechaCompra,
                    PrecioTotal = (decimal)compra.PrecioTotal, 
                                                              
                    Bocadillos = compra.BocadillosComprados.Select(cb => new BocadilloItemDTO
                    {
                        BocadilloId = cb.BocadilloId,
                        Cantidad = cb.Cantidad,
                        NombreBocadillo = cb.Bocadillo.Nombre,
                        TipoPan = cb.Bocadillo.TipoPan.Nombre,
                        PrecioUnitario = cb.Bocadillo.PVP
                    }).ToList()
                };

                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener el pedido: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearPedido([FromBody] PedirBocadilloCreateDTO createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var clienteLogueado = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == createDto.EmailCliente);

                if (clienteLogueado == null)
                {
                    clienteLogueado = new ApplicationUser
                    {
                        Nombre = createDto.NombreCliente,
                        Apellido1 = createDto.Apellido1Cliente,
                        Apellido2 = createDto.Apellido2Cliente,
                        Email = createDto.EmailCliente,
                        UserName = createDto.EmailCliente,
                        EmailConfirmed = true
                    };
                    _context.Users.Add(clienteLogueado);
                }
                else {                     
                    clienteLogueado.Nombre = createDto.NombreCliente;
                    clienteLogueado.Apellido1 = createDto.Apellido1Cliente;
                    clienteLogueado.Apellido2 = createDto.Apellido2Cliente;
                    _context.Users.Update(clienteLogueado);
                }

                var erroresStock = new List<string>();
                decimal precioTotalCalculado = 0m;
                var bocadillosComprados = new List<CompraBocadillo>();

                foreach (var itemDto in createDto.Bocadillos)
                {
                    var bocadillo = await _context.Bocadillos
                                            .FirstOrDefaultAsync(b => b.Id == itemDto.BocadilloId);

                    if (bocadillo == null)
                    {
                        erroresStock.Add($"Bocadillo ID {itemDto.BocadilloId} no existe.");
                    }
                    else if (bocadillo.Stock < itemDto.Cantidad)
                    {
                        erroresStock.Add($"Stock insuficiente para {bocadillo.Nombre}");
                    }
                    else
                    {
                        precioTotalCalculado += bocadillo.PVP * itemDto.Cantidad;
                        bocadillo.Stock -= itemDto.Cantidad;

                        bocadillosComprados.Add(new CompraBocadillo
                        {
                            BocadilloId = bocadillo.Id,
                            Cantidad = itemDto.Cantidad
                        });
                    }
                }

                if (erroresStock.Any())
                {
                    return BadRequest(new { errors = erroresStock });
                }

                var compra = new Compra
                {
                    Cliente = new List<ApplicationUser> { clienteLogueado },
                    FechaCompra = DateTime.Now,
                    nBocadillos = createDto.Bocadillos.Sum(b => b.Cantidad),
                    PrecioTotal = (float)precioTotalCalculado,
                    MetodoPago = Enum.Parse<MetodoPago>(createDto.MetodoPago, true),
                    BocadillosComprados = bocadillosComprados
                };

                _context.Compra.Add(compra);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPedido), new { id = compra.CompraId }, new
                {
                    message = "Pedido creado correctamente",
                    pedidoId = compra.CompraId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al crear el pedido: " + ex.Message });
            }
        }
    }
}