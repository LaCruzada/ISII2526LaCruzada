using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DTOsCompraMerchandising;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppForSEII2526.UT.MerchandisingController_test
{
    public class ProductoSelectControllerTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        public ProductoSelectControllerTEST()
        {
            // Seed de Tipos de Producto
            var tipoCamiseta = new TipoProducto { TipoProductoId = 1, Nombre = "Camiseta" };
            var tipoTaza = new TipoProducto { TipoProductoId = 2, Nombre = "Taza" };
            var tipoPoster = new TipoProducto { TipoProductoId = 3, Nombre = "Poster" };
            var tipoGorra = new TipoProducto { TipoProductoId = 4, Nombre = "Gorra" };

            _context.TipoProducto.AddRange(tipoCamiseta, tipoTaza, tipoPoster, tipoGorra);

            // Seed de Productos con stock variado
            _context.Producto.AddRange(
                new Producto { ProductoId = 1, Nombre = "Camiseta Roja", PVP = 15.00m, Stock = 10, TipoProducto = tipoCamiseta },
                new Producto { ProductoId = 2, Nombre = "Camiseta Azul", PVP = 20.00m, Stock = 5, TipoProducto = tipoCamiseta },
                new Producto { ProductoId = 3, Nombre = "Taza de Cerámica", PVP = 8.00m, Stock = 8, TipoProducto = tipoTaza },
                new Producto { ProductoId = 4, Nombre = "Poster Mapa", PVP = 5.00m, Stock = 0, TipoProducto = tipoPoster }, // Sin stock
                new Producto { ProductoId = 5, Nombre = "Poster Laminado", PVP = 12.00m, Stock = 3, TipoProducto = tipoPoster },
                new Producto { ProductoId = 6, Nombre = "Gorra Ajustable", PVP = 18.00m, Stock = 15, TipoProducto = tipoGorra }
            );
            _context.SaveChanges();
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [InlineData(null, null, null, 5)] // Todos los productos con stock (el ProductoId=4 tiene stock 0)
        [InlineData("Camiseta", null, null, 2)] // Solo camisetas
        [InlineData("Taza", null, null, 1)] // Solo tazas
        [InlineData("Poster", null, null, 1)] // Solo poster con stock (el de 5€ no tiene stock)
        [InlineData(null, 10.00, null, 3)] // Productos >= 10€ (Ids 1, 2, 5, 6) = 4 pero el 5 solo tiene 3 stock y pasa
        [InlineData(null, null, 15.00, 4)] // Productos <= 15€ (Ids 1, 3, 5) + 4 no tiene stock = 3
        [InlineData("Camiseta", 16.00, null, 1)] // Camisetas >= 16€ (solo la azul)
        [InlineData("Gorra", null, 20.00, 1)] // Gorras <= 20€ (solo la ajustable)
        [InlineData("Camiseta", 25.00, 30.00, 0)] // No hay camisetas entre 25-30€
        public async Task GetProductosDisponibles_TodoBien_DevuelveResultadosCorrectos(
            string? tipo,
            decimal? minPrecio,
            decimal? maxPrecio,
            int expectedCount)
        {
            // Arrange
            var controller = new ProductoSelectController(_context);

            // Act
            var actionResult = await controller.GetProductosDisponibles(tipo, minPrecio, maxPrecio);

            // Assert
            if (expectedCount > 0)
            {
                var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
                var productos = Assert.IsType<List<ProductoSelectDTO>>(okResult.Value);
                Assert.Equal(expectedCount, productos.Count);
            }
            else
            {
                Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductosDisponibles_TipoInvalido_DevuelveBadRequest()
        {
            // Arrange
            var controller = new ProductoSelectController(_context);
            var tipoInvalido = "AccesorioInexistente";

            // Act
            var actionResult = await controller.GetProductosDisponibles(tipoInvalido, null, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.NotNull(badRequestResult.Value);
            dynamic value = badRequestResult.Value;
            Assert.Contains("no es un filtro válido", value.message.ToString());
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductosDisponibles_SinStock_DevuelveNotFound()
        {
            // Arrange
            var controller = new ProductoSelectController(_context);

            // Act - Intentar obtener solo el poster sin stock
            var actionResult = await controller.GetProductosDisponibles("Poster", null, 6.00m); // Solo el poster de 5€ pero sin stock

            // Assert
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }
    }
}