using AppForMovies.UIT.Shared;
using OpenQA.Selenium;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.PedirBocadillo_UIT
{
    public class CU_PedirBocadillo_UIT : UC_UIT
    {
        private const string BOCADILLO_TEST = "Serranito"; 
        private const string CLIENTE_NOMBRE = "SeleniumUser";
        private const string CLIENTE_APELLIDO = "Automated";
        private const string CLIENTE_EMAIL = "uit@test.com";
        private const string PAGO_GOOGLE = "GooglePay";

        public CU_PedirBocadillo_UIT(ITestOutputHelper output) : base(output)
        {
        }

        private void Ir_A_Seleccion()
        {
            _driver.Navigate().GoToUrl(_URI + "PedirBocadillo/SeleccionarPedirBocadillo");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-PB-01")]
        public void HU_PB_01_Compra_Estandar_Correcta()
        {
            var selectPO = new SelectPedirBocadillo_PO(_driver, _output);
            var createPO = new CreatePedirBocadillo_PO(_driver, _output);
            var detallePO = new DetallePedido_PO(_driver, _output);

            Ir_A_Seleccion();

            selectPO.AddFirstBocadilloToCart();
            selectPO.ClickTramitar();

            createPO.RellenarFormulario(CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_EMAIL, PAGO_GOOGLE);
            createPO.ContinuarCompra();
            createPO.ConfirmarCompraModal();

            Assert.True(detallePO.EsPaginaDetalle(), "No redirigió a Detalle");
            Assert.True(detallePO.ContieneTexto(CLIENTE_NOMBRE), "Nombre incorrecto en detalle");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-PB-02")]
        public void HU_PB_02_Filtrado_Bocadillos()
        {
            var selectPO = new SelectPedirBocadillo_PO(_driver, _output);
            Ir_A_Seleccion();

            selectPO.SearchBocadillos(BOCADILLO_TEST);
            Assert.True(selectPO.CheckListOfBocadillos(BOCADILLO_TEST), "El filtro falló");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-PB-03")]
        public void HU_PB_03_Carrito_Vacio_Boton_Deshabilitado()
        {
            var selectPO = new SelectPedirBocadillo_PO(_driver, _output);
            Ir_A_Seleccion();

            Assert.True(selectPO.ComprarNotAvailable(), "El botón debería estar deshabilitado");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-PB-04")]
        public void HU_PB_04_Eliminar_Bocadillo_Actualiza_Precio()
        {
            var selectPO = new SelectPedirBocadillo_PO(_driver, _output);
            Ir_A_Seleccion();

            selectPO.AddFirstBocadilloToCart();
            selectPO.AddFirstBocadilloToCart();

            string precioAntes = selectPO.GetTotalText();
            _output.WriteLine($"Precio Inicial: {precioAntes}");

            selectPO.RemoveFirstItemFromCart();

            string precioDespues = selectPO.GetTotalText();
            _output.WriteLine($"Precio Final: {precioDespues}");

            Assert.NotEqual(precioAntes, precioDespues);
            Assert.False(selectPO.ComprarNotAvailable()); 
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-PB-05")]
        public void HU_PB_05_Datos_Obligatorios_Faltantes()
        {
            var selectPO = new SelectPedirBocadillo_PO(_driver, _output);
            var createPO = new CreatePedirBocadillo_PO(_driver, _output);

            Ir_A_Seleccion();
            selectPO.AddFirstBocadilloToCart();
            selectPO.ClickTramitar();
            createPO.RellenarFormulario("", "ApellidoTest", "email@test.com", "Tarjeta");
            createPO.ContinuarCompra();
            Thread.Sleep(1000);
            Assert.True(_driver.Url.Contains("Create"), "Debería seguir en el formulario");
            Assert.False(_driver.Url.Contains("DetallePedido"), "No debería haber avanzado");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-PB-06")]
        public void HU_PB_06_Modificar_Seleccion_Mantiene_Datos()
        {
            var selectPO = new SelectPedirBocadillo_PO(_driver, _output);
            var createPO = new CreatePedirBocadillo_PO(_driver, _output);

            Ir_A_Seleccion();
            selectPO.AddFirstBocadilloToCart();
            selectPO.ClickTramitar();

            createPO.RellenarFormulario("NombrePersistente", "", "", "");

            createPO.ClickSeguirComprando();
            Thread.Sleep(1000);

            Assert.True(_driver.Url.Contains("SeleccionarPedirBocadillo"));

            selectPO.ClickTramitar();
            Thread.Sleep(1000);

            string valorInput = createPO.GetNombreValue();
            Assert.Equal("NombrePersistente", valorInput);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-PB-Examen")]
        public void HU_PB_Examen()
        {
            var selectPO = new SelectPedirBocadillo_PO(_driver, _output);
            var createPO = new CreatePedirBocadillo_PO(_driver, _output);
            var detallePO = new DetallePedido_PO(_driver, _output);

            Ir_A_Seleccion();

            selectPO.SearchBocadillos("Serranito");
            selectPO.AddFirstBocadilloToCart();
            Assert.False(selectPO.ComprarNotAvailable());
            selectPO.SearchBocadillos("Calamares");
            selectPO.AddFirstBocadilloToCart();
            Assert.False(selectPO.ComprarNotAvailable());

            selectPO.RemoveItemFromCart(0);
            Assert.False(selectPO.ComprarNotAvailable());
            selectPO.ClickTramitar();

            createPO.RellenarFormulario(CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_EMAIL, PAGO_GOOGLE);
            createPO.ContinuarCompra();
            createPO.ConfirmarCompraModal();

            Thread.Sleep(2000);
            Assert.True(detallePO.ContieneTexto(CLIENTE_NOMBRE));
            Assert.True(detallePO.ContieneTexto("Calamares"));

        }
    }
}