using AppForSEII2526.API.Controllers.ControllerPedirBocadillo;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DTOsPedirBocadillo;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppForSEII2526.UT.BocadilloController_test
{
    public class BocadilloSelectControllerTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        public BocadilloSelectControllerTEST()
        {
            var tipoPanBarra = new TipoPan { PanId = 1, Nombre = "Barra" };
            var tipoPanBimbo = new TipoPan { PanId = 2, Nombre = "Bimbo" };

            _context.TipoPanes.AddRange(tipoPanBarra, tipoPanBimbo);

            _context.Bocadillos.AddRange(
                new Bocadillo { Id = 1, Nombre = "Bocata Normal Barra", Tamano = EnumTamaño.Normal, Stock = 10, TipoPan = tipoPanBarra, PVP = 10.0m },
                new Bocadillo { Id = 2, Nombre = "Bocata Pequeño Bimbo", Tamano = EnumTamaño.Pequeño, Stock = 10, TipoPan = tipoPanBimbo, PVP = 15.0m },
                new Bocadillo { Id = 3, Nombre = "Bocata Normal Sin Stock", Tamano = EnumTamaño.Normal, Stock = 0, TipoPan = tipoPanBarra, PVP = 10.0m },
                new Bocadillo { Id = 4, Nombre = "Bocata Normal Bimbo", Tamano = EnumTamaño.Normal, Stock = 10, TipoPan = tipoPanBimbo, PVP = 20.0m }
            );
            _context.SaveChanges();
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [InlineData(null, null, 3)]    
        [InlineData("Normal", null, 2)]  
        [InlineData("Pequeño", null, 1)] 
        [InlineData(null, 1, 1)]        
        [InlineData(null, 2, 2)]        
        [InlineData("Normal", 2, 1)]   
        [InlineData("Pequeño", 1, 0)]   

        public async Task GetBocadillosDisponibles_TodoBien_DevuelveResultadosCorrectos(
            string? tamano,
            int? tipoPanId,
            int expectedCount)
        {
            var controller = new BocadilloSelectController(_context);

            var actionResult = await controller.GetBocadillosDisponibles(tamano, tipoPanId);

            if (expectedCount > 0)
            {
                var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
                var bocadillos = Assert.IsType<List<BocadilloSelectDTO>>(okResult.Value);
                Assert.Equal(expectedCount, bocadillos.Count);
            }
            else
            {
                Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetBocadillosDisponibles_TamanoInvalido_DevuelveBadRequest()
        {
            var controller = new BocadilloSelectController(_context);
            var filtroInvalido = "Grande"; 

            var actionResult = await controller.GetBocadillosDisponibles(filtroInvalido, null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.NotNull(badRequestResult.Value);
        }
    }
}