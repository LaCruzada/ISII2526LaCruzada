using AppForMovies.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.BonosComprados_UIT
{
    public class CU_BonosBocadillos_UIT : UC_UIT
    {
        private const string bono1Nombre = "Bono Vegano";
        private const string bono1Tipo = "Vegano";
        private const string bono1Precio = "5.00 €";
        private const string bono1NBocadillos = "2";
        private const string bono1Stock = "10";

        private const string bono2Nombre = "Bono Sin Gluten";
        private const string bono2Tipo = "Sin Gluten";
        private const string bono2Precio = "12.00 €";
        private const string bono2NBocadillos = "3";
        private const string bono2Stock = "5";

        private const string clienteNombre = "Juan";
        private const string clienteApellido1 = "Pérez";
        private const string clienteApellido2 = "Saez";
        private const string metodoPagoTarjeta = "Tarjeta de Crédito/Débito";

        public CU_BonosBocadillos_UIT(ITestOutputHelper output) : base(output)
        {
        }

        private void Inicio()
        {
            _driver.Navigate().GoToUrl(_URI);
            Thread.Sleep(2000);
        }

        private void Ir_A_ComprarBonos()
        {
            var pageObject = new SelectBonoBocadillo_PO(_driver, _output);
            pageObject.WaitForBeingVisible(By.Id("MenuSeleccionarBonos"));
            _driver.FindElement(By.Id("MenuSeleccionarBonos")).Click();
            Thread.Sleep(2000);
            pageObject.WaitForBeingVisible(By.Id("searchBonos"));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-01")]
        public void ESC_01_Compra_Estandar_Bono_Vegano()
        {
            var selectBonoBocadillo_PO = new SelectBonoBocadillo_PO(_driver, _output);
            var postBonoBocadillo_PO = new PostBonoBocadillo_PO(_driver, _output);
            var detallesBonoBocadillo_PO = new DetallesBonoBocadillo_PO(_driver, _output);

            Inicio();
            Ir_A_ComprarBonos();

            selectBonoBocadillo_PO.AddBonoToCartByName(bono1Nombre);

            selectBonoBocadillo_PO.WaitForBeingClickable(By.Id("ComprarBonos"));
            _driver.FindElement(By.Id("ComprarBonos")).Click();
            Thread.Sleep(1500);

            postBonoBocadillo_PO.setNombre(clienteNombre);
            postBonoBocadillo_PO.setApellido1(clienteApellido1);
            postBonoBocadillo_PO.setApellido2(clienteApellido2);
            postBonoBocadillo_PO.setMetodoPago(metodoPagoTarjeta);

            postBonoBocadillo_PO.ContinuarCompra();
            Thread.Sleep(500);
            postBonoBocadillo_PO.ConfirmarCompra();
            Thread.Sleep(2000);

            detallesBonoBocadillo_PO.WaitForBeingVisible(By.Id("BonosComprados"));

            Assert.True(_driver.PageSource.Contains("Detalle de la Compra") ||
                       _driver.PageSource.Contains("Compra de Bonos"));
            Assert.True(_driver.PageSource.Contains(clienteNombre));
            Assert.True(_driver.PageSource.Contains(clienteApellido1));
            Assert.True(_driver.PageSource.Contains("Tarjeta"));
            Assert.True(_driver.PageSource.Contains("Fecha"));
            Assert.True(_driver.PageSource.Contains("Precio Total") ||
                       _driver.PageSource.Contains("Total"));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-02")]
        public void ESC_02_Filtrar_Bonos_Sin_Gluten()
        {
            var selectBonoBocadillo_PO = new SelectBonoBocadillo_PO(_driver, _output);
            var expectedBonos = new List<string[]> {
                new string[] { bono2Nombre, bono2Tipo, bono2NBocadillos, "12,00 €", "3" }
            };

            Inicio();
            Ir_A_ComprarBonos();
            selectBonoBocadillo_PO.SearchBonos("", "Sin Gluten");

            Assert.True(selectBonoBocadillo_PO.CheckListOfBonos(expectedBonos));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-03")]
        public void ESC_03_Carrito_Vacio_Boton_Deshabilitado()
        {
            var selectBonoBocadillo_PO = new SelectBonoBocadillo_PO(_driver, _output);

            Inicio();
            Ir_A_ComprarBonos();
            selectBonoBocadillo_PO.WaitForBeingVisible(By.Id("searchBonos"));

            Assert.True(selectBonoBocadillo_PO.ComprarNotAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-04")]
        public void ESC_04_Eliminar_Bono_Actualiza_Precio()
        {
            var selectBonoBocadillo_PO = new SelectBonoBocadillo_PO(_driver, _output);

            Inicio();
            Ir_A_ComprarBonos();

            selectBonoBocadillo_PO.AddBonoToCartByName(bono1Nombre);
            selectBonoBocadillo_PO.AddBonoToCartByName(bono2Nombre);

            Assert.False(selectBonoBocadillo_PO.ComprarNotAvailable());

            string paginaAntes = _driver.PageSource;
            selectBonoBocadillo_PO.RemoveBonoFromCart(bono1Nombre);
            Thread.Sleep(1000);

            Assert.False(selectBonoBocadillo_PO.ComprarNotAvailable());

            string paginaDespues = _driver.PageSource;
            Assert.NotEqual(paginaAntes, paginaDespues);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-05")]
        public void ESC_05_Datos_Obligatorios_NombreCliente()
        {
            var selectBonoBocadillo_PO = new SelectBonoBocadillo_PO(_driver, _output);
            var postBonoBocadillo_PO = new PostBonoBocadillo_PO(_driver, _output);

            Inicio();
            Ir_A_ComprarBonos();
            selectBonoBocadillo_PO.AddBonoToCartByName(bono1Nombre);
            selectBonoBocadillo_PO.WaitForBeingClickable(By.Id("ComprarBonos"));
            _driver.FindElement(By.Id("ComprarBonos")).Click();
            Thread.Sleep(1500);

            postBonoBocadillo_PO.setApellido1(clienteApellido1);
            postBonoBocadillo_PO.setApellido2(clienteApellido2);
            postBonoBocadillo_PO.setMetodoPago(metodoPagoTarjeta);

            string urlAntes = _driver.Url;

            postBonoBocadillo_PO.ContinuarCompra();
            Thread.Sleep(1000);

            try
            {
                postBonoBocadillo_PO.ConfirmarCompra();
                Thread.Sleep(2000);
            }
            catch { }

            Assert.False(_driver.Url.Contains("/Detallecompra"), "No debería completar la compra sin nombre");
            Assert.True(_driver.Url.Contains("/PostBonoBocadillo"), "Debería permanecer en el formulario");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "ESC-06")]
        public void ESC_06_Modificar_Bonos_Mantiene_Datos()
        {
            var selectBonoBocadillo_PO = new SelectBonoBocadillo_PO(_driver, _output);
            var postBonoBocadillo_PO = new PostBonoBocadillo_PO(_driver, _output);

            Inicio();
            Ir_A_ComprarBonos();

            selectBonoBocadillo_PO.AddBonoToCartByName(bono1Nombre);

            selectBonoBocadillo_PO.WaitForBeingClickable(By.Id("ComprarBonos"));
            _driver.FindElement(By.Id("ComprarBonos")).Click();
            Thread.Sleep(1500);

            postBonoBocadillo_PO.setNombre(clienteNombre);
            postBonoBocadillo_PO.setApellido1(clienteApellido1);
            postBonoBocadillo_PO.setApellido2(clienteApellido2);
            postBonoBocadillo_PO.setMetodoPago(metodoPagoTarjeta);

            postBonoBocadillo_PO.ModificarBonos();
            Thread.Sleep(1500);

            selectBonoBocadillo_PO.WaitForBeingVisible(By.Id("ComprarBonos"));
            Assert.True(_driver.Url.Contains("/bonos/seleccionar"));
            Assert.False(selectBonoBocadillo_PO.ComprarNotAvailable());

            _driver.FindElement(By.Id("ComprarBonos")).Click();
            Thread.Sleep(1500);

            postBonoBocadillo_PO.WaitForBeingVisible(By.Id("Nombre"));
            Thread.Sleep(1000);

            var nombreInput = _driver.FindElement(By.Id("Nombre"));
            string valorNombre = nombreInput.GetAttribute("value");

            _output.WriteLine($"Valor en campo Nombre: '{valorNombre}'");
            _output.WriteLine($"Valor esperado: '{clienteNombre}'");

            Assert.Equal(clienteNombre, valorNombre);
        }

    }
}

