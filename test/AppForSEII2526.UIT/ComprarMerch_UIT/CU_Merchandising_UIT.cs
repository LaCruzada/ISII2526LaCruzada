using AppForMovies.UIT.Shared;
using OpenQA.Selenium;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.ComprarMerch_UIT
{
    public class CU_Merchandising_UIT : UC_UIT
    {
        private const string PRODUCTO_TEST = "Camiseta Logo";
        private const string CLIENTE_NOMBRE = "SeleniumUser";
        private const string CLIENTE_APELLIDO = "Automated";
        private const string CLIENTE_EMAIL = "uit@test.com";
        private const string CLIENTE_DIRECCION = "Calle Test 123";
        private const string PAGO_TARJETA = "Tarjeta";

        public CU_Merchandising_UIT(ITestOutputHelper output) : base(output)
        {
        }

        private void Ir_A_Seleccion()
        {
            _driver.Navigate().GoToUrl(_URI + "merchandising/seleccionar");
            Thread.Sleep(2000);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-CM-01")]
        public void HU_CM_01_Compra_Estandar_Correcta()
        {
            var selectPO = new SelectMerch_PO(_driver, _output);
            var createPO = new CreateMerch_PO(_driver, _output);
            var detallePO = new DetalleMerch_PO(_driver, _output);

            Ir_A_Seleccion();

            selectPO.AddFirstProductoToCart();
            selectPO.ClickTramitar();

            createPO.RellenarFormulario(CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_EMAIL, CLIENTE_DIRECCION, PAGO_TARJETA);
            createPO.ContinuarCompra();
            createPO.ConfirmarCompraModal();

            Assert.True(_driver.Url.Contains("seleccionar"), "Debería volver a seleccionar tras compra");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-CM-02")]
        public void HU_CM_02_Filtrado_Productos()
        {
            var selectPO = new SelectMerch_PO(_driver, _output);
            Ir_A_Seleccion();

            selectPO.FiltrarPorTipo("Taza");
            Assert.True(selectPO.CheckListOfProductos("Taza"), "El filtro falló");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-CM-03")]
        public void HU_CM_03_Carrito_Vacio_Boton_Deshabilitado()
        {
            var selectPO = new SelectMerch_PO(_driver, _output);
            Ir_A_Seleccion();

            Assert.True(selectPO.ComprarNotAvailable(), "El botón debería estar deshabilitado");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-CM-04")]
        public void HU_CM_04_Eliminar_Producto_Actualiza_Precio()
        {
            var selectPO = new SelectMerch_PO(_driver, _output);
            Ir_A_Seleccion();

            selectPO.AddFirstProductoToCart();
            selectPO.AddFirstProductoToCart();

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
        [Trait("CasoPrueba", "HU-CM-05")]
        public void HU_CM_05_Datos_Obligatorios_Faltantes()
        {
            var selectPO = new SelectMerch_PO(_driver, _output);
            var createPO = new CreateMerch_PO(_driver, _output);

            Ir_A_Seleccion();
            selectPO.AddFirstProductoToCart();
            selectPO.ClickTramitar();

            createPO.RellenarFormulario("", CLIENTE_APELLIDO, CLIENTE_EMAIL, CLIENTE_DIRECCION, PAGO_TARJETA);
            createPO.ContinuarCompra();
            Thread.Sleep(1000);

            Assert.True(_driver.Url.Contains("crearcompra"), "Debería seguir en el formulario");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        [Trait("CasoPrueba", "HU-CM-06")]
        public void HU_CM_06_Modificar_Seleccion_Mantiene_Datos()
        {
            var selectPO = new SelectMerch_PO(_driver, _output);
            var createPO = new CreateMerch_PO(_driver, _output);

            Ir_A_Seleccion();
            selectPO.AddFirstProductoToCart();
            selectPO.ClickTramitar();

            createPO.RellenarFormulario("NombrePersistente", "", "", "", "");

            createPO.ClickVolverAtras();
            Thread.Sleep(1000);

            Assert.True(_driver.Url.Contains("seleccionar"));

            selectPO.ClickTramitar();
            Thread.Sleep(1000);

            string valorInput = createPO.GetNombreValue();
            Assert.Equal("NombrePersistente", valorInput);
        }
    }
}