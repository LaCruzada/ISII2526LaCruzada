using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DTOsCompraMerchandising;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppForSEII2526.UT.MerchandisingController_test
{
    public class ProductoSelectControllerTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        public ProductoSelectControllerTEST()
        {
            var tipoCamiseta = new TipoProducto { TipoProductoId = 1, Nombre = "Camiseta" };
            var tipoTaza = new TipoProducto { TipoProductoId = 2, Nombre = "Taza" };
            var tipoPoster = new TipoProducto { TipoProductoId = 3, Nombre = "Poster" };
            var tipoGorra = new TipoProducto { TipoProductoId = 4, Nombre = "Gorra" };

            _context.TipoProducto.AddRange(tipoCamiseta, tipoTaza, tipoPoster, tipoGorra);

            _context.Producto.AddRange(
                new Producto
                {
                    ProductoId = 1,
                    Nombre = "Camiseta Roja",
                    PVP = 15.00m,
                    Stock = 10,
                    TipoProducto = tipoCamiseta
                },
                new Producto
                {
                    ProductoId = 2,
                    Nombre = "Camiseta Azul",
                    PVP = 20.00m,
                    Stock = 5,
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

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [InlineData(null, null, null, 5)]
        [InlineData("Camiseta", null, null, 2)]
        [InlineData("Taza", null, null, 1)]
        [InlineData("Poster", null, null, 1)]
        [InlineData("Gorra", null, "20.00", 1)]
        [InlineData("Camiseta", "25.00", "30.00", 0)]
        public async Task GetProductosDisponibles_TodoBien_DevuelveResultadosCorrectos(
            string? tipo,
            string? minPrecioStr,
            string? maxPrecioStr,
            int expectedCount)
        {
            var controller = new ProductoSelectController(_context);
            
            decimal? minPrecio = minPrecioStr != null ? decimal.Parse(minPrecioStr) : null;
            decimal? maxPrecio = maxPrecioStr != null ? decimal.Parse(maxPrecioStr) : null;

            var actionResult = await controller.GetProductosDisponibles(tipo, minPrecio, maxPrecio);

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
            var controller = new ProductoSelectController(_context);
            var tipoInvalido = "AccesorioInexistente";

            var actionResult = await controller.GetProductosDisponibles(tipoInvalido, null, null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.NotNull(badRequestResult.Value);
            
            var valueType = badRequestResult.Value.GetType();
            var messageProp = valueType.GetProperty("message");
            Assert.NotNull(messageProp);
            
            var message = messageProp.GetValue(badRequestResult.Value)?.ToString();
            Assert.Contains("no es un filtro válido", message);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetProductosDisponibles_SinStock_DevuelveNotFound()
        {
            var controller = new ProductoSelectController(_context);

            var actionResult = await controller.GetProductosDisponibles("Poster", 4.00m, 6.00m);

            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }
    }
}