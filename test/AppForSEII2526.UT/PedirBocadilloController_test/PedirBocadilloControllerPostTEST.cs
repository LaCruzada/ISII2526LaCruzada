using Xunit;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Data;
using AppForSEII2526.API.Models;
using AppForSEII2526.API.Controllers.ControllerPedirBocadillo;
using AppForSEII2526.API.DTOs.DTOsPedirBocadillo;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 

namespace AppForSEII2526.UT.BocadilloController_test
{
    public class PedirBocadilloControllerPostTEST : AppForMovies.UT.AppForMovies4SqliteUT
    {
        private const int BOCADILLO_ID_VALIDO = 1;
        private const string BOCADILLO_NOMBRE_VALIDO = "Bocata de Prueba";
        private const int BOCADILLO_STOCK_INICIAL = 10;

        private const int BOCADILLO_ID_POCO_STOCK = 2;
        private const string BOCADILLO_NOMBRE_POCO_STOCK = "Bocata Raro";
        private const int BOCADILLO_STOCK_INICIAL_POCO = 2;

        private const int ID_NO_EXISTENTE = 99;

        public PedirBocadilloControllerPostTEST()
        {
            var tipoPanBarra = new TipoPan { PanId = 1, Nombre = "Barra" };
            _context.TipoPanes.Add(tipoPanBarra);

            _context.Bocadillos.AddRange(
                new Bocadillo
                {
                    Id = BOCADILLO_ID_VALIDO,
                    Nombre = BOCADILLO_NOMBRE_VALIDO,
                    PVP = 5.0m,
                    Stock = BOCADILLO_STOCK_INICIAL,
                    TipoPan = tipoPanBarra,
                    Tamano = EnumTamaño.Normal
                },
                new Bocadillo
                {
                    Id = BOCADILLO_ID_POCO_STOCK,
                    Nombre = BOCADILLO_NOMBRE_POCO_STOCK,
                    PVP = 3.0m,
                    Stock = BOCADILLO_STOCK_INICIAL_POCO,
                    TipoPan = tipoPanBarra,
                    Tamano = EnumTamaño.Pequeño
                }
            );

            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CrearPedido_CosasObligatorias()
        {
            var dtoBocadilloNoExiste = new PedirBocadilloCreateDTO
            {
                NombreCliente = "Test",
                Apellido1Cliente = "Test",
                EmailCliente = "test@test.com",
                MetodoPago = "Tarjeta",
                Bocadillos = new List<BocadilloPedidoItemDTO> { new BocadilloPedidoItemDTO { BocadilloId = ID_NO_EXISTENTE, Cantidad = 1 } }
            };

            var dtoSinStock = new PedirBocadilloCreateDTO
            {
                NombreCliente = "Test",
                Apellido1Cliente = "Test",
                EmailCliente = "test@test.com",
                MetodoPago = "Tarjeta",
                Bocadillos = new List<BocadilloPedidoItemDTO> { new BocadilloPedidoItemDTO { BocadilloId = BOCADILLO_ID_POCO_STOCK, Cantidad = 5 } }
            };

            var dtoSinNombre = new PedirBocadilloCreateDTO
            {
                NombreCliente = null, 
                Apellido1Cliente = "Test",
                EmailCliente = "test@test.com",
                MetodoPago = "Tarjeta",
                Bocadillos = new List<BocadilloPedidoItemDTO> { new BocadilloPedidoItemDTO { BocadilloId = BOCADILLO_ID_VALIDO, Cantidad = 1 } }
            };

            var dtoSinApellido = new PedirBocadilloCreateDTO
            {
                NombreCliente = "Test",
                Apellido1Cliente = null, 
                EmailCliente = "test@test.com",
                MetodoPago = "Tarjeta",
                Bocadillos = new List<BocadilloPedidoItemDTO> { new BocadilloPedidoItemDTO { BocadilloId = BOCADILLO_ID_VALIDO, Cantidad = 1 } }
            };

            var dtoSinPago = new PedirBocadilloCreateDTO
            {
                NombreCliente = "Test",
                Apellido1Cliente = "Test",
                EmailCliente = "test@test.com",
                MetodoPago = null, 
                Bocadillos = new List<BocadilloPedidoItemDTO> { new BocadilloPedidoItemDTO { BocadilloId = BOCADILLO_ID_VALIDO, Cantidad = 1 } }
            };

            return new List<object[]>
            {
                new object[] { dtoBocadilloNoExiste, $"Bocadillo ID {ID_NO_EXISTENTE} no existe" },
                new object[] { dtoSinStock, $"Stock insuficiente para {BOCADILLO_NOMBRE_POCO_STOCK}" },
                new object[] { dtoSinNombre, "El nombre es obligatorio" },
                new object[] { dtoSinApellido, "El primer apellido es obligatorio" },
                new object[] { dtoSinPago, "El método de pago es obligatorio" }
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CrearPedido_CosasObligatorias))]
        public async Task CrearPedido_CosasObligatorias_DevuelveBadRequest(PedirBocadilloCreateDTO dtoConError, string errorEsperado)
        {
            var controller = new PedirBocadilloController(_context);

            var validationContext = new ValidationContext(dtoConError);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(dtoConError, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
                }
            }

            var actionResult = await controller.CrearPedido(dtoConError);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.NotNull(badRequestResult.Value);

            string errorString = badRequestResult.Value.ToString();

            var propErrors = badRequestResult.Value.GetType().GetProperty("errors");
            if (propErrors != null)
            {
                var listaErrores = propErrors.GetValue(badRequestResult.Value) as List<string>;
                errorString = string.Join(", ", listaErrores);
            }
            else if (badRequestResult.Value is SerializableError serializableError)
            {
                errorString = string.Join(" ", serializableError.Values.SelectMany(v => (string[])v));
            }

            Assert.Contains(errorEsperado, errorString);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CrearPedido_TodoBien_Success_test()
        {
            var controller = new PedirBocadilloController(_context);

            var emailNuevo = "usuario.nuevo@test.com";
            var cantidadPedida = 4;
            var precioBocadillo = _context.Bocadillos.Find(BOCADILLO_ID_VALIDO).PVP;
            var precioTotalEsperado = precioBocadillo * cantidadPedida;
            var stockEsperado = BOCADILLO_STOCK_INICIAL - cantidadPedida;

            var dtoBueno = new PedirBocadilloCreateDTO
            {
                NombreCliente = "Usuario",
                Apellido1Cliente = "Nuevo",
                Apellido2Cliente = "Test",
                EmailCliente = emailNuevo,
                MetodoPago = "Paypal",
                Bocadillos = new List<BocadilloPedidoItemDTO>
                {
                    new BocadilloPedidoItemDTO { BocadilloId = BOCADILLO_ID_VALIDO, Cantidad = cantidadPedida }
                }
            };

            Assert.Equal(0, _context.Users.Count());
            Assert.Equal(0, _context.Compra.Count());

            var actionResult = await controller.CrearPedido(dtoBueno);

            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult);

            Assert.Equal(1, _context.Users.Count());
            var usuarioDb = _context.Users.First();
            Assert.Equal(emailNuevo, usuarioDb.Email);

            Assert.Equal(1, _context.Compra.Count());
            var compraDb = _context.Compra.First();
            Assert.Equal((float)precioTotalEsperado, compraDb.PrecioTotal);
            Assert.Equal(MetodoPago.Paypal, compraDb.MetodoPago);

            Assert.Equal(stockEsperado, _context.Bocadillos.Find(BOCADILLO_ID_VALIDO).Stock);
        }
    }
}