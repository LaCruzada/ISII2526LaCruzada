using Xunit;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DTOsCompraMerchandising;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.MerchandisingController_test
{
    public class CompraMerchandisingControllerPostTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        private const int PRODUCTO_ID_VALIDO = 1;
        private const string PRODUCTO_NOMBRE_VALIDO = "Camiseta Roja";
        private const int PRODUCTO_STOCK_INICIAL = 10;
        private const decimal PRODUCTO_PVP_VALIDO = 15.00m;

        private const int PRODUCTO_ID_POCO_STOCK = 2;
        private const string PRODUCTO_NOMBRE_POCO_STOCK = "Camiseta Azul";
        private const int PRODUCTO_STOCK_INICIAL_POCO = 2;
        private const decimal PRODUCTO_PVP_POCO_STOCK = 20.00m;

        private const int ID_NO_EXISTENTE = 99;

        public CompraMerchandisingControllerPostTEST()
        {
            var tipoCamiseta = new TipoProducto { TipoProductoId = 1, Nombre = "Camiseta" };
            var tipoTaza = new TipoProducto { TipoProductoId = 2, Nombre = "Taza" };
            var tipoPoster = new TipoProducto { TipoProductoId = 3, Nombre = "Poster" };
            var tipoGorra = new TipoProducto { TipoProductoId = 4, Nombre = "Gorra" };

            _context.TipoProducto.AddRange(tipoCamiseta, tipoTaza, tipoPoster, tipoGorra);

            _context.Producto.AddRange(
                new Producto
                {
                    ProductoId = PRODUCTO_ID_VALIDO,
                    Nombre = PRODUCTO_NOMBRE_VALIDO,
                    PVP = PRODUCTO_PVP_VALIDO,
                    Stock = PRODUCTO_STOCK_INICIAL,
                    TipoProducto = tipoCamiseta
                },
                new Producto
                {
                    ProductoId = PRODUCTO_ID_POCO_STOCK,
                    Nombre = PRODUCTO_NOMBRE_POCO_STOCK,
                    PVP = PRODUCTO_PVP_POCO_STOCK,
                    Stock = PRODUCTO_STOCK_INICIAL_POCO,
                    TipoProducto = tipoCamiseta
                },
                new Producto
                {
                    ProductoId = 3,
                    Nombre = "Taza de Cerámica",
                    PVP = 8.00m,
                    Stock = 8,
                    TipoProducto = tipoTaza
                },
                new Producto
                {
                    ProductoId = 4,
                    Nombre = "Poster Mapa",
                    PVP = 5.00m,
                    Stock = 0,
                    TipoProducto = tipoPoster
                },
                new Producto
                {
                    ProductoId = 5,
                    Nombre = "Poster Laminado",
                    PVP = 12.00m,
                    Stock = 3,
                    TipoProducto = tipoPoster
                },
                new Producto
                {
                    ProductoId = 6,
                    Nombre = "Gorra Ajustable",
                    PVP = 18.00m,
                    Stock = 15,
                    TipoProducto = tipoGorra
                }
            );

            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CrearCompra_CosasObligatorias()
        {
            var dtoProductoNoExiste = new ComprarMerchandisingCreateDTO
            {
                EmailCliente = "test@test.com",
                NombreCliente = "Test",
                Apellido1Cliente = "Test",
                Apellido2Cliente = "T",
                DireccionEnvio = "Dirección Test",
                MetodoPago = "PayPal",
                Productos = new List<ProductoCantidadDTO>
                {
                    new ProductoCantidadDTO { ProductoId = ID_NO_EXISTENTE, Cantidad = 1 }
                }
            };

            var dtoSinStock = new ComprarMerchandisingCreateDTO
            {
                EmailCliente = "test@test.com",
                NombreCliente = "Test",
                Apellido1Cliente = "Test",
                Apellido2Cliente = "T",
                DireccionEnvio = "Dirección Test",
                MetodoPago = "PayPal",
                Productos = new List<ProductoCantidadDTO>
                {
                    new ProductoCantidadDTO { ProductoId = PRODUCTO_ID_POCO_STOCK, Cantidad = 5 }
                }
            };

            var dtoCarritoVacio = new ComprarMerchandisingCreateDTO
            {
                EmailCliente = "test@test.com",
                NombreCliente = "Test",
                Apellido1Cliente = "Test",
                Apellido2Cliente = "T",
                DireccionEnvio = "Dirección Test",
                MetodoPago = "PayPal",
                Productos = new List<ProductoCantidadDTO>()
            };

            var dtoSinEmail = new ComprarMerchandisingCreateDTO
            {
                EmailCliente = "",
                NombreCliente = "Test",
                Apellido1Cliente = "Test",
                Apellido2Cliente = "T",
                DireccionEnvio = "Dirección Test",
                MetodoPago = "PayPal",
                Productos = new List<ProductoCantidadDTO>
                {
                    new ProductoCantidadDTO { ProductoId = PRODUCTO_ID_VALIDO, Cantidad = 1 }
                }
            };

            return new List<object[]>
            {
                new object[] { dtoProductoNoExiste, $"Producto ID {ID_NO_EXISTENTE} no existe" },
                new object[] { dtoSinStock, $"Stock insuficiente para {PRODUCTO_NOMBRE_POCO_STOCK}" },
                new object[] { dtoCarritoVacio, "No se ha añadido ningún producto al carrito" },
                new object[] { dtoSinEmail, "Todos los campos obligatorios deben rellenarse" }
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CrearCompra_CosasObligatorias))]
        public async Task CrearCompra_CosasObligatorias_DevuelveBadRequest(
            ComprarMerchandisingCreateDTO dtoConError,
            string errorEsperado)
        {
            var controller = new CompraMerchandisingController(_context);

            var actionResult = await controller.CrearCompra(dtoConError);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.NotNull(badRequestResult.Value);

            var messageProp = badRequestResult.Value.GetType().GetProperty("message");
            var errorsProp = badRequestResult.Value.GetType().GetProperty("errors");

            if (messageProp != null)
            {
                var message = messageProp.GetValue(badRequestResult.Value)?.ToString();
                Assert.Contains(errorEsperado, message);
            }
            else if (errorsProp != null)
            {
                var errors = errorsProp.GetValue(badRequestResult.Value) as List<string>;
                Assert.NotNull(errors);
                Assert.Contains(errors, e => e.Contains(errorEsperado));
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CrearCompra_TodoBien_Success_test()
        {
            var controller = new CompraMerchandisingController(_context);

            var emailNuevo = "cliente.nuevo@test.com";
            var cantidadPedida = 3;
            var precioTotalEsperado = PRODUCTO_PVP_VALIDO * cantidadPedida;
            var stockEsperado = PRODUCTO_STOCK_INICIAL - cantidadPedida;
            var direccionEnvio = "Calle Test 123, Ciudad";

            Assert.Equal(0, _context.Users.Count());

            var dtoBueno = new ComprarMerchandisingCreateDTO
            {
                EmailCliente = emailNuevo,
                NombreCliente = "Carlos",
                Apellido1Cliente = "García",
                Apellido2Cliente = "López",
                DireccionEnvio = direccionEnvio,
                MetodoPago = "TarjetaCredito",
                Productos = new List<ProductoCantidadDTO>
                {
                    new ProductoCantidadDTO { ProductoId = PRODUCTO_ID_VALIDO, Cantidad = cantidadPedida }
                }
            };

            var actionResult = await controller.CrearCompra(dtoBueno);

            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.NotNull(createdResult.Value);
            
            var valueType = createdResult.Value.GetType();
            var messageProp = valueType.GetProperty("message");
            Assert.NotNull(messageProp);
            
            var message = messageProp.GetValue(createdResult.Value)?.ToString();
            Assert.Equal("Compra realizada correctamente", message);

            Assert.Equal(1, _context.Users.Count());
            var usuarioDb = await _context.Users.FirstOrDefaultAsync();
            Assert.NotNull(usuarioDb);
            Assert.Equal(emailNuevo, usuarioDb.Email);
            Assert.Equal("Carlos", usuarioDb.Nombre);
            Assert.Equal("García", usuarioDb.Apellido1);
            Assert.Equal("López", usuarioDb.Apellido2);

            Assert.Equal(1, _context.Compra_Producto.Count());
            var compraDb = await _context.Compra_Producto
                .Include(c => c.usuario)
                .Include(c => c.ProductoCompras)
                .ThenInclude(pc => pc.Producto)
                .FirstOrDefaultAsync();

            Assert.NotNull(compraDb);
            Assert.Equal(direccionEnvio, compraDb.DireccionEnvio);
            Assert.Equal(PRODUCTO_PVP_VALIDO * cantidadPedida, compraDb.PrecioFinal);
            Assert.Equal("TarjetaCredito", compraDb.Metodo_Pago);
            Assert.Equal(1, compraDb.usuario.Count);
            Assert.Equal(emailNuevo, compraDb.usuario.First().Email);

            Assert.Equal(1, _context.ProductoCompra.Count());
            var itemDb = await _context.ProductoCompra
                .Include(pc => pc.Producto)
                .FirstOrDefaultAsync();

            Assert.NotNull(itemDb);
            Assert.Equal(compraDb.CompraId, itemDb.CompraId);
            Assert.Equal(PRODUCTO_ID_VALIDO, itemDb.ProductoId);
            Assert.Equal(cantidadPedida, itemDb.Cantidad);
            Assert.Equal(PRODUCTO_PVP_VALIDO, itemDb.PVP);

            var productoDb = await _context.Producto.FindAsync(PRODUCTO_ID_VALIDO);
            Assert.Equal(stockEsperado, productoDb.Stock);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CrearCompra_ClienteExistente_NoCreaNuevoUsuario()
        {
            var controller = new CompraMerchandisingController(_context);

            var emailExistente = "cliente.existente@test.com";
            var usuarioExistente = new ApplicationUser
            {
                Email = emailExistente,
                UserName = emailExistente,
                Nombre = "Ana",
                Apellido1 = "Martínez",
                Apellido2 = "Ruiz",
                EmailConfirmed = true
            };
            _context.Users.Add(usuarioExistente);
            await _context.SaveChangesAsync();

            var cantidadInicial = 4;
            var dtoConClienteExistente = new ComprarMerchandisingCreateDTO
            {
                EmailCliente = emailExistente,
                NombreCliente = "OtroNombre",
                Apellido1Cliente = "OtroApellido",
                Apellido2Cliente = "OtroApellido2",
                DireccionEnvio = "Nueva Dirección 456",
                MetodoPago = "GooglePay",
                Productos = new List<ProductoCantidadDTO>
                {
                    new ProductoCantidadDTO { ProductoId = PRODUCTO_ID_VALIDO, Cantidad = cantidadInicial }
                }
            };

            var actionResult = await controller.CrearCompra(dtoConClienteExistente);

            Assert.IsType<CreatedAtActionResult>(actionResult);

            Assert.Equal(1, _context.Users.Count());
            var usuarioDb = await _context.Users.FirstOrDefaultAsync();
            Assert.Equal(emailExistente, usuarioDb.Email);
            Assert.Equal("Ana", usuarioDb.Nombre);
            Assert.Equal("Martínez", usuarioDb.Apellido1);
            Assert.Equal("Ruiz", usuarioDb.Apellido2);

            Assert.Equal(1, _context.Compra_Producto.Count());
            var compraDb = await _context.Compra_Producto.FirstOrDefaultAsync();
            Assert.Equal("Nueva Dirección 456", compraDb.DireccionEnvio);
            Assert.Equal("GooglePay", compraDb.Metodo_Pago);
        }
    }
}