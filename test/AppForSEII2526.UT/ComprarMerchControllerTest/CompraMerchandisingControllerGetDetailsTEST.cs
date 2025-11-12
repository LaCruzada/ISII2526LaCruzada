using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DTOsCompraMerchandising;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.MerchandisingController_test
{
    public class CompraMerchandisingControllerGetDetailsTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        private readonly ApplicationUser _testUser;
        private readonly Compra_Producto _testCompra;
        private readonly Producto _testProducto;
        private readonly TipoProducto _testTipoProducto;

        public CompraMerchandisingControllerGetDetailsTEST()
        {
            _testTipoProducto = new TipoProducto { TipoProductoId = 1, Nombre = "Camiseta" };
            _context.TipoProducto.Add(_testTipoProducto);

            _testProducto = new Producto
            {
                ProductoId = 1,
                Nombre = "Camiseta Roja",
                PVP = 15.00m,
                Stock = 10,
                TipoProducto = _testTipoProducto
            };
            _context.Producto.Add(_testProducto);

            _testUser = new ApplicationUser
            {
                Id = "user-test-id-123",
                Nombre = "María",
                Apellido1 = "Rodríguez",
                Apellido2 = "Sánchez",
                Email = "maria.rodriguez@ejemplo.com",
                UserName = "maria.rodriguez@ejemplo.com",
                EmailConfirmed = true
            };
            _context.Users.Add(_testUser);

            _testCompra = new Compra_Producto
            {
                CompraId = 1,
                DireccionEnvio = "Calle Mayor 45, 28013 Madrid",
                FechaCompra = new DateTime(2025, 11, 9, 10, 30, 0),
                Metodo_Pago = "TarjetaCredito",
                PrecioFinal = 45.00m,
                usuario = new List<ApplicationUser> { _testUser }
            };
            _context.Compra_Producto.Add(_testCompra);

            var productoCompra = new ProductoCompra(
                compraId: _testCompra.CompraId,
                productoId: _testProducto.ProductoId,
                cantidad: 3,
                pvp: _testProducto.PVP
            );
            _context.ProductoCompra.Add(productoCompra);

            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompra_Muestra_Success_test()
        {
            var controller = new CompraMerchandisingController(_context);
            var expectedId = 1;
            var expectedName = $"{_testUser.Nombre} {_testUser.Apellido1} {_testUser.Apellido2}".Trim();
            var expectedDireccion = "Calle Mayor 45, 28013 Madrid";
            var expectedMetodoPago = "TarjetaCredito";
            var expectedPrecioTotal = 45.00m;
            var expectedProductoNombre = "Camiseta Roja";
            var expectedCantidad = 3;
            var expectedPrecioUnitario = 15.00m;

            var actionResult = await controller.GetCompra(expectedId);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dto = Assert.IsType<CompraMerchandisingDetailsDTO>(okResult.Value);

            Assert.Equal(expectedId, dto.CompraId);
            Assert.Equal(expectedName, dto.NombreCliente);
            Assert.Equal(expectedDireccion, dto.DireccionEnvio);
            Assert.Equal(expectedMetodoPago, dto.MetodoPago);
            Assert.Equal(new DateTime(2025, 11, 9, 10, 30, 0), dto.FechaCompra, precision: TimeSpan.FromSeconds(1));
            Assert.Equal(expectedPrecioTotal, dto.PrecioTotal);
            Assert.Single(dto.Productos);

            var productoDto = dto.Productos.First();
            Assert.Equal(expectedProductoNombre, productoDto.Nombre);
            Assert.Equal(expectedCantidad, productoDto.Cantidad);
            Assert.Equal(expectedPrecioUnitario, productoDto.PrecioUnitario);
            Assert.Equal("Camiseta", productoDto.Tipo);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompra_NoMuestre_DevuelveNotFound()
        {
            var controller = new CompraMerchandisingController(_context);
            var nonExistentId = 999;

            var actionResult = await controller.GetCompra(nonExistentId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.NotNull(notFoundResult.Value);

            var valueType = notFoundResult.Value.GetType();
            var messageProp = valueType.GetProperty("message");
            Assert.NotNull(messageProp);
            
            var message = messageProp.GetValue(notFoundResult.Value)?.ToString();
            Assert.Contains("Compra no encontrada", message);
        }
    }
}