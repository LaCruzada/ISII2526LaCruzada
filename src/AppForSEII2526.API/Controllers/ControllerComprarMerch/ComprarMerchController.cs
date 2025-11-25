// Controllers/CompraMerchandisingController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
using AppForSEII2526.API.DTOs.DTOsCompraMerchandising;

namespace AppForSEII2526.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraMerchandisingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompraMerchandisingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CompraMerchandising/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompraMerchandisingDetailsDTO>> GetCompra(int id)
        {
            try
            {
                var compra = await _context.Compra_Producto
                    .Include(c => c.usuario) // Navegación correcta a la lista de usuarios
                    .Include(c => c.ProductoCompras) // Tabla intermedia
                        .ThenInclude(pc => pc.Producto)
                            .ThenInclude(p => p.TipoProducto) // Incluir TipoProducto para obtener el nombre
                    .FirstOrDefaultAsync(c => c.CompraId == id);

                if (compra == null)
                    return NotFound(new { message = "Compra no encontrada" });

                var cliente = compra.usuario?.FirstOrDefault();
                var nombreCompleto = cliente != null
                    ? $"{cliente.Nombre} {cliente.Apellido1} {cliente.Apellido2}".Trim()
                    : "Cliente no especificado";

                var detalles = new CompraMerchandisingDetailsDTO
                {
                    CompraId = compra.CompraId,
                    NombreCliente = nombreCompleto,
                    DireccionEnvio = compra.DireccionEnvio, // De Compra_Producto
                    MetodoPago = compra.Metodo_Pago, // Es string, no enum
                    FechaCompra = compra.FechaCompra,
                    PrecioTotal = compra.PrecioFinal,
                    Productos = compra.ProductoCompras.Select(pc => new ProductoItemDTO
                    {
                        ProductoId = pc.ProductoId,
                        Nombre = pc.Producto.Nombre,
                        Tipo = pc.Producto.TipoProducto.Nombre, // Acceder al nombre del tipo
                        PrecioUnitario = pc.PVP, // PVP guardado en la tabla intermedia
                        Cantidad = pc.Cantidad
                    }).ToList()
                };

                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener la compra: " + ex.Message });
            }
        }

        // POST: api/CompraMerchandising
        [HttpPost]
        public async Task<ActionResult> CrearCompra([FromBody] ComprarMerchandisingCreateDTO createDto)
        {
            try
            {
                // Flujo Alternativo 4: Validación de campos obligatorios
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Flujo Alternativo 2: Verificar carrito no vacío
                if (createDto.Productos == null || !createDto.Productos.Any())
                    return BadRequest(new { message = "No se ha añadido ningún producto al carrito" });

                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(createDto.EmailCliente) ||
                    string.IsNullOrWhiteSpace(createDto.NombreCliente) ||
                    string.IsNullOrWhiteSpace(createDto.Apellido1Cliente) ||
                    string.IsNullOrWhiteSpace(createDto.DireccionEnvio) ||
                    string.IsNullOrWhiteSpace(createDto.MetodoPago))
                {
                    return BadRequest(new { message = "Todos los campos obligatorios deben rellenarse" });
                }

                if (!createDto.DireccionEnvio.Contains("Calle"))
                {
                    return BadRequest(new { message = "Error!, por favor introduce una direccion de envio valida" });
                }

                // Obtener o crear cliente
                var cliente = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == createDto.EmailCliente);

                if (cliente == null)
                {
                    cliente = new ApplicationUser
                    {
                        Nombre = createDto.NombreCliente,
                        Apellido1 = createDto.Apellido1Cliente,
                        Apellido2 = createDto.Apellido2Cliente ?? string.Empty,
                        Email = createDto.EmailCliente,
                        UserName = createDto.EmailCliente,
                        EmailConfirmed = true,
                        DireccionEnvio = createDto.DireccionEnvio // Agregar la dirección al usuario
                    };
                    _context.Users.Add(cliente);
                    await _context.SaveChangesAsync(); // Guardar el usuario primero para obtener su ID
                }

                // Procesar productos y verificar stock (Flujo alternativo 0)
                var erroresStock = new List<string>();
                decimal precioTotal = 0m;
                var productosComprados = new List<ProductoCompra>();

                foreach (var item in createDto.Productos)
                {
                    var producto = await _context.Producto
                        .FirstOrDefaultAsync(p => p.ProductoId == item.ProductoId);

                    if (producto == null)
                    {
                        erroresStock.Add($"Producto ID {item.ProductoId} no existe");
                        continue;
                    }

                    if (producto.Stock < item.Cantidad)
                    {
                        erroresStock.Add($"Stock insuficiente para {producto.Nombre}");
                        continue;
                    }

                    // Calcular precio y descontar stock
                    precioTotal += producto.PVP * item.Cantidad;
                    producto.Stock -= item.Cantidad;

                    // Crear relación usando el constructor de tu clase
                    productosComprados.Add(new ProductoCompra(
                        compraId: 0, // Se asignará al guardar
                        productoId: producto.ProductoId,
                        cantidad: item.Cantidad,
                        pvp: producto.PVP // Guardar el precio actual del producto
                    ));
                }

                if (erroresStock.Any())
                    return BadRequest(new { errors = erroresStock });

                // Crear la compra (Compra_Producto)
                var compra = new Compra_Producto
                {
                    usuario = new List<ApplicationUser> { cliente },
                    DireccionEnvio = createDto.DireccionEnvio,
                    FechaCompra = DateTime.Now,
                    Metodo_Pago = createDto.MetodoPago,
                    PrecioFinal = precioTotal
                };

                _context.Compra_Producto.Add(compra);
                await _context.SaveChangesAsync(); // Guardar primero la compra para obtener el ID

                // Ahora asignar el CompraId a los productos comprados
                foreach (var pc in productosComprados)
                {
                    pc.CompraId = compra.CompraId;
                }

                _context.ProductoCompra.AddRange(productosComprados);
                await _context.SaveChangesAsync(); // Guardar los productos comprados

                return CreatedAtAction(nameof(GetCompra), new { id = compra.CompraId }, new
                {
                    message = "Compra realizada correctamente",
                    compraId = compra.CompraId,
                    precioTotal
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al crear la compra: " + ex.Message + " | Inner: " + ex.InnerException?.Message });
            }
        }
    }
}