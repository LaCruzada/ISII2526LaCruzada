using AppForSEII2526.API.Controllers.ControllerPedirBocadillo;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.DTOs.DTOsPedirBocadillo;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace AppForSEII2526.UT.BocadilloController_test
{
    
    public class PedirBocadilloControllerGetDetailsTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        
        private readonly ApplicationUser _testUser;
        private readonly Compra _testCompra;
        private readonly Bocadillo _testBocadillo;
        private readonly TipoPan _testTipoPan;

        public PedirBocadilloControllerGetDetailsTEST()
        {
            
            _testUser = new ApplicationUser
            {
                Id = "user-test-id",
                Nombre = "Juan",
                Apellido1 = "Perez",
                Apellido2 = "Garcia", 
                Email = "juan.perez@ejemplo.com",
                UserName = "juan.perez@ejemplo.com",
                EmailConfirmed = true
            };
            _context.Users.Add(_testUser);

   
            _testTipoPan = new TipoPan { PanId = 1, Nombre = "Barra" };
            _context.TipoPanes.Add(_testTipoPan);
            _testBocadillo = new Bocadillo
            {
                Id = 1,
                Nombre = "Bocata de Prueba",
                PVP = 5.50m,
                Stock = 10,
                TipoPan = _testTipoPan,
                Tamano = EnumTamaño.Normal
            };
            _context.Bocadillos.Add(_testBocadillo);

      
            _testCompra = new Compra
            {
                CompraId = 1,
                FechaCompra = DateTime.Now,
                MetodoPago = MetodoPago.Tarjeta,
                PrecioTotal = (float)11.00m, 
                nBocadillos = 2,
                Cliente = new List<ApplicationUser> { _testUser },
                BocadillosComprados = new List<CompraBocadillo>() 
            };
            _context.Compra.Add(_testCompra);


            var compraBocadillo = new CompraBocadillo
            {
                CompraId = _testCompra.CompraId,
                BocadilloId = _testBocadillo.Id,
                Cantidad = 2,
                Bocadillo = _testBocadillo, 
                Compra = _testCompra
            };
         

            _testCompra.BocadillosComprados.Add(compraBocadillo);
            

            _context.SaveChanges(); 
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPedido_Muestre_Success_test()
        {
          
            var controller = new PedirBocadilloController(_context);
            var expectedId = 1;
            var expectedName = $"{_testUser.Nombre} {_testUser.Apellido1} {_testUser.Apellido2}".Trim();

            var actionResult = await controller.GetPedido(expectedId);

            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dto = Assert.IsType<PedidoBocadilloDetailsDTO>(okResult.Value);

            Assert.Equal(expectedId, dto.PedidoId);
            Assert.Equal(expectedName, dto.NombreCliente);
            Assert.Single(dto.Bocadillos);
            Assert.Equal(_testBocadillo.Nombre, dto.Bocadillos.First().NombreBocadillo);
        }

       
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPedido_NoMuestre_DevuelveBadRequest()
        {
        
            var controller = new PedirBocadilloController(_context);
            var nonExistentId = 999; 

      
            var actionResult = await controller.GetPedido(nonExistentId);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.NotNull(badRequestResult.Value);
        }
    }
}